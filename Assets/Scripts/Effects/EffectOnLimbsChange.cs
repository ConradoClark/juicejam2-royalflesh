using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Character;
using Licht.Unity.Effects;
using Licht.Unity.Objects;
using Licht.Unity.Pooling;
using UnityEngine;

public class EffectOnLimbsChange : BaseGameObject
{
    public ScriptPrefab Effect;
    public Vector3 Offset;
    private PlayerIdentifier _player;
    private LimbCompendium _limbCompendium;
    private PrefabPool _pool;

    protected override void OnAwake()
    {
        base.OnAwake();
        _player = PlayerIdentifier.Instance(true);
        _limbCompendium = LimbCompendiumRef.Instance(true).LimbCompendium;
        _pool = EffectsManager.Instance(true).GetEffect(Effect);
    }
    private void OnEnable()
    {
        _limbCompendium.OnLimbsChanged += LimbCompendium_OnLimbsChanged;
    }

    private void LimbCompendium_OnLimbsChanged()
    {
        if (!_pool.TryGetFromPool(out var effect)) return;
        effect.Component.transform.position = _player.transform.position + Offset;
    }

    private void OnDisable()
    {
        _limbCompendium.OnLimbsChanged -= LimbCompendium_OnLimbsChanged;
    }
}
