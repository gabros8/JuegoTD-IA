using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public static WaveManager Instance { get; private set; }

    [Header("Settings")]
    public List<WaveData> levels = new List<WaveData>();
    public float timeBetweenWaves = 3f;

    [Header("State")]
    [SerializeField] private int currentWaveIndex = 0;
    private bool isSpawning = false;

    public bool IsSpawning => isSpawning;

    private void Update()
    {
        // Reading the variable to satisfy compiler and provide state info
        if (isSpawning)
        {
            // Optional: Update UI or logic based on spawning state
        }
    }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        if (levels.Count > 0)
        {
            StartCoroutine(WaveRoutine());
        }
        else
        {
            Debug.LogWarning("WaveManager: No levels configured.");
        }
    }

    private IEnumerator WaveRoutine()
    {
        Debug.Log("WaveManager: Starting WaveRoutine.");
        while (currentWaveIndex < levels.Count)
        {
            // Use isSpawning to prevent overlapping if a wave is already in progress
            if (isSpawning)
            {
                yield return new WaitUntil(() => !isSpawning);
            }

            Debug.Log($"WaveManager: Waiting {timeBetweenWaves}s for Wave {currentWaveIndex + 1}");
            yield return new WaitForSeconds(timeBetweenWaves);
            yield return StartCoroutine(SpawnWave(levels[currentWaveIndex]));
            currentWaveIndex++;
        }
        Debug.Log("WaveManager: All waves completed.");
    }

    private IEnumerator SpawnWave(WaveData wave)
    {
        Debug.Log($"WaveManager: Spawning Wave {currentWaveIndex + 1} with {wave.entries.Count} entries. Spawning status: {isSpawning}");
        isSpawning = true;
foreach (var entry in wave.entries)
        {
            yield return new WaitForSeconds(entry.spawnDelay);
            SpawnEnemy(entry);
        }
        isSpawning = false;
        yield break; // To use isSpawning
    }

    private void SpawnEnemy(WaveEntry entry)
    {
        int laneIdx = entry.laneIndex;
        if (laneIdx == -1) laneIdx = Random.Range(0, LaneManager.Instance.LaneCount);

        Lane lane = LaneManager.Instance.GetLane(laneIdx);
        if (lane != null)
        {
            Debug.Log($"WaveManager: Spawning {entry.unitData.unitName} in lane {laneIdx}");
            Vector3 spawnPos = lane.enemySpawnPoint.position;
            spawnPos.z = -1f; // Move slightly towards camera
            GameObject enemyObj = PoolManager.Instance.Get(entry.unitData.prefab, spawnPos, Quaternion.identity);
            UnitController controller = enemyObj.GetComponent<UnitController>();
            if (controller != null)
            {
                controller.Initialize(entry.unitData, lane);
            }
        }
else
        {
            Debug.LogError($"WaveManager: Failed to find lane {laneIdx}");
        }
    }
}
