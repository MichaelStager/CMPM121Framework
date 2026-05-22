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

    public static RelicData GetByName(string relicName)
    {
        List<RelicData> all = GetAll();
        for (int i = 0; i < all.Count; i++)
        {
            if (all[i] != null && all[i].name == relicName)
            {
                return all[i];
            }
        }

        return null;
    }
    //Will return COUNT many random relics, no dups.
    public static List<RelicData> GetRandomUniqueChoices(int count)
    {
        List<RelicData> all = GetAll();
        List<RelicData> pool = new List<RelicData>(all);
        List<RelicData> choices = new List<RelicData>();

        int pickCount = Mathf.Min(count, pool.Count);
        for (int i = 0; i < pickCount; i++)
        {
            int index = Random.Range(0, pool.Count);
            choices.Add(pool[index]);
            pool.RemoveAt(index);
        }

        return choices;
    }

    // optional helper for playmode tests / hot reload scenarios
    public static void ClearCache()
    {
        cached = null;
    }
}