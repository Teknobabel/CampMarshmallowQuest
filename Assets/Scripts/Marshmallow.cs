using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using DG.Tweening.Core;

public class Marshmallow : MonoBehaviour
{
    public enum MMType
    {
        None,
        Plain,
        Peppermint,
        Sprinkles,
        Old,
        Neopolitan,
        Chocolate,
    }

    public MMType m_type = MMType.None;
    //public MeshRenderer m_material;
    public MeshRenderer[] m_roastingMaterials;
    public SkinnedMeshRenderer[] m_roastingMaterials02;
    public AudioSource m_audioSource;
    public ParticleSystem m_embers;
    public ParticleSystem m_goo;

    public GameObject m_parentObject;
    public Animation m_animation;
    public Animation m_animation2;
    public GameObject[] m_eyeMeshes;
    public GameObject[] m_eyebrowMeshes;
    public GameObject[] m_mouthMeshes;

    public GameObject[] m_accessories;

    public GameObject m_roastedMMProp;

    public GameObject m_bittenMMProp;

    public Rigidbody[] m_ragdoll;

    public Transform m_hidingSpot;

    public SpeechBubble m_speechBubble;

    public ParticleSystem[] m_cry;

    public bool m_doJump = false;
    public bool m_doLookAtPlayer = false;
    private float m_jumpTime = 0.0f;
    private float m_jumpTimer = 0.0f;
    private float m_roastTime = 10.0f;
    private float m_roastTimer = 0.0f;
    private float m_blinkTimer = 0.0f;
    private float m_blinkTime = 5.0f;
    private bool m_bitten = false;
    private int m_fearLevel = 0;

    private Color m_startingColor = Color.black;
    // Start is called before the first frame update
    void Start()
    {
        m_jumpTime = Random.Range(3.0f, 10.0f);
        m_blinkTime = Random.Range(2.0f, 6.0f);
        if (m_roastedMMProp != null) {
            MeshRenderer m = m_roastedMMProp.GetComponent<MeshRenderer>();
            m_startingColor = m.material.GetColor("_BaseColor");
        }
    }

    // Update is called once per frame
    void Update()
    {
        // if (Input.GetKeyUp(KeyCode.Return)){
        // SetFearLevel(3);
        // }
        if (m_doJump && m_fearLevel == 0) {
            m_jumpTimer += Time.deltaTime;
            if (m_jumpTimer >= m_jumpTime) {
                m_jumpTimer = 0.0f;
                m_jumpTime = Random.Range(2.0f, 6.0f);
                StartJump();
            }
        }

        if (m_doLookAtPlayer && m_fearLevel == 0){
            //turn toward player if nearby
            float dist = (this.gameObject.transform.position - Player.m_player.m_playerPosition.position).sqrMagnitude;
            if (dist < 24)
            {
                ////Debug.Log("blablabla");
                LookAtPlayer();
            }
        }

        m_blinkTimer = Mathf.Clamp(m_blinkTimer + Time.deltaTime, 0, m_blinkTime);
        if (m_blinkTimer == m_blinkTime)
        {
            Blink();
        }
    }

    public void LookAtPlayer () {
       Vector3 pos = Player.m_player.m_playerHeadPosition.position;
       pos.y = m_parentObject.transform.position.y;
       m_parentObject.transform.DOLookAt(pos, 0.5f);
    }

    public void StartJump () {
        //Animation anim = this.GetComponent<Animation>();
        m_animation.Play("MM_Jump");
    }
    public void Jump ()
    {
        // float jumpHeight = Random.Range(400, 600);
        //  Rigidbody rb = this.GetComponent<Rigidbody>();
        //     rb.AddForce(new Vector3(0, jumpHeight, 0), ForceMode.Force);
    }
   
    public void StartRoastingTalk () {
       //m_embers.Play();
        Player.MarshmallowAudio ma = Player.m_player.m_marshmallowAudio[Player.m_player.scoutsCompleted];
        m_audioSource.clip = ma.m_roasted;
        m_audioSource.Play();
   }
    private bool m_startedRoasting = false;
   public void StartRoasting () {

       //Animation anim = this.GetComponent<Animation>();
      
       if (!m_startedRoasting)
       {
        int animToPlay = Player.m_player.scoutsCompleted;
        m_roastedMMProp.gameObject.SetActive(true);
        m_startedRoasting = true;
        Player.m_player.ScareMMs(3);

        if (m_animation.isPlaying) {
            m_animation.Stop();
        }

        if (m_speechBubble != null && m_speechBubble.gameObject.activeSelf)
        {
            DisableSubtitles();
        }

        if (animToPlay == 0) {
            return;
        }

        string s = "MM01_Burn";
        switch(animToPlay) {
            case 1:
            s = "MM02_Burn";
            break;
            case 2:
            s = "MM03_Burn";
            break;
            case 3:
            s = "MM04_Burn";
            break;
            case 4:
            s = "MM05_Burn";
            break;
        }
        m_animation.Play(s);
       }
   }
    private bool roasted = false;

