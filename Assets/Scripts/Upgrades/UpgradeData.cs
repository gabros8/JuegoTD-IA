using UnityEngine;

public enum UpgradeType
{
    Health,
    Damage,
    Range,
    Speed,
    AttackSpeed,
    EnergyGen,
    MaxEnergy,
    CostReduction
}

[CreateAssetMenu(fileName = "NewUpgradeData", menuName = "Data/Upgrade Data")]
public class UpgradeData : ScriptableObject
{
    public string upgradeName;
    public string description;
    public UpgradeType type;
    public float value;
}
