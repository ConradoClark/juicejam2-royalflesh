using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Character;
using Licht.Unity.UI;
using UnityEngine;
using UnityEngine.InputSystem;

public class SelectBonesUIAction : UIAction
{
    public CustomAnimationEventListener.AnimatingLimb Limb;
    public Color SelectedColor;
    public SpriteRenderer Sprite;
    public SpriteRenderer Icon;
    private PlayerIdentifier _player;
    private LimbInventory.LimbItem[] _currentInventory;
    private int _currentIndex;
    private InputAction _verticalAction;
    private LimbItemName _itemName;
    private UILimbsManager _limbsPreview;

    public LimbInventory.LimbItem SelectedItem => _currentInventory.Length >= _currentIndex? _currentInventory[_currentIndex] : null;

    protected override void OnAwake()
    {
        base.OnAwake();
        _player = PlayerIdentifier.Instance(true);
        _verticalAction = MenuContext.PlayerInput.actions[MenuContext.UIVertical.ActionName];
        _itemName = LimbItemName.Instance(true);
        _limbsPreview = UILimbsManager.Instance(true);
    }

    public override IEnumerable<IEnumerable<Action>> DoAction()
    {
        yield break;
    }

    public override void OnSelect(bool manual)
    {
        Selected = true;
        Sprite.color = SelectedColor;
        _itemName.Text.text = _currentInventory[_currentIndex].Name;
    }

    public override void OnDeselect()
    {
        Selected = false;
        Sprite.color = Color.white;
    }

    private void Update()
    {
        if (_verticalAction.WasPerformedThisFrame() && Selected)
        {
            _currentIndex += (_verticalAction.ReadValue<float>() > 0 ? -1 : 1);
            if (_currentIndex == -1) _currentIndex = _currentInventory.Length - 1;
            if (_currentIndex == _currentInventory.Length) _currentIndex = 0;

            Icon.sprite = _limbsPreview.UILimbCompendium.Dictionary[_currentInventory[_currentIndex].Limb].Icon;
            _itemName.Text.text = _currentInventory[_currentIndex].Name;
            _limbsPreview.ChangeLimb(_currentInventory[_currentIndex]);
        }
    }
      
    public override void OnInit()
    {
        if (Limb == CustomAnimationEventListener.AnimatingLimb.None) return;
        _currentInventory = _player.Inventory.Inventory.Where(item => item.LimbType == Limb).ToArray();

        _limbsPreview.UILimbCompendium.Refresh();

        for (var index = 0; index < _currentInventory.Length; index++)
        {
            var item = _currentInventory[index];
            if (!item.Equipped) continue;
            _currentIndex = index;
            Icon.sprite = _limbsPreview.UILimbCompendium.Dictionary[item.Limb].Icon;
            _limbsPreview.ChangeLimb(_currentInventory[_currentIndex]);
            break;
        }
    }


    public bool HasMoreEquipment()
    {
        return _player.Inventory.Inventory.Any(item => !item.Equipped && item.LimbType == Limb);
    }
}
