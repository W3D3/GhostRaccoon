using UnityEngine;

public class PlayGameScript : MonoBehaviour
{
    private GamepadInput _inputHandler;

        // Start is called before the first frame update
    void Start()
    {
        _inputHandler = GetComponent<GamepadInput>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_inputHandler.IsJumpPressed())
        {
            Debug.Log("Start demo level");
        }
    }
}
