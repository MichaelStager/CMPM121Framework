using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RelicRewardManager : MonoBehaviour
{
    public PlayerController player;

    // Hook these up to your 3 UI option buttons/cards
    private List<RelicData> currentChoices = new List<RelicData>();
    private bool awaitingChoice = false;

    public bool ShouldOfferRelics(int waveNumber)
    {
        return waveNumber >= 3 && waveNumber % 3 == 0;
    }

    public void OpenChoicesForWave(int waveNumber)
    {
        if (player == null) return;

        player.SetRelicWave(waveNumber);

        List<RelicData> all = RelicLoader.GetAll();
        HashSet<string> owned = new HashSet<string>(
            player.relics
                .Where(r => r != null && r.Data != null)
                .Select(r => r.Data.name)
        );

        List<RelicData> pool = all.Where(r => r != null && !owned.Contains(r.name)).ToList();

        currentChoices.Clear();
        int count = Mathf.Min(3, pool.Count);

        for (int i = 0; i < count; i++)
        {
            int idx = Random.Range(0, pool.Count);
            currentChoices.Add(pool[idx]);
            pool.RemoveAt(idx);
        }

        awaitingChoice = currentChoices.Count > 0;

        // TODO: bind currentChoices[0..2] to your UI cards/buttons
        // card title = relic.name
        // description = $"{relic.trigger.description}, {relic.effect.description}"
        // icon = relic.sprite
        Debug.Log("[RelicReward] Opened with " + currentChoices.Count + " choices.");
    }

    // Wire these to your 3 option buttons.
    public void ChooseIndex(int index)
    {
        if (!awaitingChoice) return;
        if (index < 0 || index >= currentChoices.Count) return;

        RelicData picked = currentChoices[index];
        bool added = player.AddRelic(picked);

        Debug.Log("[RelicReward] Picked " + picked.name + " added=" + added);

        awaitingChoice = false;
        currentChoices.Clear();

        // TODO: hide reward panel and continue wave flow
    }

    public List<RelicData> GetCurrentChoices()
    {
        return currentChoices;
    }
}