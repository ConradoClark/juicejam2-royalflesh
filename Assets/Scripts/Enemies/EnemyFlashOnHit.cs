using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Licht.Impl.Orchestration;
using Licht.Unity.Builders;
using Licht.Unity.Extensions;
using Licht.Unity.Objects;
using Licht.Unity.Physics;
using Licht.Unity.Physics.CollisionDetection;
using UnityEngine;

public class EnemyFlashOnHit : BaseGameObject
{
    public Basic2DCollisionDetector HurtBoxDetector;
    public GameObject Limbs;

    public Color Color;
    private bool _enabled;

    private SpriteRenderer[] _sprites;
    private LichtPhysics _physics;

    protected override void OnAwake()
    {
        base.OnAwake();
        _sprites = Limbs.GetComponentsInChildren<SpriteRenderer>(true);
        _physics = this.GetLichtPhysics();
    }

    private void OnEnable()
    {
        _enabled = true;
        DefaultMachinery.AddBasicMachine(Flash());
    }

    private void OnDisable()
    {
        _enabled = false;
    }

    private IEnumerable<IEnumerable<Action>> Flash()
    {
        while (_enabled)
        {
            if (HurtBoxDetector.Triggers.Any(
                    t => t.TriggeredHit 
                         && ShadowComparer.IsZIndexInRange(_physics, HurtBoxDetector.Collider, t.Collider)))
            {
                var lerpFloat = 0f;
                var flash = new LerpBuilder(f =>
                    {
                        lerpFloat = f;
                        foreach (var sprite in _sprites)
                        {
                            sprite.material.SetColor("_Flash", Color.Lerp(new Color(0, 0, 0, 0), Color, f));
                        }
                    }, () => lerpFloat)
                    .SetTarget(1f)
                    .Easing(EasingYields.EasingFunction.CubicEaseOut)
                    .Over(0.2f)
                    .UsingTimer(GameTimer)
                    .Build();

                yield return flash;

                var flashBack = new LerpBuilder(f =>
                    {
                        lerpFloat = f;
                        foreach (var sprite in _sprites)
                        {
                            sprite.material.SetColor("_Flash", Color.Lerp(Color, new Color(0, 0, 0, 0), f));
                        }
                    }, () => lerpFloat)
                    .SetTarget(1f)
                    .Easing(EasingYields.EasingFunction.CubicEaseIn)
                    .Over(0.1f)
                    .UsingTimer(GameTimer)
                    .Build();

                yield return flashBack;
            }

            else yield return TimeYields.WaitOneFrameX;
        }
    }
}
