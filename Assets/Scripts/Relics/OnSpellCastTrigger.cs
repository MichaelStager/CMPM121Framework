public class OnSpellCastTrigger : RelicTriggerBase
{
    public OnSpellCastTrigger(RelicData data, RelicContext relicContext, IRelicEffect effect)
        : base(data, relicContext, effect) { }

    public override void Activate()
    {
        EventBus.Instance.OnSpellCast += OnSpellCast;
    }

    public override void Deactivate()
    {
        EventBus.Instance.OnSpellCast -= OnSpellCast;
    }

    private void OnSpellCast()
    {
        effect.Trigger();
        effect.OnEvent("cast-spell");
    }
}