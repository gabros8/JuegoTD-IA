using UnityEngine;

public class EnergyManager : MonoBehaviour
{
    public static EnergyManager Instance { get; private set; }

    [Header("Settings")]
    public float maxEnergy = 100f;
    public float startingEnergy = 50f;
    public float energyRegenPerSecond = 5f;

    [Header("State")]
    [SerializeField] private float currentEnergy;
    private bool forceDeployActive = false;

    [Header("Events")]
    public GameEvent onEnergyChanged;

    public float CurrentEnergy => currentEnergy;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
        
        currentEnergy = startingEnergy;
    }

    private void Update()
    {
        RegenerateEnergy();
    }

    private void RegenerateEnergy()
    {
        if (currentEnergy < maxEnergy)
        {
            currentEnergy += energyRegenPerSecond * Time.deltaTime;
            currentEnergy = Mathf.Min(currentEnergy, maxEnergy);
            onEnergyChanged?.Raise();

            if (forceDeployActive && currentEnergy >= 0)
            {
                forceDeployActive = false;
                // Notify UI that force deploy is no longer locking
            }
        }
    }

    public bool CanAfford(int cost)
    {
        if (forceDeployActive) return false;
        return currentEnergy >= cost;
    }

    public bool SpendEnergy(int cost)
    {
        if (currentEnergy >= cost)
        {
            currentEnergy -= cost;
            onEnergyChanged?.Raise();
            return true;
        }
        
        // Force deploy mechanic
        if (!forceDeployActive)
        {
            currentEnergy -= cost;
            forceDeployActive = true;
            onEnergyChanged?.Raise();
            return true;
        }

        return false;
    }

    public void AddEnergy(float amount)
    {
        currentEnergy = Mathf.Min(currentEnergy + amount, maxEnergy);
        onEnergyChanged?.Raise();
    }
}
