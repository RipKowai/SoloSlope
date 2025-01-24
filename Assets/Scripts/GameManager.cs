using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour 
{
    public PlayerProps player_p;
    public Text player_Score_t;
    private int playerScore;


    public void PlayerScores()
    {
        playerScore++;
        this.player_Score_t.text = playerScore.ToString();

        Debug.Log(playerScore);
        this.player_p.ResetPosition();
        this.player_p.AddStartingForce();

    }
}
