using System;
using Licht.Unity.Objects;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class CharacterSwapTest : BaseGameObject
{
    [Serializable]
    public struct LimbSwap
    {
        public GameObject[] Limbs;
    }

    public LimbSwap[] Swaps;
    private PlayerInput _playerInput;

    protected override void OnAwake()
    {
        base.OnAwake();
        _playerInput = FindObjectOfType<PlayerInput>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_playerInput.actions["debug_swap"].WasPerformedThisFrame())
        {
            foreach (var swap in Swaps)
            {
                var chosen = Random.Range(0, swap.Limbs.Length);
                for (var index = 0; index < swap.Limbs.Length; index++)
                {
                    var obj = swap.Limbs[index];
                    obj.SetActive(index == chosen);
                }
            }
        }
    }
}
