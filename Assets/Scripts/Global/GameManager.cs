using UnityEngine;

public class GameManager : MonoBehaviour
{
    private InputReader _input;
    private GameObject pauseMenu;

    private bool _isPaused = false;

    private void Start()
    {
        _input.PauseEvent += HandlePause;
        _input.ResumeEvent += HandleResume;
    }

    private void HandlePause()
    {
        pauseMenu.SetActive(true);
        _isPaused = true;
    }

    private void HandleResume()
    {
        pauseMenu.SetActive(false);
        _isPaused = false;
    }
}
