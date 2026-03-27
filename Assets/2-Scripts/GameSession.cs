using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSession : MonoBehaviour
{
    [SerializeField] public int lives = 3;
    [SerializeField] int score = 0;
    int pointsForCoinPickup = 1;
    
    [SerializeField] TextMeshProUGUI livesText;
    [SerializeField] TextMeshProUGUI scoreText;
    void Awake()
    {
        // create our Singleton
        int numberGameSessions = FindObjectsByType<GameSession>(FindObjectsSortMode.None).Length;
        if (numberGameSessions > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }
    void Start()
    {
        livesText.text = "Lives: " + lives;  
        scoreText.text = "Score: " + score;
    }

    public void IncreaseScore()
    {
        score += pointsForCoinPickup;
        scoreText.text = "Score: " + score;
    }

    public void ProcessPlayerDeath()
    {
        if (lives > 1)
        {
            TakeLife();
        }
        else
        {
            ResetGameSession();
        }
    }
    private void TakeLife()
    {
        lives--;
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
        livesText.text = "Lives: " + lives;  
    }
    public void ResetGameSession()
    {
        lives = 3;
        score = 0;
        
        FindFirstObjectByType<ScenePersist>().ResetScenePersist();
        SceneManager.LoadScene(0);
        Destroy(gameObject);
    }
}
