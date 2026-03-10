using UnityEngine;
using TMPro;

[RequireComponent(typeof(TMP_Text))]
public class DebugInfo : MonoBehaviour
{
    [SerializeField] private GameObject _player;
    private TMP_Text text;

    public void Start()
    {
        text = GetComponent<TMP_Text>();
    }
    public void Update()
    {
        SetText();
    }

    public void SetText()
    {
        text.text = "Debug Info";
        text.text += $"\nPlayer State: {_player.GetComponent<PlayerMovementStateMachine>().CurrentStateInstance}";
        text.text += $"\nInput Dir: {_player.GetComponent<PlayerController>().InputDir}";
        text.text += $"\n Is On Ground: {_player.GetComponent<PlayerMovementStateMachine>().IsOnGround()}";
    }
}
