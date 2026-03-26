using UnityEngine;

public class Enemy : SpaceObstacle
{
    public override void Move()
    {
        base.Move();

        if (rb != null)
        {
            // Appliquer directement une v?locit? au Rigidbody
            rb.linearVelocity = Vector3.back * enemySpeed;
        }
        else
        {
            // Fallback au mouvement par transform si pas de Rigidbody
            transform.position += Vector3.back * enemySpeed * Time.deltaTime;
        }

        // Les ennemis ne disparaissent qu'? z=-12 et enl?vent une vie
        if (transform.position.z < -12)
        {
            //enemies.RemoveAt(i);
        }
    }
}
