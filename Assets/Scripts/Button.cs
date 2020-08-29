using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    public enum ButtonState {
        Unselected,
        Selected,
    }

    public enum ButtonType {
        None,
        MoveType_Teleport,
        MoveType_Joystick,
        Resume,
        Quit,
        HandType_Light,
        HandType_Med,
        HandType_Dark,
        Subtitles_Enabled,
        Subtitles_Disabled,
    }
    public GameObject m_selector;

    public ButtonType m_buttonType = ButtonType.None;

    protected bool m_highlighted = false;

    private float m_scaleFactor = 1.2f;

    private ButtonState m_state = ButtonState.Unselected;

    public virtual void Highlight () {
        this.transform.localScale *= m_scaleFactor;
        m_highlighted = true;
        if (PauseMenu.m_pauseMenu.highlightedButton != null)
        {
            PauseMenu.m_pauseMenu.highlightedButton.Unhighlight();
        }
        PauseMenu.m_pauseMenu.highlightedButton = this;
    }

    public virtual void Unhighlight () {
        this.transform.localScale /= m_scaleFactor;
        m_highlighted = false;
        PauseMenu.m_pauseMenu.highlightedButton = null;
    }

    public virtual void Select () {
        m_state = ButtonState.Selected;
        if (m_selector != null) {
            m_selector.SetActive(true);
        }

        switch (m_buttonType)
        {
            case ButtonType.MoveType_Teleport:
                Player.m_player.m_locomotionType = Player.LocomotionType.Teleport;
            break;
            case ButtonType.MoveType_Joystick:
                Player.m_player.m_locomotionType = Player.LocomotionType.Translate;
            break;
            case ButtonType.Resume:
                Player.m_player.SetPauseState();
            break;
            case ButtonType.Quit:
                Application.Quit();
            break;
            case ButtonType.HandType_Light:
            Player.m_player.CycleHandColor(Player.HandType.Light);
            break;
            case ButtonType.HandType_Med:
            Player.m_player.CycleHandColor(Player.HandType.Med);
            break;
            case ButtonType.HandType_Dark:
            Player.m_player.CycleHandColor(Player.HandType.Dark);
            break;
            case ButtonType.Subtitles_Enabled:
            SubtitleManager.m_subtitleManager.subtitleState = SubtitleManager.Subtitles.Enabled;
            break;
            case ButtonType.Subtitles_Disabled:
            SubtitleManager.m_subtitleManager.subtitleState = SubtitleManager.Subtitles.Disabled;
            break;
            
        }
    }

    public void Deselect () {
        m_state = ButtonState.Unselected;
        if (m_selector != null) {
            m_selector.SetActive(false);
        }
    }
    // Start is called before the first frame update

}
