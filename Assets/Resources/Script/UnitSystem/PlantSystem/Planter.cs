using UnitSystem;
using UnityEngine;

namespace PlantSystem
{
    [RequireComponent(typeof(Unit))]
    public class Planter : MonoBehaviour
    {
        public int SeedAmount;
        private Unit unit;

        private void Awake()
        {
            TryGetComponent(out unit);
        }

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.Q) && SeedAmount > 0)
            {
                var isPlanted = PlantManager.Plant(unit);
                if (isPlanted) SeedAmount--;
            }
        }
    }
}