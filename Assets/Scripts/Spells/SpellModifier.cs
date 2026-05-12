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
    public virtual string GetProjectileTrajectory()=> inner.GetProjectileTrajectory();
    public virtual float GetProjectileScale() => inner.GetProjectileScale();
    public virtual IEnumerable<Vector3> GetShotDirections(Vector3 baseDirection)
    => inner.GetShotDirections(baseDirection);

    public virtual int GetExtraCastCount() => inner.GetExtraCastCount();
    public virtual float GetExtraCastDelay() => inner.GetExtraCastDelay();

    // Forward behavior
    public virtual bool IsReady() => inner.IsReady();

    public virtual IEnumerator Cast(Vector3 where, Vector3 target, Hittable.Team team)
    {
        yield return inner.Cast(where, target, team);
    }
}