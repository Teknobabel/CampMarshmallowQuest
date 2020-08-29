using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireFlicker : MonoBehaviour
{
    public float m_minFlickerTime = 0.1f;
    public float m_maxFlickerTime = 0.2f;
    public Light m_light;
    private float m_maxIntensity = 0.0f;
    private float m_minIntensity = 0.0f;
    private float t = 0.0f;
    private float m_time = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        float i = m_light.intensity;
        m_maxIntensity = i;
        m_minIntensity = i * 0.5f;
        m_time = UnityEngine.Random.Range(m_minFlickerTime, m_maxFlickerTime);
        
    }

    // Update is called once per frame
    void Update()
    {
        t += Time.deltaTime;
        if (t >= m_time) {
            m_time = UnityEngine.Random.Range(m_minFlickerTime, m_maxFlickerTime);
            t = 0.0f;
            m_light.intensity = UnityEngine.Random.Range(m_minIntensity, m_maxIntensity);
        }
    }
}
