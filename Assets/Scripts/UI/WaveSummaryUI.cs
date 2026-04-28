using UnityEngine;
using UnityEngine.UI;

public class WaveSummaryUI : MonoBehaviour
{
    public GameObject panel;
    public Text titleText;
    public Text statsText;
    public Button continueButton;

    private EnemySpawner spawner;

    void Start()
    {
        panel.SetActive(false);

        continueButton.onClick.AddListener(OnContinueClicked);
    }

    public void SetSpawner(EnemySpawner enemySpawner)
    {
        spawner = enemySpawner;
    }

    public void Show(WaveStats stats)
    {
        panel.SetActive(true);

        titleText.text = "Wave " + stats.waveNumber + " Complete!";

        statsText.text =
            "Enemies defeated: " + stats.enemiesKilled + "\n" +
            "Damage dealt: " + stats.damageDealt + "\n" +
            "Damage taken: " + stats.damageTaken + "\n" +
            "Time spent: " + stats.TimeSpent.ToString("0.0") + " seconds";
    }

    public void Hide()
    {
        panel.SetActive(false);
    }

    private void OnContinueClicked()
    {
        Hide();

        if (spawner != null)
        {
            spawner.NextWave();
        }
    }
}