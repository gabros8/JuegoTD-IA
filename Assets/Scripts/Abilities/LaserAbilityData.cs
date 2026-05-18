using UnityEngine;

[CreateAssetMenu(fileName = "LaserAbility", menuName = "Abilities/Laser")]
public class LaserAbilityData : AbilityData
{
    public float damage = 50f;

    public override void Execute(Vector3 position, int laneIndex = -1)
    {
        if (laneIndex == -1) return;

        Lane lane = LaneManager.Instance.GetLane(laneIndex);
        if (lane != null)
        {
            var units = new System.Collections.Generic.List<UnitController>(lane.GetUnits());
            foreach (var unit in units)
            {
                if (unit.Faction == UnitFaction.Enemy)
                {
                    IDamageable damageable = unit.GetComponent<IDamageable>();
                    damageable?.TakeDamage(damage);
                }
            }
            Debug.Log("Laser fired in lane " + laneIndex);
        }
    }
}
