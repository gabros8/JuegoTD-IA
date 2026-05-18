using UnityEngine;

public abstract class AbilityData : ScriptableObject
{
    public string abilityName;
    public string description;
    public float cooldown;
    public int energyCost;
    public Sprite icon;

    public abstract void Execute(Vector3 position, int laneIndex = -1);
}
