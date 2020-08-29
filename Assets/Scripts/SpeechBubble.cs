using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeechBubble : MonoBehaviour
{
    public Text m_text;
    public Transform m_followTransform;
    public Transform m_lookatTarget;

    private Animation m_animation;

    // Start is called before the first frame update
    void Start()
    {
        m_animation = this.GetComponent<Animation>();
        m_lookatTarget = Player.m_player.m_playerHeadPosition;
    }

    void LateUpdate ()
    {
        if (m_followTransform != null && this.gameObject.activeSelf){

            Vector3 pos = m_followTransform.position;
            //this.gameObject.transform.position = pos;
        }

        if (m_lookatTarget != null) {
            this.gameObject.transform.LookAt(m_lookatTarget, Vector3.up);
        }
    }

    // Update is called once per frame
    public void SetText(string s)
    {
        m_text.text = s;
        if (m_animation != null) {
            m_animation.Stop();
            m_animation.Play();
        }
        
    }
}
