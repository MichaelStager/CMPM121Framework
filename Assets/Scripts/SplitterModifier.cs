using UnityEngine;
using System.Collections;

public class SplitterModifier : SpellModifier
{
    private readonly int extraProjectilesPerSide; // e.g. 1 => total 3 shots (left, center, right)
    private readonly float angleStepDegrees;      // e.g. 15f => -15, 0, +15
    private readonly float manaMultiplier;

    public SplitterModifier(
        ISpell inner,
        int extraProjectilesPerSide = 1,
        float angleStepDegrees = 15f,
        float manaMultiplier = 1.5f
    ) : base(inner)
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

    public override IEnumerator Cast(Vector3 where, Vector3 target, Hittable.Team team)
    {
        LastCast = Time.time;

        Vector3 baseDir = (target - where);
        if (baseDir.sqrMagnitude < 0.0001f)
            baseDir = Vector3.right; // fallback

        int resolvedDamage = GetDamage();
        float speed = GetProjectileSpeed();

        // fire from -N ... 0 ... +N
        for (int i = -extraProjectilesPerSide; i <= extraProjectilesPerSide; i++)
        {
            Vector3 dir = Rotate2D(baseDir.normalized, i * angleStepDegrees);

            GameManager.Instance.projectileManager.CreateProjectile(
                0,
                GetProjectileTrajectory(),
                where,
                dir,      // direction
                speed,      // speed
                (other, impact) =>
                {
                    if (other.team != team)
                    {
                        other.Damage(new Damage(resolvedDamage, Damage.Type.ARCANE));
                    }
                }
            );
        }

        yield return new WaitForEndOfFrame();
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