using UnityEngine;
using System;
using System.Collections.Generic;
public class ProjectileManager : MonoBehaviour
{
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameManager.Instance.projectileManager = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreateProjectile(Projectile projectile,Vector3 where, Vector3 direction, Action<Hittable,Vector3> onHit) // This needs a game object to spawn 
    {
        GameObject new_projectile = Instantiate(projectile.projectileObject, where + direction.normalized*1.1f, Quaternion.Euler(0,0,Mathf.Atan2(direction.y, direction.x)*Mathf.Rad2Deg));
        new_projectile.GetComponent<ProjectileController>().movement = MakeMovement(projectile.trajectory, RPNEvaluator.RPNEvaluator.Evaluate(projectile.speed,null));
        new_projectile.GetComponent<ProjectileController>().OnHit += onHit;
        new_projectile.GetComponent<ProjectileController>().SetLifetime(RPNEvaluator.RPNEvaluator.Evaluatef(projectile.lifetime, new Dictionary<string, float>())); // might need to add to this dictionary.
    }

   // public void CreateProjectile(int which, string trajectory, Vector3 where, Vector3 direction, float speed, Action<Hittable, Vector3> onHit, float lifetime)
   // {
    //    GameObject new_projectile = Instantiate(projectiles[which], where + direction.normalized * 1.1f, Quaternion.Euler(0, 0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg));
     //   new_projectile.GetComponent<ProjectileController>().movement = MakeMovement(trajectory, speed);
     //   new_projectile.GetComponent<ProjectileController>().OnHit += onHit;
     //   new_projectile.GetComponent<ProjectileController>().SetLifetime(lifetime);
   // }

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
        return null;
    }

}
