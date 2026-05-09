using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Collections;
using System.Linq;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemy;    
    public SpawnPoint[] SpawnPoints;

    public IEnumerator SpawnEnemy(Spawn spawn, EnemyData enemyData, int wave)
    {
        SpawnPoint spawn_point = GetSpawnPoint(spawn.location);
        Vector2 offset = Random.insideUnitCircle * 1.8f;

        Vector3 initial_position = spawn_point.transform.position + new Vector3(offset.x, offset.y, 0);
        GameObject new_enemy = Instantiate(enemy, initial_position, Quaternion.identity);

        SpriteRenderer sr = new_enemy.GetComponent<SpriteRenderer>();
        EnemyController en = new_enemy.GetComponent<EnemyController>();

        Dictionary<string, int> variables = new Dictionary<string, int>();
        variables["wave"] = wave;
        variables["base"] = enemyData.hp;

        int finalHp = enemyData.hp;

        if (!string.IsNullOrEmpty(spawn.hp))
        {
            finalHp = RPNEvaluator.RPNEvaluator.Evaluate(spawn.hp, variables);
        }

        int finalDamage = enemyData.damage;

        if (!string.IsNullOrEmpty(spawn.damage))
        {
            variables["base"] = enemyData.damage;
            finalDamage = RPNEvaluator.RPNEvaluator.Evaluate(spawn.damage, variables);
        }

        sr.sprite = GameManager.Instance.enemySpriteManager.Get(enemyData.sprite);

        en.hp = new Hittable(finalHp, Hittable.Team.MONSTERS, new_enemy);
        en.speed = enemyData.speed;
        en.damage = finalDamage;

        GameManager.Instance.AddEnemy(new_enemy);

        yield return null;
    }

    SpawnPoint GetSpawnPoint(string location)
    {
        if (string.IsNullOrEmpty(location) || location == "random")
        {
            return SpawnPoints[Random.Range(0, SpawnPoints.Length)];
        }

        List<SpawnPoint> matchingPoints = new List<SpawnPoint>();

        foreach (SpawnPoint point in SpawnPoints)
        {
            if (point.name.ToLower().Contains(location.Replace("random ", "").ToLower()))
            {
                matchingPoints.Add(point);
            }
        }

        if (matchingPoints.Count > 0)
        {
            return matchingPoints[Random.Range(0, matchingPoints.Count)];
        }

        Debug.LogWarning("No spawn point found for location: " + location + ". Using random.");
        return SpawnPoints[Random.Range(0, SpawnPoints.Length)];
    }
}