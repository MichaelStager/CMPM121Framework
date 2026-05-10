using UnityEngine;
using System.Collections;

public class DamageAmpModifier : SpellModifier
{
    private readonly float damageMultiplier;
    private readonly float manaMultiplier;

    public DamageAmpModifier(ISpell inner, float damageMultiplier = 1.5f, float manaMultiplier = 1.5f)
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

    public override IEnumerator Cast(Vector3 where, Vector3 target, Hittable.Team team)
    {
        // keep cooldown behavior consistent
        LastCast = Time.time;

        int resolvedDamage = GetDamage();

        GameManager.Instance.projectileManager.CreateProjectile(
            0,
            "straight",
            where,
            target - where,
            15f,
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