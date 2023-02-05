using System;
using System.Collections.Generic;
using Licht.Impl.Orchestration;
using Licht.Unity.Effects;
using Licht.Unity.Objects;
using UnityEngine;

public class MiniStageSpawner : BaseGameObject
{
    public float InitialSpawnX;
    public float SpawnXDistance;

    public float SpawnOffset;
    private float _currentMiniStage;

    public ScriptPrefab Stage;

    private PlayerIdentifier _player;
    private EffectsManager _effects;

    protected override void OnAwake()
    {
        base.OnAwake();
        _player = PlayerIdentifier.Instance(true);
        _effects = EffectsManager.Instance(true);
    }

    private void OnEnable()
    {
        DefaultMachinery.AddBasicMachine(HandleSpawn());
    }

    private IEnumerable<IEnumerable<Action>> HandleSpawn()
    {
        while (enabled)
        {
            if (_currentMiniStage == 0)
            {
                while (_player.transform.position.x < InitialSpawnX)
                {
                    yield return TimeYields.WaitOneFrameX;
                }

                if (_effects.GetEffect(Stage).TryGetFromPool(out var stage))
                {
                    stage.Component.transform.position = new Vector3(InitialSpawnX + SpawnOffset,0,0);
                }

                _currentMiniStage++;
            }

            while (_player.transform.position.x < InitialSpawnX + SpawnXDistance * _currentMiniStage)
            {
                yield return TimeYields.WaitOneFrameX;
            }

            if (_effects.GetEffect(Stage).TryGetFromPool(out var stg))
            {
                stg.Component.transform.position = new Vector3(InitialSpawnX + SpawnXDistance * _currentMiniStage + SpawnOffset, 0, 0);
            }

            _currentMiniStage++;

            yield return TimeYields.WaitOneFrameX;
        }
    }
}