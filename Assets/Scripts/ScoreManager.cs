using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
public class ScoreManager : MonoBehaviour
{
    private int score = 0;

    public Text score_text;

    public void PlayerScore()
    {
        score++;
        score_text.text = score.ToString();
    }
}
