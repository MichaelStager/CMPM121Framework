using UnityEngine;

public class TestSpell2 : Spell
{
    public TestSpell2(SpellCaster owner) : base(owner)
    {
    }

    public override string GetName()
    {
        return "Test2";
    }

    public override int GetManaCost()
    {
        return 5;
    }

    public override int GetDamage()
    {
        return 5;
    }

    public override float GetCooldown()
    {
        return 0.25f;
    }

    public override int GetIcon()
    {
        return 2;
    }

}
