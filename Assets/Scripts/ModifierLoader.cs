using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;

public class ModifierLoader
{
    private static Dictionary<string, ModifierData> cached;

    public static Dictionary<string, ModifierData> GetAll()
    {
        if (cached != null) return cached;

        TextAsset json = Resources.Load<TextAsset>("Modifiers"); // exact filename in Resources
        if (json == null)
        {
            Debug.LogError("Could not find Resources/Modifiers.json");
            cached = new Dictionary<string, ModifierData>();
            return cached;
        }

        cached = JsonConvert.DeserializeObject<Dictionary<string, ModifierData>>(json.text)
                 ?? new Dictionary<string, ModifierData>();

        return cached;
    }

    public static bool TryGet(string id, out ModifierData data)
    {
        return GetAll().TryGetValue(id, out data);
    }
}

