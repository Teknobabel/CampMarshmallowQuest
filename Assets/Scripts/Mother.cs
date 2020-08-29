using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mother : MonoBehaviour
{
    public GameObject[] m_eyeMeshes;

    public GameObject[] m_mouthMeshes;

    public Animation m_animation;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
}
