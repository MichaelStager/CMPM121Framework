using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class Debugger : MonoBehaviour
{
    List<EnemyData> enimes;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
     enimes = EnemyDataLoader.GetEnemies();
        Debug.Log(enimes[1].name);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
