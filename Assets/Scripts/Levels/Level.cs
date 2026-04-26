using System.Collections.Generic;
using UnityEngine;
using System;
public class Level
{
    /*"spawns": [
            {
                "enemy": "zombie",
                "count": "5 wave +",
                "hp": "base 5 wave * +",
                "delay": "5",
                "sequence": [1,2,3],
                "location": "random"
            },
    */

    public string name;
    public int waves;
    public Spawn[] spawns; 

}
