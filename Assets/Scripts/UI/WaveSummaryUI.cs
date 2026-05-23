using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WaveSummaryUI : MonoBehaviour
{
    public Image rewardIcon;
    public TMP_Text rewardNameText;
    public Button acceptSpellButton;
    public TMP_Text acceptSpellButtonText;
    private ISpell pendingRewardSpell;

    public GameObject panel;
    public TMP_Text titleText;
    public TMP_Text statsText;
    public Button continueButton;

    private WaveManager spawner;
    private bool rewardGiven;

    public PlayerController player;

    [Header("Relic Reward")]
    public RewardScreenManager rewardScreenManager;
    private int shownWaveNumber;
    private bool waitingOnRelicChoice;

    void Start()
    {
        if (panel != null)
        {
            panel.SetActive(false);
        }

        if (continueButton != null)
        {
            continueButton.onClick.AddListener(OnContinueClicked);
        }

        if (acceptSpellButton != null)
        {
            acceptSpellButton.onClick.AddListener(OnAcceptSpellClicked);
        }
    }

    public void SetSpawner(WaveManager enemySpawner)
    {
        spawner = enemySpawner;
    }

    public void Show(WaveStats stats)
    {
        rewardGiven = false;
        shownWaveNumber = stats.waveNumber;
        waitingOnRelicChoice = false;
        FindPlayerIfNeeded();

        pendingRewardSpell = new SpellBuilder().BuildRandom(player.spellcaster, 2);
        panel.SetActive(true);

        titleText.text = "Wave " + stats.waveNumber + " Complete!";

        statsText.text =
            "Enemies defeated: " + stats.enemiesKilled + "\n" +
            "Damage dealt: " + stats.damageDealt + "\n" +
            "Damage taken: " + stats.damageTaken + "\n" +
            "Time spent: " + stats.TimeSpent.ToString("0.0") + " seconds";

        rewardNameText.text = pendingRewardSpell.GetName();
        GameManager.Instance.spellIconManager.PlaceSprite(pendingRewardSpell.GetIcon(),rewardIcon);

        UpdateAcceptButton();
    }

    public void Hide()
    {
        panel.SetActive(false);
    }

    private void OnContinueClicked()
    {
        // If this is a relic wave and we have a reward manager, open relic reward first.
        bool isRelicWave = shownWaveNumber >= 3 && shownWaveNumber % 3 == 0;

        if (isRelicWave && rewardScreenManager != null && !waitingOnRelicChoice)
        {
            waitingOnRelicChoice = true;

            // Keep wave summary hidden while relic choice is shown.
            Hide();

            // Open reward flow at this exact wave number.
            rewardScreenManager.BeginWaveEndRewards(shownWaveNumber);
            return;
        }

        // Non-relic wave OR relic flow already completed -> continue normally.
        Hide();

        if (spawner != null)
        {
            spawner.NextWave();
        }
    }

    private void GiveSpellReward()
    {
        if (rewardGiven)
        {
            return;
        }

        if (player == null && GameManager.Instance.player != null)
        {
            player = GameManager.Instance.player.GetComponent<PlayerController>();
        }

        if (player == null || player.spellcaster == null)
        {
            return;
        }

        ISpell rewardSpell = new SpellBuilder().BuildRandom(player.spellcaster, 2);
        bool added = player.spellcaster.AddSpell(rewardSpell);
        rewardGiven = added;

        if (added && player.spellui != null)
        {
            player.spellui.Refresh();
        }
    }

    private void OnAcceptSpellClicked()
    {
        if (rewardGiven)
        {
            return;
        }

        FindPlayerIfNeeded();

        if (player == null || player.spellcaster == null || pendingRewardSpell == null)
        {
            return;
        }

        if (player.spellcaster.IsInventoryFull())
        {
            UpdateAcceptButton();
            return;
        }

        bool added = player.spellcaster.AddSpell(pendingRewardSpell);
        rewardGiven = added;

        if (added)
        {
            pendingRewardSpell = null;

            if (player.spellui != null)
            {
                player.spellui.Refresh();
            }
        }

        UpdateAcceptButton();
    }

    private void UpdateAcceptButton()
    {
        FindPlayerIfNeeded();

        if (acceptSpellButton == null || acceptSpellButtonText == null)
        {
            return;
        }

        if (rewardGiven)
        {
            acceptSpellButton.interactable = false;
            acceptSpellButtonText.text = "Accepted";
            return;
        }

        if (player != null && player.spellcaster != null && player.spellcaster.IsInventoryFull())
        {
            acceptSpellButton.interactable = false;
            acceptSpellButtonText.text = "Inventory Full";
            return;
        }

        acceptSpellButton.interactable = true;
        acceptSpellButtonText.text = "Accept Spell";
    }

    private void FindPlayerIfNeeded()
    {
        if (player == null && GameManager.Instance.player != null)
        {
            player = GameManager.Instance.player.GetComponent<PlayerController>();
        }
    }

    public void RefreshRewardState()
    {
        UpdateAcceptButton();
    }

    public void OnRelicFlowCompleteContinue()
    {
        waitingOnRelicChoice = false;

        if (spawner != null)
        {
            spawner.NextWave();
        }
    }
}
