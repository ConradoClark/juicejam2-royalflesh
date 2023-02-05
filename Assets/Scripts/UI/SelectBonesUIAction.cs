using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Character;
using Licht.Impl.Orchestration;
using Licht.Unity.UI;
using UnityEngine;
using UnityEngine.InputSystem;

public class SelectBonesUIAction : UIAction
{
    public Sprite EmptySprite;
    public CustomAnimationEventListener.AnimatingLimb Limb;
    public Color SelectedColor;
    public SpriteRenderer Sprite;
    public SpriteRenderer Icon;
    private PlayerIdentifier _player;
    private LimbInventory.LimbItem[] _currentInventory;
    private int _currentIndex;
    private InputAction _verticalAction;
    private LimbItemName _itemName;
    public LimbItemName CustomItemName;
    private UILimbsManager _limbsPreview;
    private PayUp _payUp;
    private bool _paying;
    private LimbCompendium _limbCompendium;

    public LimbInventory.LimbItem SelectedItem => _currentInventory.Length >= _currentIndex &&
                                                  _currentInventory.Length>0 ? _currentInventory[_currentIndex] : null;

    protected override void OnAwake()
    {
        base.OnAwake();
        _player = PlayerIdentifier.Instance(true);
        _verticalAction = MenuContext.PlayerInput.actions[MenuContext.UIVertical.ActionName];
        _itemName = CustomItemName ?? LimbItemName.Instance(true);
        _limbsPreview = UILimbsManager.Instance(true);
        _payUp = PayUp.Instance(true);
        _limbCompendium = LimbCompendiumRef.Instance(true).LimbCompendium;
    }

    public override IEnumerable<IEnumerable<Action>> DoAction()
    {
        if (!_payUp.isActiveAndEnabled || _paying) yield break;

        _paying = true;
        if (_currentInventory.Length == 0)
        {
            yield return _payUp.Die().AsCoroutine();
            yield break;
        }

        var currentItem = _currentInventory[_currentIndex];
        if (currentItem.Equipped)
        {
            currentItem.Equipped = false;
            _limbCompendium.ChangeLimbs(_player.Inventory.Inventory);
        }

        _player.Inventory.Inventory.Remove(currentItem);

        _currentInventory = _player.Inventory.Inventory.Where(item => item.LimbType == Limb
                                                                      || Limb == CustomAnimationEventListener.AnimatingLimb.None).ToArray();

        _currentIndex--;
        if (_currentIndex == -1) _currentIndex = _currentInventory.Length - 1;
        if (_currentIndex == -1)
        {
            Icon.sprite = EmptySprite;
        }

        Icon.sprite = null;
        _itemName.Text.text = "PAYING...";
        _itemName.Value.text = "";
        yield return _payUp.Pay(currentItem.Value).AsCoroutine();

        if (_currentIndex >= 0 && _currentIndex <= _currentInventory.Length)
        {
            Icon.sprite = _limbsPreview.UILimbCompendium.Dictionary[_currentInventory[_currentIndex].Limb].Icon;
        }
            
        UpdateTexts();
        _paying = false;
    }

    public override void OnSelect(bool manual)
    {
        Selected = true;
        Sprite.color = SelectedColor;
        if (manual) UpdateTexts();
    }

    public override void OnDeselect()
    {
        Selected = false;
        Sprite.color = Color.white;
    }

    private void UpdateTexts()
    {
        if (_currentInventory.Length == 0)
        {
            _itemName.Text.text = "NO LIMBS LEFT!";
            if (_itemName.Value != null) _itemName.Value.text = "";
            return;
        }

        _itemName.Text.text = _currentInventory[_currentIndex].Name;

        if (_itemName.Value != null)
        {
            _itemName.Value.text = $"{_currentInventory[_currentIndex].Value} Coins";
        }
    }

    private void Update()
    {
        if (_verticalAction.WasPerformedThisFrame() && Selected && _currentInventory.Length > 0)
        {
            _currentIndex += (_verticalAction.ReadValue<float>() > 0 ? -1 : 1);
            if (_currentIndex == -1) _currentIndex = _currentInventory.Length - 1;
            if (_currentIndex == _currentInventory.Length) _currentIndex = 0;

            Icon.sprite = _limbsPreview.UILimbCompendium.Dictionary[_currentInventory[_currentIndex].Limb].Icon;
            UpdateTexts();
            if (_limbsPreview.UILimbCompendium.enabled) _limbsPreview.ChangeLimb(_currentInventory[_currentIndex]);
        }
    }

    public override void OnInit()
    {
        _currentInventory = _player.Inventory.Inventory.Where(item => item.LimbType == Limb
                                                                      || Limb == CustomAnimationEventListener.AnimatingLimb.None).ToArray();

        if (_limbsPreview.UILimbCompendium.enabled) _limbsPreview.UILimbCompendium.Refresh();

        for (var index = 0; index < _currentInventory.Length; index++)
        {
            var item = _currentInventory[index];
            if (!item.Equipped) continue;
            _currentIndex = index;
            Icon.sprite = _limbsPreview.UILimbCompendium.Dictionary[item.Limb].Icon;
            if (_limbsPreview.UILimbCompendium.enabled) _limbsPreview.ChangeLimb(_currentInventory[_currentIndex]);
            UpdateTexts();
            break;
        }

        if (_currentInventory.All(item=>!item.Equipped))
        {
            Icon.sprite = null;
            _itemName.Text.text = "Nothing";
            if (_itemName.Value!=null) _itemName.Value.text = "";
        }
    }


    public bool HasMoreEquipment()
    {
        return _player.Inventory.Inventory.Any(item => !item.Equipped && item.LimbType == Limb);
    }
}
