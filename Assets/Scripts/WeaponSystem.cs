using System.Collections.Generic;
using UnityEngine;

public class WeaponSystem : MonoBehaviour
{
    private GameObject playerShip;

    [SerializeField] private GameObject bulletPrefab;
    public GameObject BulletPrefab
    {
        get { return bulletPrefab; }
        set { bulletPrefab = value; }
    }

    [SerializeField] private GameObject bulletPrefab1;
    public GameObject BulletPrefab1
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

    private Enemy[] enemy;
    private Asteroid[] asteroid;
    private List<GameObject> enemies;
    private List<GameObject> asteroids;

    [SerializeField] private float fireRate = 3f;
    public float FireRate
    {
        get { return fireRate; }
        set { fireRate = value; }
    }
    private float time = 0;

    private void Start()
    {
        bulletCount = 1;
        playerShip = FindAnyObjectByType<PlayerController>().PlayerShip;

        collisionSetup = FindAnyObjectByType<CollisionSetup>();

        enemies = FindAnyObjectByType<SpawnManager>().Enemies;
        asteroids = FindAnyObjectByType<SpawnManager>().Asteroids;
    }

    private void Update()
    {
        time += Time.deltaTime;

        if (time >= fireRate)
        {
            TurretFire();
            time = 0f;
        }
    }

    public void FireBullet(Vector3 direction)
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

    private void TurretFire()
    {
        GameObject target = GetClosestTarget();
        if (target == null) return;

        Vector3 shooterPos = playerShip.transform.position;
        Vector3 targetPos = target.transform.position;

        Rigidbody targetRb = target.GetComponent<Rigidbody>();

        Vector3 targetVelocity = Vector3.zero;
        if (targetRb != null)
        {
            targetVelocity = targetRb.linearVelocity;
        }

        float distance = Vector3.Distance(shooterPos, targetPos);
        float bulletSpeed = bulletPrefab1.GetComponent<TurretBullet>().BulletSpeed;
        float timeToHit = distance / bulletSpeed;

        Vector3 futurePosition = targetPos + targetVelocity * timeToHit;
        Vector3 direction = (futurePosition - shooterPos).normalized;
        Vector3 spawnPosition = shooterPos + new Vector3(0, 0, 0.5f);

        GameObject bullet = Instantiate(bulletPrefab1, spawnPosition, Quaternion.identity);

        collisionSetup.SetupCollisionComponents(bullet, true, false, "Bullet");

        TurretBullet bulletScript = bullet.GetComponent<TurretBullet>();
        bulletScript.SetDirection(direction);

        bullets.Add(bullet);
    }

    private GameObject GetClosestTarget()
    {
        GameObject closest = null;
        float minDistance = Mathf.Infinity;
        Vector3 currentPosition = playerShip.transform.position;

        foreach (GameObject enemy in enemies)
        {
            if (enemy == null) continue;

            float distance = Vector3.Distance(currentPosition, enemy.transform.position);

            if (distance < minDistance)
            {
                minDistance = distance;
                closest = enemy;
            }
        }

        foreach (GameObject asteroid in asteroids)
        {
            if (asteroid == null) continue;

            float distance = Vector3.Distance(currentPosition, asteroid.transform.position);

            if (distance < minDistance)
            {
                minDistance = distance;
                closest = asteroid;
            }
        }

        return closest;
    }
}
