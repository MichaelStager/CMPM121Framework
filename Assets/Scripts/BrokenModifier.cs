using UnityEngine;
using System.Collections;

public class BrokenModifier : SpellModifier
{
    private readonly float damageMultiplier;
    private readonly float manaMultiplier;

    public BrokenModifier(ISpell inner, float damageMultiplier = 0.5f, float manaMultiplier = 0.5f)
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
        return "Broken " + inner.GetName();
    }
}