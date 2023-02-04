using Assets.Scripts.Character;

public class LimbDrop : Collectable
{
    public LimbInventory.LimbItem Item;
    public Float FloatEffect;
    public FlashColorRandomizeEffect ColorEffect;

    private LimbCompendium _limbCompendium;
    private LimbInventory _limbInventory;

    protected override void OnAwake()
    {
        base.OnAwake();
        _limbCompendium = LimbCompendium.Instance();
        _limbInventory = LimbInventory.Instance(true);
    }

    protected override void OnStartEffect()
    {
        CollectableSprite.sprite = _limbCompendium.Dictionary[Item.Limb].Icon;
        FloatEffect.enabled = false;
        ColorEffect.enabled = false;
        base.OnStartEffect();
    }

    protected override void OnDropEffect()
    {
        base.OnDropEffect();
        FloatEffect.enabled = true;
        ColorEffect.enabled = true;
    }

    protected override void OnPickup()
    {
        base.OnPickup();
        _limbInventory.Inventory.Add(Item);
    }
}
