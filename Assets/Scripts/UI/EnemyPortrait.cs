using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Licht.Unity.Objects;
using Licht.Unity.Pooling;
using UnityEngine;

public class EnemyPortrait : EffectPoolable
{
    public HPCounter HPCounter;
    public GameObject Actor;
    private EnemyPortraitHandler _enemyPortraitHandler;
    private SpriteRenderer[] _sprites;
    protected override void OnAwake()
    {
        base.OnAwake();
        _enemyPortraitHandler = EnemyPortraitHandler.Instance(true);
        _sprites = GetComponentsInChildren<SpriteRenderer>(true);
    }

    private void OnEnable()
    {
       
    }

    private void OnDisable()
    {
        _enemyPortraitHandler.RemovePortrait(this);
    }

    public override void OnActivation()
    {
        _enemyPortraitHandler.AddPortrait(this);
    }

    public void Show()
    {
        foreach (var sprite in _sprites)
        {
            sprite.enabled = true;
        }
    }

    public void Hide()
    {
        foreach (var sprite in _sprites)
        {
            sprite.enabled = false;
        }
    }
}
