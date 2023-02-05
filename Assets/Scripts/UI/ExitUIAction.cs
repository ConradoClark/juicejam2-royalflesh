using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Character;
using Licht.Unity.Objects;
using Licht.Unity.UI;
using UnityEngine;

public class ExitUIAction : UIAction
{
    public Color SelectedColor;
    public SpriteRenderer Sprite;

    private WannaChangeYourBones _wannaChangeYourBones;
    private PlayerIdentifier _player;
    private LimbCompendium _limbCompendium;
    public SelectBonesUIAction[] SelectedBones;

    protected override void OnAwake()
    {
        base.OnAwake();
        _wannaChangeYourBones = WannaChangeYourBones.Instance(true);
        _player = PlayerIdentifier.Instance(true);
        _limbCompendium = LimbCompendiumRef.Instance(true).LimbCompendium;
    }

    public override IEnumerable<IEnumerable<Action>> DoAction()
    {
        _wannaChangeYourBones.Deactivate();

        var changed = SelectedBones.Any(selection => 
            selection.SelectedItem is { Equipped: false });

        if (changed)
        {
            foreach (var item in _player.Inventory.Inventory)
            {
                item.Equipped = SelectedBones.Any(selection => selection.SelectedItem == item);
            }

            _limbCompendium.ChangeLimbs(_player.Inventory.Inventory);
        }

        yield break;
    }

    public override void OnSelect(bool manual)
    {
        Selected = true;
        Sprite.color = SelectedColor;
    }

    public override void OnDeselect()
    {
        Selected = false;
        Sprite.color = Color.white;
    }

    public override void OnInit()
    {
    }
}
