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
    public int CurrentMiniStage { get; private set; }

    public ScriptPrefab Stage;
    public ScriptPrefab PayUpStage;

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
            if (CurrentMiniStage == 0)
            {
                while (_player.transform.position.x < InitialSpawnX)
                {
                    yield return TimeYields.WaitOneFrameX;
                }

                if (_effects.GetEffect(Stage).TryGetFromPool(out var stage))
                {
                    stage.Component.transform.position = new Vector3(InitialSpawnX + SpawnOffset,0,0);
                }

                CurrentMiniStage++;
            }

            while (_player.transform.position.x < InitialSpawnX + SpawnXDistance * CurrentMiniStage)
            {
                yield return TimeYields.WaitOneFrameX;
            }

            if (_effects.GetEffect((CurrentMiniStage-1) % 3 == 0 ? PayUpStage : Stage).TryGetFromPool(out var stg))
            {
                stg.Component.transform.position = new Vector3(InitialSpawnX + SpawnXDistance * CurrentMiniStage + SpawnOffset, 0, 0);
            }

            CurrentMiniStage++;

            yield return TimeYields.WaitOneFrameX;
        }
    }
}