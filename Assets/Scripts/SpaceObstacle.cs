using UnityEngine;

public class SpaceObstacle : MovingEntity
{
    private PlayerController playerController;

    private void Start()
    {
        playerController = FindAnyObjectByType<PlayerController>();
    }

    protected virtual void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerController.HandlePlayerHit(gameObject);
        }
    }
}
