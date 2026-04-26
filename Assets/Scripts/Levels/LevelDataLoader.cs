using UnityEngine;
using Newtonsoft.Json;
using System.Collections.Generic;

public class LevelDataLoader : MonoBehaviour
{
    //Returns a list of enemies
    /*
    public List<Leveldata> GetEnemies()
    {
        TextAsset enemyJson = Resources.Load<TextAsset>("enemies");

        if (enemyJson == null)
        {
            Debug.LogError("Could not find enemies.json in Resources folder.");
            return null;
        }

        return JsonConvert.DeserializeObject<List<EnemyData>>(enemyJson.text);

    }
    */

    public List<Level> GetLevels()
    {
        TextAsset levelJson = Resources.Load<TextAsset>("levels");

        if (levelJson == null)
        {
            Debug.LogError("Could not find levels.json in Resources folder.");
            return null;
        }

        return JsonConvert.DeserializeObject<List<Level>>(levelJson.text);

    }
}
