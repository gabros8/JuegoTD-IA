using UnityEngine;

public class CombatHandler : MonoBehaviour
{
    private UnitController controller;

    private void Awake()
    {
        controller = GetComponent<UnitController>();
    }

    public IDamageable ScanForTarget(Lane lane, float range, UnitFaction faction)
    {
        // Use OverlapBox for precise lane detection if needed, 
        // but for now, we use the lane's registered units.
        var units = lane.GetUnits();
        IDamageable closestTarget = null;
        float minDistance = float.MaxValue;

        foreach (var unit in units)
        {
            if (unit.Faction != faction && unit.CurrentState != UnitState.Dead)
            {
                IDamageable damageable = unit.GetComponent<IDamageable>();
                if (damageable != null && damageable.CurrentHealth > 0)
                {
                    float distance = Vector3.Distance(transform.position, unit.transform.position);
                    if (distance <= range && distance < minDistance)
                    {
                        minDistance = distance;
                        closestTarget = damageable;
                    }
                }
            }
        }

        return closestTarget;
    }

    public void PerformAttack(IDamageable target, float damage)
    {
        if (target != null)
        {
            target.TakeDamage(damage);
        }
    }
}
