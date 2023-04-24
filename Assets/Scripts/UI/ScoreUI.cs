using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreUI : MonoBehaviour
{
    public static ScoreUI instance;

    [SerializeField] private TextMeshProUGUI displayText;
    public int score;
    private void Start()
    {
        instance = this;
    }
    public void IncrementScore(int amount)
    {
        Debug.Log("ScoreUI.Incrementscore by amount: " + amount);
        score += amount;
        displayText.text = "Score: " + score;
    }
}
