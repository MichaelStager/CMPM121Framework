using UnityEngine;

public class BubbleModifier : SpellModifier
{
    private readonly float damageMultiplier;
    private readonly float manaMultiplier;
    private readonly string trajectory;
    private readonly float sizeMultiplier;

    public BubbleModifier(ISpell inner, float damageMultiplier, float manaMultiplier, float sizeMultiplier, string trajectory)
     : base(inner)
    {
        this.damageMultiplier = damageMultiplier;
        this.manaMultiplier = manaMultiplier;
        this.sizeMultiplier = sizeMultiplier;
        this.trajectory = trajectory;
    }

    public override float GetProjectileScale()
    {
        return inner.GetProjectileScale() * sizeMultiplier;
    }

    public override int GetDamage()
    {
        return Mathf.RoundToInt(inner.GetDamage() * damageMultiplier);
    }

    public override int GetManaCost()
    {
        return Mathf.RoundToInt(inner.GetManaCost() * manaMultiplier);
    }

    public override string GetProjectileTrajectory()
    {
        return trajectory; // "withering"
    }

    public override string GetName()
    {
        return "Bubble " + inner.GetName();
    }
}