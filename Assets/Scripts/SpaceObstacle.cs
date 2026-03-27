using UnityEngine;

public abstract class SpaceObstacle : MovingEntity
{
    private PlayerController playerController;

    private int lives;
    private GameObject playerDamageEffect;

    private void Start()
    {
        playerController = FindAnyObjectByType<PlayerController>();
        playerDamageEffect = FindAnyObjectByType<UIManager>().PlayerDamageEffect;
    }

    protected virtual void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerController.HandlePlayerHit(gameObject);
        }
    }

    public override void Move()
    {
        base.Move();

        if (transform.position.z < -12)
        {
            lives = FindAnyObjectByType<GameManager>().Lives--;
            // Enlever un point de vie au joueur
            //lives--;

            // Effet visuel pour montrer que l'ast?ro?de a travers?
            if (playerDamageEffect != null)
            {
                Instantiate(playerDamageEffect, transform.position, Quaternion.identity);
            }

            // Destruction de l'ast?ro?de
            Destroy(this.gameObject);

            // V?rifier si le joueur n'a plus de vies
            if (lives <= 0)
            {
                gameManager.GameOver();
            }
        }
    }
}
