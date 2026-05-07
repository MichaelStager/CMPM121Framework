using UnityEngine;

public class SpawnRunTimeData
{
   
    public Spawn spawn;
    public EnemyData enemyData;

    public int totalCount;
    public int spawnedSoFar;
    public int sequenceIndex;

    public SpawnRunTimeData(Spawn spawn, EnemyData enemyData, int totalCount)
    {
        this.spawn = spawn;
        this.enemyData = enemyData;
        this.totalCount = totalCount;

        spawnedSoFar = 0;
        sequenceIndex = 0;
    }

    public bool IsDone()
    {
        return spawnedSoFar >= totalCount;
    }

    public int GetNextBatchSize()
    {
        if (spawn.sequence == null || spawn.sequence.Length == 0)
        {
            return 1;
        }

        int batchSize = spawn.sequence[sequenceIndex];

        sequenceIndex++;

        if (sequenceIndex >= spawn.sequence.Length)
        {
            sequenceIndex = 0;
        }

        return batchSize;
    }
}
    

