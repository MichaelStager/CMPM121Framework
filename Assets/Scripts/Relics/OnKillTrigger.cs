public class OnKillTrigger : RelicTriggerBase
{
    public OnKillTrigger(RelicData data, RelicContext relicContext, IRelicEffect effect)
        : base(data, relicContext, effect) { }

    public override void Activate()
    {
        EventBus.Instance.OnEnemyKilled += OnEnemyKilled;
    }

    public override void Deactivate()
    {
        EventBus.Instance.OnEnemyKilled -= OnEnemyKilled;
    }

    private void OnEnemyKilled()
    {
        effect.Trigger();
    }
}