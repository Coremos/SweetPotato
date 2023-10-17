using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class GroundForceSettings : MonoBehaviour
{
    public float DetailDistance;

    private Terrain terrain;

    private void Start()
    {
        terrain = GetComponent<Terrain>();

    }

    private void Update()
    {
        if (terrain != null)
        {
            terrain.detailObjectDistance = DetailDistance;
        }
    }
}
