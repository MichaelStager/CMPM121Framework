using UnityEngine;
using System.Collections;

public class BrokenModifier : SpellModifier
{
    private readonly float damageMultiplier;
    private readonly float manaMultiplier;

    public BrokenModifier(ISpell inner, float damageMultiplier = 0.5f, float manaMultiplier = 0.5f)
        : base(inner)
    {
        this.damageMultiplier = damageMultiplier;
        this.manaMultiplier = manaMultiplier;
    }

    public override int GetDamage()
    {
        return Mathf.RoundToInt(inner.GetDamage() * damageMultiplier);
    }

    public override int GetManaCost()
    {
        return Mathf.RoundToInt(inner.GetManaCost() * manaMultiplier);
    }

    public override string GetName()
    {
        return "Broken " + inner.GetName();
    }

    public override IEnumerator Cast(Vector3 where, Vector3 target, Hittable.Team team)
    {
        LastCast = Time.time;

        int resolvedDamage = GetDamage();
        float resolvedSpeed = GetProjectileSpeed();
        float resolvedSize = GetProjectileScale();
        string resolvedTrajectory = GetProjectileTrajectory();

        GameManager.Instance.projectileManager.CreateProjectile(
            0,
            resolvedTrajectory,
            where,
            target - where,
            resolvedSpeed,
            resolvedSize,
            (other, impact) =>
            {
                if (other.team != team)
                {
                    other.Damage(new Damage(resolvedDamage, Damage.Type.ARCANE));
                }
            }
        );

        yield return new WaitForEndOfFrame();
    }
}