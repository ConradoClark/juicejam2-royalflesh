using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Licht.Unity.Objects;
using UnityEngine;

public class EnemyPortraitHandler : SceneObject<EnemyPortraitHandler>
{
    public float MinDistanceToShow;
    private PlayerIdentifier _playerIdentifier;
    private List<EnemyPortrait> _currentPortraits;

    public Transform Portrait1Reference;
    public Transform Portrait2Reference;
    private Transform[] _portraitFrames;

    private void Awake()
    {
        _playerIdentifier = PlayerIdentifier.Instance(true);
    }

    private void OnEnable()
    {
        _portraitFrames = new[]
        {
            Portrait1Reference,
            Portrait2Reference
        };
    }

    public void AddPortrait(EnemyPortrait portrait)
    {
        _currentPortraits ??= new List<EnemyPortrait>();
        _currentPortraits.Add(portrait);
    }

    public void RemovePortrait(EnemyPortrait portrait)
    {
        _currentPortraits ??= new List<EnemyPortrait>();
        if (_currentPortraits.Contains(portrait)) _currentPortraits.Remove(portrait);
    }

    private void Update()
    {
        var portraitsWithDistance = _currentPortraits.Select(p => (portrait: p, distance:
            Vector2.Distance(p.Actor.transform.position, _playerIdentifier.transform.position)))
            .Where(p=>p.distance <= MinDistanceToShow)
            .OrderBy(p=>p.distance)
            .ToArray();

        foreach (var portrait in _currentPortraits)
        {
            portrait.Hide();
        }

        for (var index = 0; index < portraitsWithDistance.Length; index++)
        {
            var portrait = portraitsWithDistance[index];
            if (index > 1)
            {
                break;
            }
            
            portrait.portrait.transform.position = _portraitFrames[index].position;
            portrait.portrait.Show();
        }
    }
}
