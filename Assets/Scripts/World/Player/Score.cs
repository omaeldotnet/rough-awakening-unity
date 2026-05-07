using TMPro;
using UnityEngine;

public class Score : MonoBehaviour
{
    public static Score instance;

    public TextMeshProUGUI scoreText;
    public float score;
    public float dyingPenalty = 500;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        score = PlayerPrefs.GetFloat("Score", 0);
        if (scoreText == null)
        {
            scoreText = GetComponent<TextMeshProUGUI>();
        }
 
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T)) // Press 'T' to test
        {
            score += 100;
            
        }

        if (Input.GetKeyDown(KeyCode.Y)) // Press 'T' to test
        {
            ResetScore();
        }

        scoreText.text = "SCORE: " + score.ToString();

    }


    public void AddScore()
    {
        score += 100;
        PlayerPrefs.SetFloat("Score", score);
        
    }

    public void MinusScore()
    {
        score = Mathf.Max(0, score - dyingPenalty); // Simpler penalty logic
        PlayerPrefs.SetFloat("Score", score);
       
    }

    public void ResetScore()
    {
        score = Mathf.Max(0, score - dyingPenalty); // Simpler penalty logic
        PlayerPrefs.SetFloat("Score", score);
       
    }
}