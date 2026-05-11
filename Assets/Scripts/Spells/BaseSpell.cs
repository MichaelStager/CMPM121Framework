using UnityEngine;
using System.Collections;

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

    public virtual IEnumerator Cast(Vector3 where, Vector3 target, Hittable.Team team)
    {
        Team = team;
        LastCast = Time.time;

        int resolvedDamage = GetDamage();
        float speed = GetProjectileSpeed();

        GameManager.Instance.projectileManager.CreateProjectile(
            0,
            GetProjectileTrajectory(),
            where,
            target - where,
            speed,
            (other, impact) => OnHitWithDamage(other, impact, resolvedDamage)
        );

        yield return new WaitForEndOfFrame();
    }

    protected virtual void OnHitWithDamage(Hittable other, Vector3 impact, int damage)
    {
        if (other.team != Team)
        {
            other.Damage(new Damage(damage, Damage.Type.ARCANE));
        }
    }
}