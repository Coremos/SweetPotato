using UnityEngine;

namespace UnitSystem.PickupSystem
{
    public abstract class Pickupable : MonoBehaviour
    {
        public float Distance;
        public abstract void Pickup();

        protected virtual void Awake()
        {
            PickupableManager.RegistUnit(this);
        }

        protected virtual void Unregist()
        {
            PickupableManager.UnregistUnit(this);
        }
    }
}