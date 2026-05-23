public static class RelicFactory
{
    public static Relic Create(RelicData data, RelicContext relicContext)
    {
        if (data == null || data.trigger == null || data.effect == null)
        {
            UnityEngine.Debug.LogWarning("Invalid relic data.");
            return null;
        }

        IRelicEffect effect = CreateEffect(data, relicContext);
        if (effect == null)
        {
            UnityEngine.Debug.LogWarning("Unknown relic effect: " + data.effect.type);
            return null;
        }

        IRelicTrigger trigger = CreateTrigger(data, relicContext, effect);
        if (trigger == null)
        {
            UnityEngine.Debug.LogWarning("Unknown relic trigger: " + data.trigger.type);
            return null;
        }

        return new Relic(data, trigger, effect);
    }

    private static IRelicTrigger CreateTrigger(RelicData data, RelicContext relicContext, IRelicEffect effect)
    {
        switch (data.trigger.type)
        {
            case "take-damage":
                return new TakeDamageTrigger(data, relicContext, effect);

            case "on-kill":
                return new OnKillTrigger(data, relicContext, effect);

            case "stand-still":
                return new StandStillTrigger(data, relicContext, effect);

            case "cast-spell":
                return new OnSpellCastTrigger(data, relicContext, effect);

            default:
                return null;
        }
    }

    private static IRelicEffect CreateEffect(RelicData data, RelicContext relicContext)
    {
        switch (data.effect.type)
        {
            case "gain-mana":
                return new GainManaEffect(data, relicContext);

            case "gain-spellpower":
                return new GainSpellPowerEffect(data, relicContext);
            case "heal-player":
                return new HealPlayerEffect(data, relicContext);

            default:
                return null;
        }
    }
}