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
    public override Vector2 LatestDirection => PhysicsObject.LatestDirection;
}

