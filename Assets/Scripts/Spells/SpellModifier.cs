using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class SpellModifier : ISpell
{
    protected readonly ISpell inner;

    protected SpellModifier(ISpell inner)
    {
        this.inner = inner;
    }

    // Forward runtime state
    public virtual float LastCast
    {
        get => inner.LastCast;
        set => inner.LastCast = value;
    }

    public virtual SpellCaster Owner => inner.Owner;
    public virtual Hittable.Team Team => inner.Team;

    // Forward identity / stats
    public virtual string GetName() => inner.GetName();
    public virtual int GetIcon() => inner.GetIcon();
    public virtual int GetManaCost() => inner.GetManaCost();
    public virtual int GetDamage() => inner.GetDamage();
    public virtual float GetCooldown() => inner.GetCooldown();

    public virtual float GetProjectileSpeed() => inner.GetProjectileSpeed();
    public virtual string GetProjectileTrajectory() => inner.GetProjectileTrajectory();
    public virtual float GetProjectileScale() => inner.GetProjectileScale();

    public virtual IEnumerable<Vector3> GetShotDirections(Vector3 baseDirection) =>
        inner.GetShotDirections(baseDirection);

    public virtual int GetExtraCastCount() => inner.GetExtraCastCount();
    public virtual float GetExtraCastDelay() => inner.GetExtraCastDelay();

    // Forward readiness
    public virtual bool IsReady() => inner.IsReady();

    // Unified cast execution at modifier layer so composed getters are respected.
    public virtual IEnumerator Cast(Vector3 where, Vector3 target, Hittable.Team team)
    {
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