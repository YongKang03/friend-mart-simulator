using UnityEngine;

public class ScoreSystem : MonoBehaviour
{
    public int totalScore = 0;

    int customerBaseScore = 50;
    int customerWaitingScore = 150;
    float customerPerItemWaitingTime = 5.0f;
    float customerBaseWaitingTime = 60.0f;
    int customerItemScore = 10;
    int customerPenaltyScore = -100;

    float givenCustomerWaitingTime = 0.0f;
    int obtainedCustomerTimeScore = 0;
    int obtainedCustomerItemScore = 0;
    int customerFinalScore = 0;

    public GameplayUIController gameplayUIController;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddCustomerServingScore(float waitingTime, int noOfItem)
    {
        // Add extra time on base watiting time based on items bought by customer
        givenCustomerWaitingTime = customerBaseWaitingTime + (customerPerItemWaitingTime * (float)noOfItem);

        // Calculate time score based on the customer waiting time and given customer waiting time
        obtainedCustomerTimeScore = (int)(customerWaitingScore * (1 - (waitingTime / givenCustomerWaitingTime)));

        // Calculate item score based on the items bought by customer
        obtainedCustomerItemScore = customerItemScore * noOfItem;

        // Add base, time and item score
        customerFinalScore = customerBaseScore + obtainedCustomerTimeScore + obtainedCustomerItemScore;

        Debug.Log($"Obtained customer time score: {obtainedCustomerTimeScore}");
        Debug.Log($"Obtained customer item score: {obtainedCustomerItemScore}");
        Debug.Log($"Final score: {customerFinalScore}");

        // Modify the score
        ModifyScore(customerFinalScore);
    }

    public void MinusCustomerServingScore()
    {
        ModifyScore(customerPenaltyScore);
    }

    void ModifyScore(int score)
    {
        totalScore += score;
        gameplayUIController.UpdateScoreText(totalScore);
        Debug.Log($"Obtained score: {score}. Total score: {totalScore}");
    }

    public int GetTotalScore()
    {
        return totalScore;
    }

}
