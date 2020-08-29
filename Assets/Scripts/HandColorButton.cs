using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandColorButton : Button
{
    public HandColorButton[] m_linkedButtons;
    // Start is called before the first frame update
    void Start()
    {
        if (m_buttonType == ButtonType.HandType_Light && Player.m_player.handType == Player.HandType.Light) {
            Highlight();
        } else if  (m_buttonType == ButtonType.HandType_Med && Player.m_player.handType == Player.HandType.Med){
            Highlight();
        } else if  (m_buttonType == ButtonType.HandType_Dark && Player.m_player.handType == Player.HandType.Dark){
            Highlight();
        } else {
            Unhighlight();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Select() {
        switch (m_buttonType)
        {
            case Button.ButtonType.HandType_Light:
            Player.m_player.CycleHandColor(Player.HandType.Light);
            break;
            case Button.ButtonType.HandType_Med:
            Player.m_player.CycleHandColor(Player.HandType.Med);
            break;
            case Button.ButtonType.HandType_Dark:
            Player.m_player.CycleHandColor(Player.HandType.Dark);
            break;
            
        }
    }

    public override void Highlight() {
        m_selector.SetActive(false);
        m_highlighted = true;
    }

    public override void Unhighlight() {
        m_selector.SetActive(true);
        m_highlighted = false;
    }

    void OnTriggerEnter (Collider other) {
        if ((other.gameObject.tag == "Hand" || other.gameObject.tag == "OffHand" || other.gameObject.tag == "HeldBranch") && !m_highlighted) {
            Highlight();
            Select();
            foreach (HandColorButton b in m_linkedButtons){
                b.Unhighlight();
            }
           
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
