using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MovingEntity
{
    [SerializeField] private GameObject explosionPrefab;
    public GameObject ExplosionPrefab
    {
        get { return explosionPrefab; }
        set { explosionPrefab = value; }
    }

    [SerializeField] private GameObject playerShip;
    public GameObject PlayerShip
    {
        get { return playerShip; }
        set { playerShip = value; }
    }
    [SerializeField] private float playerSpeed = 5.0f;
    public float PlayerSpeed
    {
        get { return playerSpeed; }
        set { playerSpeed = value; }
    }

    private int lives;

    private WeaponSystem weaponSystem;
    private List<GameObject> powerUps;

    private List<GameObject> enemies;
    private List<GameObject> asteroids;

    private void Start()
    {
        gameManager = FindAnyObjectByType<GameManager>();
        weaponSystem = FindAnyObjectByType<WeaponSystem>();
        powerUps = FindAnyObjectByType<SpawnManager>().PowerUps;

        enemies = FindAnyObjectByType<SpawnManager>().Enemies;
        asteroids = FindAnyObjectByType<SpawnManager>().Asteroids;
    }

    public override void Move()
    {
        // D�placement du joueur
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // D�placement sur le plan XZ
        Vector3 movement = new Vector3(horizontalInput, 0, verticalInput) * playerSpeed * Time.deltaTime;
        playerShip.transform.position += movement;

        // Calcul des angles de rotation pour les deux axes
        float tiltAngleZ = -horizontalInput * 30f; // Inclinaison lat�rale (gauche/droite)
        float tiltAngleX = verticalInput * 15f;    // Inclinaison longitudinale (avant/arri�re)

        // Cr�ation d'une rotation qui combine les deux inclinaisons
        Quaternion targetRotation = Quaternion.Euler(tiltAngleX, 0, tiltAngleZ);

        // Application de la rotation avec un lissage pour un effet plus naturel
        playerShip.transform.rotation = Quaternion.Slerp(playerShip.transform.rotation, targetRotation, 5f * Time.deltaTime);

        // Si aucun input, retour progressif � la rotation neutre
        if (horizontalInput == 0 && verticalInput == 0)
        {
            playerShip.transform.rotation = Quaternion.Slerp(playerShip.transform.rotation, Quaternion.identity, 5f * Time.deltaTime);
        }

        // Limites de l'�cran pour le joueur
        Vector3 playerPos = playerShip.transform.position;
        playerPos.x = Mathf.Clamp(playerPos.x, -8.4f, 8.4f);
        playerPos.z = Mathf.Clamp(playerPos.z, -11, 0f);
        playerShip.transform.position = playerPos;

        // Tir
        if (Input.GetKeyDown(KeyCode.Space))
        {
            weaponSystem.FireBullet();
        }
    }
    public void HandlePlayerHit(GameObject hitObject)
    {
        // Destruction de l'objet qui a touch� le joueur
        Instantiate(explosionPrefab, hitObject.transform.position, Quaternion.identity);

        if (hitObject.CompareTag("Enemy"))
        {
            Destroy(hitObject);
            enemies.Remove(hitObject);
        }
        else if (hitObject.CompareTag("Asteroid"))
        {
            Destroy(hitObject);
            asteroids.Remove(hitObject);
        }

        // Perte d'une vie
        lives = FindAnyObjectByType<GameManager>().Lives--;
        //lives--;

        if (lives <= 0)
        {
            gameManager.GameOver();
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("PowerUp"))
        {
            // Le joueur a collecté un power-up
            WeaponPowerUp weaponPowerUp = collision.gameObject.GetComponent<WeaponPowerUp>();
            weaponPowerUp.ApplyPowerUp(); // a fix
            Destroy(collision.gameObject);
            powerUps.Remove(collision.gameObject);
        }
    }
}
