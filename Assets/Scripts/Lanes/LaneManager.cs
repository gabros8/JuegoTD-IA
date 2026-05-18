using System.Collections.Generic;
using UnityEngine;

public class LaneManager : MonoBehaviour
{
    public static LaneManager Instance { get; private set; }

    [SerializeField] private List<Lane> lanes = new List<Lane>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public int LaneCount => lanes.Count;

    public Lane GetLane(int index)
    {
        if (index >= 0 && index < lanes.Count)
            return lanes[index];
        return null;
    }

    public List<Lane> GetAllLanes() => lanes;

    public void AddLane(Lane lane)
    {
        if (!lanes.Contains(lane))
            lanes.Add(lane);
    }
}
