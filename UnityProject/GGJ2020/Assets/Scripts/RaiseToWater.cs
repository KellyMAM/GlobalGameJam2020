using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaiseToWater : MonoBehaviour
{
    float startOffset;
    public bool useOffset = true;

    void Start()
    {
        if (useOffset) startOffset = transform.position.y;
        else startOffset = 0.1f;
    }

    void Update()
    {
        transform.position = new Vector3(transform.position.x, WaterSystem.DamLevel + startOffset, transform.position.z);
    }
}
