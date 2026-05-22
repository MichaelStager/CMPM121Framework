using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectorController : MonoBehaviour
{
    public TextMeshProUGUI label;
    public Image background;

    private string classId;
    private WaveManager waveManager;

    public void SetCharacter(string newClassId, WaveManager newWaveManager)
    {
        classId = newClassId;
        waveManager = newWaveManager;
        label.text = classId;
    }

    public void SelectCharacter()
    {
        waveManager.SelectCharacter(classId);
    }

    public void SetSelected(bool selected)
    {
        background.color = selected
            ? new Color(0.45f, 0.45f, 0.45f, 1f)
            : Color.white;
    }

    public string ClassId
    {
        get { return classId; }
    }
}