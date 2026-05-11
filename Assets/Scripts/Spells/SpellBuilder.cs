using System;
using System.Collections.Generic;
using UnityEngine;

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
        if (ModifierLoader.TryGet("speed_amp", out var speedAmp))
        {
            float speedMult = ModifierValueResolver.EvalFloat(speedAmp.speed_multiplier, vars, 1.5f);
            spell = new SpeedModifier(spell, speedMult, 1f);
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
        if (ModifierLoader.TryGet("homing", out var homing))
        {
            float dmgMult = ModifierValueResolver.EvalFloat(homing.damage_multiplier, vars, 0.75f);
            int manaAdd = Mathf.RoundToInt(ModifierValueResolver.EvalFloat(homing.mana_adder, vars, 10f));

            spell = new HomingModifier(spell, dmgMult, manaAdd);
        }

        return spell;
    }
}