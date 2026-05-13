using UnityEngine;

public class DoublerModifier : SpellModifier
{
    private readonly float secondCastDelay;
    private readonly float manaMultiplier;

    public DoublerModifier(ISpell inner, float secondCastDelay = 0.15f, float manaMultiplier = 2.0f)
        : base(inner)
    {
        this.secondCastDelay = secondCastDelay;
        this.manaMultiplier = manaMultiplier;
    }

    public override int GetManaCost()
    {
        return Mathf.RoundToInt(inner.GetManaCost() * manaMultiplier);
    }

    public override string GetName()
    {
        return "Doubled " + inner.GetName();
    }

    public override int GetExtraCastCount()
    {
        return inner.GetExtraCastCount() + 1;
    }

    public override float GetExtraCastDelay()
    {
        // choose max so multiple timing modifiers remain stable
        return Mathf.Max(inner.GetExtraCastDelay(), secondCastDelay);
    }
}