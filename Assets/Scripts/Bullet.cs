using System.Collections.Generic;
using UnityEngine;

public class Bullet : MovingEntity
{
    private ExplosionManager explosionManager;
    private GameObject explosionPrefab;

    private List<GameObject> enemies;
    private List<GameObject> bullets;
    private SpawnManager spawnManager;

    private int score;

    private void Start()
    {
        explosionManager = FindAnyObjectByType<ExplosionManager>();
        explosionPrefab = FindAnyObjectByType<PlayerController>().ExplosionPrefab;

        enemies = FindAnyObjectByType<SpawnManager>().Enemies;
        bullets = FindAnyObjectByType<WeaponSystem>().Bullets;
        spawnManager = FindAnyObjectByType<SpawnManager>();

        score = FindAnyObjectByType<GameManager>().Score;
    }

    public void HandleBulletEnemyCollision(GameObject bullet, GameObject enemy)
    {
        // Explosion avec effet de fragmentation
        if (explosionManager != null)
        {
            explosionManager.ExplodeObject(enemy);
        }
        else
        {
            // Fallback vers l'explosion originale
            Instantiate(explosionPrefab, enemy.transform.position, Quaternion.identity);
        }

        // Destruction de l'ennemi
        Destroy(enemy, 0.1f); // Court d�lai pour permettre � l'explosion de commencer
        enemies.Remove(enemy);

        // Destruction de la balle
        Destroy(bullet);
        bullets.Remove(bullet);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // Balle touche ennemi
            HandleBulletEnemyCollision(gameObject, collision.gameObject);
            score += 100;

            // Chance de générer un power-up
            if (Random.value < 0.5f)
            {
                spawnManager.SpawnPowerUp(collision.transform.position);
            }
        }
        else if (collision.gameObject.CompareTag("Asteroid"))
        {
            // Balle touche astéroïde
            HandleBulletEnemyCollision(gameObject, collision.gameObject);
            score += 50;
        }
    }

    public override void Move()
    {
        // Ajouter des forces au Rigidbody au lieu de d?placer la Transform
        if (rb != null)
        {
            // R?initialiser la v?locit? et appliquer une nouvelle force
            rb.linearVelocity = Vector3.forward * bulletSpeed;
        }
        else
        {
            // Fallback au mouvement par transform si pas de Rigidbody
            transform.position += Vector3.forward * bulletSpeed * Time.deltaTime;
        }

        // Suppression des balles qui sortent de l'?cran
        if (transform.position.z > 9) // Chang? de y ? z
        {
            Destroy(this);
            //bullets.RemoveAt(i);
        }
    }
}
