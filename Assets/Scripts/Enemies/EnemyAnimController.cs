using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Licht.Unity.Objects;
using Licht.Unity.Physics;
using UnityEngine;

public class EnemyAnimController : BaseGameObject
{
    public bool Flip { get; private set; }
    private bool _knockBack;
    private SpriteRenderer[] _sprites;
    public Animator Animator;
    public LichtPhysicsObject PhysicsObject;

    private PlayerIdentifier _player;

    public void SetKnockBack(bool knockBack)
    {
        _knockBack = knockBack;
    }

    public void SetWalking(bool walking)
    {
        Animator.SetBool("Walking", walking);
    }

    private void OnEnable()
    {
        SetKnockBack(false);
    }

    protected override void OnAwake()
    {
        base.OnAwake();
        _sprites = GetComponentsInChildren<SpriteRenderer>(true);
        _player = PlayerIdentifier.Instance();
    }

    public void TriggerRightAttack()
    {
        Animator.SetTrigger("RightAtk");
    }

    public void TriggerLeftAttack()
    {
        Animator.SetTrigger("LeftAtk");
    }

    public void SetLockOn(bool lockOn)
    {
        IsLockedOn = lockOn;
    }

    public bool IsLockedOn { get; private set; }

    private void Update()
    {
        if (_knockBack) return;

        Flip = IsLockedOn ? _player.ShadowRef.position.x < transform.position.x : PhysicsObject.LatestDirection.x < 0;

        Animator.SetBool("FlipX", Flip);
        foreach (var sprite in _sprites)
        {
            sprite.flipX = Flip;
        }
    }
}
