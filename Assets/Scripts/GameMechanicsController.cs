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
    public float gameStartTime;
    public bool isGameOver = false;
    public int wave = 0;
    public float waveStartTime = 0f;
    public float waveCountdown;
    public bool waveStarted = false;

    [Header("Settings")]
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject playerBase;
    [SerializeField] private GameEconomyManager gameEconomyManager;
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
        gameStartTime = Time.time;

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GameOver()
    {

    }
}