    private Color startRoastingColor = Color.white;
    public bool Roast () {
       
    //    if (m_roastTimer == 0.0f) {
    //         Player.m_player.m_ambientForestAudio.clip = Player.m_player.m_sfx[1];
    //         Player.m_player.m_ambientForestAudio.Play();
    //    }
            m_roastTimer = Mathf.Clamp(m_roastTimer += Time.deltaTime, 0, m_roastTime);
            float roastValue = Mathf.Lerp(-1.0f, 1.0f, m_roastTimer / 7.0f);

            foreach (MeshRenderer mr in m_roastingMaterials){
                mr.material.SetFloat("_DissolveAmount", roastValue);
            }

            foreach (SkinnedMeshRenderer mr in m_roastingMaterials02){
                mr.material.SetFloat("_DissolveAmount", roastValue);
            }
            
            ////Debug.Log(m_roastTimer);
            if (m_roastTimer > 3.0f && !roasted){
                
                roasted = true;
                //m_embers.Stop();
                // Player.m_player.m_ambientForestAudio.clip = Player.m_player.m_sfx[1];
                // Player.m_player.m_ambientForestAudio.Play();

                MeshRenderer m = m_roastedMMProp.GetComponent<MeshRenderer>();
                startRoastingColor = m.material.GetColor("_BaseColor");

                if (Player.m_player.scoutsCompleted == 0)
                {
                    Player.m_player.scoutInFocus.SignalDoneRoasting();
                } else if (Player.m_player.scoutsCompleted == 4)
                {
                    
                    Player.m_player.m_scouts[0].SignalToBiteMM();
                }
                return true;
            } 
            else if (m_roastTimer > 5.0f && roasted)
            {
                //transition from brown to black

                Color finalColor = Color.black;
                MeshRenderer m = m_roastedMMProp.GetComponent<MeshRenderer>();
                float l = Mathf.Lerp(0.0f, 1.0f, (m_roastTimer-5.0f) / (m_roastTime-5.0f));
                
                Color newColor = Color.Lerp(startRoastingColor, finalColor, l);
                //float r = m_roastTimer-3.0f;
                //float r2 = m_roastTime-3.0f;
                ////Debug.Log(r + " / " + r2);
                m.material.SetColor("_BaseColor", newColor);
                Player.m_player.roastedMMColor = newColor;
                //float lerpValue = Mathf.Lerp(0.0f, 1.0f, m_roastTimer / m_roastTime + 3.0f);

            }
            return false;
    }

    public void ResetRoastedMaterials () {
        m_roastTimer = 0.0f;
        m_startedRoasting = false;
        m_roastedMMProp.gameObject.SetActive(false);
        m_bittenMMProp.gameObject.SetActive(false);
        m_bitten = false;
         foreach (MeshRenderer mr in m_roastingMaterials){
                mr.material.SetFloat("_DissolveAmount", -1.0f);
            }

            foreach (SkinnedMeshRenderer mr in m_roastingMaterials02){
                mr.material.SetFloat("_DissolveAmount", -1.0f);
            }
            if (m_roastedMMProp != null) {
            MeshRenderer m = m_roastedMMProp.GetComponent<MeshRenderer>();
            m.material.SetColor("_BaseColor", m_startingColor);
        }
    }

    public void PutOnStickTalk () {
        Player.MarshmallowAudio ma = Player.m_player.m_marshmallowAudio[Player.m_player.scoutsCompleted];
        m_audioSource.clip = ma.m_putOnStick;
        m_audioSource.Play();
        //Player.m_player.m_ambientForestAudio.PlayOneShot(Player.m_player.m_sfx[0], 1.0f);
        Player.m_player.m_ambientForestAudio.clip = Player.m_player.m_sfx[0];
         Player.m_player.m_ambientForestAudio.Play();
    }

    public void PlayStabIdle (int clipNum) {
        Player.MarshmallowAudio ma = Player.m_player.m_marshmallowAudio[Player.m_player.scoutsCompleted];
        if (clipNum < ma.m_stabIdle.Length)
        {
            m_audioSource.clip = ma.m_stabIdle[clipNum];
            m_audioSource.Play();
        }
    }

