using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Licht.Unity.Objects;

public class LimbAttack : BaseGameObject
{
    public float AttackCooldownInMs;
    public int Priority;
    public CustomAnimationEventListener[] ListenersToEnable;
    public LimbAttackType Type;

    public enum LimbAttackType
    {
        Left,
        Right
    }
}