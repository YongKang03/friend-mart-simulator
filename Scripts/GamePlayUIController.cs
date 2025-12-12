using TMPro;
using UnityEngine;

public class GameplayUIController : MonoBehaviour
{
    public TextMeshProUGUI scoreText;

    public TextMeshProUGUI timerText;
    public float startHour = 10f;
    public float endHour = 22f;
    public float currentTime = 10f;

    private void Start()
    {
        scoreText.text = "Score : 0";
    }

    void Update()
    {
        if (currentTime < 22f)
        {
            currentTime += Time.deltaTime / 30f;
            currentTime = Mathf.Min(currentTime, 22f);
        }

        currentTime = Mathf.Min(currentTime, endHour);

        int hour = Mathf.FloorToInt(currentTime);
        int minutes = Mathf.FloorToInt((currentTime - hour) * 60);
        timerText.text = $"Time: {hour:D2}:{minutes:D2}";
    }

    public void UpdateScoreText(int score)
    {
        scoreText.text = $"Score : {score}";
    }

    public void ResetScoreText()
    {
        scoreText.text = "Score : 0";
    }
}
