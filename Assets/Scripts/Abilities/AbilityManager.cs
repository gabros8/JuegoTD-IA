using System.Collections.Generic;
using UnityEngine;

public class AbilityManager : MonoBehaviour
{
    public static AbilityManager Instance { get; private set; }

    private Dictionary<AbilityData, float> cooldowns = new Dictionary<AbilityData, float>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Update()
    {
        List<AbilityData> keys = new List<AbilityData>(cooldowns.Keys);
        foreach (var key in keys)
        {
            if (cooldowns[key] > 0)
            {
                cooldowns[key] -= Time.deltaTime;
            }
        }
    }

    public bool CanUse(AbilityData ability)
    {
        if (cooldowns.ContainsKey(ability) && cooldowns[ability] > 0) return false;
        return EnergyManager.Instance.CanAfford(ability.energyCost);
    }

    public void UseAbility(AbilityData ability, Vector3 position, int laneIndex = -1)
    {
        if (CanUse(ability))
        {
            if (EnergyManager.Instance.SpendEnergy(ability.energyCost))
            {
                ability.Execute(position, laneIndex);
                if (cooldowns.ContainsKey(ability)) cooldowns[ability] = ability.cooldown;
                else cooldowns.Add(ability, ability.cooldown);
            }
        }
    }

    public float GetCooldownNormalized(AbilityData ability)
    {
        if (cooldowns.ContainsKey(ability) && ability.cooldown > 0)
            return Mathf.Clamp01(cooldowns[ability] / ability.cooldown);
        return 0;
    }
}
