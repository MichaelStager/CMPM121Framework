using UnityEngine;
using System;

public class ProjectileManager : MonoBehaviour
{
    public GameObject[] projectiles;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameManager.Instance.projectileManager = this;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void CreateProjectile(
        int which,
        string trajectory,
        Vector3 where,
        Vector3 direction,
        float speed,
        float size,
        Hittable.Team sourceTeam,
        Action<Hittable, Vector3> onHit)
    {
        GameObject new_projectile = Instantiate(
            projectiles[which],
            where + direction.normalized * 1.1f,
            Quaternion.Euler(0, 0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg)
        );

        new_projectile.transform.localScale *= size;

        ProjectileController pc = new_projectile.GetComponent<ProjectileController>();
        pc.sourceTeam = sourceTeam;
        pc.movement = MakeMovement(trajectory, speed);
        pc.OnHit += onHit;
    }

    public ProjectileMovement MakeMovement(string name, float speed)
    {
        if (name == "straight")
        {
            return new StraightProjectileMovement(speed);
        }
        if (name == "homing")
        {
            return new HomingProjectileMovement(speed);
        }
        if (name == "spiraling")
        {
            return new SpiralingProjectileMovement(speed);
        }
        if (name == "withering")
        {
            return new WitheringProjectileMovement(speed);
        }
        return null;
    }
}