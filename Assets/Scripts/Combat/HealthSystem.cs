using System;
using UnityEngine;

public class HealthSystem : MonoBehaviour, IDamageable
{
    [SerializeField] private float maxHealth;
    [SerializeField] private float currentHealth;
    [SerializeField] private UnitFaction faction;

    public event Action OnDeath;
    public event Action<float> OnHealthChanged;

    public float CurrentHealth => currentHealth;
    public float MaxHealth => maxHealth;
    public UnitFaction Faction => faction;
    public Transform Transform => transform;

    public void Initialize(float maxHp, UnitFaction unitFaction)
    {
        maxHealth = maxHp;
        currentHealth = maxHp;
        faction = unitFaction;
        OnHealthChanged?.Invoke(currentHealth);
    }

    public void TakeDamage(float amount)
    {
        if (currentHealth <= 0) return;

        currentHealth -= amount;
        OnHealthChanged?.Invoke(currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        OnDeath?.Invoke();
    }
}
