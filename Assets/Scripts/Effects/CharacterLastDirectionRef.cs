using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class CharacterLastDirectionRef : LatestDirectionRef
{
    public CharacterAnimController AnimController;
    private Vector2 _latestNonZeroDirection;
    public override Vector2 LatestDirection
    {
        get
        {
            _latestNonZeroDirection = new Vector2(
                Mathf.Abs(_latestNonZeroDirection.x) > Mathf.Abs(AnimController.LatestDirection.x)
                    ? _latestNonZeroDirection.x
                    : AnimController.LatestDirection.x,
                Mathf.Abs(_latestNonZeroDirection.y) > Mathf.Abs(AnimController.LatestDirection.y)
                    ? _latestNonZeroDirection.y
                    : AnimController.LatestDirection.y
            );
            return _latestNonZeroDirection;
        }
    }
}
