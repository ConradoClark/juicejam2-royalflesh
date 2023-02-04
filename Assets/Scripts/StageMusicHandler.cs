using System;
using System.Collections.Generic;
using Licht.Impl.Orchestration;
using Licht.Unity.Objects;
using UnityEngine;

public class StageMusicHandler: BaseGameObject
{
    public AudioSource Song;
    public StageIntro Intro;
    protected override void OnAwake()
    {
        base.OnAwake();
    }

    private void OnEnable()
    {
        DefaultMachinery.AddBasicMachine(HandleMusic());
    }

    private IEnumerable<IEnumerable<Action>> HandleMusic()
    {
        while (!Intro.IsOver)
        {
            yield return TimeYields.WaitOneFrameX;
        }

        Song.Play();
    }
}
