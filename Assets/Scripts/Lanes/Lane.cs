using System.Collections.Generic;
using UnityEngine;

public class Lane : MonoBehaviour
{
    [Header("Spawn Points")]
    public Transform playerSpawnPoint;
    public Transform enemySpawnPoint;

    [Header("Targets")]
    public Transform playerTarget; // Where enemies go
    public Transform enemyTarget; // Where allies go

    [Header("Bounds")]
    public float laneWidth = 10f;
    public float laneHeight = 2f;

    private readonly List<UnitController> unitsInLane = new List<UnitController>();

    public void RegisterUnit(UnitController unit)
    {
        if (!unitsInLane.Contains(unit))
            unitsInLane.Add(unit);
    }

    public void UnregisterUnit(UnitController unit)
    {
        if (unitsInLane.Contains(unit))
            unitsInLane.Remove(unit);
    }

    public List<UnitController> GetUnits() => unitsInLane;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Vector3 center = transform.position;
        Gizmos.DrawWireCube(center, new Vector3(laneWidth, laneHeight, 0.1f));
        
        if (playerSpawnPoint) { Gizmos.color = Color.green; Gizmos.DrawSphere(playerSpawnPoint.position, 0.2f); }
        if (enemySpawnPoint) { Gizmos.color = Color.red; Gizmos.DrawSphere(enemySpawnPoint.position, 0.2f); }
    }
}
