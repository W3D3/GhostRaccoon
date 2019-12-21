using TMPro;
using UnityEngine;

public class PlayerInfoScript : MonoBehaviour
{
    private TextMeshProUGUI _text;

    public Color Player1Color;
    public Color Player2Color;

    public int ActivePlayer;

    // Start is called before the first frame update
    void Start()
    {
        _text = GetComponent<TextMeshProUGUI>();

        _text.text = "Raccoon 1";
        _text.color = Player1Color;
    }

    public void SetActivePlayer(int player)
    {
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
}
