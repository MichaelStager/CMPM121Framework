using UnityEngine;
using System.Collections.Generic;

public class SpellUIContainer : MonoBehaviour
{
    public GameObject[] spellUIs;
    public PlayerController player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Refresh();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetSpells(List<ISpell> spells, int currentIndex)
    {
        for (int i = 0; i < spellUIs.Length; i++)
        {
            if (i < spells.Count)
            {
                spellUIs[i].SetActive(true);

                SpellUI spellUI = spellUIs[i].GetComponent<SpellUI>();
                spellUI.SetSpell(spells[i]);

                if (spellUI.highlight != null)
                {
                    spellUI.highlight.SetActive(i == currentIndex);
                }
            }
            else
            {
                spellUIs[i].SetActive(false);
            }
        }
    }

    public void Refresh()
    {
        if (player == null || player.spellcaster == null)
        {
            for (int i = 0; i < spellUIs.Length; i++)
            {
                spellUIs[i].SetActive(false);
            }

            return;
        }

        SetSpells(player.spellcaster.spells, player.spellcaster.currentSpellIndex);
    }
}
