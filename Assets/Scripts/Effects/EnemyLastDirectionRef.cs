using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Licht.Unity.Physics;
using UnityEngine;

public class EnemyLastDirectionRef : LatestDirectionRef
{
    public LichtPhysicsObject PhysicsObject;
    public EnemyAnimController AnimController;
    public override Vector2 LatestDirection => AnimController.IsLockedOn ? 
        AnimController.Flip ? Vector2.left : Vector2.right :
        PhysicsObject.LatestDirection;
}

