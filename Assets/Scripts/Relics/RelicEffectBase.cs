using System.Collections.Generic;
using UnityEngine;

public abstract class RelicEffectBase : IRelicEffect
{
    protected readonly RelicData data;
    protected readonly RelicContext relicContext;

    protected RelicEffectBase(RelicData data, RelicContext relicContext)
    {
        this.data = data;
        this.relicContext = relicContext;
    }

    public abstract void Trigger();

    public virtual void OnEvent(string eventType) { }

    protected int EvaluateAmountOrDefault(int fallback = 0)
    {
        if (data == null || data.effect == null || string.IsNullOrWhiteSpace(data.effect.amount))
        {
            return fallback;
        }

        string expr = data.effect.amount;

        int numeric;
        if (int.TryParse(expr, out numeric))
        {
            return numeric;
        }

        Dictionary<string, int> vars = relicContext.BuildRpnVars();
        return RPNEvaluator.RPNEvaluator.Evaluate(expr, vars);
    }
}