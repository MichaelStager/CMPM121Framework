public abstract class RelicTriggerBase : IRelicTrigger
{
    protected readonly RelicContext ctx;
    protected readonly IRelicEffect effect;
    protected readonly RelicData data;

    protected RelicTriggerBase(RelicData data, RelicContext ctx, IRelicEffect effect)
    {
        this.data = data;
        this.ctx = ctx;
        this.effect = effect;
    }

    public abstract void Activate();
    public abstract void Deactivate();
}