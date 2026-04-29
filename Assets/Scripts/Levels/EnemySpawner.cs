using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Collections;
using System.Linq;

public class EnemySpawner : MonoBehaviour
{
    public Image level_selector;
    public GameObject button;
    public GameObject enemy;
    public SpawnPoint[] SpawnPoints;

    public WaveSummaryUI waveSummaryUI;

    int wave;
    List<EnemyData> enimes;
    List<Level> levels;
    Level selectedLevel;

    void Start()
    {
        wave = 1;

        levels = LevelDataLoader.GetLevels();
        enimes = EnemyDataLoader.GetEnemies();

        if (waveSummaryUI != null)
        {
            waveSummaryUI.SetSpawner(this);
        }

        GameObject[] selectors = new GameObject[levels.Count];

        for (int i = 0; i < levels.Count; i++)
        {
            selectors[i] = Instantiate(button, level_selector.transform);
            selectors[i].transform.localPosition = new Vector3(0, (i + 1) * 130);
            selectors[i].GetComponent<MenuSelectorController>().spawner = this;
            selectors[i].GetComponent<MenuSelectorController>().SetLevel(levels[i]);
        }
    }

    public void StartLevel(Level currentLevel)
    {
        selectedLevel = currentLevel;
        level_selector.gameObject.SetActive(false);

        GameManager.Instance.player.GetComponent<PlayerController>().StartLevel();

        StartCoroutine(SpawnWave(selectedLevel));
    }

    public void NextWave()
    {
        wave++;

        if (selectedLevel.waves > 0 && wave > selectedLevel.waves)
        {
            GameManager.Instance.state = GameManager.GameState.GAMEOVER;
            Debug.Log("You beat all waves!");
            return;
        }

        StartCoroutine(SpawnWave(selectedLevel));
    }

    IEnumerator SpawnWave(Level level)
    {
        GameManager.Instance.state = GameManager.GameState.COUNTDOWN;
        GameManager.Instance.countdown = 3;

        for (int i = 3; i > 0; i--)
        {
            yield return new WaitForSeconds(1);
            GameManager.Instance.countdown--;
        }

        GameManager.Instance.state = GameManager.GameState.INWAVE;

        GameManager.Instance.StartWaveStats(wave);

        foreach (Spawn spawn in level.spawns)
        {
            EnemyData enemyData = enimes.FirstOrDefault(e => e.name == spawn.enemy);

            if (enemyData == null)
            {
                Debug.LogError("Enemy type " + spawn.enemy + " not found in enemy data!");
                continue;
            }

            Dictionary<string, int> variables = new Dictionary<string, int>();
            variables["wave"] = wave;
            variables["base"] = enemyData.hp;

            int count = RPNEvaluator.RPNEvaluator.Evaluate(spawn.count, variables);

            float delay = spawn.delay;

            if (delay <= 0)
            {
                delay = 0.5f;
            }

            int spawnedSoFar = 0;
            int sequenceIndex = 0;

            while (spawnedSoFar < count)
            {
                int batchSize = 1;

                if (spawn.sequence != null && spawn.sequence.Length > 0)
                {
                    batchSize = spawn.sequence[sequenceIndex];

                    sequenceIndex++;

                    if (sequenceIndex >= spawn.sequence.Length)
                    {
                        sequenceIndex = 0;
                    }
                }

                batchSize = Mathf.Min(batchSize, count - spawnedSoFar);

                for (int i = 0; i < batchSize; i++)
                {
                    yield return StartCoroutine(SpawnEnemy(spawn, enemyData));
                    spawnedSoFar++;
                }

                yield return new WaitForSeconds(delay);
            }
        }

        yield return new WaitWhile(() => GameManager.Instance.enemy_count > 0);

        GameManager.Instance.state = GameManager.GameState.WAVEEND;

        WaveStats stats = GameManager.Instance.EndWaveStats();

        if (waveSummaryUI != null)
        {
            waveSummaryUI.Show(stats);
        }
        else
        {
            Debug.LogWarning("WaveSummaryUI is missing on EnemySpawner.");
        }
    }


    IEnumerator SpawnEnemy(Spawn spawn, EnemyData enemyData)
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