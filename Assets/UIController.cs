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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (InputController.IsJumpPressed())
        {
            if (_isGameOver)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            } else if (_isWinning)
            {
                SceneManager.LoadScene(NextLevelName);
            }
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
