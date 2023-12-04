using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DisplayScore : MonoBehaviour
{
    public TMP_Text score;
    public GameMechanicsController gameMechanicsController;

    // Update is called once per frame
    void Update()
    {
        score.text = "Your Score is : " + gameMechanicsController.numEnemyEliminated * 10 + "!!!";
    }
}
