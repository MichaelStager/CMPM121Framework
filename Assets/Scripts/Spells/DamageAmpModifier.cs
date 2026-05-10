using UnityEngine;

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

    public override string GetName()
    {
        // Optional: show modifier in UI name
        return "Damage-Amplified " + inner.GetName();
    }
}