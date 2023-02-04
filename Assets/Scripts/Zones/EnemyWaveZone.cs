using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Licht.Impl.Orchestration;
using Licht.Unity.Effects;
using Licht.Unity.Objects;
using Licht.Unity.Pooling;
using UnityEngine;

public class EnemyWaveZone : WaveZone
{
    public float DelayBetweenWavesInSeconds;
    private EffectsManager _effects;

    public override WaveZoneManager.ZoneState Type => WaveZoneManager.ZoneState.Fight;

    protected override void OnAwake()
    {
        base.OnAwake();
        _effects = EffectsManager.Instance();
    }

    [Serializable]
    public struct EnemySpawn
    {
        public ScriptPrefab Enemy;
        public Vector3 Position;
    }

    [Serializable]
    public struct EnemyWave
    {
        public EnemySpawn[] Spawns;
    }

    public EnemyWave[] EnemyWaves;


    public override IEnumerable<IEnumerable<Action>> RunWave()
    {
        foreach (var enemyWave in EnemyWaves)
        {
            var enemies = new List<IPoolableComponent>();
            foreach (var spawn in enemyWave.Spawns)
            {
                var pool = _effects.GetEffect(spawn.Enemy);
                if (!pool.TryGetFromPool(out var enemy)) continue;
                enemies.Add(enemy);
                enemy.Component.transform.position = new Vector3(transform.position.x, 0, 0) + spawn.Position;
            }

            while (enemies.Any(e => e.IsActive))
            {
                yield return TimeYields.WaitOneFrameX;
            }

            yield return TimeYields.WaitSeconds(GameTimer, DelayBetweenWavesInSeconds);
        }
    }
}

