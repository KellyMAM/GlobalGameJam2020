using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaiseToWater : MonoBehaviour
{
    float startOffset;

    void Start()
    {
        startOffset = transform.position.y;
    }

    void Update()
    {
        transform.position = new Vector3(transform.position.x, WaterSystem.DamLevel + startOffset, transform.position.z);
    }
}
