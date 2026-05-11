using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WaveSummaryUI : MonoBehaviour
{
    public GameObject panel;
    public TMP_Text titleText;
    public TMP_Text statsText;
    public Button continueButton;

    private WaveManager spawner;

    public PlayerController player;

    void Start()
    {
        panel.SetActive(false);
        continueButton.onClick.AddListener(OnContinueClicked);
    }

    public void SetSpawner(WaveManager enemySpawner)
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
            "Time spent: " + stats.TimeSpent.ToString("0.0") + " seconds\n\n" +
            "Reward: Arcane Blast";
    }

    public void Hide()
    {
        panel.SetActive(false);
    }

    private void OnContinueClicked()
    {
        Hide();

        GiveSpellReward();
        if (spawner != null)
        {
            spawner.NextWave();
        }
    }

    private void GiveSpellReward()
    {
        if (player == null || player.spellcaster == null)
        {
            return;
        }

        ISpell rewardSpell = new SpellBuilder().Build(player.spellcaster);
        bool added = player.spellcaster.AddSpell(rewardSpell);

        if (added && player.spellui != null)
        {
            player.spellui.Refresh();
        }
    }
}