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
    private bool _flip;
    private SpriteRenderer[] _sprites;
    public Animator Animator;
    public LichtPhysicsObject PhysicsObject;

    public void SetWalking(bool walking)
    {
        Animator.SetBool("Walking", walking);
    }

    protected override void OnAwake()
    {
        base.OnAwake();
        _sprites = GetComponentsInChildren<SpriteRenderer>(true);
    }

    private void Update()
    {
        _flip = PhysicsObject.LatestDirection.x < 0;
        Animator.SetBool("FlipX", _flip);
        foreach (var sprite in _sprites)
        {
            sprite.flipX = _flip;
        }
    }
}
