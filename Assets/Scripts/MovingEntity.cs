using UnityEngine;

public class MovingEntity : MonoBehaviour
{
    private GameManager gameManager;
    public float bulletSpeed = 10.0f;
    public float enemySpeed = 3.0f;
    public float asteroidSpeed = 2.0f;

    void Start()
    {
        gameManager = FindAnyObjectByType<GameManager>();
    }

    void MoveEnemies()
    {
        for (int i = enemies.Count - 1; i >= 0; i--)
        {
            if (enemies[i] != null)
            {
                // Utiliser le Rigidbody pour le mouvement
                Rigidbody rb = enemies[i].GetComponent<Rigidbody>();
                if (rb != null)
                {
                    // Appliquer directement une v?locit? au Rigidbody
                    rb.linearVelocity = Vector3.back * enemySpeed;
                }
                else
                {
                    // Fallback au mouvement par transform si pas de Rigidbody
                    enemies[i].transform.position += Vector3.back * enemySpeed * Time.deltaTime;
                }

                // Les ennemis ne disparaissent qu'? z=-12 et enl?vent une vie
                if (enemies[i].transform.position.z < -12)
                {
                    // Enlever un point de vie au joueur
                    lives--;

                    // Effet visuel pour montrer que l'ennemi a travers?
                    if (playerDamageEffect != null)
                    {
                        Instantiate(playerDamageEffect, enemies[i].transform.position, Quaternion.identity);
                    }

                    // Destruction de l'ennemi
                    Destroy(enemies[i]);
                    enemies.RemoveAt(i);

                    // V?rifier si le joueur n'a plus de vies
                    if (lives <= 0)
                    {
                        GameOver();
                    }
                }
            }
            else
            {
                enemies.RemoveAt(i);
            }
        }
    }

    void MoveAsteroids()
    {
        for (int i = asteroids.Count - 1; i >= 0; i--)
        {
            if (asteroids[i] != null)
            {
                // Direction al?atoire pour chaque ast?ro?de
                float randomX = Random.Range(-0.5f, 0.5f);

                // Utiliser le Rigidbody pour le mouvement
                Rigidbody rb = asteroids[i].GetComponent<Rigidbody>();
                if (rb != null)
                {
                    // Appliquer directement une v?locit? au Rigidbody
                    rb.linearVelocity = new Vector3(randomX, 0, -1) * asteroidSpeed;

                    // Appliquer une rotation
                    asteroids[i].transform.Rotate(0, 30 * Time.deltaTime, 0);
                }
                else
                {
                    // Fallback au mouvement par transform si pas de Rigidbody
                    Vector3 movement = new Vector3(randomX, 0, -1) * asteroidSpeed * Time.deltaTime;
                    asteroids[i].transform.position += movement;
                    asteroids[i].transform.Rotate(0, 30 * Time.deltaTime, 0);
                }

                // Les ast?ro?des ne disparaissent qu'? z=-12 et enl?vent une vie
                if (asteroids[i].transform.position.z < -12)
                {
                    // Enlever un point de vie au joueur
                    lives--;

                    // Effet visuel pour montrer que l'ast?ro?de a travers?
                    if (playerDamageEffect != null)
                    {
                        Instantiate(playerDamageEffect, asteroids[i].transform.position, Quaternion.identity);
                    }

                    // Destruction de l'ast?ro?de
                    Destroy(asteroids[i]);
                    asteroids.RemoveAt(i);

                    // V?rifier si le joueur n'a plus de vies
                    if (lives <= 0)
                    {
                        GameOver();
                    }
                }
            }
            else
            {
                asteroids.RemoveAt(i);
            }
        }
    }

    void MoveBullets()
    {
        for (int i = bullets.Count - 1; i >= 0; i--)
        {
            if (bullets[i] != null)
            {
                // Ajouter des forces au Rigidbody au lieu de d?placer la Transform
                Rigidbody rb = bullets[i].GetComponent<Rigidbody>();
                if (rb != null)
                {
                    // R?initialiser la v?locit? et appliquer une nouvelle force
                    rb.linearVelocity = Vector3.forward * bulletSpeed;
                }
                else
                {
                    // Fallback au mouvement par transform si pas de Rigidbody
                    bullets[i].transform.position += Vector3.forward * bulletSpeed * Time.deltaTime;
                }

                // Suppression des balles qui sortent de l'?cran
                if (bullets[i].transform.position.z > 9) // Chang? de y ? z
                {
                    Destroy(bullets[i]);
                    bullets.RemoveAt(i);
                }
            }
            else
            {
                bullets.RemoveAt(i);
            }
        }
    }
}
