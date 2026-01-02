using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private int currentScore;

    public int CurrentScore => currentScore;

    public void AddScore(int score)
    {
        currentScore += score;
    }

    public void ClearScore()
    {
        currentScore = 0;
    }

    public void CalculateGold()
    {

    }
}
