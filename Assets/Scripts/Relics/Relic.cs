using UnityEngine;

public class Relic
{
    public RelicData Data { get; private set; }

    private readonly IRelicTrigger trigger;
    private readonly IRelicEffect effect;

    public Relic(RelicData data, IRelicTrigger triggerImpl, IRelicEffect effectImpl)
    {
        Data = data;
        trigger = triggerImpl;
        effect = effectImpl;
    }

    public void Activate()
    {
        trigger.Activate();
    }

    public void Deactivate()
    {
        trigger.Deactivate();
    }

    public string GetLabel()
    {
        return Data != null ? Data.name : "Relic";
    }

    // Useful for UI highlight once we add temporary-state effects.
    public bool IsActive()
    {
        // Placeholder for now; we’ll wire real state in effect classes later.
        return false;
    }
}