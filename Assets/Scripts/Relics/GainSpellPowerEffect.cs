public class GainSpellPowerEffect : RelicEffectBase
{
    private bool isTemporaryActive;
    private int temporaryAmount;

    public GainSpellPowerEffect(RelicData data, RelicContext ctx) : base(data, ctx)
    {
        isTemporaryActive = false;
        temporaryAmount = 0;
    }

    public override void Trigger()
    {
        if (relicContext.player == null || relicContext.player.spellcaster == null) return;

        int amount = EvaluateAmountOrDefault(0);
        bool hasUntil = data != null && data.effect != null && !string.IsNullOrWhiteSpace(data.effect.until);

        // For temporary effects, do not stack infinitely while active.
        if (hasUntil && isTemporaryActive)
        {
            return;
        }

        relicContext.player.spellcaster.spellPower += amount;

        if (hasUntil)
        {
            isTemporaryActive = true;
            temporaryAmount = amount;
        }
    }

    public override void OnEvent(string eventType)
    {
        if (!isTemporaryActive || data == null || data.effect == null) return;

        // until: move
        if (data.effect.until == "move" && eventType == "move")
        {
            RemoveTemporaryBonus();
            return;
        }

        // until: cast-spell
        if (data.effect.until == "cast-spell" && eventType == "cast-spell")
        {
            RemoveTemporaryBonus();
            return;
        }
    }

    private void RemoveTemporaryBonus()
    {
        if (relicContext.player == null || relicContext.player.spellcaster == null) return;

        relicContext.player.spellcaster.spellPower -= temporaryAmount;
        temporaryAmount = 0;
        isTemporaryActive = false;
    }
}