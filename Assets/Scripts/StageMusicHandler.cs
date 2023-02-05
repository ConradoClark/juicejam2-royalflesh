using System;
using System.Collections.Generic;
using Licht.Impl.Orchestration;
using Licht.Unity.Objects;
using UnityEngine;

public class StageMusicHandler: BaseUIObject
{
    public AudioSource Song;
    public StageIntro Intro;
    private WannaChangeYourBones _wannaChangeYourBones;

    public AudioClip StageSong;
    public AudioClip WannaChangeYourBonesClip;

    private float _currentStageSongTime;

    protected override void OnAwake()
    {
        base.OnAwake();
        _wannaChangeYourBones = WannaChangeYourBones.Instance(true);
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

        start:
        Song.clip = StageSong;
        Song.time = _currentStageSongTime;
        Song.Play();

        while (!_wannaChangeYourBones.isActiveAndEnabled)
        {
            yield return TimeYields.WaitOneFrameX;
        }

        _currentStageSongTime = Song.time;
        Song.Stop();

        yield return TimeYields.WaitSeconds(UITimer, 1);
        Song.clip = WannaChangeYourBonesClip;
        Song.time = 0f;
        Song.Play();

        while (_wannaChangeYourBones.isActiveAndEnabled)
        {
            yield return TimeYields.WaitOneFrameX;
        }

        Song.Stop();

        goto start;

    }
}
