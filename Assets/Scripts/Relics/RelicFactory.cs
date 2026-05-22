using UnityEngine;

public static class RelicFactory
{
    public static Relic Create(RelicData data, RelicContext ctx)
    {
        if (data == null || data.trigger == null || data.effect == null)
        {
            Debug.LogWarning("Invalid relic data.");
            return null;
        }

        IRelicEffect effect = CreateEffect(data, ctx);
        if (effect == null)
        {
            Debug.LogWarning("Unknown relic effect: " + data.effect.type);
            return null;
        }

        IRelicTrigger trigger = CreateTrigger(data, ctx, effect);
        if (trigger == null)
        {
            Debug.LogWarning("Unknown relic trigger: " + data.trigger.type);
            return null;
        }

        return new Relic(data, trigger, effect);
    }

    private static IRelicTrigger CreateTrigger(RelicData data, RelicContext ctx, IRelicEffect effect)
    {
        // take-damage, on-kill, stand-still, etc.
        return null;
    }

    private static IRelicEffect CreateEffect(RelicData data, RelicContext ctx)
    {
        // gain-mana, gain-spellpower (+temporary until), etc.
        return null;
    }
}