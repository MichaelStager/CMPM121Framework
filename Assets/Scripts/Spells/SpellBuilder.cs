using System.Collections.Generic;
using UnityEngine;

public class SpellBuilder
{
    public ISpell BuildRandom(SpellCaster owner, int modifierCount)
    {
        ISpell spell = new BaseSpell(owner);

        var vars = new Dictionary<string, float>
        {
            { "wave", 1 },
            { "power", owner.spellPower }
        };

        List<string> possibleModifiers = new List<string>
        {
           // "homing",
          //  "chaos",
           // "damage_amp",
           // "speed_amp",
            "splitter",
           // "doubler",
            "massive"
        };

        for (int i = 0; i < modifierCount; i++)
        {
            string modifierId = PickRandomModifier(possibleModifiers);
            possibleModifiers.Remove(modifierId);

            spell = ApplyModifier(spell, modifierId, vars);
        }

        return spell;
    }

    private string PickRandomModifier(List<string> possibleModifiers)
    {
        int index = Random.Range(0, possibleModifiers.Count);
        return possibleModifiers[index];
    }

    private ISpell ApplyModifier(ISpell spell, string modifierId, Dictionary<string, float> vars)
    {
        if (modifierId == "homing" && ModifierLoader.TryGet("homing", out var homing))
        {
            float dmgMult = ModifierValueResolver.EvalFloat(homing.damage_multiplier, vars, 0.75f);
            int manaAdd = Mathf.RoundToInt(ModifierValueResolver.EvalFloat(homing.mana_adder, vars, 10f));
            return new HomingModifier(spell, dmgMult, manaAdd);
        }

        if (modifierId == "chaos" && ModifierLoader.TryGet("chaos", out var chaos))
        {
            float dmgMult = ModifierValueResolver.EvalFloat(chaos.damage_multiplier, vars, 1.5f);
            return new ChaosModifier(spell, dmgMult);
        }

        if (modifierId == "damage_amp" && ModifierLoader.TryGet("damage_amp", out var damageAmp))
        {
            float dmgMult = ModifierValueResolver.EvalFloat(damageAmp.damage_multiplier, vars, 1.5f);
            float manaMult = ModifierValueResolver.EvalFloat(damageAmp.mana_multiplier, vars, 1.5f);
            float scaleMult = ModifierValueResolver.EvalFloat(damageAmp.scale_multiplier, vars, 1.5f);
            return new DamageAmpModifier(spell, dmgMult, manaMult,scaleMult);
        }
        if (modifierId == "massive" && ModifierLoader.TryGet("massive", out var massive))
        {
            float damgMult = ModifierValueResolver.EvalFloat(massive.delay, vars, 0.15f);
            float manaMult = ModifierValueResolver.EvalFloat(massive.mana_multiplier, vars, 2.0f);
            float scaleMult = ModifierValueResolver.EvalFloat(massive.scale_multiplier, vars, 2.0f);
            float speedMult = ModifierValueResolver.EvalFloat(massive.speed_multiplier, vars, .3f);
            return new MassiveModifier(spell, damgMult, manaMult,scaleMult,speedMult);
        }

        if (modifierId == "speed_amp" && ModifierLoader.TryGet("speed_amp", out var speedAmp))
        {
            float speedMult = ModifierValueResolver.EvalFloat(speedAmp.speed_multiplier, vars, 1.5f);
            return new SpeedModifier(spell, speedMult, 1f);
        }

        if (modifierId == "splitter" && ModifierLoader.TryGet("splitter", out var splitter))
        {
            float angle = ModifierValueResolver.EvalFloat(splitter.angle, vars, 15f);
            float manaMult = ModifierValueResolver.EvalFloat(splitter.mana_multiplier, vars, 1.5f);
            return new SplitterModifier(spell, 1, angle, manaMult);
        }

        if (modifierId == "doubler" && ModifierLoader.TryGet("doubler", out var doubler))
        {
            float delay = ModifierValueResolver.EvalFloat(doubler.delay, vars, 0.15f);
            float manaMult = ModifierValueResolver.EvalFloat(doubler.mana_multiplier, vars, 2.0f);
            return new DoublerModifier(spell, delay, manaMult);
        }

        return spell;
    }
}