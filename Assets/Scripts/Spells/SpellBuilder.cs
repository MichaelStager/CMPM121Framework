using System.Collections.Generic;

public class SpellBuilder
{
         public ISpell Build(SpellCaster owner)
         {
        ISpell spell = new BaseSpell(owner);

        // Runtime variables for RPN expressions
        var vars = new Dictionary<string, float>
        {
            { "wave", 1 }, // adjust member if needed, hard coded number for now but we will need to get the wave number later.
            { "power", 0f } // replace with actual player power stat when available
        };

        if (ModifierLoader.TryGet("damage_amp", out var damageAmp))
        {
            float dmgMult = ModifierValueResolver.EvalFloat(damageAmp.damage_multiplier, vars, 1.5f);
            float manaMult = ModifierValueResolver.EvalFloat(damageAmp.mana_multiplier, vars, 1.5f);
            spell = new DamageAmpModifier(spell, dmgMult, manaMult);
        }

        if (ModifierLoader.TryGet("splitter", out var splitter))
        {
            float angle = ModifierValueResolver.EvalFloat(splitter.angle, vars, 15f);
            float manaMult = ModifierValueResolver.EvalFloat(splitter.mana_multiplier, vars, 1.5f);
            spell = new SplitterModifier(spell, 1, angle, manaMult);
        }

        if (ModifierLoader.TryGet("doubler", out var doubler))
        {
            float delay = ModifierValueResolver.EvalFloat(doubler.delay, vars, 0.15f);
            float manaMult = ModifierValueResolver.EvalFloat(doubler.mana_multiplier, vars, 2.0f);
            spell = new DoublerModifier(spell, delay, manaMult);
        }
        return spell;
    }
}