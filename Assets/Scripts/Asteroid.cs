using UnityEngine;

public class Asteroid : SpaceObstacle
{
    public override void Move()
    {
        base.Move();

        // Direction al?atoire pour chaque ast?ro?de
        float randomX = Random.Range(-0.5f, 0.5f);

        if (rb != null)
        {
            // Appliquer directement une v?locit? au Rigidbody
            rb.linearVelocity = new Vector3(randomX, 0, -1) * asteroidSpeed;

            // Appliquer une rotation
            transform.Rotate(0, 30 * Time.deltaTime, 0);
        }
        else
        {
            // Fallback au mouvement par transform si pas de Rigidbody
            Vector3 movement = new Vector3(randomX, 0, -1) * asteroidSpeed * Time.deltaTime;
            transform.position += movement;
            transform.Rotate(0, 30 * Time.deltaTime, 0);
        }

        // Les ast?ro?des ne disparaissent qu'? z=-12 et enl?vent une vie
        if (transform.position.z < -12)
        {
            //asteroids.RemoveAt(i);
        }
    }
}
