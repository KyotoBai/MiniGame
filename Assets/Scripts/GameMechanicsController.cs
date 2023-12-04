using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMechanicsController : MonoBehaviour
{
    public delegate void WaveBeginHandler();
    public delegate void WaveEndHandler();
    public delegate void GameOverHandler();
    public event WaveBeginHandler OnWaveBegin;
    public event WaveEndHandler OnWaveEnd;
    public event GameOverHandler OnGameOver;

    [Header("Runtime Data")]
    public float gameBeginTime;
    public bool isGameOver = false;
    public int wave = 0;
    public float waveStartTime = 0f;
    public float waveEndTime = 0f;
    public float waveCountdown;
    public bool waveStarted = false;
    public int numEnemyEliminated = 0;

    [Header("Settings")]
    [SerializeField] private List<float> waves = new List<float>();
    [SerializeField] private float timeBeforeEverything;
    [SerializeField] private float timeBeforeWaves;
    [SerializeField] private int coinsPerEnemyEliminated = 25;

    [Header("Components")]
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject playerBase;
    [SerializeField] private GameEconomyManager gameEconomyManager;
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private GameObject enemyParent;

    private Health playerHealth;
    private Health playerBaseHealth;
    private bool gameOverBroadcasted = false;

    // Start is called before the first frame update
    void Start()
    {
        if (gameOverUI != null)
        {
            gameOverUI.SetActive(false);
        }
        if (player != null)
        {
            playerHealth = player.GetComponent<Health>();
        }
        if (playerBase != null)
        {
            playerBaseHealth = playerBase.GetComponent<Health>();
        }
        if (playerHealth != null)
        {
            playerHealth.onHealthDepletedEvent += GameOver;
        }
        if (playerBaseHealth != null)
        {
            playerBaseHealth.onHealthDepletedEvent += GameOver;
        }
        gameBeginTime = Time.time;
        waveEndTime = Time.time + timeBeforeEverything;
        waveCountdown = timeBeforeWaves;
        waveStarted = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isGameOver)
        {
            Time.timeScale = 0;
        }
        if (wave >= waves.Count)
        {
            GameOver();
        }

        if (waveStarted)
        {
            float waveElapsed = Time.time - waveStartTime;
            float waveTime = waves[wave];
            if (waveElapsed > waveTime)
            {
                // wave end
                waveStarted = false;
                wave += 1;
                waveEndTime = Time.time;
                OnWaveEnd?.Invoke();
            }
            waveCountdown = waveTime - waveElapsed;
        } else
        {
            // wave not started
            waveCountdown = timeBeforeWaves - (Time.time - waveEndTime);
            if (waveCountdown <= 0f)
            {
                waveStarted = true;
                waveStartTime = Time.time;
                OnWaveBegin?.Invoke();
            }
        }
        
        // for the last wave, if all enemies are eliminated, game over
        // check children of enemyParent
        if (wave == waves.Count - 1 && waveStarted && Time.time - waveStartTime > 1f)
        {
            if (enemyParent.transform.childCount == 0)
            {
                GameOver();
            }
        }
    }

    public void GameOver()
    {
        isGameOver = true;
        gameOverUI?.SetActive(true);
        if (!gameOverBroadcasted)
        {
            OnGameOver?.Invoke();
            gameOverBroadcasted = true;
        }
    }

    public bool StartNextWave()
    {
        if (!isGameOver && !waveStarted)
        {
            waveStarted = true;
            waveStartTime = Time.time;
            OnWaveBegin?.Invoke();
            return true;
        }
        return false;
    }

   public void EnemyEliminated()
    {
        numEnemyEliminated += 1;
        gameEconomyManager.AddCoins(coinsPerEnemyEliminated);
    }
}
