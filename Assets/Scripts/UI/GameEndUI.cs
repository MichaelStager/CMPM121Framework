using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameEndUI : MonoBehaviour
{
    public GameObject panel;
    public TMP_Text titleText;
    public TMP_Text messageText;
    public Button returnButton;

    private void Start()
    {
        panel.SetActive(false);
        returnButton.onClick.AddListener(ReturnToStart);
    }

    public void ShowWin()
    {
        GameManager.Instance.state = GameManager.GameState.GAMEOVER;

        panel.SetActive(true);
        titleText.text = "You Win!";
        messageText.text = "You survived every wave. Nice work!";
    }

    public void ShowLoss()
    {
        GameManager.Instance.state = GameManager.GameState.GAMEOVER;

        panel.SetActive(true);
        titleText.text = "You Died!";
        messageText.text = "The enemies overwhelmed you.";
    }

    public void ReturnToStart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}