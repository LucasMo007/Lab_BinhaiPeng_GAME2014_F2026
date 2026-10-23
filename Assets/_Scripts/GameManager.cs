using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Lives")]
    [SerializeField] private int maxLives = 3;
    [SerializeField] private float invincibleTime = 1f;

    [Header("UI")]
    [SerializeField] private TMP_Text livesText;
    [SerializeField] private TMP_Text scoreText;

    [Header("Audio")]
    [SerializeField] private AudioClip hitSound;
    [SerializeField] private AudioClip loseSound;

    private int lives;
    private int score;
    private float invincibleUntil = 0f;

    public int Lives => lives;
    public int Score => score;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        lives = maxLives;
        score = 0;
        UpdateUI();
    }

    // Called when the player collides with an enemy/obstacle
    public void LoseLife()
    {
        // Short invincibility so one hit doesn't take several lives
        if (Time.time < invincibleUntil) return;
        invincibleUntil = Time.time + invincibleTime;

        lives--;
        Debug.Log("Player lost a life! Lives left: " + lives);
        UpdateUI();

        if (lives <= 0)
        {
            GameOver();
        }
        else if (hitSound != null)
        {
            AudioSource.PlayClipAtPoint(hitSound, Camera.main.transform.position, 0.6f);
        }
    }

    // Called when a bullet destroys an enemy/obstacle
    public void AddScore(int points)
    {
        score += points;
        UpdateUI();
    }

    private void GameOver()
    {
        Debug.Log("Game Over! Final score: " + score);
        PlayerPrefs.SetInt("FinalScore", score);

        if (loseSound != null)
        {
            AudioSource.PlayClipAtPoint(loseSound, Camera.main.transform.position, 0.8f);
        }
        SceneManager.LoadScene("End");
    }

    private void UpdateUI()
    {
        if (livesText != null) livesText.text = "Lives: " + lives;
        if (scoreText != null) scoreText.text = "Score: " + score;
    }
}
