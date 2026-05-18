using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DeploymentSlot : MonoBehaviour
{
    [Header("Data")]
    public UnitData unitData;

    [Header("UI Elements")]
    public Image unitIcon;
    public Image cooldownOverlay;
    public TextMeshProUGUI costText;
    public Button deployButton;

    private float currentCooldown;
    private bool isReady = true;

    private void Start()
    {
        if (unitData != null)
        {
            unitIcon.sprite = unitData.icon;
            costText.text = unitData.energyCost.ToString();
            deployButton.onClick.AddListener(OnDeployClicked);
        }
    }

    private void Update()
    {
        UpdateCooldown();
        UpdateAvailability();
    }

    private void UpdateCooldown()
    {
        if (currentCooldown > 0)
        {
            currentCooldown -= Time.deltaTime;
            cooldownOverlay.fillAmount = currentCooldown / unitData.deploymentCooldown;
        }
        else
        {
            isReady = true;
            cooldownOverlay.fillAmount = 0;
        }
    }

    private void UpdateAvailability()
    {
        bool canAfford = EnergyManager.Instance.CanAfford(unitData.energyCost);
        deployButton.interactable = isReady && canAfford;
    }

    public void OnDeployClicked()
    {
        Debug.Log($"DeploymentSlot: Clicked on {unitData.unitName}. Ready: {isReady}. Energy: {EnergyManager.Instance.CurrentEnergy}");
        if (isReady && EnergyManager.Instance.SpendEnergy(unitData.energyCost))
        {
            // For now, spawn in a random lane. In a full game, this would be a drag-and-drop.
            int laneIdx = Random.Range(0, LaneManager.Instance.LaneCount);
            Debug.Log($"DeploymentSlot: Deploying {unitData.unitName} to lane {laneIdx}");
            SpawnUnit(laneIdx);
            
            currentCooldown = unitData.deploymentCooldown;
            isReady = false;
        }
        else if (!isReady)
        {
            Debug.LogWarning($"DeploymentSlot: {unitData.unitName} is on cooldown.");
        }
        else
        {
            Debug.LogWarning($"DeploymentSlot: Not enough energy for {unitData.unitName}. Cost: {unitData.energyCost}");
        }
    }

    private void SpawnUnit(int laneIndex)
    {
        Lane lane = LaneManager.Instance.GetLane(laneIndex);
        if (lane != null)
        {
            Vector3 spawnPos = lane.playerSpawnPoint.position;
            spawnPos.z = -1f; // Move slightly towards camera
            GameObject unitObj = PoolManager.Instance.Get(unitData.prefab, spawnPos, Quaternion.identity);
            UnitController controller = unitObj.GetComponent<UnitController>();
            controller.Initialize(unitData, lane);
        }
    }
}
