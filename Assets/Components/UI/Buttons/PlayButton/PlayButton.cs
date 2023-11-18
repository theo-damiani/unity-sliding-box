using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PlayButton : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private Sprite playIcon;
    [SerializeField] private Sprite pauseIcon;
    [SerializeField] private UnityEvent OnPlayEventList;
    [SerializeField] private UnityEvent OnPauseEventList;

    private bool isPaused = false;
    private bool _oldIsPaused = false;

    public void Play()
    {
        isPaused = false;
        if (icon) icon.sprite = pauseIcon;

        OnPlayEventList?.Invoke();
    }

    public void PlayWithoutRaising()
    {
        isPaused = false;
        if (icon) icon.sprite = pauseIcon;
    }

    public void Pause()
    {
        if (isPaused)
        {
            return;
        }
        isPaused = true;
        if (icon) icon.sprite = playIcon;

        OnPauseEventList?.Invoke();
    }

    public void TogglePlayPause()
    {
        if (isPaused)
        {
            _oldIsPaused = true;
            Play();
        }
        else
        {
            _oldIsPaused = false;
            Pause();
        }
    }

    public void RestoreState()
    {
        if (_oldIsPaused)
        {
            Pause();
        }
        else
        {
            Play();
        }
    }
}
