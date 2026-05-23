using UnityEngine;

public class HealPlayerEffect : RelicEffectBase
{
    public HealPlayerEffect(RelicData data, RelicContext relicContext) : base(data, relicContext) { }

    public override void Trigger()
    {
        if (relicContext.player == null || relicContext.player.hp == null) return;

        int amount = EvaluateAmountOrDefault(0);
        Hittable hp = relicContext.player.hp;
        hp.hp = Mathf.Min(hp.hp + amount, hp.max_hp);
    }
}