using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMechanicsController : MonoBehaviour
{
    public delegate void WaveBeginHandler();
    public delegate void WaveEndHandler();
    public event WaveBeginHandler OnWaveBegin;
    public event WaveEndHandler OnWaveEnd;

    [Header("Runtime Data")]
    public float gameBeginTime;
    public bool isGameOver = false;
    public int wave = 0;
    public float waveStartTime = 0f;
    public float waveEndTime = 0f;
    public float waveCountdown;
    public bool waveStarted = false;

    [Header("Settings")]
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject playerBase;
    [SerializeField] private GameEconomyManager gameEconomyManager;
    [SerializeField] private List<float> waves = new List<float>();
    [SerializeField] private float timeBeforeWaves;

    private Health playerHealth;
    private Health playerBaseHealth;

    // Start is called before the first frame update
    void Start()
    {
        if (player != null)
        {
            playerHealth = player.GetComponent<Health>();
        }
        if (playerBase != null)
        {
            playerHealth = playerBase.GetComponent<Health>();
        }
        if (playerHealth != null)
        {
            playerHealth.onHealthDepletedEvent += GameOver;
        }
        if (playerBaseHealth != null)
        {
            playerBaseHealth.onHealthDepletedEvent += GameOver;
        }
        gameBeginTime = Time.unscaledTime;
        waveEndTime = Time.unscaledTime;
        waveCountdown = timeBeforeWaves;
        waveStarted = false;

    }

    // Update is called once per frame
    void Update()
    {
        if (wave >= waves.Count)
        {
            GameOver();
        }

        if (waveStarted)
        {
            float waveElapsed = Time.unscaledTime - waveStartTime;
            float waveTime = waves[wave];
            if (waveElapsed > waveTime)
            {
                // wave end
                waveStarted = false;
                wave += 1;
                waveEndTime = Time.unscaledTime;
                OnWaveEnd?.Invoke();
            }
            waveCountdown = waveTime - waveElapsed;
        } else
        {
            // wave not started
            waveCountdown = Time.unscaledTime - waveEndTime;
            if (waveCountdown > timeBeforeWaves)
            {
                waveStarted = true;
                waveStartTime = Time.unscaledTime;
                OnWaveBegin?.Invoke();
            }
        }
        
    }

    public void GameOver()
    {

    }
}
