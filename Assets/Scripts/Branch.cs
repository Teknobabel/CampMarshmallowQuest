using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Branch : MonoBehaviour
{
    public Hand m_hand;
    public MeshRenderer m_meshRenderer;
    public AudioSource m_audioSource;
    public AudioClip[] m_audioClips;

    private float m_coolDownTime = 2.0f;
    private float m_coolDownTimer = 0.0f;
    
    void Start()
    {
        
    }

    void OnTriggerStay (Collider other) {
       // if (other.tag == "Fire" && m_hand.handState != Hand.HandState.RoastedMMonStick){
           if (other.tag == "Fire" && m_hand.m_MMs[0].roastTimer < m_hand.m_MMs[0].roastTime){
            m_hand.RoastMarshmallow();
            // m_roastTimer += Time.deltaTime;
            // float roastValue = Mathf.Lerp(-1.0f, 1.0f, m_roastTimer / m_roastTime);
            // m_meshRenderer.material.SetFloat("_DissolveAmount", roastValue);
        }
        // if (other.tag == "Fire" && m_hand.handState == Hand.HandState.MMonStick) {
        //     m_roastTimer += Time.deltaTime;
        //     if (m_roastTimer >= m_roastTime){
        //         m_hand.RoastMarshmallow();
        //     }
        // }
    }

    void OnTriggerExit (Collider other) {
       // m_roastTimer = 0.0f;
       if (other.tag == "Fire") {
            m_hand.m_MMs[0].m_embers.Stop();
            // m_audioSource.Stop();
            // m_audioSource.loop = false;
        }
    }

    void OnTriggerEnter (Collider other) {
        if (other.tag == "HeldMM"){
            //Debug.Log("Branch touched");
            Marshmallow mm = other.gameObject.GetComponent<Marshmallow>();
            Player.m_player.LetGoOfMarshmallow();
            Player.m_player.playerState = Player.PlayerState.RoastingMarshmallow;
            m_hand.PutMarshmallowOnStick(mm);

            if (Player.m_player.scoutsCompleted == 0)
            {
                PickUpTrigger.m_pickUpTrigger.Disable();
            }

        }  else if (other.tag == "MM" && m_hand.handState == Hand.HandState.Empty) {

            // make sure left hand is empty
            if (Player.m_player.m_hands[0].handState == Hand.HandState.Empty) {
                Marshmallow mm = other.gameObject.GetComponent<Marshmallow>();
                Player.m_player.LetGoOfMarshmallow();
                Player.m_player.playerState = Player.PlayerState.RoastingMarshmallow;
                mm.m_parentObject.SetActive(false);
                m_hand.PutMarshmallowOnStick(mm);

                if (Player.m_player.scoutsCompleted == 0)
            {
                PickUpTrigger.m_pickUpTrigger.Disable();
            }

            }
        }
        else if (other.tag == "Fire") {

            m_hand.m_MMs[0].m_embers.Play();

            // m_audioSource.clip = m_audioClips[0];
            // m_audioSource.loop = true;
            // m_audioSource.Play();
            
            if (m_coolDownTimer == m_coolDownTime && ( m_hand.handState == Hand.HandState.MMonStick || m_hand.handState == Hand.HandState.RoastedMMonStick)) {

                m_hand.m_audioSource.PlayOneShot(Player.m_player.m_sfx[1]);
                m_coolDownTimer = 0;

            }

            if (m_hand.handState == Hand.HandState.MMonStick && !m_hand.m_MMs[0].startedRoasting) {

                 m_hand.StartRoasting();
            }
            
        } else if (other.tag == "Face") {

            if (!m_hand.m_MMs[0].bitten && m_hand.handState == Hand.HandState.RoastedMMonStick) {
                m_hand.m_MMs[0].Bite();
            }
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        m_coolDownTimer = Mathf.Clamp(m_coolDownTimer+Time.deltaTime, 0, m_coolDownTime);
    }

    
}
