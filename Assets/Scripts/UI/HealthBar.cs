using UnityEngine;
using UnityEngine.UIElements;

public class HealthBar : MonoBehaviour
{
    public GameObject slider;

    public Hittable hp;

    float old_perc = -1f;

    void Update()
    {
        if (hp == null) return;

        UpdateBar();
    }

    public void SetHealth(Hittable newHp)
    {
        hp = newHp;

        if (hp == null)
        {
            Debug.LogError("HealthBar was given a null Hittable.");
            return;
        }

        UpdateBar();
    }

    void UpdateBar()
    {
        if (slider == null)
        {
            Debug.LogError("HealthBar is missing its slider object.");
            return;
        }

        if (hp.max_hp <= 0)
        {
            Debug.LogError($"HealthBar cannot display health because max_hp is {hp.max_hp} on {gameObject.name}.");
            return;
        }

        float perc = hp.hp * 1.0f / hp.max_hp;

        // Keeps the health bar between 0% and 100%
        perc = Mathf.Clamp01(perc);
        if (Mathf.Abs(old_perc - perc) > 0.01f)
        {
            slider.transform.localScale = new Vector3(perc, 1, 1);
            slider.transform.localPosition = new Vector3(-(1 - perc) / 2, 0, 0);
            old_perc = perc;
        }
    }


}
