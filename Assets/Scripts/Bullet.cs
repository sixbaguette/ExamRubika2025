using System.Collections.Generic;
using UnityEngine;

public class Bullet : MovingEntity
{
    private ExplosionManager explosionManager;
    private PlayerController explosionPrefab;

    private List<GameObject> enemies;
    private List<GameObject> bullets;

    private void Start()
    {

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
            gameManager.HandleBulletEnemyCollision(gameObject, collision.gameObject);
            gameManager.score += 100;

            // Chance de générer un power-up
            if (Random.value < 0.5f)
            {
                gameManager.SpawnPowerUp(collision.transform.position);
            }
        }
        else if (collision.gameObject.CompareTag("Asteroid"))
        {
            // Balle touche astéroïde
            gameManager.HandleBulletEnemyCollision(gameObject, collision.gameObject);
            gameManager.score += 50;
        }
    }
}
