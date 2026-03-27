using UnityEngine;

public class TurretBullet : Bullet
{
    private Vector3 direction;

    public void SetDirection(Vector3 dir)
    {
        direction = dir.normalized;
    }

    public override void Move()
    {
        base.Move();

        if (rb != null)
        {
            // R?initialiser la v?locit? et appliquer une nouvelle force
            rb.linearVelocity = direction * bulletSpeed;
        }
        else
        {
            // Fallback au mouvement par transform si pas de Rigidbody
            transform.position += direction * bulletSpeed * Time.deltaTime;
        }
    }
}
