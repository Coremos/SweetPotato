using UnityEngine;

public static class UnityExtension
{
    public static void ResetTransform(this GameObject go)
    {
        if (null == go) return;

        go.transform.localRotation = Quaternion.identity;
        go.transform.localPosition = Vector3.zero;
        go.transform.localScale = Vector3.one;
    }
    public static void ResetRectTransform(this GameObject go)
    {
        if (null == go) return;

        RectTransform rect = go.GetComponent<RectTransform>();
        if (rect == null) return;

        rect.localRotation = Quaternion.identity;
        rect.localPosition = Vector3.zero;
        rect.localScale = Vector3.one;
    }
    public static void AddPosition(this Transform tr, Vector3 vPos)
    {
        if (null == tr) return;

        Vector3 vPosition = tr.position;
        vPosition += vPos;
        tr.position = vPosition;
    }

    public static void AddPosition(this Transform tr, float vX = 0f, float vY = 0f, float vZ = 0f)
    {
        if (null == tr) return;

        Vector3 vPosition = tr.position;
        vPosition += new Vector3(vX, vY, vZ);
        tr.position = vPosition;
    }

    public static float GetSquaredDistance(this Vector3 position1, Vector3 position2)
    {
        float xDifference = position1.x - position2.x;
        float zDifference = position1.z - position2.z;

        return xDifference * xDifference + zDifference * zDifference;
    }

    public static void SetParent(this GameObject obj_, GameObject parent_, bool worldPositionStays_ = true)
    {
        obj_.transform.SetParent(parent_.transform, worldPositionStays_);
    }
}