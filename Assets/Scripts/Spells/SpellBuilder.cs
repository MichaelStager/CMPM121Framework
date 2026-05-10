using UnityEngine;
using System.IO;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Collections.Generic;
using System;

public class SpellBuilder
{

    public Spell Build(SpellCaster owner)
    {
        
        return GetRandomSpell(owner);
    }

   
    public SpellBuilder()
    {        
    }

    //Is working with generating a random number betwen 0,2
    public static Spell GetRandomSpell(SpellCaster owner)
    {
        int pick = UnityEngine.Random.Range(0, 3);
        switch (pick)
        {
            case 0: return new Spell(owner);
            case 1: return new TestSpell1(owner);
            case 2: return new TestSpell2(owner);
            default: return new Spell(owner);
        }
    }

}
