using System;
using Newtonsoft.Json;

[Serializable]
public class RelicData
{
    public string name;
    public int sprite;
    public RelicTriggerData trigger;
    public RelicEffectData effect;
}

[Serializable]
public class RelicTriggerData
{
    public string description;
    public string type;
    public string amount;
}

[Serializable]
public class RelicEffectData
{
    public string description;
    public string type;
    public string amount;
    public string until;
}