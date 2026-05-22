public static class RelicFactory
{
    public static Relic Create(RelicData data, RelicContext ctx)
    {
        if (data == null || data.trigger == null || data.effect == null)
        {
            UnityEngine.Debug.LogWarning("Invalid relic data.");
            return null;
        }

        IRelicEffect effect = CreateEffect(data, ctx);
        if (effect == null)
        {
            UnityEngine.Debug.LogWarning("Unknown relic effect: " + data.effect.type);
            return null;
        }

        IRelicTrigger trigger = CreateTrigger(data, ctx, effect);
        if (trigger == null)
        {
            UnityEngine.Debug.LogWarning("Unknown relic trigger: " + data.trigger.type);
            return null;
        }

        return new Relic(data, trigger, effect);
    }

    private static IRelicTrigger CreateTrigger(RelicData data, RelicContext ctx, IRelicEffect effect)
    {
        switch (data.trigger.type)
        {
            case "take-damage":
                return new TakeDamageTrigger(data, ctx, effect);

            case "on-kill":
                return new OnKillTrigger(data, ctx, effect);

            case "stand-still":
                return new StandStillTrigger(data, ctx, effect);

            default:
                return null;
        }
    }

    private static IRelicEffect CreateEffect(RelicData data, RelicContext ctx)
    {
        switch (data.effect.type)
        {
            case "gain-mana":
                return new GainManaEffect(data, ctx);

            case "gain-spellpower":
                return new GainSpellPowerEffect(data, ctx);

            default:
                return null;
        }
    }
}