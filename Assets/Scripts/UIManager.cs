using System.Collections;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private int lives;
    private int score;
    private float gameTime;

    [SerializeField] private TMPro.TMP_Text scoreText;
    public TMPro.TMP_Text ScoreText
    {
        get { return scoreText; }
        set { scoreText = value; }
    }
    [SerializeField] private TMPro.TMP_Text livesText;
    public TMPro.TMP_Text LivesText
    {
        get { return livesText; }
        set { livesText = value; }
    }
    [SerializeField] private TMPro.TMP_Text powerupMessageText; // Pour afficher les messages de powerup
    public TMPro.TMP_Text PowerupMessageText
    {
        get { return powerupMessageText; }
        set { powerupMessageText = value; }
    }
    [SerializeField] private TMPro.TMP_Text timeText; // Pour afficher le temps �coul�
    public TMPro.TMP_Text TimeText
    {
        get { return scoreText; }
        set { scoreText = value; }
    }
    [SerializeField] private GameObject playerDamageEffect; // Effet visuel quand un ennemi traverse
    public GameObject PlayerDamageEffect
    {
        get { return playerDamageEffect; }
        set { playerDamageEffect = value; }
    }
    [SerializeField] private GameObject gameOverPanel;
    public GameObject GameOverPanel
    {
        get { return gameOverPanel; }
        set { gameOverPanel = value; }
    }

    private void Start()
    {
        lives = FindAnyObjectByType<GameManager>().Lives;
        score = FindAnyObjectByType<GameManager>().Score;
        UpdateUI();
        if (gameOverPanel) gameOverPanel.SetActive(false);
        if (powerupMessageText) powerupMessageText.gameObject.SetActive(false);
    }

    private void Update()
    {
        // Affichage du temps de jeu (optionnel)
        gameTime += Time.deltaTime;

        if (timeText != null)
        {
            int minutes = Mathf.FloorToInt(gameTime / 60);
            int seconds = Mathf.FloorToInt(gameTime % 60);
            timeText.text = string.Format("Time: {0:00}:{1:00}", minutes, seconds);
        }
    }

    public IEnumerator ShowPowerupMessage(string message)
    {
        if (powerupMessageText != null)
        {
            powerupMessageText.text = message;
            powerupMessageText.gameObject.SetActive(true);
            yield return new WaitForSeconds(2f);
            powerupMessageText.gameObject.SetActive(false);
        }
        yield return null;
    }

    public void UpdateUI()
    {
        score = FindAnyObjectByType<GameManager>().Score;
        // Mise � jour des textes de score et de vies
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score;
        }
        lives = FindAnyObjectByType<GameManager>().Lives;
        if (livesText != null)
        {
            livesText.text = "Lives: " + lives;
        }
    }
}
