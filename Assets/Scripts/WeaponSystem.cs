using System.Collections.Generic;
using UnityEngine;

public class WeaponSystem : MonoBehaviour
{
    private GameObject playerShip;

    private GameObject bulletPrefab;
    public GameObject BulletPrefab
    {
        get { return bulletPrefab; }
        set { bulletPrefab = value; }
    }

    private int bulletCount = 1; // Nombre de projectiles tir�s simultan�ment
    public int BulletCount
    {
        get { return bulletCount; }
        set { bulletCount = value; }
    }
    private float bulletSpacing = 0.5f; // Espacement horizontal entre les projectiles
    public float BulletSpacing
    {
        get { return bulletSpacing; }
    }
    private int maxBulletCount = 5; // Limite maximale de projectiles simultan�s
    public int MaxBulletCount
    {
        get { return maxBulletCount; }
    }

    private List<GameObject> bullets = new List<GameObject>();
    public List<GameObject> Bullets
    {
        get { return bullets; }
        set { bullets = value; }
    }

    private CollisionSetup collisionSetup;

    private void Start()
    {
        bulletCount = 1;
        playerShip = FindAnyObjectByType<PlayerController>().PlayerShip;

        collisionSetup = FindAnyObjectByType<CollisionSetup>();
    }

    public void FireBullet()
    {
        // Calcul de la position de d�part pour centrer les projectiles
        float startX = -((bulletCount - 1) * bulletSpacing) / 2;

        // Cr�ation de plusieurs balles c�te � c�te
        for (int i = 0; i < bulletCount; i++)
        {
            // Calcule la position avec l'offset horizontal
            Vector3 bulletOffset = new Vector3(startX + (i * bulletSpacing), -0.5f, 0.5f);
            Vector3 spawnPosition = playerShip.transform.position + bulletOffset;

            // Instanciation du projectile
            GameObject bullet = Instantiate(bulletPrefab, spawnPosition, Quaternion.identity);

            // Configuration des composants de collision pour la balle
            // Les projectiles doivent avoir un Rigidbody pour les collisions
            collisionSetup.SetupCollisionComponents(bullet, true, false, "Bullet");

            // Ajouter le script de gestion de collision � la balle
            bullet.AddComponent<BulletCollider>();

            bullets.Add(bullet);
        }

        // Son de tir
        AudioSource audioSource = GetComponent<AudioSource>();
        if (audioSource != null)
        {
            audioSource.Play();
        }
    }
}
