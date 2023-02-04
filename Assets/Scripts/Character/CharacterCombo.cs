using System;
using System.Collections.Generic;
using Licht.Impl.Orchestration;
using Licht.Unity.Objects;

public class CharacterCombo : BaseGameObject
{
    public float DurationPerHitInSeconds;

    public int CurrentCombo;
    public CharacterStats Stats;
    private bool _enabled;

    public float CurrentRemainingTimeInSecondsToBreak;

    private int _internalHitCounter;

    public event Action<int> OnComboHit;
    public event Action OnComboEnd;

    private void OnEnable()
    {
        _enabled = true;
        Stats.OnConnectingHit += Stats_OnConnectingHit;
        DefaultMachinery.AddBasicMachine(HandleCombo());
    }

    private void Stats_OnConnectingHit()
    {
        _internalHitCounter++;
    }

    private void OnDisable()
    {
        _enabled = false;
    }

    private IEnumerable<IEnumerable<Action>> HandleCombo()
    {
        var comboBroken = false;
        while (_enabled)
        {
            while (_internalHitCounter == 0) yield return TimeYields.WaitOneFrameX;
            CurrentRemainingTimeInSecondsToBreak = DurationPerHitInSeconds;

            yield return TimeYields.WaitSeconds(GameTimer, DurationPerHitInSeconds, fn =>
            {
                CurrentRemainingTimeInSecondsToBreak = DurationPerHitInSeconds - (float) fn * 0.001f;
            }, () =>
                {
                    if (!Stats.BeingHit) return false;
                    comboBroken = true;
                    return true;

                }, () =>
            {
                var reset = _internalHitCounter != CurrentCombo;
                if (!reset) return false;
                CurrentCombo = _internalHitCounter;
                OnComboHit?.Invoke(CurrentCombo);
                return true;
            });

            if (comboBroken)
            {
                // combo broken effect
            }

            CurrentCombo = _internalHitCounter = 0;
            OnComboEnd?.Invoke();

            yield return TimeYields.WaitOneFrameX;
        }
    }
}
