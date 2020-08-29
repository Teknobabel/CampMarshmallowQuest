using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAt : MonoBehaviour
{
    public bool m_doLookAt = false;

    public Transform m_target;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (m_doLookAt && m_target != null) {
            this.gameObject.transform.LookAt(m_target, Vector3.down);
            this.gameObject.transform.rotation*=Quaternion.Euler(-110, 0, 0);
        }
    }

    public bool doLookAt {get{return m_doLookAt;} set{m_doLookAt = value;}}
    public Transform target {get{return m_target;} set{m_target = value;}}
}
