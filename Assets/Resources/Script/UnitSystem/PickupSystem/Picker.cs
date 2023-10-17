using UnityEngine;

namespace UnitSystem.PickupSystem
{
    public class Picker : MonoBehaviour
    {
        public float Distance;

        private void FixedUpdate()
        {
            var pickupables = PickupableManager.GetPickupables(this);
            for (int index = 0; index < pickupables.Count; index++)
            {
                pickupables[index].Pickup();
            }
        }
    }
}