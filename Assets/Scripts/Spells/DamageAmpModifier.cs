using UnityEngine;
using System.Collections;

public class DamageAmpModifier : SpellModifier
{
    private readonly float damageMultiplier;
    private readonly float manaMultiplier;
    private readonly float sizeMultiplier;

    public DamageAmpModifier(ISpell inner, float damageMultiplier = 1.5f, float manaMultiplier = 1.5f, float scaleMultipler = 1.5f)
        : base(inner)
    {
        this.damageMultiplier = damageMultiplier;
        this.manaMultiplier = manaMultiplier;
        this.sizeMultiplier = scaleMultipler;
    }

    public override int GetDamage()
    {
        return Mathf.RoundToInt(inner.GetDamage() * damageMultiplier);
    }

    public override int GetManaCost()
    {
        return Mathf.RoundToInt(inner.GetManaCost() * manaMultiplier);
    }
    public override float GetProjectileScale()
    {
        return inner.GetProjectileScale() * sizeMultiplier;
    }

    public override IEnumerator Cast(Vector3 where, Vector3 target, Hittable.Team team)
    {
        // keep cooldown behavior consistent
        LastCast = Time.time;

        int resolvedDamage = GetDamage();
        float speed = GetProjectileSpeed();

        GameManager.Instance.projectileManager.CreateProjectile(
            0,
            GetProjectileTrajectory(),
            where,
            target - where,
            speed,
            GetProjectileScale(),
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