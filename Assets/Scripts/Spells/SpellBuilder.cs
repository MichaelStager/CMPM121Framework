public class SpellBuilder
{
    public ISpell Build(SpellCaster owner)
    {
        ISpell spell = new BaseSpell(owner);
        spell = new DamageAmpModifier(spell, 1.5f, 1.5f);
        return spell;
    }

    public SpellBuilder()
    {
    }
}