using System.Collections.Generic;
using System.Linq;

namespace UnitSystem.PickupSystem
{
    public class PickupableManager
    {
        private static List<Pickupable> units = new List<Pickupable>();

        public static void RegistUnit(Pickupable unit)
        {
            units.Add(unit);
        }

        public static void UnregistUnit(Pickupable unit)
        {
            units.Remove(unit);
        }

        public static Pickupable GetPickupable(Picker picker)
        {
            return units
                .Where(unit => unit.gameObject.activeInHierarchy)
                .FirstOrDefault(unit => IsInPickableDistance(picker, unit));
        }

        public static List<Pickupable> GetPickupables(Picker picker)
        {
            return units
                .Where(unit => IsInPickableDistance(picker, unit))
                .ToList();
        }

        public static void ClearUnit()
        {
            units.Clear();
        }

        public static bool IsInPickableDistance(Picker picker, Pickupable unit)
        {
            float distance = picker.transform.position.GetSquaredDistance(unit.transform.position);
            return distance < picker.Distance * picker.Distance + unit.Distance * unit.Distance;
        }
    }
}