using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    private GameObject powerUpPrefab;
    private GameObject enemyPrefab;
    private GameObject asteroidPrefab;

    public float initialSpawnRate = 2.0f; // Taux de spawn initial

    private float nextSpawnTime;
    public float NextSpawnTime
    {
        get { return nextSpawnTime; }
        set { nextSpawnTime = value; }
    }
    private float spawnRate = 2.0f;
    public float SpawnRate
    {
        get { return spawnRate; }
        set { spawnRate = value; }
    }

    private List<GameObject> enemies = new List<GameObject>();
    public List<GameObject> Enemies
    {
        get { return enemies; }
        set { enemies = value; }
    }
    private List<GameObject> asteroids = new List<GameObject>();
    public List<GameObject> Asteroids
    {
        get { return asteroids; }
        set { asteroids = value; }
    }
    private List<GameObject> powerUps = new List<GameObject>();
    public List<GameObject> PowerUps
    {
        get { return powerUps; }
        set { powerUps = value; }
    }

    private CollisionSetup collisionSetup;

    private void Start()
    {
        spawnRate = initialSpawnRate;
        nextSpawnTime = Time.time + spawnRate;

        collisionSetup = FindAnyObjectByType<CollisionSetup>();
    }

    void SpawnEnemiesAndAsteroids()
    {
        if (Time.time > nextSpawnTime)
        {
            if (Random.value < 0.3f)
            {
                // Spawn d'un ennemi
                float randomX = Random.Range(-8f, 8f);
                // Position de spawn sur l'axe Z au lieu de Y
                Vector3 spawnPosition = new Vector3(randomX, 0, 9);
                GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);

                // Configuration des composants de collision pour l'ennemi
                collisionSetup.SetupCollisionComponents(enemy, true, false, "Enemy");

                // Ajouter le script de gestion de collision � l'ennemi
                enemy.AddComponent<EnemyCollider>();

                enemies.Add(enemy);
            }
            else
            {
                // Spawn d'un ast�ro�de
                float randomX = Random.Range(-8f, 8f);
                // Position de spawn sur l'axe Z au lieu de Y
                Vector3 spawnPosition = new Vector3(randomX, 0, 9);
                GameObject asteroid = Instantiate(asteroidPrefab, spawnPosition, Quaternion.identity);

                // Configuration des composants de collision pour l'ast�ro�de
                collisionSetup.SetupCollisionComponents(asteroid, true, false, "Asteroid");

                // Ajouter le script de gestion de collision � l'ast�ro�de
                asteroid.AddComponent<AsteroidCollider>();

                asteroids.Add(asteroid);
            }

            nextSpawnTime = Time.time + spawnRate;
        }
    }

    public void SpawnPowerUp(Vector3 position)
    {
        GameObject powerUp = Instantiate(powerUpPrefab, position, Quaternion.identity);

        // Configuration des composants de collision pour le power-up
        collisionSetup.SetupCollisionComponents(powerUp, true, false, "PowerUp");

        // Ajouter le script de gestion de collision au power-up
        powerUp.AddComponent<PowerUpCollider>();

        powerUps.Add(powerUp);
    }
}
