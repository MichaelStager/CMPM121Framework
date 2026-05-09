using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;

public class LoadSpellData
{
    public static List<Spell> GetSpells()
    {
        TextAsset spellJson = Resources.Load<TextAsset>("spells");

        if (spellJson == null)
        {
            Debug.LogError("Could not find spells.json in Resources folder.");
            return null;
        }

        return JsonConvert.DeserializeObject<List<Spell>>(spellJson.text);

    }
}
