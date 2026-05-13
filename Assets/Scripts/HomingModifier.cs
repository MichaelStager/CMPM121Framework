using UnityEngine;

public class HomingModifier : SpellModifier
{
    private readonly float damageMultiplier;
    private readonly int manaAdder;

    public HomingModifier(ISpell inner, float damageMultiplier = 0.75f, int manaAdder = 10)
        : base(inner)
    {
        this.damageMultiplier = damageMultiplier;
        this.manaAdder = manaAdder;
    }

    public override int GetDamage()
    {
        return Mathf.RoundToInt(inner.GetDamage() * damageMultiplier);
    }

    public override int GetManaCost()
    {
        return inner.GetManaCost() + manaAdder;
    }

    public override string GetProjectileTrajectory()
    {
        return "homing";
    }

    public override string GetName()
    {
        return "Homing " + inner.GetName();
    }
}
