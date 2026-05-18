using UnityEngine;

public enum UnitFaction
{
    Allied,
    Enemy
}

[CreateAssetMenu(fileName = "NewUnitData", menuName = "Data/Unit Data")]
public class UnitData : ScriptableObject
{
    [Header("Identity")]
    public string unitName;
    public UnitFaction faction;
    public GameObject prefab;
    public Sprite icon;

    [Header("Stats")]
    public float maxHealth = 100f;
    public float attackDamage = 10f;
    public float attackRange = 1f;
    public float movementSpeed = 2f;
    public float attackSpeed = 1f; // Attacks per second
    public float attackCooldown = 1f; // Alternatively, use this instead of attackSpeed
    
    [Header("Economy")]
    public int energyCost = 10;
    public float deploymentCooldown = 5f;

    [Header("Visuals")]
    public RuntimeAnimatorController animatorController;
}
