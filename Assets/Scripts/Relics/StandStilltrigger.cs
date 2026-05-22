using UnityEngine;

public class StandStillTrigger : RelicTriggerBase
{
    private float timer;
    private float requiredSeconds;
    private bool currentlyStill;

    public StandStillTrigger(RelicData data, RelicContext ctx, IRelicEffect effect)
        : base(data, ctx, effect)
    {
        timer = 0f;
        currentlyStill = false;
        requiredSeconds = 3f;

        if (data != null && data.trigger != null && !string.IsNullOrWhiteSpace(data.trigger.amount))
        {
            float parsed;
            if (float.TryParse(data.trigger.amount, out parsed))
            {
                requiredSeconds = parsed;
            }
        }
    }

    public override void Activate()
    {
        EventBus.Instance.OnPlayerMoveInput += OnMoveInput;
        CoroutineManager.Instance.Run(TickRoutine());
    }

    public override void Deactivate()
    {
        EventBus.Instance.OnPlayerMoveInput -= OnMoveInput;
        // CoroutineManager cleanup style may vary; simple guard by disabling with bool if needed.
        isActive = false;
    }

    private bool isActive = true;

    private void OnMoveInput(Vector2 movement)
    {
        bool isStillNow = movement.sqrMagnitude <= 0.0001f;

        if (!isStillNow)
        {
            timer = 0f;
            currentlyStill = false;
            effect.OnEvent("move"); // for "until move"
            return;
        }

        currentlyStill = true;
    }

    private System.Collections.IEnumerator TickRoutine()
    {
        isActive = true;

        while (isActive)
        {
            if (currentlyStill)
            {
                timer += Time.deltaTime;
                if (timer >= requiredSeconds)
                {
                    effect.Trigger();
                    timer = 0f; // prevents per-frame spam while remaining still
                }
            }

            yield return null;
        }
    }
}