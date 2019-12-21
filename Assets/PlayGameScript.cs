using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayGameScript : MonoBehaviour
{
    private GamepadInput _inputHandler;

    public string FirstLevel;
    
    void Start()
    {
        _inputHandler = GetComponent<GamepadInput>();
    }
    
    void Update()
    {
        if (_inputHandler.IsJumpPressed())
        {
            Debug.Log("Start first level");
            SceneManager.LoadScene(FirstLevel);
        }
    }
}
