using GGJ;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterSystem : MonoBehaviour
{
    public int TotalLogs;
    int _currentLogCount;

    float targetPosition;
    float t = 0;
    float lastDamPosition;

    public void Start()
    {
        DamCollector collector = FindObjectOfType<DamCollector>();
        collector.OnLogPlaced += Collector_OnLogPlaced;

        targetPosition = -1;
        DamLevel = -1;
    }

    public void OnDestroy()
    {
        DamCollector collector = FindObjectOfType<DamCollector>();
        if(collector != null)
        {
            collector.OnLogPlaced -= Collector_OnLogPlaced;
        }
    }

    private void Collector_OnLogPlaced(Log obj)
    {
       _currentLogCount++;

        Mathf.Clamp(_currentLogCount, 0, TotalLogs);

        lastDamPosition = DamLevel;
        t = 0;
    }

    public void Update()
    {
        float damLevel = _currentLogCount / (float)TotalLogs;

        targetPosition = -1 + damLevel;
  
        DamLevel = Mathf.Lerp(lastDamPosition, targetPosition, t/2f);

        t = Mathf.Clamp(t + Time.deltaTime, 0, 2);
        transform.position = new Vector3(transform.position.x, DamLevel, transform.position.z);
    }

    public static float DamLevel;
}
