using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SizeJitter : MonoBehaviour
{
    public float m_offset = 0.2f;
    public float m_interval = 0.1f;
    private Transform m_target;
    private Vector3 m_startSize = Vector3.zero;
    private float m_timer = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        m_target = this.gameObject.transform;
        m_startSize = m_target.localScale;
        
    }

    // Update is called once per frame
    void Update()
    {
        m_timer = Mathf.Clamp(m_timer + Time.deltaTime, 0, m_interval);

        if (m_timer == m_interval)
        {
            m_timer = 0.0f;
            float r = Random.Range(m_offset * -1, m_offset);
            Vector3 v = m_startSize + (m_startSize * r);
            m_target.localScale = v;
        }
    }
}
