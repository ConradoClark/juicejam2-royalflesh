using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Licht.Impl.Orchestration;
using Licht.Unity;
using Licht.Unity.CharacterControllers;
using Licht.Unity.Objects;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterCombatHandler : BaseGameObject
{
    public LichtTopDownMoveController MoveController;
    public ScriptInput AttackInput;
    public CharacterAnimController AnimController;
    public GameObject Limbs;
    private bool _enabled;
    private PlayerInput _playerInput;
    private InputAction _attackAction;
    private LimbAttack[] _attacks;
    private LimbAttack[] _currentCombo;

    public float BufferingTimeInMs;

    private bool _isAtkBuffering;

    protected override void OnAwake()
    {
        base.OnAwake();
        _playerInput = BasicToolbox.Instance(true).GetComponent<PlayerInput>();
        _attackAction = _playerInput.actions[AttackInput.ActionName];
        _attacks = Limbs.GetComponentsInChildren<LimbAttack>();
    }

    private void OnEnable()
    {
        _enabled = true;
        DefaultMachinery.AddBasicMachine(HandleBuffer());
        DefaultMachinery.AddBasicMachine(HandleCombat());
        _currentCombo = _attacks.OrderBy(atk => atk.Priority).ToArray();
    }
    private void OnDisable()
    {
        _enabled = false;
    }

    private IEnumerable<IEnumerable<Action>> HandleBuffer()
    {
        while (_enabled)
        {
            if (_attackAction.WasPerformedThisFrame())
            {
                _isAtkBuffering = true;
                yield return TimeYields.WaitMilliseconds(GameTimer, BufferingTimeInMs,
                    breakCondition: () => !_isAtkBuffering, resetCondition: () => _attackAction.WasPerformedThisFrame());
                _isAtkBuffering = false;
            }
            else
            {
                yield return TimeYields.WaitOneFrameX;
            }
        }
    }

    private IEnumerable<IEnumerable<Action>> HandleCombat()
    {
        while (_enabled)
        {
            if (_isAtkBuffering && !MoveController.IsBlocked)
            {
                _isAtkBuffering = false;
                MoveController.BlockMovement(this);

                for (var index = 0; index < _currentCombo.Length; index++)
                {
                    var attack = _currentCombo[index];
                    if (index > 0 && !_isAtkBuffering) break;
                    switch (attack.Type)
                    {
                        case LimbAttack.LimbAttackType.Left:
                            AnimController.TriggerLeftAttack();
                            break;
                        case LimbAttack.LimbAttackType.Right:
                            AnimController.TriggerRightAttack();
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                    yield return TimeYields.WaitMilliseconds(GameTimer, attack.AttackCooldownInMs);
                }

                _isAtkBuffering = false;

                MoveController.UnblockMovement(this);
            }
            else yield return TimeYields.WaitOneFrameX;
        }
    }
}
