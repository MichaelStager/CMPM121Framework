using System.Collections.Generic;
using UnityEngine;

public class RelicContext
{
    public PlayerController player;
    public int currentWave;

    public Dictionary<string, int> BuildRpnVars()
    {
        return new Dictionary<string, int>
        {
            { "wave", currentWave }
        };
    }

    public Vector2 lastMovement;
}