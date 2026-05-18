using System.Collections.Generic;
using UnityEngine;

public class ReserveManager : MonoBehaviour
{
    public static ReserveManager Instance { get; private set; }

    private Dictionary<UnitData, int> reserveUnits = new Dictionary<UnitData, int>();

    [Header("Events")]
    public GameEvent onReserveChanged;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void AddUnit(UnitData data)
    {
        if (reserveUnits.ContainsKey(data))
        {
            reserveUnits[data]++;
        }
        else
        {
            reserveUnits.Add(data, 1);
        }
        onReserveChanged?.Raise();
    }

    public int GetCount(UnitData data)
    {
        if (reserveUnits.ContainsKey(data))
            return reserveUnits[data];
        return 0;
    }

    public bool HasUnit(UnitData data)
    {
        return reserveUnits.ContainsKey(data) && reserveUnits[data] > 0;
    }

    public void UseUnit(UnitData data)
    {
        if (HasUnit(data))
        {
            reserveUnits[data]--;
            onReserveChanged?.Raise();
        }
    }

    public Dictionary<UnitData, int> GetAllReserves() => reserveUnits;
}
