using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Licht.Unity.Extensions;
using Licht.Unity.Objects;
using Licht.Unity.Physics;
using UnityEngine;
public class ShadowComparer : BaseGameObject
{
    public LichtPhysicsObject PhysicsObject;
    public BoxCollider2D BoundsCollider;

    protected override void OnAwake()
    {
        base.OnAwake();
    }

    private void OnEnable()
    {
        PhysicsObject.AddCustomObject(this);
    }

    public static bool IsZIndexInRange(LichtPhysics physics, Collider2D source, Collider2D target)
    {
        if (!physics.TryGetPhysicsObjectByCollider(source, out var sourceObject)) return false;
        if (!physics.TryGetPhysicsObjectByCollider(target, out var targetObject)) return false;
        if (!sourceObject.TryGetCustomObject(out ShadowComparer sourceShadow)) return false;
        if (!targetObject.TryGetCustomObject(out ShadowComparer targetShadow)) return false;

        var sourceShadowRange = new
        {
            Min = sourceShadow.BoundsCollider.transform.position.y +
                    sourceShadow.BoundsCollider.offset.y - sourceShadow.BoundsCollider.size.y * 0.5f,
            Max = sourceShadow.BoundsCollider.transform.position.y +
                    sourceShadow.BoundsCollider.offset.y + sourceShadow.BoundsCollider.size.y * 0.5f
        };

        var targetShadowRange = new
        {
            Min = targetShadow.BoundsCollider.transform.position.y +
                targetShadow.BoundsCollider.offset.y - targetShadow.BoundsCollider.size.y * 0.5f,
            Max = targetShadow.BoundsCollider.transform.position.y +
                  targetShadow.BoundsCollider.offset.y + targetShadow.BoundsCollider.size.y * 0.5f
        };

        var intersects = !(sourceShadowRange.Max < targetShadowRange.Min
                           || targetShadowRange.Max < sourceShadowRange.Min);

        return intersects;
    }
}
