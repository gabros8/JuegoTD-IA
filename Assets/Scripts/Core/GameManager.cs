using UnityEngine;

public enum GameState
{
    Playing,
    Paused,
    Victory,
    Defeat
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Settings")]
    public int maxLives = 3;

    [Header("State")]
    [SerializeField] private int currentLives;
    [SerializeField] private GameState currentState;

    [Header("Events")]
    public GameEvent onLivesChanged;
    public GameEvent onGameOver;
    public GameEvent onVictory;

    public int CurrentLives => currentLives;
    public GameState CurrentState => currentState;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
        
        currentLives = maxLives;
        currentState = GameState.Playing;
    }

    public void LoseLife()
    {
        if (currentState != GameState.Playing) return;

        currentLives--;
        onLivesChanged?.Raise();

        if (currentLives <= 0)
        {
            GameOver();
        }
    }

    private void GameOver()
    {
        currentState = GameState.Defeat;
        onGameOver?.Raise();
        Time.timeScale = 0;
    }

    public void WinGame()
    {
        currentState = GameState.Victory;
        onVictory?.Raise();
        Time.timeScale = 0;
    }
}
