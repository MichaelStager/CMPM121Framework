using UnityEngine;
using System.Collections;

public class SpeedModifier : SpellModifier
{
    private readonly float speedMultiplier;
    private readonly float manaMultiplier;

    public SpeedModifier(ISpell inner, float speedMultiplier = 1.5f, float manaMultiplier = 1.25f)
        : base(inner)
    {
        this.speedMultiplier = speedMultiplier;
        this.manaMultiplier = manaMultiplier;
    }

    public override int GetManaCost()
    {
        return Mathf.RoundToInt(inner.GetManaCost() * manaMultiplier);
    }

    public override float GetProjectileSpeed()
    {
        return inner.GetProjectileSpeed() * speedMultiplier;
    }

    public override string GetName()
    {
        return "Swift " + inner.GetName();
    }
}
