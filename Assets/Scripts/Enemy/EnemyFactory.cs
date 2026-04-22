using NUnit.Framework;
using UnityEngine;
using Newtonsoft.Json;
public class EnemyFactory : MonoBehaviour
{
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        JsonConvert.DeserializeObject<Enemy> ("Assets/Resources/enemies.json" );
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
