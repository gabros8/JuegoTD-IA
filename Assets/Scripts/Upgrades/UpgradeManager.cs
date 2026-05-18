using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager Instance { get; private set; }

    [Header("Pool")]
    public List<UpgradeData> allUpgrades = new List<UpgradeData>();

    [Header("Events")]
    public GameEvent onUpgradeAvailable;

    private List<UpgradeData> pendingUpgrades = new List<UpgradeData>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void OnUnitReachedEnd()
    {
        // For now, just trigger an event. In a real game, this would show a UI with 3 choices.
        onUpgradeAvailable?.Raise();
    }

    public List<UpgradeData> GetRandomUpgrades(int count)
    {
        List<UpgradeData> result = new List<UpgradeData>();
        List<UpgradeData> pool = new List<UpgradeData>(allUpgrades);
        
        for (int i = 0; i < count && pool.Count > 0; i++)
        {
            int idx = Random.Range(0, pool.Count);
            result.Add(pool[idx]);
            pool.RemoveAt(idx);
        }
        return result;
    }

    public void ApplyUpgrade(UpgradeData upgrade)
    {
        switch (upgrade.type)
        {
            case UpgradeType.EnergyGen:
                EnergyManager.Instance.energyRegenPerSecond += upgrade.value;
                break;
            case UpgradeType.MaxEnergy:
                EnergyManager.Instance.maxEnergy += upgrade.value;
                break;
            // Unit specific upgrades would need a way to modify all instances or the template
            // For this prototype, we'll focus on global stats or simple modifiers
        }
    }
}
