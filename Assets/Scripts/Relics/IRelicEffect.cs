public interface IRelicEffect
{
    // Called when trigger condition occurs.
    void Trigger();

    // Optional lifecycle callback for temporary effects.
    void OnEvent(string eventType);
}