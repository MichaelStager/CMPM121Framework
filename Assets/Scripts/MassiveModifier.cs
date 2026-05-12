using UnityEngine;

public class MassiveModifier : SpellModifier
{
    private readonly float damageMultiplier;
    private readonly float manaMultiplier;
    private readonly float sizeMultiplier;
    private readonly float speedMultiplier;

    public MassiveModifier(
        ISpell inner,
        float damageMultiplier = 2.0f,
        float manaMultiplier = 2.0f,
        float sizeMultiplier = 5.0f,
        float speedMultiplier = 0.3f
    ) : base(inner)
    {
        this.damageMultiplier = damageMultiplier;
        this.manaMultiplier = manaMultiplier;
        this.sizeMultiplier = sizeMultiplier;
        this.speedMultiplier = speedMultiplier;
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

    public override float GetProjectileSpeed()
    {
        return inner.GetProjectileSpeed() * speedMultiplier;
    }

    public override string GetName()
    {
        return "Massive " + inner.GetName();
    }
}