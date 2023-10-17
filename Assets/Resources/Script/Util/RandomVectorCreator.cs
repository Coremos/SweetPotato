using UnityEngine;

public class RandomVectorCreator
{
    private const float RADIUS = 200.0f;
    private const float HIGHEST_HEIGHT = 500.0f;

    public static Vector3 GetStandWorldPosition(Vector3 offset = new Vector3())
    {
        var vector = InnerCircle(RADIUS) + offset;
        vector.y = GetPositionHeight(vector);
        return vector;
    }

    private static Vector3 InnerCircle(float radius)
    {
        float value = Random.Range(0.0f, 1.0f);
        float angle = Random.Range(0, 360);

        var vector = new Vector3();
        vector.x = Mathf.Cos(angle * Mathf.Deg2Rad) * radius * value;
        vector.y = 0.0f;
        vector.z = Mathf.Sin(angle * Mathf.Deg2Rad) * radius * value;

        return vector;
    }

    public static float GetPositionHeight(Vector3 position)
    {
        position.y = HIGHEST_HEIGHT;
        var ray = new Ray(position, Vector3.down);
        Physics.Raycast(ray, out var hitInfo, Mathf.Infinity, LayerMask.NameToLayer("Terrain"));

        return hitInfo.point.y;
    }
}