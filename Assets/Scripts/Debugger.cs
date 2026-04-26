using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class Debugger : MonoBehaviour
{
    List<EnemyData> enimes;
    List<Level> levels;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
     enimes = EnemyDataLoader.GetEnemies();
     levels = LevelDataLoader.GetLevels();
        Debug.Log(levels[1].spawns[1].sequence[1]);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
