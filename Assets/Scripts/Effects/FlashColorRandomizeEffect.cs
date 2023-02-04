using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Licht.Impl.Orchestration;
using Licht.Unity.Objects;
using UnityEngine;
using Random = UnityEngine.Random;

public class FlashColorRandomizeEffect : BaseGameObject
{
    public Color[] Colors;
    public float ChangeFrequencyInMs;
    public SpriteRenderer SpriteRenderer;

    private bool _enabled;

    private void OnEnable()
    {
        _enabled = true;
        DefaultMachinery.AddBasicMachine(Flash());
    }

    private void OnDisable()
    {
        _enabled = false;
    }

    private IEnumerable<IEnumerable<Action>> Flash()
    {
        
        while (_enabled)
        {
            var frequency = Math.Max(1, ChangeFrequencyInMs);
            yield return TimeYields.WaitMilliseconds(GameTimer, ChangeFrequencyInMs);

            if (Colors.Length == 0) continue;

            var randomColor = Colors[Random.Range(0, Colors.Length)];
            SpriteRenderer.material.SetColor("_Flash", randomColor);
        }
    }
}
