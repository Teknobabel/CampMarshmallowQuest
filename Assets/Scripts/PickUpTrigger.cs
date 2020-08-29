using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PickUpTrigger : MonoBehaviour
{
    public static PickUpTrigger m_pickUpTrigger;
    public Animation m_anim;
    public AudioSource m_audioSource;
    public AudioClip[] m_audioClips;

    public Marshmallow m_mm;

    void Awake()
    {
        m_pickUpTrigger = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Enable ()
    {
        CapsuleCollider c = (CapsuleCollider)this.GetComponent<CapsuleCollider>();
        c.enabled = true;

    }

    public void Disable () 
    {
        m_anim.Stop();
        m_mm.m_doJump = true;
        m_mm.DisableSubtitles();
        this.gameObject.SetActive(false);
    }

    void OnTriggerEnter (Collider other) {
        if (other.gameObject.tag == "Hand"){
            m_anim.Play();
            Collider c = this.GetComponent<Collider>();
            c.enabled = false;
        }
    }

    public void Wave () {
        m_mm.m_animation.Stop();
        m_mm.m_animation.Play("MM01_Wave02");
    }

    public void Jump () {
        m_mm.StartJump();
    }

    public void LookAtPlayer () {
       
       Vector3 pos = Player.m_player.m_playerHeadPosition.position;
       pos.y = m_mm.m_animation.transform.position.y;
       //this.transform.LookAt(pos);
       m_mm.m_animation.transform.DOLookAt(pos, 0.5f);
    }

    // private void StartTrackingPlayerHead () {
    //     if (m_headLookAt != null) {
    //        m_headLookAt.doLookAt = true;
    //        m_headLookAt.target = Player.m_player.m_playerHeadPosition;
    //    }
    // }

    public void PlayAudio (int audioNum) {

        if (audioNum < m_audioClips.Length)
        {
            m_audioSource.PlayOneShot(m_audioClips[audioNum]);
        }
    }

    public void SetEyeMesh (int meshNum)
    {
        m_mm.SetEyeMesh(meshNum);
    }

    public void SetEyebrowMesh (int meshNum)
    {
        m_mm.SetEyebrowMesh(meshNum);
    }

    public void SetMouthMesh (int meshNum)
    {
        m_mm.SetMouthMesh(meshNum);
    }

    public void SetSubtitle (int subtitleNum)
    {
       // SubtitleManager.m_subtitleManager.SetSubtitle(subtitleNum);
        m_mm.SetSubtitle(subtitleNum);
    }

    public void DisableSubtitles () 
    {
        //SubtitleManager.m_subtitleManager.DisableSubtitles();
        m_mm.DisableSubtitles();
    }
}
