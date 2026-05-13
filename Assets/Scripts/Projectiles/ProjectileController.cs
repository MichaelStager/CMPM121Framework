using UnityEngine;
using System;
using System.Collections;

public class ProjectileController : MonoBehaviour
{
    public float lifetime;
    public event Action<Hittable, Vector3> OnHit;
    public ProjectileMovement movement;

    
    public Hittable.Team sourceTeam;

    void Start()
    {
    }

    void Update()
    {
        movement.Movement(transform);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("projectile")) return;

        if (collision.gameObject.CompareTag("unit"))
        {
            Hittable target = null;

            var ec = collision.gameObject.GetComponent<EnemyController>();
            if (ec != null) target = ec.hp;

            var pc = collision.gameObject.GetComponent<PlayerController>();
            if (pc != null) target = pc.hp;

            if (target == null) return;

            // Friendly unit: ignore (do not damage, do not destroy)
            if (target.team == sourceTeam) return;

            // Enemy: apply hit + destroy
            OnHit?.Invoke(target, transform.position);
            Destroy(gameObject);
            return;
        }

        // Non-unit collision (wall/obstacle/etc.)
        Destroy(gameObject);
    }

    public void SetLifetime(float lifetime)
    {
        StartCoroutine(Expire(lifetime));
    }

    IEnumerator Expire(float lifetime)
    {
        yield return new WaitForSeconds(lifetime);
        Destroy(gameObject);
    }
}