    public void Deactivate () {
        if (m_speechBubble != null && m_speechBubble.gameObject.activeSelf){
            DisableSubtitles();
        }
        this.gameObject.SetActive(false);
    }

    public void PutOnStick () {

        //Animation anim = this.GetComponent<Animation>();
        int animToPlay = Player.m_player.scoutsCompleted;

        if (m_animation.isPlaying) {
            m_animation.Stop();
        }

        if (m_speechBubble != null && m_speechBubble.gameObject.activeSelf){
            DisableSubtitles();
        }

        string s = "MM01_Stab";
        switch(animToPlay) {
            case 1:
            s = "MM02_Stab";
            break;
            case 2:
            s = "MM04_Stab";
            break;
            case 3:
            s = "MM03_Stab";
            break;
            case 4:
            s = "MM05_Stab";
            break;
        }
        m_animation.Play(s);

        m_goo.Play();

        //Player.m_player.ScareMMs(2);

    }

    public void PlayStabIdleAnim () {
        int animToPlay = Player.m_player.scoutsCompleted;

        if (m_animation.isPlaying) {
            m_animation.Stop();
        }

        string s = "MM01_Stab_Idle";
        switch(animToPlay) {
            case 1:
            s = "MM02_Stab_Idle";
            break;
            case 2:
            s = "MM04_Stab_Idle";
            break;
            case 3:
            s = "MM03_Stab_Idle";
            break;
            case 4:
            s = "MM05_Stab_Idle";
            break;
        }
        m_animation.Play(s);
    }

    public void Grab () {

        //Animation anim = this.GetComponent<Animation>();
        int animToPlay = Player.m_player.scoutsCompleted;

        if (m_animation.isPlaying) {
            m_animation.Stop();
        }
        
        string s = "MM01_Wave";
        switch(animToPlay) {
            case 1:
            s = "MM02_Wave";
            break;
            case 2:
            s = "MM03_Wave";
            break;
            case 3:
            s = "MM04_Wave";
            break;
            case 4:
            s = "MM05_Wave";
            break;
        }
        m_animation.Play(s);
    }

    public void SetAccessories (Marshmallow mm) {

        m_type = mm.m_type;

        switch (mm.m_type){
            case Marshmallow.MMType.Chocolate:
            m_accessories[0].SetActive(true);
            m_accessories[1].SetActive(false);
            m_accessories[2].SetActive(false);
            m_accessories[3].SetActive(false);
            break;
            case Marshmallow.MMType.Sprinkles:
            m_accessories[0].SetActive(false);
            m_accessories[1].SetActive(true);
            m_accessories[2].SetActive(false);
            m_accessories[3].SetActive(false);
            break;
            case Marshmallow.MMType.Old:
            m_accessories[0].SetActive(false);
            m_accessories[1].SetActive(false);
            m_accessories[2].SetActive(true);
            m_accessories[3].SetActive(false);
            break;
            case Marshmallow.MMType.Peppermint:
            m_accessories[0].SetActive(false);
            m_accessories[1].SetActive(false);
            m_accessories[2].SetActive(false);
            m_accessories[3].SetActive(true);
            break;
            case Marshmallow.MMType.Plain:
            m_accessories[0].SetActive(false);
            m_accessories[1].SetActive(false);
            m_accessories[2].SetActive(false);
            m_accessories[3].SetActive(false);
            break;
        }
    }

    public void SetFearLevel (int amount) {

        m_fearLevel = Mathf.Clamp(m_fearLevel + amount, 0, 100);

        if (m_fearLevel == 1) {
            m_animation.Stop();
            
            Cower();
        } else if (m_fearLevel == 2) {
            m_parentObject.transform.position = m_hidingSpot.position;
        } else if (m_fearLevel == 3) {
            m_animation.Stop();

            switch (m_type)
            {
                case Marshmallow.MMType.Chocolate:
                    m_animation2["MM_Brian_RunScared"].time = Random.Range(0.0f, m_animation2["MM_Brian_RunScared"].length);
                    m_animation2.Play("MM_Brian_RunScared");
                break;
                case Marshmallow.MMType.Peppermint:
                m_animation2["MM_Pepper_RunScared"].time = Random.Range(0.0f, m_animation2["MM_Pepper_RunScared"].length);
                    m_animation2.Play("MM_Pepper_RunScared");
                break;
                case Marshmallow.MMType.Old:
                m_animation2["MM_Mort_RunScared"].time = Random.Range(0.0f, m_animation2["MM_Mort_RunScared"].length);
                    m_animation2.Play("MM_Mort_RunScared");
                break;
                case Marshmallow.MMType.Sprinkles:
                m_animation2["MM_Hank_RunScared"].time = Random.Range(0.0f, m_animation2["MM_Hank_RunScared"].length);
                    m_animation2.Play("MM_Hank_RunScared");
                break;
                case Marshmallow.MMType.Plain:
                m_animation2["MM_Jeff_RunScared"].time = Random.Range(0.0f, m_animation2["MM_Jeff_RunScared"].length);
                    m_animation2.Play("MM_Jeff_RunScared");
                break;
            }
            
        }
    }

