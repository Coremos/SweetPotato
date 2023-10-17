using System.Collections.Generic;
using System.Linq;
using UnitSystem;
using UnityEngine;

namespace PlantSystem
{
    public class PlantManager
    {
        #region Properties
        private const float PLANT_RADIUS = 10.0f;
        private const float PLANT_RADIUS_SQUARED = PLANT_RADIUS * PLANT_RADIUS;
        private static List<Vector2> plantsPosition = new List<Vector2>();
        #endregion

        #region Methods
        public static bool Plant(Unit planter)
        {
            var position = GetUnitPosition(planter);
            if (IsPlantablePosition(position))
            {
                AddPlant(position);
                return true;
            }
            return false;
        }

        private static bool IsPlantablePosition(Vector2 position)
        {
            return !plantsPosition.Any(plantPosition => IsIntersection(plantPosition, position));
        }

        private static bool IsIntersection(Vector2 vector1, Vector2 vector2)
        {
            return GetDistance(vector1, vector2) < PLANT_RADIUS_SQUARED;
        }

        private static Vector2 GetUnitPosition(Unit unit)
        {
            var vector = new Vector2();
            vector.x = unit.transform.position.x;
            vector.y = unit.transform.position.z;
            return vector;
        }

        private static float GetDistance(Vector2 source, Vector2 destination)
        {
            float xDifference = source.x - destination.x;
            float yDiffernece = source.y - destination.y;
            return xDifference * xDifference + yDiffernece * yDiffernece;
        }

        private static void AddPlant(Vector2 position)
        {
            plantsPosition.Add(position);
            // Instantiate plant object and need subscribe OnHarvestPlant
        }

        private void OnHarvestPlant(Unit plant)
        {
            // Need implement
        }
        #endregion
    }
}