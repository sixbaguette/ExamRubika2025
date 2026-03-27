using UnityEngine;

public abstract class MovingEntity : MonoBehaviour, IMovable
{
    protected GameManager gameManager;

    protected float bulletSpeed = 10.0f;
    public float BulletSpeed
    {
        get { return bulletSpeed; }
        set { bulletSpeed = value; }
    }
    protected float enemySpeed = 3.0f;
    public float EnemySpeed
    {
        get { return enemySpeed; }
        set { enemySpeed = value; }
    }
    protected float asteroidSpeed = 2.0f;
    public float AsteroidSpeed
    {
        get { return asteroidSpeed; }
        set { asteroidSpeed = value; }
    }

    protected Rigidbody rb;

    void Start()
    {
        gameManager = FindAnyObjectByType<GameManager>();
    }

    public virtual void Move()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (gameManager == null)
        {
            gameManager = FindAnyObjectByType<GameManager>();
        }
        else
        {
            return;
        }
    }
}
