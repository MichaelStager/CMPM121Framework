public class SpellBuilder
{
    public ISpell Build(SpellCaster owner)
    {
        ISpell spell = new BaseSpell(owner);
        spell = new  DamageAmpModifier(spell, 5f, 5f);
        spell = new DoublerModifier(spell,0.15f,.5f);

        return spell;
    }

    public SpellBuilder()
    {
    }
}