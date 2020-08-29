using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampFireFlicker : MonoBehaviour
{
    public GameObject[] m_objects;

    public float m_minRotateSpeed = 0.1f;
    public float m_maxRotateSpeed = 0.2f;

    private float m_rotateSpeed = 0.0f,
    m_time = 0.0f;

    private int m_currentObject = -1;
    // Start is called before the first frame update
    void Start()
    {
        m_rotateSpeed = Random.Range(m_minRotateSpeed, m_maxRotateSpeed);
        m_currentObject = Random.Range(0, m_objects.Length);
        m_objects[m_currentObject].SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        m_time += Time.deltaTime;
        if (m_time >= m_rotateSpeed)
        {
            m_time = 0;
            m_rotateSpeed = Random.Range(m_minRotateSpeed, m_maxRotateSpeed);
            m_objects[m_currentObject].SetActive(false);
            int i = Random.Range(0, m_objects.Length);
            while (i == m_currentObject && m_objects.Length > 1)
            {
                i = Random.Range(0, m_objects.Length);
            }
            m_currentObject = i;
            m_objects[i].SetActive(true);
        }
    }
}
