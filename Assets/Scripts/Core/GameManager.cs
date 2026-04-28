using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

public class GameManager
{
    public enum GameState
    {
        PREGAME,
        INWAVE,
        WAVEEND,
        COUNTDOWN,
        GAMEOVER
    }

    public GameState state;

    public int countdown;

    private static GameManager theInstance;

    public static GameManager Instance
    {
        get
        {
            if (theInstance == null)
                theInstance = new GameManager();

            return theInstance;
        }
    }

    public GameObject player;

    public ProjectileManager projectileManager;
    public SpellIconManager spellIconManager;
    public EnemySpriteManager enemySpriteManager;
    public PlayerSpriteManager playerSpriteManager;
    public RelicIconManager relicIconManager;

    private List<GameObject> enemies;

    public int enemy_count
    {
        get { return enemies.Count; }
    }

    public WaveStats currentWaveStats;

    public void StartWaveStats(int waveNumber)
    {
        currentWaveStats = new WaveStats(waveNumber);
    }

    public WaveStats EndWaveStats()
    {
        if (currentWaveStats != null)
        {
            currentWaveStats.End();
        }

        return currentWaveStats;
    }

    public void AddEnemy(GameObject enemy)
    {
        enemies.Add(enemy);
    }

    public void RemoveEnemy(GameObject enemy)
    {
        bool removed = enemies.Remove(enemy);

        if (removed && currentWaveStats != null)
        {
            currentWaveStats.AddEnemyKilled();
        }
    }

    public GameObject GetClosestEnemy(Vector3 point)
    {
        if (enemies == null || enemies.Count == 0) return null;
        if (enemies.Count == 1) return enemies[0];

        return enemies.Aggregate((a, b) =>
            (a.transform.position - point).sqrMagnitude <
            (b.transform.position - point).sqrMagnitude ? a : b);
    }

    private GameManager()
    {
        enemies = new List<GameObject>();

        EventBus.Instance.OnDamage += TrackDamage;
    }

    private void TrackDamage(Vector3 where, Damage dmg, Hittable target)
    {
        if (currentWaveStats == null) return;
        if (target == null) return;

        if (target.team == Hittable.Team.MONSTERS)
        {
            currentWaveStats.AddDamageDealt(dmg.amount);
        }
        else if (target.team == Hittable.Team.PLAYER)
        {
            currentWaveStats.AddDamageTaken(dmg.amount);
        }
    }
}