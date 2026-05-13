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
    public override string GetName()
    {
        return "Empowered " + inner.GetName();
    }

}