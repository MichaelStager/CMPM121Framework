using UnityEngine;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.IO;
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
    int wave;
    List<EnemyData> enimes;
    List<Level> levels;
    Level selectedLevel;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        wave = 0;
        levels = LevelDataLoader.GetLevels();
        GameObject[] selectors = new GameObject[levels.Count];
        for (int i=0; i < levels.Count; i++)
        {
            selectors[i] = Instantiate(button, level_selector.transform);
            selectors[i].transform.localPosition = new Vector3(0, (i+1)*130);
            selectors[i].GetComponent<MenuSelectorController>().spawner = this;
            selectors[i].GetComponent<MenuSelectorController>().SetLevel(levels[i]);

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartLevel(Level currentLevel)
    {
        selectedLevel = currentLevel;
        level_selector.gameObject.SetActive(false);
        // this is not nice: we should not have to be required to tell the player directly that the level is starting
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
        //--------------------------------------------------------- all above is fine.
        for (int i= 0; i < level.spawns.Length; i++)
        {
            yield return SpawnEnemy();
        }
        yield return new WaitWhile(() => GameManager.Instance.enemy_count > 0);
        GameManager.Instance.state = GameManager.GameState.WAVEEND;
        //Increase the wave we are on after clearing all enemies
       
    }

    IEnumerator SpawnEnemy()
    {
        SpawnPoint spawn_point = SpawnPoints[Random.Range(0, SpawnPoints.Length)];
        Vector2 offset = Random.insideUnitCircle * 1.8f;
                
        Vector3 initial_position = spawn_point.transform.position + new Vector3(offset.x, offset.y, 0);
        GameObject new_enemy = Instantiate(enemy, initial_position, Quaternion.identity);

        new_enemy.GetComponent<SpriteRenderer>().sprite = GameManager.Instance.enemySpriteManager.Get(0);
        EnemyController en = new_enemy.GetComponent<EnemyController>();
        en.hp = new Hittable(50, Hittable.Team.MONSTERS, new_enemy);
        en.speed = 10;
        GameManager.Instance.AddEnemy(new_enemy);
        yield return new WaitForSeconds(0.5f);
    }
}
