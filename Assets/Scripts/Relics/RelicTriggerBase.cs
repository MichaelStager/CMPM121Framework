public abstract class RelicTriggerBase : IRelicTrigger
{
    protected readonly RelicContext relicContext;
    protected readonly IRelicEffect effect;
    protected readonly RelicData data;

    protected RelicTriggerBase(RelicData data, RelicContext relicContext, IRelicEffect effect)
    {
        this.data = data;
        this.relicContext = relicContext;
        this.effect = effect;
    }

    public abstract void Activate();
    public abstract void Deactivate();
}