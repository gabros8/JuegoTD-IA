using UnityEngine;

public interface IDamageable
{
    float CurrentHealth { get; }
    float MaxHealth { get; }
    void TakeDamage(float amount);
    void Die();
    UnitFaction Faction { get; }
    Transform Transform { get; }
}
