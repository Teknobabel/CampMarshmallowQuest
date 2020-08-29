using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public static PauseMenu m_pauseMenu;

    public Button[] m_buttons;
    private bool m_pauseMenuActive = false;

    private Button m_highlightedButton = null;

    void Awake () {
        m_pauseMenu = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void OnStart () {

       // Player.m_player.SetPlayerInputState(1);

        foreach (Button b in m_buttons)
        {
            if (b.m_buttonType == Button.ButtonType.MoveType_Teleport)
            {
                if (Player.m_player.locomotionType == Player.LocomotionType.Teleport) {
                    b.Select();
                } else {
                    b.Deselect();
                }
            }

            if (b.m_buttonType == Button.ButtonType.MoveType_Joystick)
            {
                if (Player.m_player.locomotionType == Player.LocomotionType.Translate) {
                    b.Select();
                } else {
                    b.Deselect();
                }
            } 

            if (b.m_buttonType == Button.ButtonType.HandType_Light) {
                if (Player.m_player.handType == Player.HandType.Light) {
                  b.Select();
                } else {
                    b.Deselect();
                }
            }

            if (b.m_buttonType == Button.ButtonType.HandType_Med) {
                if (Player.m_player.handType == Player.HandType.Med) {
                  b.Select();
                } else {
                    b.Deselect();
                }
            }

            if (b.m_buttonType == Button.ButtonType.HandType_Dark) {
                if (Player.m_player.handType == Player.HandType.Dark) {
                  b.Select();
                } else {
                    b.Deselect();
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (m_pauseMenuActive) {

            Hand h = Player.m_player.m_hands[0];
            Vector3 fwd = h.transform.TransformDirection(Vector3.forward);
            RaycastHit objectHit;
             Debug.DrawRay(h.transform.position, fwd * 500, Color.green);
                if (Physics.Raycast(h.transform.position, fwd, out objectHit, 500))
                {
                    //do something if hit object ie
                    if(objectHit.transform.tag =="Button"){
                        Button b = objectHit.transform.GetComponent<Button>();
                        if (m_highlightedButton == null || m_highlightedButton != b) {
                            b.Highlight();
                        }
                    }
                } else if (m_highlightedButton != null) {
                    m_highlightedButton.Unhighlight();
                    m_highlightedButton = null;
                }
            }
    }

    public void ButtonPress ()
    {
        if (m_pauseMenuActive) {

            Hand h = Player.m_player.m_hands[0];
            Vector3 fwd = h.transform.TransformDirection(Vector3.forward);
            RaycastHit objectHit;
             Debug.DrawRay(h.transform.position, fwd * 500, Color.green);
                if (Physics.Raycast(h.transform.position, fwd, out objectHit, 500))
                {
                    //do something if hit object ie
                    if(objectHit.transform.tag =="Button"){
                        Button b = objectHit.transform.GetComponent<Button>();
                        
                        b.Select();
                        // if (b.m_buttonType == Button.ButtonType.MoveType_Joystick && Player.m_player.locomotionType != Player.LocomotionType.Translate) {
                        //     Player.m_player.SetLocomotionType(Player.LocomotionType.Translate);
                        // } else if (b.m_buttonType == Button.ButtonType.MoveType_Teleport && Player.m_player.locomotionType != Player.LocomotionType.Teleport) {
                        //     Player.m_player.SetLocomotionType(Player.LocomotionType.Teleport);
                        // }

                        OnStart();
                    }
                }
            }
    }

    public bool pauseMenuActive {get{return m_pauseMenuActive;}set{m_pauseMenuActive = value;}}
    public Button highlightedButton {get{return m_highlightedButton;}set{m_highlightedButton = value;}}
}
