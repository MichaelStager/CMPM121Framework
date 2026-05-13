using System;
using UnityEngine;

[Serializable]
public class ModifierData 
{
    public string name;
    public string description;

    public string damage_multiplier;
    public string mana_multiplier;
    public string cooldown_multiplier;
    public string speed_multiplier;
    public string scale_multiplier;

    public string mana_adder;
    public string delay;
    public string angle;

    public string projectile_trajectory;

    public int weight;
}
