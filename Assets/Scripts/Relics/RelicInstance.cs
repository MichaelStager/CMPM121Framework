using System.Collections.Generic;
using UnityEngine;

public class RelicInstance
{
    private readonly RelicData data;
    private readonly PlayerController player;

    private bool effectActive;
    private int activeSpellPowerBonus;
    private float standStillTimer;

    public RelicInstance(RelicData relicData, PlayerController owner)
    {
        data = relicData;
        player = owner;
        effectActive = false;
        activeSpellPowerBonus = 0;
        standStillTimer = 0f;
    }

    public RelicData GetData()
    {
        return data;
    }

    public bool IsActive()
    {
        return effectActive;
    }

    public string GetLabel()
    {
        if (data == null)
        {
            return "Relic (invalid)";
        }

        if (effectActive)
        {
            return data.name + " (active)";
        }

        return data.name;
    }

    /// <summary>
    /// Call this once per frame from PlayerController.Update.
    /// movementMagnitude should be owner unit movement magnitude.
    /// </summary>
    public void Tick(float dt, float movementMagnitude, Dictionary<string, int> vars)
    {
        if (data == null || data.trigger == null)
        {
            return;
        }

        // Stand-still trigger: trigger once when timer crosses threshold.
        if (data.trigger.type == "stand-still")
        {
            if (movementMagnitude <= 0.01f)
            {
                standStillTimer += dt;

                float threshold = ParseFloatOrDefault(data.trigger.amount, 3f);
                if (standStillTimer >= threshold)
                {
                    TryActivateEffect(vars);
                    // prevent re-firing every frame while still
                    standStillTimer = 0f;
                }
            }
            else
            {
                standStillTimer = 0f;

                // Handle "until move" deactivation.
                if (effectActive && data.effect != null && data.effect.until == "move")
                {
                    DeactivateTemporaryEffect();
                }
            }
        }
    }

    /// <summary>
    /// Call when player takes damage.
    /// </summary>
    public void OnPlayerDamaged(Dictionary<string, int> vars)
    {
        if (data == null || data.trigger == null) return;
        if (data.trigger.type == "take-damage")
        {
            TryActivateEffect(vars);
        }
    }

    /// <summary>
    /// Call when player kills an enemy.
    /// </summary>
    public void OnEnemyKilled(Dictionary<string, int> vars)
    {
        if (data == null || data.trigger == null) return;
        if (data.trigger.type == "on-kill")
        {
            TryActivateEffect(vars);
        }
    }

    /// <summary>
    /// Call when player casts a spell (after successful cast).
    /// </summary>
    public void OnSpellCast()
    {
        if (!effectActive || data == null || data.effect == null) return;

        if (data.effect.until == "cast-spell")
        {
            DeactivateTemporaryEffect();
        }
    }

    private void TryActivateEffect(Dictionary<string, int> vars)
    {
        if (data == null || data.effect == null || player == null || player.spellcaster == null)
        {
            return;
        }

        string effectType = data.effect.type;
        int amount = EvaluateAmount(data.effect.amount, vars);

        if (effectType == "gain-mana")
        {
            player.spellcaster.mana = Mathf.Min(
                player.spellcaster.mana + amount,
                player.spellcaster.max_mana
            );
            return;
        }

        if (effectType == "gain-spellpower")
        {
            // If temporary and already active, do not stack repeatedly.
            if (HasUntilCondition() && effectActive)
            {
                return;
            }

            player.spellcaster.spellPower += amount;

            if (HasUntilCondition())
            {
                effectActive = true;
                activeSpellPowerBonus = amount;
            }
        }
    }

    private void DeactivateTemporaryEffect()
    {
        if (!effectActive || player == null || player.spellcaster == null)
        {
            return;
        }

        player.spellcaster.spellPower -= activeSpellPowerBonus;
        activeSpellPowerBonus = 0;
        effectActive = false;
    }

    private bool HasUntilCondition()
    {
        return data != null
            && data.effect != null
            && !string.IsNullOrWhiteSpace(data.effect.until);
    }

    private int EvaluateAmount(string amountExpr, Dictionary<string, int> vars)
    {
        if (string.IsNullOrWhiteSpace(amountExpr))
        {
            return 0;
        }

        if (int.TryParse(amountExpr, out int numeric))
        {
            return numeric;
        }

        try
        {
            return RPNEvaluator.RPNEvaluator.Evaluate(amountExpr, vars);
        }
        catch
        {
            Debug.LogWarning("Failed to evaluate relic amount expression: " + amountExpr);
            return 0;
        }
    }

    private float ParseFloatOrDefault(string s, float fallback)
    {
        if (string.IsNullOrWhiteSpace(s))
        {
            return fallback;
        }

        if (float.TryParse(s, out float value))
        {
            return value;
        }

        return fallback;
    }
}