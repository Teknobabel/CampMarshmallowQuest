using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Scout : MonoBehaviour
{
    public enum ScoutType {
        None,
        Gus,
        Artie,
        Lina,
        Jared,
        Sarah,
    }

    public enum ScoutState
    {
        None,
        Incomplete,
        Complete,
        InProgress,
    }

    public ScoutType m_scoutType = ScoutType.None;

    public GameObject m_branch;

    //public Marshmallow[] m_marshmallows;
    public GameObject m_roastedMarshmallow;

    public AudioSource m_audioSource;
    public GameObject m_marshmallowMouth;

    public GameObject[] m_eyeMeshes;

    public GameObject[] m_mouthMeshes;

    public GameObject[] m_eyebrowMeshes;

    public LineRenderer m_lineRenderer;

    public Transform[] m_lineSegments;
    public AudioClip m_interactSFX;
    public LookAt m_headLookAt;
    public SpeechBubble m_speechBubble;

    private bool m_doLineRenderer = false;
    private ScoutState m_scoutState = ScoutState.Incomplete;
    // Start is called before the first frame update

    private bool m_doLookAtPlayer = false;
    private bool m_introLookAt = false;
    //private bool m_amLookingAtPlayer = false;
    //private float m_lookAtTimer = 0.0f;
    //private float m_lookAtTime = 1.0f;

    private float m_blinkTimer = 0.0f;
    private float m_blinkTime = 5.0f;

    private Quaternion m_startingRotation = Quaternion.identity;
    private Quaternion m_startingHeadRotation = Quaternion.identity;
    void Start()
    {
        m_startingRotation = this.transform.rotation;
        if (m_headLookAt != null){m_startingHeadRotation = m_headLookAt.gameObject.transform.localRotation;}
        m_blinkTime = Random.Range(2.0f, 6.0f);
    }

    void Update()
    {
        m_blinkTimer = Mathf.Clamp(m_blinkTimer + Time.deltaTime, 0, m_blinkTime);
        if (m_blinkTimer == m_blinkTime)
        {
            Blink();
        }

        if (m_doLineRenderer) {
            m_lineRenderer.SetWidth(0.05f, 0.05f);
            m_lineRenderer.useWorldSpace = true;
            m_lineRenderer.positionCount = m_lineSegments.Length;
            Vector3[] points = new Vector3[m_lineSegments.Length];
            for (int i = 0; i < m_lineSegments.Length; i++) {
                points[i] = m_lineSegments[i].position;
            }
            m_lineRenderer.SetPositions(points);
        }
    }

    public void ScoutCompleteTalk () {

        if (Player.m_player.scoutsCompleted < Player.m_player.m_scoutAudio.Length)
        {
            Player.ScoutAudio sa = Player.m_player.m_scoutAudio[Player.m_player.scoutsCompleted];
            m_audioSource.clip = sa.m_complete;
            m_audioSource.Play();
        }
    }

    public void ScoutComplete () {
        Player.m_player.playerState = Player.PlayerState.None;
        Player.m_player.ScoutComplete();

        Animation anim = this.GetComponent<Animation>();
        anim.Play("Scout_EatMM");
    }
    public void GiveStick (Marshmallow m) {

        m_branch.gameObject.SetActive(true);
        //m_marshmallowMouth.SetActive(true);
        m_scoutState = ScoutState.Complete;

        MeshRenderer mr = m_roastedMarshmallow.GetComponent<MeshRenderer>();
        mr.material.SetColor("_BaseColor", Player.m_player.roastedMMColor);
        m_roastedMarshmallow.SetActive(true);

        m_audioSource.PlayOneShot(m_interactSFX, 1.0f);
        Animation anim = this.GetComponent<Animation>();
        int animToPlay = Player.m_player.scoutsCompleted;

        if (anim.isPlaying) {
            anim.Stop();
        }

        //string s = "Scout01_Complete";
        string s = "Scout01_Talk_Complete";
        switch(animToPlay) {
            case 1:
            s = "Scout02_Talk_Complete";
            break;
            case 2:
            s = "Scout03_Talk_Complete";
            break;
            case 3:
            s = "Scout04_Talk_Complete";
            break;
            case 4:
            s = "Scout05_Talk_Complete";
            break;
        }
        anim.Play(s);
    }

    public void SignalDoneRoasting () {
        Animation anim = this.GetComponent<Animation>();
        if (anim.isPlaying) {
            anim.Stop();
        }

        string s = "Scout01_Talk_DoneRoasting";
        anim.Play(s);
    }

    public void PlayDoneRoastingAudio (int num) {
        Player.ScoutAudio ma = Player.m_player.m_scoutAudio[Player.m_player.scoutsCompleted];

        switch (num) {
            case 0:

                m_audioSource.clip = ma.m_MMdoneRoasting;
                m_audioSource.Play();
                
            break;
            case 1:

                m_audioSource.clip = ma.m_MMdoneRoasting02;
                m_audioSource.Play();
                
            break;
            case 2:

                m_audioSource.clip = ma.m_MMdoneRoasting03;
                m_audioSource.Play();
                
            break;
            case 3:

                m_audioSource.clip = ma.m_MMdoneRoasting04;
                m_audioSource.Play();
                
            break;
        }
        
    }

    public void SignalToBiteMM () {
        //Debug.Log("signal to bite mm");
        Animation anim = this.GetComponent<Animation>();
        if (anim.isPlaying) {
            anim.Stop();
        }

        string s = "Scout01_Talk_BiteMM";
        anim.Play(s);
    }

    public void PlayBiteMMAudio () {
        //Debug.Log("play bite audio");
        Player.ScoutAudio ma = Player.m_player.m_scoutAudio[Player.m_player.scoutsCompleted];
        if (ma.m_biteMM != null){
            m_audioSource.clip = ma.m_biteMM;
            m_audioSource.Play();
        }
    }

    public void DuckMusic (int doDuck) {
        if (doDuck == 0) {
            Player.m_player.DuckMusic(true);
        } else {
            Player.m_player.DuckMusic(false);
        }
        
    }

    public void GiveUnroastedMMOnStick() {
        
        Animation anim = this.GetComponent<Animation>();
        // if (!anim.IsPlaying("Scout_GiveUnroastedMM")) {
        //     //Debug.Log("Attempting to hand over unroasted MM");
        //     anim.Play("Scout_GiveUnroastedMM");
        // }
        
        int animToPlay = Player.m_player.scoutsCompleted;

        if (!anim.IsPlaying("Scout_GiveUnroastedMM")) {

            //Debug.Log("Attempting to hand over unroasted MM");
            
            string s = "Scout01_GiveUnroastedMM";
            switch(animToPlay) {
                case 1:
                s = "Scout02_GiveUnroastedMM";
                break;
                case 2:
                s = "Scout03_GiveUnroastedMM";
                break;
                case 3:
                s = "Scout04_GiveUnroastedMM";
                break;
                case 4:
                s = "Scout05_GiveUnroastedMM";
                break;
            }
            anim.Play(s);
        }
    }

    public void PlayUnroastedAudio () {
        Player.ScoutAudio ma = Player.m_player.m_scoutAudio[Player.m_player.scoutsCompleted];
        m_audioSource.clip = ma.m_MMnotRoasted;
        m_audioSource.Play();
    }
    
    public void Talk ()
    {
        Player.ScoutAudio ma = Player.m_player.m_scoutAudio[Player.m_player.scoutsCompleted];
        m_audioSource.clip = ma.m_start;
        //m_audioSource.clip = ma.m_complete;
        m_audioSource.Play();
        
        m_scoutState = ScoutState.InProgress;
        
        
    }

    public void DisableCampfireSong ()
    {
        Player.m_player.m_musicAudioSources[5].Stop();
    }

    public void PlayStartAnim () {
        Animation anim = this.GetComponent<Animation>();
        int animToPlay = Player.m_player.scoutsCompleted;

        if (anim.isPlaying) {
            anim.Stop();
        }

        //string s = "Scout01_Talk_Complete";
        string s = "Scout01_Talk_Start";
        switch(animToPlay) {
            case 1:
            //s = "Scout02_Talk_Complete";
            s = "Scout02_Talk_Start";
            break;
            case 2:
            s = "Scout03_Talk_Start";
            //s = "Scout03_Talk_Complete";
            break;
            case 3:
            s = "Scout04_Talk_Start";
            //s = "Scout04_Talk_Complete";
            break;
            case 4:
            s = "Scout05_Talk_Start";
            break;
        }
        anim.Play(s);
    }

    public void GivePlayerStick () {
        m_branch.gameObject.SetActive(false);
        Player.m_player.GrabStick();
    }

    
    public void LookAtPlayer () {
       
       Vector3 pos = Player.m_player.m_playerHeadPosition.position;
       pos.y = this.transform.position.y;
       //this.transform.LookAt(pos);
       this.transform.DOLookAt(pos, 0.5f);
       StartTrackingPlayerHead();

       
    }

    private void StartTrackingPlayerHead () {
        if (m_headLookAt != null) {
           m_headLookAt.doLookAt = true;
           m_headLookAt.target = Player.m_player.m_playerHeadPosition;
       }
    }

    public void LookAtPlayerWithoutHeadTracking () {
        Vector3 pos = Player.m_player.m_playerHeadPosition.position;
       pos.y = this.transform.position.y;
       this.transform.DOLookAt(pos, 0.5f);
    }

    public void LookAtFire () {
       // this.transform.rotation = m_startingRotation;
       this.transform.DORotate(m_startingRotation.eulerAngles, 0.5f);
       if (m_headLookAt != null && m_headLookAt.doLookAt) {
           m_headLookAt.doLookAt = false;
           m_headLookAt.gameObject.transform.localRotation = m_startingHeadRotation;
           }
    }

    public void StartSinging () {
        Animation anim = this.GetComponent<Animation>();
        int animToPlay = Player.m_player.scoutsCompleted;

        if (anim.isPlaying) {
            anim.Stop();
        }

        float time = 0.0f;
        foreach (Scout scout in Player.m_player.m_scouts)
        {
            Animation a = scout.GetComponent<Animation>();
            if (a.IsPlaying("Scout_Idle_Sing01")) {

                time = a["Scout_Idle_Sing01"].time;
                break;
            }
        }

        anim["Scout_Idle_Sing01"].time = time;
        anim.Play("Scout_Idle_Sing01");
    }
    public void WaitForRoastedMM () {
         Animation anim = this.GetComponent<Animation>();
        int animToPlay = Player.m_player.scoutsCompleted;

        if (anim.isPlaying) {
            anim.Stop();
        }

        float time = 0.0f;
        foreach (Scout scout in Player.m_player.m_scouts)
        {
            Animation a = scout.GetComponent<Animation>();
            if (a.IsPlaying("Scout_Idle_Sing01")) {

                time = a["Scout_Idle_Sing01"].time;
                break;
            }
        }

        anim["Scout_Idle_Sing02"].time = time;
        anim.Play("Scout_Idle_Sing02");

        if (Player.m_player.scoutsCompleted == 0)
        {
            PickUpTrigger.m_pickUpTrigger.Enable();
        }
    }

    public void SetEyeMesh (int meshNum)
    {
        for (int i =0; i < m_eyeMeshes.Length; i++) {
            if (i == meshNum)
            {
                m_eyeMeshes[i].gameObject.SetActive(true);
            } else {
                m_eyeMeshes[i].gameObject.SetActive(false);
            }
        }
    }

    public void EnableMallowMouth ()
    {
        m_marshmallowMouth.SetActive(true);
    }

    public void DisableCampireSongs (){
        Player.m_player.m_musicAudioSources[5].Stop();
    }

    public void SetEyebrowMesh (int meshNum)
    {
        for (int i =0; i < m_eyebrowMeshes.Length; i++) {
            if (i == meshNum)
            {
                m_eyebrowMeshes[i].gameObject.SetActive(true);
            } else {
                m_eyebrowMeshes[i].gameObject.SetActive(false);
            }
        }
    }

    public void SetMouthMesh (int meshNum)
    {
        for (int i =0; i < m_mouthMeshes.Length; i++) {
            if (i == meshNum)
            {
                m_mouthMeshes[i].gameObject.SetActive(true);
            } else {
                m_mouthMeshes[i].gameObject.SetActive(false);
            }
        }
    }

    public void Blink () {
        Animation anim = this.GetComponent<Animation>();
        if (anim.isPlaying) 
        {
            
            // int animToPlay = Player.m_player.scoutsCompleted;
            // anim.Play("Blink");
        }
        
        m_blinkTimer = 0.0f;
        m_blinkTime = Random.Range(2.0f, 6.0f);
    }

    public void SetPlayerInputState (int state)
    {
        Player.m_player.SetPlayerInputState(state);
    }

    public void EnableLineRenderer () {
        m_doLineRenderer = true;
    }

    public void DisableLineRenderer () {
        m_doLineRenderer = false;
        m_lineRenderer.positionCount = 0;
    }

    public void SetSubtitle (int subtitleNum)
    {
        //SubtitleManager.m_subtitleManager.SetSubtitle(subtitleNum);

        if (SubtitleManager.m_subtitleManager.subtitleState == SubtitleManager.Subtitles.Enabled && m_speechBubble != null) {

            if (!m_speechBubble.gameObject.activeSelf) {
                m_speechBubble.gameObject.SetActive(true);
            }

            m_speechBubble.SetText(SubtitleManager.m_subtitleManager.GetSubtitle(subtitleNum));
        }
    }

    public void DisableSubtitles () 
    {
        //SubtitleManager.m_subtitleManager.DisableSubtitles();

        if (m_speechBubble != null) {
             m_speechBubble.gameObject.SetActive(false);
        }
    }

    public ScoutState scoutState {get{return m_scoutState;}}
    public bool introLookAt {get{return m_introLookAt;} set{m_introLookAt = value;}}
}
