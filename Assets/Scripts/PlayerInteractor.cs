using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractor : MonoBehaviour
{
    public Transform m_target;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //this.gameObject.transform.eulerAngles = Vector3.zero;
        // Vector3 pos = this.gameObject.transform.position;
        // pos.y = 0;
        // this.gameObject.transform.position = pos;

        Vector3 playerPos = m_target.position;
        playerPos.y = 0;
        Vector3 playerDirection = m_target.forward;
        Quaternion playerRotation = m_target.rotation;
        float spawnDistance = 0.63f;
 
        Vector3 targetPos = playerPos + playerDirection*spawnDistance;
        this.gameObject.transform.position = targetPos;

    }

    public void OnTriggerEnter (Collider other) {
        if (other.gameObject.tag == "Scout" && Player.m_player.playerState == Player.PlayerState.None){
            Scout s = other.gameObject.GetComponent<Scout>();
            
            if (VOTrigger.m_VOTrigger.introActive)
            {
                foreach (Scout thisS in Player.m_player.m_scouts) {
                    if (thisS.introLookAt && thisS.m_scoutType != s.m_scoutType) {
                        thisS.introLookAt = false;
                        thisS.LookAtFire();
                        thisS.StartSinging();
                        break;
                    }
                }

                VOTrigger.m_VOTrigger.Disable();

                // shrink player interactor
                CapsuleCollider c = this.GetComponent<CapsuleCollider>();
                c.radius = 0.25f; //start is .83

            }

            if (s.scoutState == Scout.ScoutState.Incomplete)
            {
                //Debug.Log("Talking to Scout");
                Player.m_player.playerState = Player.PlayerState.GatheringMarshmallow;
                Player.m_player.scoutInFocus = s;
                s.PlayStartAnim();
            }

        } 
    }
}
