using UnityEngine;
using System.Collections;

public class DoublerModifier : SpellModifier
{
    private readonly float secondCastDelay;
    private readonly float manaMultiplier;

    // secondCastDelay: time between first and second shot
    // manaMultiplier: total mana multiplier for casting twice (commonly 2.0f)
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

    public override IEnumerator Cast(Vector3 where, Vector3 target, Hittable.Team team)
    {
        // one cooldown event for the whole doubled cast
        LastCast = Time.time;

        // first cast now
        yield return inner.Cast(where, target, team);

        // delay then second cast
        if (secondCastDelay > 0f)
            yield return new WaitForSeconds(secondCastDelay);

        yield return inner.Cast(where, target, team);
    }
}