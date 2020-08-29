using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubtitleButton : Button
{
    public Button m_linkedButton;
    void Start () {
        if (m_buttonType == ButtonType.Subtitles_Enabled && SubtitleManager.m_subtitleManager.subtitleState == SubtitleManager.Subtitles.Enabled) {
            Highlight();
        } else if  (m_buttonType == ButtonType.Subtitles_Disabled && SubtitleManager.m_subtitleManager.subtitleState == SubtitleManager.Subtitles.Disabled){
            Highlight();
        } else {
            Unhighlight();
        }
    }

    public override void Highlight() {
        m_selector.SetActive(false);
        
    }

    public override void Unhighlight() {
        m_selector.SetActive(true);
    }

    public override void Select() {
        switch (m_buttonType)
        {
            case ButtonType.Subtitles_Enabled:
            SubtitleManager.m_subtitleManager.subtitleState = SubtitleManager.Subtitles.Enabled;
            break;
            case ButtonType.Subtitles_Disabled:
            SubtitleManager.m_subtitleManager.subtitleState = SubtitleManager.Subtitles.Disabled;
            break;
            
        }
    }

    void OnTriggerEnter (Collider other) {
        if (other.gameObject.tag == "Hand" || other.gameObject.tag == "OffHand" || other.gameObject.tag == "HeldBranch") {
            Highlight();
            Select();
            m_linkedButton.Unhighlight();
           
            if (other.gameObject.tag == "HeldBranch")
            {
                Branch b = other.gameObject.GetComponent<Branch>();
                Hand h = b.m_hand;
                h.m_audioSource.PlayOneShot(h.m_interactSFX, 0.5f);
            } else {
                 Hand h = other.gameObject.GetComponent<Hand>();
                 h.m_audioSource.PlayOneShot(h.m_interactSFX, 0.5f);
            }
            
        }
    }
}
