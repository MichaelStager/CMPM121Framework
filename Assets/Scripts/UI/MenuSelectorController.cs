using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MenuSelectorController : MonoBehaviour
{
    public TextMeshProUGUI label;
    public Level level;
    public WaveManager spawner;
    
    public void SetLevel(Level selectedlevel)
    {
        level = selectedlevel;
        label.text = selectedlevel.name;
    }

    public void StartLevel()
    {
        spawner.StartLevel(level);
    }
}
