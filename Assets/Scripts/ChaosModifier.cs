using UnityEngine;

public class ChaosModifier : SpellModifier
{
    private readonly float damageMultiplier;

    public ChaosModifier(ISpell inner, float damageMultiplier = 1.5f)
        : base(inner)
    {
        this.damageMultiplier = damageMultiplier;
    }

    public override int GetDamage()
    {
        return Mathf.RoundToInt(inner.GetDamage() * damageMultiplier);
    }

    public override string GetProjectileTrajectory()
    {
        return "spiraling";
    }

    public override string GetName()
    {
        return "Chaotic " + inner.GetName();
    }
}
