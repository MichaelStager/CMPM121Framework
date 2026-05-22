using UnityEngine;

public class GainManaEffect : RelicEffectBase
{
    public GainManaEffect(RelicData data, RelicContext relicContext) : base(data, relicContext) { }

    public override void Trigger()
    {
        if (relicContext.player == null || relicContext.player.spellcaster == null) return;

        int amount = EvaluateAmountOrDefault(0);
        relicContext.player.spellcaster.mana = Mathf.Min(
            relicContext.player.spellcaster.mana + amount,
            relicContext.player.spellcaster.max_mana
        );
    }
}