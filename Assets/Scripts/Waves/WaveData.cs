using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class WaveEntry
{
    public UnitData unitData;
    public int laneIndex = -1; // -1 for random
    public float spawnDelay = 1f;
}

[CreateAssetMenu(fileName = "NewWaveData", menuName = "Data/Wave Data")]
public class WaveData : ScriptableObject
{
    public List<WaveEntry> entries = new List<WaveEntry>();
}