    public void Cower () {
        SetEyeMesh(1);
        SetMouthMesh(2);
        float animLength = m_animation["Cower"].length;
            float startTime = Random.Range(0.0f, animLength);
            m_animation["Cower"].time = startTime;
            m_animation.Play("Cower");
    }

    public void Cry () {

        foreach (ParticleSystem ps in m_cry)
        {
            ps.Play();
        }
    }

    public void GrabTalk ()
    {
        Player.MarshmallowAudio ma = Player.m_player.m_marshmallowAudio[Player.m_player.scoutsCompleted];
        m_audioSource.clip = ma.m_grab;
        m_audioSource.Play();
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

    public void Bite () {
        ////Debug.Log("Biting MM");

        m_roastedMMProp.SetActive(false);
        m_bittenMMProp.SetActive(true);
        m_bitten = true;
        m_audioSource.PlayOneShot(Player.m_player.m_hands[1].m_branch.GetComponent<Branch>().m_audioClips[0]);

        MeshRenderer m = m_roastedMMProp.GetComponent<MeshRenderer>();
        MeshRenderer m2 = m_bittenMMProp.GetComponent<MeshRenderer>();
        Color c = m.material.GetColor("_BaseColor");
        m2.material.SetColor("_BaseColor", c);

        if (Player.m_player.scoutsCompleted == 4) {

            // trigger end game
            Animation anim = Player.m_player.GetComponent<Animation>();
            anim.Play();
        }
    }

    public void Blink () {
        //Animation anim = this.GetComponent<Animation>();
        // if (!anim.isPlaying) 
        // {
        //     int animToPlay = Player.m_player.scoutsCompleted;
        //     anim.Play("Blink");
        // }
        
        m_blinkTimer = 0.0f;
        m_blinkTime = Random.Range(2.0f, 6.0f);
    }

    public void DuckMusic (int doDuck) 
    {
        bool duck = true;
        if (doDuck == 1) {duck = false;}
        Player.m_player.DuckMusic(duck);
    }
    public void EnableRagdoll () {
        foreach (Rigidbody r in m_ragdoll) {
            r.isKinematic = false;
            r.useGravity = true;
        }
    }

    public void DisableRagdoll () {
        foreach (Rigidbody r in m_ragdoll) {
            r.isKinematic = true;
            r.useGravity = false;
        }
    }

    void OnDisable ()
    {
        if (m_audioSource != null && m_audioSource.isPlaying)
        {
            m_audioSource.Stop();
        }
        SetEyebrowMesh(99);
        // if (Player.m_player.isDuckingMusic) {
        //     Player.m_player.DuckMusic(false);
        // }
    }
    
    void OnTriggerEnter(Collider other) {

        if (other.gameObject.tag == "JumpTrigger")
        {
            m_animation.Play("MM_Jump02");
        } else if (other.gameObject.tag == "MMonStick"&& m_fearLevel == 0) {
            SetFearLevel(1);
        }
    }

    public void StartWalking ()
    {
        m_animation.Play("Walk");
    }

    public void SetSubtitle (int subtitleNum)
    {
        //SubtitleManager.m_subtitleManager.SetSubtitle(subtitleNum);

        if (SubtitleManager.m_subtitleManager.subtitleState == SubtitleManager.Subtitles.Enabled && m_speechBubble != null) {
            
            if (!m_speechBubble.gameObject.activeSelf) {
                m_speechBubble.gameObject.SetActive(true);
            }

            m_speechBubble.m_text.text = SubtitleManager.m_subtitleManager.GetSubtitle(subtitleNum);
        }
    }

    public void DisableSubtitles () 
    {
        //SubtitleManager.m_subtitleManager.DisableSubtitles();

        if (m_speechBubble != null) {
             m_speechBubble.gameObject.SetActive(false);
        }
    }

    public float roastTimer {get{return m_roastTimer;}}
    public float roastTime {get{return m_roastTime;}}
    public bool isRoasted {get{return roasted;} set {roasted = value;}}
    public bool startedRoasting {get{return m_startedRoasting;}}
    public bool bitten {get{return m_bitten;}}
    public int fearLevel {get{return m_fearLevel;}}
}
