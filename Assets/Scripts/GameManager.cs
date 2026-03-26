using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private int score;
    public int Score
    {
        get { return score; }
        set { score = value; }
    }
    private int lives;
    public int Lives
    {
        get { return lives; }
        set { lives = value; }
    }

    private CollisionSetup collisionSetup;
    private GameObject playerShip;
    private GameObject gameOverPanel;
    private int initialSpawnRate;
    private int bulletCount;
    private float nextSpawnTime;
    private float spawnRate;

    private SpawnManager spawnManager;
    private PlayerController playerController;
    private UIManager uiManager;
    private Bullet bullet;
    private Enemy enemy;
    private Asteroid asteroid;

    private TMPro.TMP_Text timeText;
    [SerializeField] private TMPro.TMP_Text countdownText;

    private List<GameObject> enemies;
    private List<GameObject> powerUps;
    private List<GameObject> asteroids;
    private List<GameObject> bullets;


    [Header("Difficulty Settings")]
    public float minSpawnRate = 0.5f; // Taux de spawn minimal (plus difficile)
    public float spawnRateDifficulty = 0.1f; // R�duction du taux de spawn par minute
    private float gameTime = 0f; // Temps de jeu �coul�

    private bool isGameOver = false;
    private float restartCountdown = 3.0f;

    void Start()
    {
        // Initialisation
        score = 0;
        lives = 3;

        gameTime = 0f;

        playerShip = FindAnyObjectByType<PlayerController>().PlayerShip;
        timeText = FindAnyObjectByType<UIManager>().TimeText;
        gameOverPanel = FindAnyObjectByType<UIManager>().GameOverPanel;

        spawnRate = FindAnyObjectByType<SpawnManager>().SpawnRate;
        nextSpawnTime = FindAnyObjectByType<SpawnManager>().NextSpawnTime;

        bullets = FindAnyObjectByType<WeaponSystem>().Bullets;
        powerUps = FindAnyObjectByType<SpawnManager>().PowerUps;
        enemies = FindAnyObjectByType<SpawnManager>().Enemies;
        asteroids = FindAnyObjectByType<SpawnManager>().Asteroids;

        collisionSetup = FindAnyObjectByType<CollisionSetup>();

        spawnManager = FindAnyObjectByType<SpawnManager>();
        playerController = FindAnyObjectByType<PlayerController>();
        uiManager = FindAnyObjectByType<UIManager>();

        // Ajouter le script de gestion de collision au joueur
        if (playerShip.GetComponent<PlayerController>() == null)
        {
            playerShip.AddComponent<PlayerController>();
        }

        // S'assurer que le joueur a les composants n�cessaires pour les collisions
        collisionSetup.SetupCollisionComponents(playerShip, true, false, "Player");
    }

    void Update()
    {
        if (!isGameOver)
        {
            // Augmentation du temps de jeu
            gameTime += Time.deltaTime;

            // Calcul du nouveau taux de spawn en fonction du temps �coul� (en minutes)
            float minutesPlayed = gameTime / 2f;
            spawnRate = Mathf.Max(minSpawnRate, initialSpawnRate - (spawnRateDifficulty * minutesPlayed));

            // Affichage du temps de jeu (optionnel)
            if (timeText != null)
            {
                int minutes = Mathf.FloorToInt(gameTime / 60);
                int seconds = Mathf.FloorToInt(gameTime % 60);
                timeText.text = string.Format("Time: {0:00}:{1:00}", minutes, seconds);
            }

            spawnManager.SpawnEnemiesAndAsteroids();

            // Gestion des entr�es du joueur
            playerController.Move();

            bullet = FindAnyObjectByType<Bullet>(); // a rework
            if (bullet != null)
            {
                bullet.Move();
            }

            asteroid = FindAnyObjectByType<Asteroid>(); // a rework
            if (asteroids != null)
            {
                asteroid.Move();
            }

            enemy = FindFirstObjectByType<Enemy>(); // a rework
            if (enemy != null)
            {
                enemy.Move();
            }

            // Mise � jour de l'UI
            uiManager.UpdateUI();
        }

        // Gestion du d�compte de red�marrage
        if (isGameOver)
        {
            restartCountdown -= Time.deltaTime;

            // Mise � jour du texte avec la valeur arrondie � l'entier sup�rieur
            if (countdownText != null)
            {
                countdownText.text = "Red�marrage dans: " + Mathf.Ceil(restartCountdown).ToString();
            }

            // Lorsque le d�compte atteint z�ro
            if (restartCountdown <= 0)
            {
                RestartGame();
            }
        }
    }

    public void GameOver()
    {
        // Affichage du panel de game over
        gameOverPanel.SetActive(true);

        // Initialisation du compte � rebours
        isGameOver = true;
        restartCountdown = 3.0f;

        // Mise � jour initiale du texte de d�compte
        if (countdownText != null)
        {
            countdownText.text = "Red�marrage dans: " + Mathf.Ceil(restartCountdown).ToString();
            countdownText.gameObject.SetActive(true);
        }

        // Note: ne pas arr�ter le temps ici puisque nous voulons que le d�compte fonctionne
        // Time.timeScale = 0; -- retirez cette ligne s'il elle est pr�sente
    }

    public void RestartGame()
    {
        // R�initialisation du statut de game over
        isGameOver = false;

        // Masquage du texte de d�compte
        if (countdownText != null)
        {
            countdownText.gameObject.SetActive(false);
        }

        // Remise � z�ro du jeu
        Time.timeScale = 1;

        // Destruction de tous les objets
        foreach (GameObject enemy in enemies)
        {
            Destroy(enemy);
        }
        enemies.Clear();

        foreach (GameObject asteroid in asteroids)
        {
            Destroy(asteroid);
        }
        asteroids.Clear();

        foreach (GameObject bullet in bullets)
        {
            Destroy(bullet);
        }
        bullets.Clear();

        foreach (GameObject powerUp in powerUps)
        {
            Destroy(powerUp);
        }
        powerUps.Clear();

        // R�initialisation des variables
        score = 0;
        lives = 3;
        bulletCount = 1;
        gameTime = 0f;
        spawnRate = initialSpawnRate;
        nextSpawnTime = Time.time + spawnRate;

        // Masquage du panel de game over
        gameOverPanel.SetActive(false);

        // Replacement du joueur
        playerShip.transform.position = new Vector3(0, 0, -7);
        playerShip.transform.rotation = Quaternion.identity;
    }
}