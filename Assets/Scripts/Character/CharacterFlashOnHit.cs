using System;
using System.Collections.Generic;
using Licht.Impl.Orchestration;
using Licht.Unity.Builders;
using Licht.Unity.Objects;
using UnityEngine;

public class CharacterFlashOnHit : BaseGameObject
{
    public CharacterStats Stats;
    public GameObject Limbs;

    public float InvincibilityDurationInSeconds;

    public float FlashFrequencyInMs;
    public Color Color;
    private bool _enabled;
    private SpriteRenderer[] _sprites;

    protected override void OnAwake()
    {
        base.OnAwake();
        _sprites = Limbs.GetComponentsInChildren<SpriteRenderer>(true);
    }

    private void OnEnable()
    {
        _enabled = true;
        Stats.OnDamage += Stats_OnDamage;
    }

    private void OnDisable()
    {
        _enabled = false;
        Stats.OnDamage -= Stats_OnDamage;
    }

    private void Stats_OnDamage(CharacterStats.DamageEventHandler obj)
    {
        if (obj.Deadly) return;
        DefaultMachinery.AddBasicMachine(HandleDamage());
    }

    private IEnumerable<IEnumerable<Action>> HandleDamage()
    {
        Stats.CanBeHit = false;
        DefaultMachinery.AddBasicMachine(Flash());
        yield return TimeYields.WaitSeconds(GameTimer, InvincibilityDurationInSeconds);
        Stats.CanBeHit = true;
    }

    private IEnumerable<IEnumerable<Action>> Flash()
    {
        while (_enabled && !Stats.CanBeHit)
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
                .BreakIf(()=> Stats.CanBeHit)
                .Over(FlashFrequencyInMs * 0.0005f)
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
                .BreakIf(() => Stats.CanBeHit)
                .Over(FlashFrequencyInMs * 0.0005f)
                .UsingTimer(GameTimer)
                .Build();

            yield return flashBack;
        }
    }
}
