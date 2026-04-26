using System.Collections.Generic;
using UnityEngine;
using System;

public class Spawn
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

    public string enemy;
    public string count;
    public string hp;
    public int delay;
    public int[] sequence;
    public string location;
}
