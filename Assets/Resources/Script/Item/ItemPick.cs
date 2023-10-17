using UnitSystem.PickupSystem;

public class ItemPick : Pickupable
{
    private Item item;

    protected override void Awake()
    {
        base.Awake();
        item = GetComponent<Item>();
    }
    public override void Pickup()
    {
        item.GiveToMe();
        Unregist();
    }
}