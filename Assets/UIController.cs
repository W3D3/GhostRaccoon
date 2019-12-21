using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    public GameObject PressText;
    public GameObject AButton;
    public GameObject ContinueText;
    public GameObject RestartText;

    public GamepadInput InputController;

    public string NextLevelName;

    private bool _isGameOver = false;
    private bool _isWinning = false;

    private TextMeshProUGUI _text;

    public Color Player1Color;
    public Color Player2Color;

    public int ActivePlayer;
    
    void Start()
    {
        _text = GetComponentInChildren<TextMeshProUGUI>();

        _text.text = "Raccoon 1";
        _text.color = Player1Color;
    }
    
    void Update()
    {
        if (InputController.IsJumpPressed())
        {
            if (_isGameOver)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
            else if (_isWinning)
            {
                SceneManager.LoadScene(NextLevelName);
            }
        }
    }

    public void SetActivePlayer(int player)
    {
        if (_text == null)
            return;

        ActivePlayer = player;
        if (player == 0)
        {
            _text.text = "Raccoon 1";
            _text.color = Player1Color;
        }
        else
        {
            _text.text = "Raccoon 2";
            _text.color = Player2Color;
        }
    }

    public void DisplayGameOver()
    {
        PressText.SetActive(true);
        AButton.SetActive(true);
        RestartText.SetActive(true);
        _isGameOver = true;
    }

    public void DisplayNextLevel()
    {
        PressText.SetActive(true);
        AButton.SetActive(true);
        ContinueText.SetActive(true);
        _isWinning = true;
    }
}
