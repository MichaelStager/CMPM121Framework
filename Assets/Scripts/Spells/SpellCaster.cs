using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpellCaster 
{
    public int mana;
    public int max_mana;
    public int mana_reg;
    public Hittable.Team team;
    public List<ISpell> spells = new List<ISpell>();
    public int currentSpellIndex = 0;
    public int maxSpells = 4;
    public int spellPower;


    public IEnumerator ManaRegeneration()
    {
        while (true)
        {
            mana += mana_reg;
            mana = Mathf.Min(mana, max_mana);
            yield return new WaitForSeconds(1);
        }
    }

    public SpellCaster(int mana, int mana_reg, Hittable.Team team)
    {
        this.mana = mana;
        this.max_mana = mana; //1000 is for debug   
        this.mana_reg = mana_reg;
        this.team = team;
        AddSpell(new BaseSpell(this));
    }

    public IEnumerator Cast(Vector3 where, Vector3 target)
    {
        if (spells.Count == 0)
        {
            yield break;
        }

        for (int i = 0; i < spells.Count; i++)
        {
            int index = (currentSpellIndex + i) % spells.Count;
            ISpell spell = spells[index];

            if (spell.IsReady() && mana >= spell.GetManaCost())
            {
                Debug.Log($"Spell={spell.GetName()} Damage={spell.GetDamage()} ManaCost={spell.GetManaCost()}");
                mana -= spell.GetManaCost();
                yield return spell.Cast(where, target, team);

                currentSpellIndex = (index + 1) % spells.Count;
                yield break;
            }
        }

        yield break;
    }

    public bool AddSpell(ISpell newSpell)
    {
        if (spells.Count >= maxSpells)
        {
            return false;
        }

        spells.Add(newSpell);
        return true;
    }

    public void DropSpell(int index)
    {
        if (index < 0 || index >= spells.Count)
        {
            return;
        }

        spells.RemoveAt(index);

        if (currentSpellIndex >= spells.Count)
        {
            currentSpellIndex = 0;
        }
    }

    public bool IsInventoryFull()
    {
        return spells.Count >= maxSpells;
    }

    public bool HasInventorySpace()
    {
        return spells.Count < maxSpells;
    }


}
