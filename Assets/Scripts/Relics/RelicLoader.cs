using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

public static class RelicLoader
{
    private static List<RelicData> cached;

    public static List<RelicData> GetAll()
    {
        if (cached != null)
        {
            return cached;
        }

        TextAsset json = Resources.Load<TextAsset>("relics");
        if (json == null)
        {
            Debug.LogError("Could not find Resources/relics.json");
            cached = new List<RelicData>();
            return cached;
        }

        cached = JsonConvert.DeserializeObject<List<RelicData>>(json.text) ?? new List<RelicData>();
        return cached;
    }

    public static void ClearCache()
    {
        cached = null;
    }
}