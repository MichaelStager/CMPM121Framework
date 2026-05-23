using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Collections;
using System.Linq;

public  class WaveManager : MonoBehaviour
{
    public Image character_selector;
    public GameObject characterButton;

    private Dictionary<string, CharacterClassData> classes;
    private List<CharacterSelectorController> characterButtons = new List<CharacterSelectorController>();
    private string selectedClass = "mage";

    public Image level_selector; 
    public GameObject button;   
    public WaveSummaryUI waveSummaryUI; 
    public GameEndUI gameEndUI;        

    public int wave;   
    List<EnemyData> enemies;    
    List<Level> levels; 
    Level selectedLevel;    

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

        classes = CharacterClassLoader.GetClasses();

        int index = 0;
        foreach (string classId in classes.Keys)
        {
            GameObject selector = Instantiate(characterButton, character_selector.transform);
            selector.transform.localPosition = new Vector3(index * 180, 0, 0);

            CharacterSelectorController controller = selector.GetComponent<CharacterSelectorController>();
            controller.SetCharacter(classId, this);

            Button buttonComponent = selector.GetComponent<Button>();
            buttonComponent.onClick.AddListener(controller.SelectCharacter);

            characterButtons.Add(controller);
            index++;
        }

        SelectCharacter(selectedClass);
    }

    public void StartLevel(Level currentLevel)
    {
        selectedLevel = currentLevel;
        level_selector.gameObject.SetActive(false);
        character_selector.gameObject.SetActive(false);

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
        PlayerController player = GameManager.Instance.player.GetComponent<PlayerController>();
        player.ApplyStatsForWave(wave);

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
                GameManager.Instance.ResetEnemyList();
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

    public void SelectCharacter(string classId)
    {
        selectedClass = classId;

        PlayerController player = GameManager.Instance.player.GetComponent<PlayerController>();
        player.selectedClass = selectedClass;

        foreach (CharacterSelectorController button in characterButtons)
        {
            button.SetSelected(button.ClassId == selectedClass);
        }
    }


}
