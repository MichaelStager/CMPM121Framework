using UnityEngine;

public class TestSpell1 : Spell
{
    public TestSpell1(SpellCaster owner) : base(owner)
    {
    }

    public override string GetName()
    {
        return "Test1";
    }

    public override int GetManaCost()
    {
        return 3;
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
