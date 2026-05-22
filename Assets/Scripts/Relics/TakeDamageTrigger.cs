using System.Diagnostics;

public class TakeDamageTrigger : RelicTriggerBase
{
    public TakeDamageTrigger(RelicData data, RelicContext relicContext, IRelicEffect effect)
        : base(data, relicContext, effect) { }

    public override void Activate()
    {
        EventBus.Instance.OnPlayerDamaged += OnPlayerDamaged;
    }

    public override void Deactivate()
    {
        EventBus.Instance.OnPlayerDamaged -= OnPlayerDamaged;
    }

    private void OnPlayerDamaged(Damage dmg, Hittable target)
    {
        effect.Trigger();
       
    }
}