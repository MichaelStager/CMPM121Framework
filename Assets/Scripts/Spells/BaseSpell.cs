using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

//Spell interface
public class BaseSpell : ISpell
{
    public float LastCast { get; set; }

    public SpellCaster Owner { get; private set; }

    public Hittable.Team Team { get; private set; }

    public virtual float GetProjectileSpeed()
    {
        return 15f;
    }
    public virtual string GetProjectileTrajectory()
    {
        return "straight";
    }
    public virtual float GetProjectileScale()
    {
        return 1f;
    }
    public BaseSpell(SpellCaster owner)
    {
        Owner = owner;
    }

    public virtual string GetName()
    {
        return "Bolt";
    }

    public virtual int GetManaCost()
    {
        return 10;
    }

    public virtual int GetDamage()
    {
        return 100;
    }

    public virtual float GetCooldown()
    {
        return 0.75f;
    }

    public virtual int GetIcon()
    {
        return 0;
    }

    public virtual bool IsReady()
    {
        return LastCast + GetCooldown() < Time.time;
    }
    public virtual IEnumerable<Vector3> GetShotDirections(Vector3 baseDirection)
    {
        if (baseDirection.sqrMagnitude < 0.0001f)
            baseDirection = Vector3.right;

        yield return baseDirection.normalized;
    }

    public virtual int GetExtraCastCount() => 0;
    public virtual float GetExtraCastDelay() => 0f;

    public virtual IEnumerator Cast(Vector3 where, Vector3 target, Hittable.Team team)
    {
        // One cooldown event for the whole (possibly multi-cast) action
        LastCast = Time.time;

        Vector3 baseDir = target - where;
        if (baseDir.sqrMagnitude < 0.0001f)
            baseDir = Vector3.right;

        int totalCasts = 1 + Mathf.Max(0, GetExtraCastCount());
        float extraDelay = Mathf.Max(0f, GetExtraCastDelay());

        for (int c = 0; c < totalCasts; c++)
        {
            int resolvedDamage = GetDamage();
            float speed = GetProjectileSpeed();
            float size = GetProjectileScale();
            string trajectory = GetProjectileTrajectory();

            foreach (var dir in GetShotDirections(baseDir))
            {
                GameManager.Instance.projectileManager.CreateProjectile(
                  0,
                  trajectory,
                  where,
                  dir,
                  speed,
                  size,
                  team,
                  (other, impact) =>
                  {
                     if (other.team != team)
                     {
                          other.Damage(new Damage(resolvedDamage, Damage.Type.ARCANE));
                     }
                  }
                );
            }

            if (c < totalCasts - 1 && extraDelay > 0f)
                yield return new WaitForSeconds(extraDelay);
        }

        yield return new WaitForEndOfFrame();
    }
}