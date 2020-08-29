using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MMJumpSFX : MonoBehaviour
{

    public AudioSource m_audiosource;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void DoJump () {
        m_audiosource.Play();
    }
}
