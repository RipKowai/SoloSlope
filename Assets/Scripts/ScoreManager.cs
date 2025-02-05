using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
public class ScoreManager : MonoBehaviour
{
    private int score = 0;

    public TextMeshProUGUI score_text;

    public void PlayerScore()
    {
        score++;
        score_text.text = score.ToString();
    }
}
