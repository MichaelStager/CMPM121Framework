using UnityEngine;

public class WaveStats
{
    public int waveNumber;
    public int enemiesKilled;
    public int damageDealt;
    public int damageTaken;

    private float startTime;
    private float endTime;

    public float TimeSpent
    {
        get { return endTime - startTime; }
    }

    public WaveStats(int waveNumber)
    {
        this.waveNumber = waveNumber;
        startTime = Time.time;
        endTime = startTime;
    }

    public void End()
    {
        endTime = Time.time;
    }

    //Find where enemey is killed and call this
    public void AddEnemyKilled()
    {
        enemiesKilled++;
    }

    //Find where damage is dealth and call this
    public void AddDamageDealt(int amount)
    {
        damageDealt += amount;
    }

    //Find where damage is taken and call this
    public void AddDamageTaken(int amount)
    {
        damageTaken += amount;
    }
}