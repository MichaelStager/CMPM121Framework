using UnityEngine;
using Newtonsoft.Json;
using System.Collections.Generic;

public class EnemyFactory : MonoBehaviour
{
    //Returns a list of enemies
    public List<Enemy> GetEnemies()
    {
        TextAsset enemyJson = Resources.Load<TextAsset>("enemies");

        if (enemyJson == null)
        {
            Debug.LogError("Could not find enemies.json in Resources folder.");
            return null;
        }

        return JsonConvert.DeserializeObject<List<Enemy>>(enemyJson.text);

        
    }
}