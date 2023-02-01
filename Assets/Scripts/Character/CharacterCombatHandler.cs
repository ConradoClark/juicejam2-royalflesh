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
        DefaultMachinery.AddBasicMachine(HandleCombat());
    }
    private void OnDisable()
    {
        _enabled = false;
    }

    private IEnumerable<IEnumerable<Action>> HandleCombat()
    {
        while (_enabled)
        {
            if (_attackAction.WasPerformedThisFrame())
            {
                MoveController.BlockMovement(this);
                switch (_attacks[0].Type)
                {
                    case LimbAttack.LimbAttackType.Left:
                        break;
                    case LimbAttack.LimbAttackType.Right:
                        AnimController.TriggerRightAttack();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                yield return TimeYields.WaitMilliseconds(GameTimer, _attacks[0].AttackCooldownInMs);
                MoveController.UnblockMovement(this);
            }
            else yield return TimeYields.WaitOneFrameX;
        }
    }
}
