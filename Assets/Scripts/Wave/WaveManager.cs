using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Collections;
using System.Linq;

public class WaveManager : MonoBehaviour
{
    public Image level_selector; //Moved to different class
    public GameObject button;   //Moved to different class

    public WaveSummaryUI waveSummaryUI; //Moved to different class
    public GameEndUI gameEndUI;         //Moved to different class

    int wave;   //Moved to different class
    List<EnemyData> enemies;    //Moved to different class
    List<Level> levels; //Moved to different class
    Level selectedLevel;    //Moved to different class

    public EnemySpawner enemySpawner;

    void Start()
    {
        wave = 1;

        if (waveSummaryUI != null)
        {
            waveSummaryUI.SetSpawner(this);
        }

        //-------Level and enemy loading----------
        levels = LevelDataLoader.GetLevels();
        enemies = EnemyDataLoader.GetEnemies();

        //-------Button Setup----------
        GameObject[] selectors = new GameObject[levels.Count];

        for (int i = 0; i < levels.Count; i++)
        {
            selectors[i] = Instantiate(button, level_selector.transform);
            selectors[i].transform.localPosition = new Vector3(0, (i) * 130);
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
        List<SpawnRunTimeData> activeSpawns = new List<SpawnRunTimeData>();
        foreach (Spawn spawn in level.spawns)
        {
            EnemyData enemyData = enemies.FirstOrDefault(e => e.name == spawn.enemy);

            if (enemyData == null)
            {
                Debug.LogError("Enemy type " + spawn.enemy + " not found in enemy data!");
                continue;
            }

            Dictionary<string, int> variables = new Dictionary<string, int>();
            variables["wave"] = wave;
            variables["base"] = enemyData.hp;

            int totalCount = RPNEvaluator.RPNEvaluator.Evaluate(spawn.count, variables);

            if (totalCount <= 0)
            {
                continue;
            }

            activeSpawns.Add(new SpawnRunTimeData(spawn, enemyData, totalCount));
        }

        while (activeSpawns.Any(spawnData => !spawnData.IsDone()))
        {
            float longestDelay = 0.5f;

            foreach (SpawnRunTimeData spawnData in activeSpawns)
            {
                if (spawnData.IsDone())
                {
                    continue;
                }

                int batchSize = spawnData.GetNextBatchSize();
                batchSize = Mathf.Min(batchSize, spawnData.totalCount - spawnData.spawnedSoFar);

                for (int i = 0; i < batchSize; i++)
                {
                    yield return StartCoroutine(enemySpawner.SpawnEnemy(spawnData.spawn, spawnData.enemyData, wave));
                    spawnData.spawnedSoFar++;
                }

                if (spawnData.spawn.delay > longestDelay)
                {
                    longestDelay = spawnData.spawn.delay;
                }
            }

            yield return new WaitForSeconds(longestDelay);
        }

        yield return new WaitWhile(() => GameManager.Instance.enemy_count > 0);

        WaveStats stats = GameManager.Instance.EndWaveStats();

        if (selectedLevel.waves > 0 && wave >= selectedLevel.waves)
        {
            GameManager.Instance.state = GameManager.GameState.GAMEOVER;

            if (gameEndUI != null)
            {
                gameEndUI.ShowWin();
            }
            else
            {
                Debug.LogWarning("GameEndUI is missing on EnemySpawner.");
            }

            yield break;
        }

        GameManager.Instance.state = GameManager.GameState.WAVEEND;

        if (waveSummaryUI != null)
        {
            waveSummaryUI.Show(stats);
        }
        else
        {
            Debug.LogWarning("WaveSummaryUI is missing on EnemySpawner.");
        }
    }
}
