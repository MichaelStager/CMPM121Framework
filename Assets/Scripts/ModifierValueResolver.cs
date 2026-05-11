using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public static class ModifierValueResolver
{
    public static float EvalFloat(
        string expr,
        Dictionary<string, float> vars,
        float fallback = 0f)
    {
        if (string.IsNullOrWhiteSpace(expr))
            return fallback;

        // Fast path: plain numeric strings like "1.5" or "10"
        if (float.TryParse(expr, NumberStyles.Float, CultureInfo.InvariantCulture, out float parsed))
            return parsed;

        try
        {
            return RPNEvaluator.RPNEvaluator.Evaluatef(expr, vars ?? new Dictionary<string, float>());
        }
        catch (Exception e)
        {
            Debug.LogWarning($"Failed to evaluate expression '{expr}': {e.Message}");
            return fallback;
        }
    }
}