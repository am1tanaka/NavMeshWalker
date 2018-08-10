using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AM1.Nav;

public class ClickDetector : MonoBehaviour {

    [TooltipAttribute("クリックした時に向かう場所"), SerializeField]
    private Vector3 targetPosition;
    [TooltipAttribute("クリックを知らせる相手"), SerializeField]
    private NavController sendObject;

    private void Awake()
    {
        if (sendObject == null)
        {
            GameObject go = GameObject.FindGameObjectWithTag("Player");
            if (go != null)
            {
                sendObject = go.GetComponent<NavController>();
            }
            else
            {
                Debug.Log(name + " : Send Objectを設定するか、Playerタグのオブジェクトを作成してください。");
            }
        }
    }

    private void OnMouseDown() {
        if (sendObject)
        {
            sendObject.SetDestination(targetPosition);
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(targetPosition, 0.1f);
    }
#endif
}
