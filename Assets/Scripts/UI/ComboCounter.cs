using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Licht.Impl.Orchestration;
using Licht.Unity.Extensions;
using Licht.Unity.Objects;
using TMPro;
using UnityEngine;

public class ComboCounter : BaseGameObject
{
    public CharacterCombo ComboTracker;
    public Vector3 SourcePosition;
    public Vector3 TargetPosition;
    private bool _showing;

    public TMP_Text ComboText;

    public SpriteRenderer TimerSprite;
    public float TimerSizeAtMax;
    public float TimerSizeAtMin;


    private void OnEnable()
    {
        ComboTracker.OnComboHit += ComboTracker_OnComboHit;
        ComboTracker.OnComboEnd += ComboTracker_OnComboEnd;
    }

    private void OnDisable()
    {
        ComboTracker.OnComboHit -= ComboTracker_OnComboHit;
        ComboTracker.OnComboEnd -= ComboTracker_OnComboEnd;
    }

    private void Update()
    {
        if (ComboTracker.DurationPerHitInSeconds == 0) return;
        TimerSprite.size = new Vector2(TimerSizeAtMin + (TimerSizeAtMax - TimerSizeAtMin)
            * (ComboTracker.CurrentRemainingTimeInSecondsToBreak / ComboTracker.DurationPerHitInSeconds), 
        TimerSprite.size.y);
    }

    private void ComboTracker_OnComboEnd()
    {
        if (_showing)
        {
            DefaultMachinery.AddBasicMachine(HideCounter());
        }
    }

    private void ComboTracker_OnComboHit(int obj)
    {
        if (!_showing)
        {
            DefaultMachinery.AddBasicMachine(ShowCounter());
        }

        ComboText.text = obj.ToString().PadLeft(3, '0');
    }

    private IEnumerable<IEnumerable<Action>> AnimateComboText()
    {
        yield break;
    }

    private IEnumerable<IEnumerable<Action>> ShowCounter()
    {
        _showing = true;
        yield return transform.GetAccessor()
            .LocalPosition
            .X.SetTarget(SourcePosition.x)
            .Over(0.2f)
            .Easing(EasingYields.EasingFunction.QuarticEaseOut)
            .UsingTimer(GameTimer)
            .Build();
    }

    private IEnumerable<IEnumerable<Action>> HideCounter()
    {
        _showing = false;
        yield return transform.GetAccessor()
            .LocalPosition
            .X.SetTarget(TargetPosition.x)
            .Over(0.2f)
            .Easing(EasingYields.EasingFunction.QuarticEaseOut)
            .UsingTimer(GameTimer)
            .Build();
    }
}
