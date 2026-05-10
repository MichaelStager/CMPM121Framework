using UnityEngine;
using System.Collections;

public class BaseSpell : ISpell
{
    public float LastCast { get; set; }

    public SpellCaster Owner { get; private set; }

    public Hittable.Team Team { get; private set; }

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

        GameManager.Instance.projectileManager.CreateProjectile(
            0,                  // sprite index
            "straight",         // trajectory
            where,              // origin
            target - where,     // direction vector
            15f,                // speed
            OnHit               // collision callback
        );

        yield return new WaitForEndOfFrame();
    }

    protected virtual void OnHit(Hittable other, Vector3 impact)
    {
        if (other.team != Team)
        {
            other.Damage(new Damage(GetDamage(), Damage.Type.ARCANE));
        }
    }
}