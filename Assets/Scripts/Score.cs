using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    [SerializeField] TMP_Text scoreText;
    int score = 0;
    float timeWhenInstantiated;

    private void Start()
    {
        timeWhenInstantiated = Time.time; 
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            float timeElapsed = Time.time - timeWhenInstantiated; 
            int pointsToAdd = CalculatePointsToAdd(timeElapsed); 
            score += pointsToAdd; 
            print("Score: " + score); 
            UpdateScoreText(); 
        }
    }

    void UpdateScoreText()
    {
        scoreText.text = "Score: " + score.ToString();
    }

    int CalculatePointsToAdd(float timeElapsed)
    {

        float maxTime = 10f;
        float normalizedTime = Mathf.Clamp01(timeElapsed / maxTime);
        int maxPoints = 31;
        int points = Mathf.RoundToInt(maxPoints * (1f - normalizedTime));

        return points;
    }
}
