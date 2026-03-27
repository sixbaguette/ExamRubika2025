using UnityEngine;

public class BasicBullet : Bullet
{
    public override void Move()
    {
        base.Move();

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
    }
}
