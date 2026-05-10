using UnityEngine;

public class TestSpell : Spell
{
    public TestSpell(SpellCaster owner) : base(owner)
    {
    }

    public override string GetName()
    {
        return "Test";
    }

    public override int GetManaCost()
    {
        return 50;
    }

    public override int GetDamage()
    {
        return 300;
    }

    public override float GetCooldown()
    {
        return 0.75f;
    }

    public override int GetIcon()
    {
        return 1;
    }

}
