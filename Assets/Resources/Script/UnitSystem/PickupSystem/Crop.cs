using UnitSystem.PickupSystem;
using UnityEngine;

[RequireComponent(typeof(Ally))]
public class Crop : Pickupable
{
    private Ally ally;

    protected override void Awake()
    {
        base.Awake();
        ally = GetComponent<Ally>();
    }

    public override void Pickup()
    {
        if (ally.SetActivate())
        {
            Unregist();
        }
    }
}