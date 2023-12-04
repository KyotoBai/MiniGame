using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class DisplayTimeCountDown : MonoBehaviour
{
    public TMP_Text msg;
    public GameMechanicsController gameMechanicsController;
    void Start()
    {
        if (gameMechanicsController != null)
        {
            gameMechanicsController.OnWaveBegin += OnWaveBegin;
            gameMechanicsController.OnWaveEnd += OnWaveEnd;
        }
    }

    void Update()
    {
        if (gameMechanicsController.waveStarted == false)
        {
            msg.text = "Next Wave in " + (int)gameMechanicsController.waveCountdown + " seconds!";
        }
    }

    // Update is called once per frame
    private void OnWaveBegin()
    {

        msg.text = "The " + GetStringFromIndex(gameMechanicsController.wave);
    }

    private void OnWaveEnd()
    {
        
    }

    private string GetStringFromIndex(int waveNum)
    {
        switch (waveNum)
        {
            case 0:
                return "FIRST Wave";
            case 1:
                return "SECOND Wave";
            case 2:
                return "THIRD Wave";
            case 3:
                return "FOURTH Wave";
            case 4:
                return "FIFTH Wave";
            default:
                return "Error";
        }
    }
}
