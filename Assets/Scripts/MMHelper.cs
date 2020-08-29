using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MMHelper : MonoBehaviour
{
    public Marshmallow mm;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Cower () {
        mm.Cower();
    }

    public void Run () {
        mm.SetEyeMesh(0);
        mm.SetMouthMesh(3);
        mm.m_animation["Walk"].speed = 3.0f;
        mm.StartWalking();
    }
}
