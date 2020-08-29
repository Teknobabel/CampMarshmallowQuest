using DG.Tweening;
using DG.Tweening.Core;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
  private Color m_roastedMMColor = Color.white;
  private float m_turnAmount = 25f;
  private List<float> m_musicValues = new List<float>();
  private Vector3 m_tempPlayerPosition = Vector3.zero;
  public static Player m_player;
  public Hand[] m_hands;
  public Scout[] m_scouts;
  public Marshmallow[] m_campsiteMMs;
  public Player.ScoutAudio[] m_scoutAudio;
  public AudioClip[] m_musicAudio;
  public AudioClip[] m_campfireSongAudio;
  public AudioClip[] m_sfx;
  public Player.MarshmallowAudio[] m_marshmallowAudio;
  public AudioClip[] m_endGameAudio;
  public Transform m_playerPosition;
  public Transform m_playerHeadPosition;
  public Transform m_creditsArea;
  public GameObject m_creditsText;
  public GameObject m_finalBranch;
  public Transform m_pauseArea;
  public GameObject m_motherDetector;
  public GameObject m_motherMM;
  public GameObject[] m_controllerObjects;
  public AudioSource m_ambientForestAudio;
  public AudioSource[] m_musicAudioSources;
  public GameObject m_headsetTracker;
  public SkinnedMeshRenderer[] m_handMeshes;
  public Material[] m_handMaterials;
  public Material[] m_gooMaterials;
  public GameObject[] m_controllerTutorialObjects;
  private Player.PlayerState m_playerState = Player.PlayerState.None;
  private Scout m_scoutInFocus;
  private Marshmallow m_marshmallowInFocus;
  public Player.LocomotionType m_locomotionType;
  public GameObject m_menuHandPointer;
  public LayerMask m_layerMask;
  private Player.HandType m_handType;
  private int m_scoutsCompleted = 0;
  private int m_musicLevel;
  private int m_handColor;
  private bool m_doDetectMother;
  private bool m_pauseState;
  private bool m_isDuckingMusic;

  private void Awake()
  {
    Player.m_player = this;
  }

  private void Start()
  {
    this.SetLocomotionType(this.m_locomotionType);
  }

  public void TurnRight()
  {
    Vector3 localEulerAngles = Player.m_player.m_playerPosition.localEulerAngles;
    localEulerAngles.y += this.m_turnAmount;
    Player.m_player.m_playerPosition.localEulerAngles = localEulerAngles;
  }

  public void TurnLeft()
  {
    Vector3 localEulerAngles = Player.m_player.m_playerPosition.localEulerAngles;
    localEulerAngles.y -= this.m_turnAmount;
    Player.m_player.m_playerPosition.localEulerAngles = localEulerAngles;
  }

  public void GrabStick()
  {
    foreach (Hand hand in this.m_hands)
    {
      if (hand.m_handType == Hand.HandType.Right)
      {
        hand.GrabStick();
        break;
      }
    }
  }

  public void LetGoOfStick()
  {
    foreach (Hand hand in this.m_hands)
    {
      if (hand.m_handType == Hand.HandType.Right)
      {
        hand.Reset();
        break;
      }
    }
  }

  public void LetGoOfMarshmallow()
  {
    foreach (Hand hand in this.m_hands)
    {
      if (hand.m_handType == Hand.HandType.Left)
      {
        hand.Reset();
        break;
      }
    }
  }

  public void DuckMusic(bool doDuck)
  {
    //Debug.Log((object) ("Ducking Music: " + doDuck.ToString()));
    if (doDuck)
    {
      this.m_isDuckingMusic = true;
      foreach (AudioSource musicAudioSource in this.m_musicAudioSources)
      {
        AudioSource asource = musicAudioSource;
        float volume = asource.volume;
        this.m_musicValues.Add(volume);
        DOTween.To((DOGetter<float>) (() => asource.volume), (DOSetter<float>) (x => asource.volume = x), volume * asource.GetComponent<AudioDucking>().m_duckValue, 0.5f);
      }
    }
    else
    {
      this.m_isDuckingMusic = false;
      for (int index = 0; index < this.m_musicValues.Count; ++index)
      {
        AudioSource asource = this.m_musicAudioSources[index];
        DOTween.To((DOGetter<float>) (() => asource.volume), (DOSetter<float>) (x => asource.volume = x), this.m_musicValues[index], 2f);
      }
      this.m_musicValues.Clear();
    }
  }

  public void UpdateMusicLevel(int amt)
  {
    this.m_musicLevel += amt;
    //Debug.Log((object) ("Updating music level: " + (object) this.m_musicLevel));
    switch (this.m_musicLevel)
    {
      case 1:
        this.m_musicAudioSources[0].clip = this.m_musicAudio[0];
        this.m_musicAudioSources[0].Play();
        this.m_musicAudioSources[5].clip = this.m_campfireSongAudio[0];
        this.m_musicAudioSources[5].Play();
        Material[] materials1 = this.m_handMeshes[0].materials;
        materials1[1] = this.m_gooMaterials[0];
        this.m_handMeshes[0].materials = materials1;
        Material[] materials2 = this.m_handMeshes[1].materials;
        materials2[1] = this.m_gooMaterials[0];
        this.m_handMeshes[1].materials = materials2;
        break;
      case 2:
        this.m_musicAudioSources[1].clip = this.m_musicAudio[1];
        this.m_musicAudioSources[1].Play();
        this.m_musicAudioSources[5].clip = this.m_campfireSongAudio[1];
        this.m_musicAudioSources[5].Play();
        Material[] materials3 = this.m_handMeshes[0].materials;
        materials3[2] = this.m_gooMaterials[1];
        this.m_handMeshes[0].materials = materials3;
        Material[] materials4 = this.m_handMeshes[1].materials;
        materials4[2] = this.m_gooMaterials[1];
        this.m_handMeshes[1].materials = materials4;
        break;
      case 3:
        this.m_musicAudioSources[2].clip = this.m_musicAudio[2];
        this.m_musicAudioSources[2].Play();
        this.m_musicAudioSources[5].clip = this.m_campfireSongAudio[2];
        this.m_musicAudioSources[5].Play();
        Material[] materials5 = this.m_handMeshes[0].materials;
        materials5[3] = this.m_gooMaterials[2];
        this.m_handMeshes[0].materials = materials5;
        Material[] materials6 = this.m_handMeshes[1].materials;
        materials6[3] = this.m_gooMaterials[2];
        this.m_handMeshes[1].materials = materials6;
        break;
      case 4:
        this.m_musicAudioSources[3].clip = this.m_musicAudio[3];
        this.m_musicAudioSources[3].Play();
        this.m_musicAudioSources[5].clip = this.m_campfireSongAudio[3];
        this.m_musicAudioSources[5].Play();
        Material[] materials7 = this.m_handMeshes[0].materials;
        materials7[4] = this.m_gooMaterials[3];
        this.m_handMeshes[0].materials = materials7;
        Material[] materials8 = this.m_handMeshes[1].materials;
        materials8[4] = this.m_gooMaterials[3];
        this.m_handMeshes[1].materials = materials8;
        break;
      case 5:
        //this.m_musicAudioSources[5].Stop();
        Material[] materials9 = this.m_handMeshes[0].materials;
        materials9[5] = this.m_gooMaterials[4];
        this.m_handMeshes[0].materials = materials9;
        Material[] materials10 = this.m_handMeshes[1].materials;
        materials10[5] = this.m_gooMaterials[4];
        this.m_handMeshes[1].materials = materials10;
        break;
    }
  }

  public void ScareMMs(int numToScare)
  {
    List<Marshmallow> marshmallowList = new List<Marshmallow>();
    foreach (Marshmallow campsiteMm in this.m_campsiteMMs)
    {
      if (campsiteMm.gameObject.activeInHierarchy && campsiteMm.fearLevel < 3)
        marshmallowList.Add(campsiteMm);
    }
    int num = UnityEngine.Random.Range(1, numToScare + 1);
    for (int index1 = 0; index1 < num; ++index1)
    {
      if (marshmallowList.Count > 0)
      {
        int index2 = UnityEngine.Random.Range(0, marshmallowList.Count);
        Marshmallow marshmallow = marshmallowList[index2];
        marshmallowList.RemoveAt(index2);
        marshmallow.SetFearLevel(1);
      }
    }
  }

  private void Update()
  {
    // if (Input.GetKeyUp(KeyCode.Return)){
    //   Animation a = this.GetComponent<Animation>();
    //   a.Play("Mother_Appear");
    // }

    if (!this.m_doDetectMother)
      return;

    RaycastHit hitInfo;
    if (!UnityEngine.Physics.Raycast(this.m_motherDetector.transform.position, this.m_motherDetector.transform.TransformDirection(Vector3.forward), out hitInfo, 5000f, (int) this.m_layerMask) || !(hitInfo.transform.tag == "Mother"))
      return;
    this.m_doDetectMother = false;
    Animation component = this.GetComponent<Animation>();
    if (component.isPlaying)
      component.Stop();
    component.Play("Game_End");
  }

  public void EnableMonster()
  {
    this.m_motherMM.GetComponent<Animation>().Play();
  }

  public Hand.HandState BranchState()
  {
    foreach (Hand hand in this.m_hands)
    {
      if (hand.m_handType == Hand.HandType.Right)
        return hand.handState;
    }
    return Hand.HandState.None;
  }

  public void ScoutComplete()
  {
    ++this.m_scoutsCompleted;
    if (this.m_scoutsCompleted != this.m_scouts.Length)
      return;
    //Debug.Log((object) "Spawning final branch");
    this.m_finalBranch.SetActive(true);
  }

  public void ScoutsLookAtMonster()
  {
    Vector3 position = this.m_motherMM.transform.position;
    position.y = this.m_playerPosition.position.y;
    foreach (Scout scout in this.m_scouts)
    {
      float duration = UnityEngine.Random.Range(0.3f, 0.75f);
      scout.transform.DOLookAt(position, duration, AxisConstraint.None, new Vector3?());
    }
  }

  public void ScoutsPointAtMonster()
  {
    foreach (Component scout in this.m_scouts)
    {
      Animation component = scout.GetComponent<Animation>();
      if (component.isPlaying)
        component.Stop();
      component.Play("Scout_MotherAppear");
    }
  }

  public void ScoutsLookAtPlayer()
  {
    Vector3 position = Player.m_player.m_playerHeadPosition.position;
    position.y = this.m_playerPosition.position.y;
    foreach (Scout scout in this.m_scouts)
    {
      float duration = UnityEngine.Random.Range(0.3f, 0.75f);
      scout.transform.DOLookAt(position, duration, AxisConstraint.None, new Vector3?());
      scout.SetMouthMesh(2);
    }
  }

  public void ScoutsLookAround()
  {
    foreach (Scout scout in this.m_scouts)
    {
      Animation component = scout.GetComponent<Animation>();
      foreach (AnimationState animationState in component)
      {
        if (animationState.name == "LookAround")
          animationState.time = UnityEngine.Random.Range(0.0f, animationState.length);
      }
      scout.SetEyeMesh(0);
      scout.SetMouthMesh(5);
      component.Play("LookAround");
    }
  }

  public void ScoutsStopSinging()
  {
    AudioSource asource = this.m_musicAudioSources[5];
    DOTween.To((DOGetter<float>) (() => asource.volume), (DOSetter<float>) (x => asource.volume = x), 0.0f, 0.25f);
  }

  public void PlayEndGameAudio(int audioNum)
  {
    if (audioNum == 3 || audioNum == 4)
    {
      AudioSource component = this.m_motherMM.GetComponent<AudioSource>();
      component.clip = this.m_endGameAudio[audioNum];
      component.Play();
    }
    else
    {
      AudioSource audioSource = this.m_hands[0].m_audioSource;
      audioSource.clip = this.m_endGameAudio[audioNum];
      audioSource.Play();
    }
  }

  public void EnableMotherDetector()
  {
    this.m_doDetectMother = true;
  }

  public void MovePlayer(int moveTo)
  {
    this.m_playerPosition.position = this.m_creditsArea.position;
    foreach (GameObject controllerObject in this.m_controllerObjects)
      controllerObject.SetActive(false);
    this.m_hands[0].gameObject.SetActive(false);
    this.m_hands[1].gameObject.SetActive(false);

    this.m_ambientForestAudio.Stop();
    this.m_musicAudioSources[0].Stop();
    this.m_musicAudioSources[1].Stop();
    this.m_musicAudioSources[2].Stop();
    this.m_musicAudioSources[3].Stop();
  }

  public void SetMotherMouthMesh(int meshNum)
  {
    this.m_motherMM.GetComponent<Mother>().SetMouthMesh(meshNum);
  }

  public void MotherRoar()
  {
    this.m_motherMM.GetComponent<Mother>().m_animation.Play("Mother_Roar");
  }

  public void SetPlayerInputState(int state)
  {
    switch (state)
    {
      case 0:
        this.DuckMusic(false);
        this.SetLocomotionType(this.m_locomotionType);
        break;
      case 1:
        this.DuckMusic(true);
        if (this.m_locomotionType == Player.LocomotionType.Teleport)
        {
          this.m_controllerObjects[0].SetActive(false);
          this.m_controllerObjects[1].SetActive(false);
          this.m_controllerObjects[2].SetActive(false);
          break;
        }
        if (this.m_locomotionType != Player.LocomotionType.Translate)
          break;
        this.m_controllerObjects[3].SetActive(false);
        break;
    }
  }

  public void SetPauseState()
  {
    if (!this.m_pauseState)
    {
      this.m_pauseState = true;
      this.m_menuHandPointer.gameObject.SetActive(true);
      PauseMenu.m_pauseMenu.pauseMenuActive = true;
      PauseMenu.m_pauseMenu.OnStart();
      this.m_tempPlayerPosition = Player.m_player.m_playerPosition.position;
      Player.m_player.m_playerPosition.position = this.m_pauseArea.position;
      Vector3 eulerAngles = this.m_pauseArea.rotation.eulerAngles;
      eulerAngles.y = this.m_headsetTracker.transform.rotation.eulerAngles.y;
      this.m_pauseArea.rotation = Quaternion.Euler(eulerAngles.x, eulerAngles.y, eulerAngles.z);
      this.m_pauseArea.GetComponent<AudioSource>().Play();
      foreach (AudioSource musicAudioSource in this.m_musicAudioSources)
        musicAudioSource.Pause();
      if (this.m_locomotionType == Player.LocomotionType.Teleport)
      {
        this.m_controllerObjects[0].SetActive(false);
        this.m_controllerObjects[1].SetActive(false);
        this.m_controllerObjects[2].SetActive(false);
      }
      else
      {
        if (this.m_locomotionType != Player.LocomotionType.Translate)
          return;
        this.m_controllerObjects[3].SetActive(false);
      }
    }
    else
    {
      this.m_pauseState = false;
      this.m_menuHandPointer.gameObject.SetActive(false);
      PauseMenu.m_pauseMenu.pauseMenuActive = false;
      Player.m_player.m_playerPosition.position = this.m_tempPlayerPosition;
      this.m_pauseArea.GetComponent<AudioSource>().Stop();
      foreach (AudioSource musicAudioSource in this.m_musicAudioSources)
        musicAudioSource.Play();
      this.SetLocomotionType(this.m_locomotionType);
    }
  }

  public void ScreenShake()
  {
    this.m_playerPosition.DOShakePosition(2f, 0.1f, 15, 90f, false, true);
  }

  public void PlayCreditsMusic()
  {
    
    this.m_creditsArea.GetComponent<AudioSource>().Play();
    this.m_creditsText.SetActive(true);
  }

  public void CycleHandColor(Player.HandType hType)
  {
    switch (hType)
    {
      case Player.HandType.Light:
        this.m_handType = Player.HandType.Light;
        foreach (Renderer handMesh in this.m_handMeshes)
          handMesh.material = this.m_handMaterials[0];
        break;
      case Player.HandType.Med:
        this.m_handType = Player.HandType.Med;
        foreach (Renderer handMesh in this.m_handMeshes)
          handMesh.material = this.m_handMaterials[1];
        break;
      case Player.HandType.Dark:
        this.m_handType = Player.HandType.Dark;
        foreach (Renderer handMesh in this.m_handMeshes)
          handMesh.material = this.m_handMaterials[2];
        break;
    }
  }

  public void CycleHandColor()
  {
    ++this.m_handColor;
    if (this.m_handColor >= this.m_handMaterials.Length)
      this.m_handColor = 0;
    Material handMaterial = this.m_handMaterials[this.m_handColor];
    foreach (Renderer handMesh in this.m_handMeshes)
      handMesh.material = handMaterial;
    switch (this.m_handColor)
    {
      case 0:
        this.m_handType = Player.HandType.Light;
        break;
      case 1:
        this.m_handType = Player.HandType.Med;
        break;
      case 2:
        this.m_handType = Player.HandType.Dark;
        break;
    }
  }

  public void SetLocomotionType(Player.LocomotionType l)
  {
    this.m_locomotionType = l;
    if (this.m_locomotionType == Player.LocomotionType.Teleport)
    {
      this.m_controllerObjects[0].SetActive(true);
      this.m_controllerObjects[1].SetActive(true);
      this.m_controllerObjects[2].SetActive(true);
      this.m_controllerObjects[3].SetActive(false);
      this.m_controllerObjects[4].SetActive(false);
      this.m_controllerTutorialObjects[0].SetActive(false);
      this.m_controllerTutorialObjects[1].SetActive(true);
    }
    else
    {
      if (this.m_locomotionType != Player.LocomotionType.Translate)
        return;
      this.m_controllerObjects[0].SetActive(false);
      this.m_controllerObjects[1].SetActive(false);
      this.m_controllerObjects[2].SetActive(false);
      this.m_controllerObjects[3].SetActive(true);
      this.m_controllerObjects[4].SetActive(true);
      this.m_controllerTutorialObjects[0].SetActive(true);
      this.m_controllerTutorialObjects[1].SetActive(false);
    }
  }

  public void SetSubtitle(int subtitleNum)
  {
    //SubtitleManager.m_subtitleManager.SetSubtitle(subtitleNum);
    m_scouts[0].SetSubtitle(subtitleNum);
  }

  public void DisableSubtitles()
  {
    //SubtitleManager.m_subtitleManager.DisableSubtitles();
    m_scouts[0].DisableSubtitles();
  }

  public void SetMouthMesh (int meshNum) {
    m_scouts[0].SetMouthMesh(meshNum);
  }

  public Player.PlayerState playerState
  {
    get
    {
      return this.m_playerState;
    }
    set
    {
      this.m_playerState = value;
    }
  }

  public Scout scoutInFocus
  {
    get
    {
      return this.m_scoutInFocus;
    }
    set
    {
      this.m_scoutInFocus = value;
    }
  }

  public Marshmallow marshMallowInFocus
  {
    get
    {
      return this.m_marshmallowInFocus;
    }
    set
    {
      this.m_marshmallowInFocus = value;
    }
  }

  public int scoutsCompleted
  {
    get
    {
      return this.m_scoutsCompleted;
    }
  }

  public Player.LocomotionType locomotionType
  {
    get
    {
      return this.m_locomotionType;
    }
  }

  public Player.HandType handType
  {
    get
    {
      return this.m_handType;
    }
  }

  public bool isDuckingMusic
  {
    get
    {
      return this.m_isDuckingMusic;
    }
  }

  public Color roastedMMColor
  {
    get
    {
      return this.m_roastedMMColor;
    }
    set
    {
      this.m_roastedMMColor = value;
    }
  }

  [Serializable]
  public struct ScoutAudio
  {
    public AudioClip m_start;
    public AudioClip m_complete;
    public AudioClip m_MMNotOnStick;
    public AudioClip m_MMnotRoasted;
    public AudioClip m_MMdoneRoasting;
    public AudioClip m_MMdoneRoasting02;
    public AudioClip m_MMdoneRoasting03;
    public AudioClip m_MMdoneRoasting04;
    public AudioClip m_biteMM;
  }

  [Serializable]
  public struct MarshmallowAudio
  {
    public AudioClip m_grab;
    public AudioClip m_putOnStick;
    public AudioClip m_roasted;
    public AudioClip[] m_stabIdle;
  }

  public enum PlayerState
  {
    None,
    GatheringMarshmallow,
    RoastingMarshmallow,
  }

  public enum LocomotionType
  {
    Teleport,
    Translate,
  }

  public enum HandType
  {
    Light,
    Med,
    Dark,
  }
}
