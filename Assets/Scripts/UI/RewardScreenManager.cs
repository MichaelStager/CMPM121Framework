using UnityEngine;

public class RewardScreenManager : MonoBehaviour
{
    [Header("Root Panels")]
    public GameObject rewardUIRoot;
    public GameObject spellRewardPanel;  
    public GameObject relicRewardPanel;  

    [Header("Managers")]
    public RelicRewardManager relicRewardManager;

    [Header("References")]
    public PlayerController player;
    public WaveManager waveManager; 

    private bool rewardsActive = false;
    private bool spellRewardDone = false;
    private bool relicRewardDone = false;
    private int rewardWave = 1;

    public WaveSummaryUI waveSummaryUI;

    // Call this when entering wave-end reward phase.
    public void BeginWaveEndRewards(int waveNumber)
    {
        rewardWave = waveNumber;
        rewardsActive = true;

        spellRewardDone = false;
        relicRewardDone = (waveNumber % 3 != 0); 

        rewardUIRoot.SetActive(true);

        // Start with spell reward every wave.
        ShowSpellRewardUI();

    }

    private void ShowSpellRewardUI()
    {
        spellRewardPanel.SetActive(true);
        relicRewardPanel.SetActive(false);

        // TODO: keep your existing spell reward setup logic here.
        // Example: spellRewardGenerator.BuildChoices();
    }

    private void ShowRelicRewardUI()
    {
        spellRewardPanel.SetActive(false);
        relicRewardPanel.SetActive(true);

        relicRewardManager.player = player;
        relicRewardManager.OpenChoicesForWave(rewardWave);

        // TODO: Bind relicRewardManager.GetCurrentChoices() to your 3 relic UI cards/buttons.
    }

    // Hook this to your EXISTING spell reward "choice picked" callback.
    public void OnSpellRewardChosen()
    {
        if (!rewardsActive) return;

        spellRewardDone = true;
        TryAdvanceRewardFlow();
    }

    // Hook each relic button to pass 0, 1, or 2.
    public void OnRelicChosen(int choiceIndex)
    {
        if (!rewardsActive) return;

        relicRewardManager.ChooseIndex(choiceIndex);
        relicRewardDone = true;

        TryAdvanceRewardFlow();
    }

    private void TryAdvanceRewardFlow()
    {
        if (!spellRewardDone)
        {
            return;
        }

        // If relic reward still pending for this wave, show it now.
        if (!relicRewardDone)
        {
            ShowRelicRewardUI();
            return;
        }

        // Both done -> close rewards + continue.
        CompleteWaveEndAndContinue();
    }

    private void CompleteWaveEndAndContinue()
    {
        rewardsActive = false;

        spellRewardPanel.SetActive(false);
        relicRewardPanel.SetActive(false);
        rewardUIRoot.SetActive(false);

        if (waveSummaryUI != null)
        {
            waveSummaryUI.OnRelicFlowCompleteContinue();
        }
        else
        {
            Debug.LogWarning("[Rewards] Missing WaveSummaryUI reference; cannot continue to next wave.");
        }

        Debug.Log("[Rewards] Completed spell + relic flow for wave " + rewardWave);
    }
}