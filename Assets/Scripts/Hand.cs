using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    public enum HandState {
        None,
        Empty,
        Full,
        MMonStick,
        RoastedMMonStick,
    }

    public enum HandType {
        None,
        Right,
        Left,
    }

public Marshmallow[] m_MMs;
public HandType m_handType = HandType.None;

public GameObject m_branch;
public GameObject m_goo;

public AudioSource m_audioSource;

public AudioClip m_interactSFX;

public Animator m_animator;
    private HandState m_state = HandState.Empty;

    //private Marshmallow m_MMinFocus = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PutMarshmallowOnStick (Marshmallow mm) {
        //Debug.Log("Putting Marshmallow on Stick");
        m_MMs[0].gameObject.SetActive(true);
        m_MMs[0].SetAccessories(mm);
        m_goo.gameObject.SetActive(true);
        m_MMs[0].PutOnStick();

        mm.Deactivate();

        m_state = HandState.MMonStick;
        Player.m_player.UpdateMusicLevel(1);
        // foreach (Marshmallow thisMM in m_MMs) {
        //         if (thisMM.m_type == mm.m_type){
        //             thisMM.gameObject.SetActive(true);
        //             mm.gameObject.SetActive(false);
        //             thisMM.PutOnStick();
        //             m_state = HandState.MMonStick;
        //             Player.m_player.UpdateMusicLevel(1);
        //         }
        //     }
    }

    public void StartRoasting () {
        //Debug.Log("Start Roasting");
       // m_state = HandState.RoastedMMonStick;
       foreach (Marshmallow thisMM in m_MMs) {
                if (thisMM.gameObject.activeSelf){
                    thisMM.StartRoasting();
                }
            }
    }
    public void RoastMarshmallow ()
    {
//        //Debug.Log("Continue Roasting");

        foreach (Marshmallow thisMM in m_MMs) {
                if (thisMM.gameObject.activeSelf){
                    bool isDone = thisMM.Roast();
                    if (isDone) {
                        //Debug.Log("Marshmallow is fully roasted");
                        m_state = HandState.RoastedMMonStick;
                    }
                   
                }
            }
        
    }

    public void Reset ()
    {
        if (m_goo != null) {m_goo.SetActive(false);}
        m_state = HandState.Empty;
        if (m_branch != null) {
             m_branch.gameObject.SetActive(false);
             m_MMs[0].ResetRoastedMaterials();
             m_MMs[0].gameObject.SetActive(false);
        }

        m_MMs[0].isRoasted = false;
        m_MMs[0].DisableRagdoll();
        //  foreach (Marshmallow thisMM in m_MMs) {
        //         if (thisMM.gameObject.activeSelf){
        //             thisMM.gameObject.SetActive(false);
        //         }
        //     }
    }

    public void GrabStick ()
    {
        if (m_branch != null) {
            //m_MMs[0].gameObject.SetActive(false);
            m_branch.gameObject.SetActive(true);
            m_audioSource.PlayOneShot(m_interactSFX, 0.5f);
           m_animator.SetInteger("Pose", 2);
        }
    }

    public void OnTriggerEnter (Collider other) {
        if (m_state != HandState.Full && other.gameObject.tag == "MM" && m_handType == HandType.Left &&
        Player.m_player.playerState == Player.PlayerState.GatheringMarshmallow){
            //Debug.Log("Grabbing Marshmallow");
           Marshmallow mm = other.GetComponent<Marshmallow>();
            Marshmallow thisMM = m_MMs[0];
            // foreach (Marshmallow thisMM in m_MMs) {
            //     if (thisMM.m_type == mm.m_type){
                    Player.m_player.marshMallowInFocus = thisMM;
                    thisMM.gameObject.SetActive(true);
                    thisMM.SetAccessories(mm);
                    thisMM.Grab();
                    mm.m_parentObject.SetActive(false);
                    m_state = HandState.Full;

                    m_audioSource.PlayOneShot(m_interactSFX, 0.5f);
            //     }
            // }

            if (Player.m_player.scoutsCompleted == 0)
            {
                PickUpTrigger.m_pickUpTrigger.Disable();
            }
        } else if (other.gameObject.tag == "Branch"){
            Player.m_player.GrabStick();
            Player.m_player.playerState = Player.PlayerState.GatheringMarshmallow;
            //m_audioSource.PlayOneShot(m_interactSFX, 0.5f);
            other.gameObject.SetActive(false);
        }
        // else if (other.gameObject.tag == "Scout" && Player.m_player.playerState == Player.PlayerState.None){
        //     Scout s = other.gameObject.GetComponent<Scout>();
            
        //     if (VOTrigger.m_VOTrigger.introActive)
        //     {
        //         foreach (Scout thisS in Player.m_player.m_scouts) {
        //             if (thisS.introLookAt && thisS.m_scoutType != s.m_scoutType) {
        //                 thisS.introLookAt = false;
        //                 thisS.LookAtFire();
        //                 thisS.StartSinging();
        //                 break;
        //             }
        //         }

        //         VOTrigger.m_VOTrigger.Disable();
        //     }

        //     if (s.scoutState == Scout.ScoutState.Incomplete)
        //     {
        //         //Debug.Log("Talking to Scout");
        //         Player.m_player.playerState = Player.PlayerState.GatheringMarshmallow;
        //         Player.m_player.scoutInFocus = s;
        //         s.PlayStartAnim();
        //     }

        // } 
        else if (other.gameObject.tag == "Scout" && Player.m_player.playerState == Player.PlayerState.RoastingMarshmallow){

            Hand.HandState bs = Player.m_player.BranchState();
            Scout s = other.gameObject.GetComponent<Scout>();

            if (bs == Hand.HandState.RoastedMMonStick) {

                if (s.scoutState == Scout.ScoutState.InProgress && Player.m_player.scoutInFocus != null & Player.m_player.scoutInFocus.m_scoutType == s.m_scoutType)
                {
                    //Debug.Log("Handing over roasted marshmallow");
                    //m_audioSource.PlayOneShot(m_interactSFX, 1.0f);
                    m_animator.SetInteger("Pose", 99);
                    Player.m_player.LetGoOfStick();
                    Player.m_player.scoutInFocus.GiveStick(Player.m_player.marshMallowInFocus);
                    Player.m_player.marshMallowInFocus = null;
                    Player.m_player.scoutInFocus = null;
                }
            } else if (s.scoutState == Scout.ScoutState.InProgress) {
                
                s.GiveUnroastedMMOnStick();
            }
        }
    }
    public HandState handState {get{return m_state;}}
}


