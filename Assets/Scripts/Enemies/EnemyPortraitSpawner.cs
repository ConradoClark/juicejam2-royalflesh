using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Licht.Unity.Objects;
using UnityEngine;

public class EnemyPortraitSpawner : MonoBehaviour
{
    public EnemyPortrait Portrait  { get; private set; }
    public ScriptPrefab PortraitPrefab;
    private EnemyPortraitPoolManager _poolManager;
    private void Awake()
    {
        _poolManager = EnemyPortraitPoolManagerRef.Instance(true).PoolManager;
    }

    private void OnEnable()
    {
        if (!_poolManager.Effects[PortraitPrefab].TryGetFromPool(out var portrait)) return;
        Portrait = portrait;
        Portrait.Actor = gameObject;
    }

    private void OnDisable()
    {
        if (Portrait != null) _poolManager.Effects[PortraitPrefab].Release(Portrait);
    }
}
