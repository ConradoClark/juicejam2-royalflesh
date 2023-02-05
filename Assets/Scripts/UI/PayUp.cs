using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Licht.Impl.Orchestration;
using Licht.Unity.Builders;
using Licht.Unity.Extensions;
using Licht.Unity.Objects;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;

public class PayUp : SceneObject<PayUp>
{
    private DefaultGameTimer _gameTimer;
    private DefaultUITimer _uiTimer;
    private BasicMachinery<object> _defaultMachinery;
    private PlayerIdentifier _player;

    public SpriteRenderer Square;
    public GameObject Content;
    public MiniStageSpawner StageSpawner;

    public TMP_Text RequiredText;
    public TMP_Text AmountPaidText;

    public int AmountRequired;
    public int Paid;

    public SelectBonesUIAction _uiAction;

    private void Awake()
    {
        _player = PlayerIdentifier.Instance(true);
        _gameTimer = DefaultGameTimer.Instance(true);
        _uiTimer = DefaultUITimer.Instance(true);
        _defaultMachinery = DefaultMachinery.Instance(true).MachineryRef.Machinery;
    }

    public void Activate()
    {
        Square.color = new Color(0, 0, 0, 0);
        gameObject.SetActive(true);
    }

    public void Deactivate()
    {
        _defaultMachinery.AddBasicMachine(ExitLimbMenu());
    }

    private void OnEnable()
    {
        _defaultMachinery.AddBasicMachine(SlowDownTime());
    }

    private IEnumerable<IEnumerable<Action>> ExitLimbMenu()
    {
        Content.SetActive(false);
        var speedUp = new LerpBuilder(f => _gameTimer.TimerRef.Timer.Multiplier = f,
                () => (float)_gameTimer.TimerRef.Timer.Multiplier)
            .SetTarget(1)
            .Over(1)
            .Easing(EasingYields.EasingFunction.QuadraticEaseIn)
            .UsingTimer(_uiTimer.TimerRef.Timer)
            .Build();

        var sqr = Square.GetAccessor()
            .Color
            .ToColor(new Color(0, 0, 0, 0))
            .Over(1)
            .Easing(EasingYields.EasingFunction.QuadraticEaseIn)
            .UsingTimer(_uiTimer.TimerRef.Timer)
            .Build();

        yield return speedUp.Combine(sqr);


        gameObject.SetActive(false);
    }

    private IEnumerable<IEnumerable<Action>> SlowDownTime()
    {
        AmountRequired = 5 + StageSpawner.CurrentMiniStage * 5;
        RequiredText.text = $"{AmountRequired} Coins";
        AmountPaidText.text = $"0 Coins";

        var slowDown = new LerpBuilder(f => _gameTimer.TimerRef.Timer.Multiplier = f,
                () => (float)_gameTimer.TimerRef.Timer.Multiplier)
            .SetTarget(0.0001f)
            .Over(1)
            .Easing(EasingYields.EasingFunction.QuadraticEaseIn)
            .UsingTimer(_uiTimer.TimerRef.Timer)
            .Build();

        var sqr = Square.GetAccessor()
            .Color
            .ToColor(new Color(0, 0, 0, 0.985f))
            .Over(1)
            .Easing(EasingYields.EasingFunction.QuadraticEaseIn)
            .UsingTimer(_uiTimer.TimerRef.Timer)
            .Build();

        yield return slowDown.Combine(sqr);

        Content.SetActive(true);
        foreach (var item in _player.Inventory.Inventory)
        {
            item.New = false;
        }
    }

    public IEnumerable<IEnumerable<Action>> Pay(int amount)
    {
        float currentPaid = Paid;
        Paid += amount;

        var payment = new LerpBuilder(f => currentPaid = f, () => currentPaid)
            .SetTarget(Paid)
            .Over(1)
            .UsingTimer(_uiTimer.TimerRef.Timer)
            .Easing(EasingYields.EasingFunction.QuadraticEaseIn)
            .Build();

        foreach (var step in payment)
        {
            AmountPaidText.text = $"{Mathf.RoundToInt(currentPaid)} Coins";
            yield return step.AsEnumerable();
        }

        if (Paid >= AmountRequired)
        {
            RequiredText.text = "NOTHING";
            AmountPaidText.text = "THANKS";
            yield return TimeYields.WaitSeconds(_uiTimer.TimerRef.Timer, 2);
            Deactivate();
        }

        else if (_player.Inventory.Inventory.Count == 0)
        {
            RequiredText.text = "Your LIFE";
        }
    }

    public IEnumerable<IEnumerable<Action>> Die()
    {
        // load death scene
        yield break;
    }
}
