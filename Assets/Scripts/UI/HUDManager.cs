using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUDManager : MonoBehaviour
{
    [Header("Energy UI")]
    public Slider energyBar;
    public TextMeshProUGUI energyText;

    [Header("Health UI")]
    public TextMeshProUGUI livesText;

    private void Start()
    {
        UpdateEnergyUI();
        UpdateLivesUI();
    }

    public void UpdateEnergyUI()
    {
        if (EnergyManager.Instance != null)
        {
            energyBar.maxValue = EnergyManager.Instance.maxEnergy;
            energyBar.value = EnergyManager.Instance.CurrentEnergy;
            energyText.text = $"Ink: {Mathf.FloorToInt(EnergyManager.Instance.CurrentEnergy)} / {EnergyManager.Instance.maxEnergy}";
        }
    }

    public void UpdateLivesUI()
    {
        if (GameManager.Instance != null)
        {
            livesText.text = $"Lives: {GameManager.Instance.CurrentLives}";
        }
    }
}
