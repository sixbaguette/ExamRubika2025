// Script pour les projectiles
using UnityEngine;

public class BulletCollider : MonoBehaviour
{
    private GameManager gameManager;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    // Utilisons OnCollisionEnter au lieu de OnTriggerEnter

}