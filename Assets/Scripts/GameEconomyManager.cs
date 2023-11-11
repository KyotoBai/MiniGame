using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEconomyManager : MonoBehaviour
{
    [SerializeField] private int initialCoinsCount = 100;

    // Variable to store coins count
    private int coinsCount;

    // Singleton instance for global access
    public static GameEconomyManager Instance { get; private set; }

    void Awake()
    {
        // If there is an instance, and it's not me, destroy myself.
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Keeps the object alive when loading new scenes
        }
        coinsCount = initialCoinsCount;
    }

    // Method to add coins and update the total count
    public void AddCoins(int amount)
    {
        if (amount < 0)
        {
            Debug.LogError("Attempt to add a negative amount of coins. Amount: " + amount);
            return;
        }

        coinsCount += amount;
        // Debug.Log("Coins added. New total: " + coinsCount);

        // Update UI and other game elements as necessary
    }

    // Method to deduct coins and update the total count
    public bool SpendCoins(int amount)
    {
        if (amount > coinsCount)
        {
            Debug.LogError("Attempt to spend more coins than available. Requested: " + amount + ", Available: " + coinsCount);
            return false;
        }

        coinsCount -= amount;
        // Debug.Log("Spent coin: " + amount + ", Remaining: " + coinsCount);

        // Update UI and other game elements as necessary
        return true;
    }

    // Getter to retrieve the current coin count
    public int GetCoinsCount()
    {
        return coinsCount;
    }

}
