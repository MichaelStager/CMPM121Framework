using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class WaveSummaryUI : MonoBehaviour
{
    public Image rewardIcon;
    public TMP_Text rewardNameText;
    public Button acceptSpellButton;
    public TMP_Text acceptSpellButtonText;
    private ISpell pendingRewardSpell;

    public GameObject relicChoicesRoot;
    public Button[] relicButtons;
    public Image[] relicIcons;
    public TMP_Text[] relicNames;
    public TMP_Text[] relicDescriptions;

    private List<RelicData> pendingRelicChoices = new List<RelicData>();
    private bool relicChoiceRequired;
    private bool relicChosen;
    private int currentWaveNumber;

    public GameObject panel;
    public TMP_Text titleText;
    public TMP_Text statsText;
    public Button continueButton;

    private WaveManager spawner;
    private bool rewardGiven;

    public PlayerController player;

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

        currentWaveNumber = stats.waveNumber;
        relicChosen = false;
        relicChoiceRequired = stats.waveNumber >= 3 && stats.waveNumber % 3 == 0;

        if (relicChoicesRoot != null)
        {
            relicChoicesRoot.SetActive(relicChoiceRequired);
        }

        if (relicChoiceRequired)
        {
            BuildRelicChoices(stats.waveNumber);
        }

        UpdateAcceptButton();
    }


    public void Hide()
    {
        panel.SetActive(false);
    }

    private void OnContinueClicked()
    {
        if (relicChoiceRequired && !relicChosen)
        {
            return;
        }

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




    private void BuildRelicChoices(int waveNumber)
    {
        pendingRelicChoices.Clear();

        FindPlayerIfNeeded();
        if (player == null) return;

        player.SetRelicWave(waveNumber);

        List<RelicData> all = RelicLoader.GetAll();
        HashSet<string> owned = new HashSet<string>(
            player.relics
                .Where(r => r != null && r.Data != null)
                .Select(r => r.Data.name)
        );

        List<RelicData> pool = all
            .Where(r => r != null && !owned.Contains(r.name))
            .ToList();

        int count = Mathf.Min(3, pool.Count);

        for (int i = 0; i < count; i++)
        {
            int index = Random.Range(0, pool.Count);
            pendingRelicChoices.Add(pool[index]);
            pool.RemoveAt(index);
        }

        for (int i = 0; i < relicButtons.Length; i++)
        {
            bool hasChoice = i < pendingRelicChoices.Count;
            relicButtons[i].gameObject.SetActive(hasChoice);

            if (!hasChoice) continue;

            RelicData relic = pendingRelicChoices[i];
            relicNames[i].text = relic.name;
            relicDescriptions[i].text = relic.trigger.description + ", " + relic.effect.description;
            GameManager.Instance.relicIconManager.PlaceSprite(relic.sprite, relicIcons[i]);

            int choiceIndex = i;
            relicButtons[i].onClick.RemoveAllListeners();
            relicButtons[i].onClick.AddListener(() => ChooseRelic(choiceIndex));
        }
    }


    private void ChooseRelic(int index)
    {
        if (!relicChoiceRequired || relicChosen) return;
        if (index < 0 || index >= pendingRelicChoices.Count) return;

        if (player.AddRelic(pendingRelicChoices[index]))
        {
            relicChosen = true;

            if (relicChoicesRoot != null)
            {
                relicChoicesRoot.SetActive(false);
            }
        }
    }


}
