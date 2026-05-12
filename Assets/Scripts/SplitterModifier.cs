using UnityEngine;
using System.Collections.Generic;

public class SplitterModifier : SpellModifier
{
    private readonly int extraProjectilesPerSide;
    private readonly float angleStepDegrees;
    private readonly float manaMultiplier;

    public SplitterModifier(ISpell inner, int extraProjectilesPerSide = 1, float angleStepDegrees = 15f, float manaMultiplier = 1.5f)
        : base(inner)
    {
        this.extraProjectilesPerSide = Mathf.Max(0, extraProjectilesPerSide);
        this.angleStepDegrees = angleStepDegrees;
        this.manaMultiplier = manaMultiplier;
    }

    public override int GetManaCost()
    {
        return Mathf.RoundToInt(inner.GetManaCost() * manaMultiplier);
    }

    public override string GetName()
    {
        return "Split " + inner.GetName();
    }

    public override IEnumerable<Vector3> GetShotDirections(Vector3 baseDirection)
    {
        if (baseDirection.sqrMagnitude < 0.0001f)
            baseDirection = Vector3.right;

        baseDirection = baseDirection.normalized;

        for (int i = -extraProjectilesPerSide; i <= extraProjectilesPerSide; i++)
        {
            yield return Rotate2D(baseDirection, i * angleStepDegrees);
        }
    }

    private Vector3 Rotate2D(Vector3 v, float degrees)
    {
        float rad = degrees * Mathf.Deg2Rad;
        float cos = Mathf.Cos(rad);
        float sin = Mathf.Sin(rad);

        return new Vector3(
            v.x * cos - v.y * sin,
            v.x * sin + v.y * cos,
            v.z
        );
    }
}