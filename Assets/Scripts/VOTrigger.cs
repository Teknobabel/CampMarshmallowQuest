using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VOTrigger : MonoBehaviour
{
    public Animation m_anim;
    public AudioSource m_audioSource;
    public AudioSource[] m_audioSources;
    public AudioClip[] m_audioClips;

    public static VOTrigger m_VOTrigger;

    private bool m_introActive = false;

    // Start is called before the first frame update
    void Awake()
    {
        m_VOTrigger = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Disable ()
    {
        m_anim.Stop();
        m_introActive = false;
        Scout s = Player.m_player.m_scouts[0];
        s.DisableSubtitles();
        //this.gameObject.SetActive(false);
    }

    public void DuckMusic (int doDuck) {
        if (doDuck == 0) {
            Player.m_player.DuckMusic(true);
        } else {
            Player.m_player.DuckMusic(false);
        }
        
    }

    public void PlayAudio (int audioNum) {
        //m_audioSources[audioNum].Play();

        if (audioNum < m_audioClips.Length)
        {
            m_audioSource.PlayOneShot(m_audioClips[audioNum]);
        }
    }

    public void LookAtPlayer (int scoutNum) {

        Scout s = Player.m_player.m_scouts[scoutNum];
        s.SetEyeMesh(0);
        s.SetMouthMesh(2);
        s.LookAtPlayer();
        s.introLookAt = true;
    }

    public void LookAtFire (int scoutNum) {
        Scout s = Player.m_player.m_scouts[scoutNum];
        s.SetEyeMesh(1);
        s.LookAtFire();
    }

    public void ScoutWave (int scoutNum) {
        Scout s = Player.m_player.m_scouts[scoutNum];
        Animation anim = s.GetComponent<Animation>();
        anim.Play("Wave");
    }

    public void ScoutSing (int scoutNum) {
        //sync w other singers

        float time = 0.0f;
        foreach (Scout scout in Player.m_player.m_scouts)
        {
            Animation a = scout.GetComponent<Animation>();
            if (a.IsPlaying("Scout_Idle_Sing01")) {

                time = a["Scout_Idle_Sing01"].time;
                break;
            }
        }

        Scout s = Player.m_player.m_scouts[scoutNum];
        Animation anim = s.GetComponent<Animation>();
        anim["Scout_Idle_Sing01"].time = time;
        anim.Play("Scout_Idle_Sing01");
    }

    void OnTriggerEnter (Collider other) {
        if (other.gameObject.tag == "Hand"){
            if (Player.m_player.scoutsCompleted == 0) {
                //Debug.Log("Playing Intro VO");
                m_anim.Play();
                m_introActive = true;
                // m_audioSources[0].Play();
                // m_audioSources[1].PlayDelayed(1.0f);
                // m_audioSources[2].PlayDelayed(2.0f);
                Collider c = this.GetComponent<Collider>();
                c.enabled = false;
            }
        }
    }

    public void SetSubtitle (int subtitleNum)
    {
        //SubtitleManager.m_subtitleManager.SetSubtitle(subtitleNum);
        Scout s = Player.m_player.m_scouts[0];
        s.SetSubtitle(subtitleNum);
    }

    public void DisableSubtitles () 
    {
       // SubtitleManager.m_subtitleManager.DisableSubtitles();
       Scout s = Player.m_player.m_scouts[0];
        s.DisableSubtitles();
    }

    public void SetEyeMesh (int meshNum)
    {
        Scout s = Player.m_player.m_scouts[0];
        s.SetEyeMesh(meshNum);
    }

    public void SetEyebrowMesh (int meshNum)
    {
        Scout s = Player.m_player.m_scouts[0];
        s.SetEyebrowMesh(meshNum);
    }

    public void SetMouthMesh (int meshNum)
    {
        Scout s = Player.m_player.m_scouts[0];
        s.SetMouthMesh(meshNum);
    }


    public bool introActive {get{return m_introActive;}}
}
