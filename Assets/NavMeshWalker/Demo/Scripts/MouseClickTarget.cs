using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AM1.Nav;

/// <summary>
/// マウスのある場所にターゲットを設定するテスト用クラスです。
/// resetDestinationDistanceに設定した距離よりも、マウスが移動していたら、新しくルートを設定します。
/// </summary>
[RequireComponent(typeof(NavController))]
public class MouseClickTarget : MonoBehaviour {

    [Header("NavMesh")]
    [TooltipAttribute("目的地を変更する距離"), SerializeField]
    float resetDestinationDistance = 0.5f;
    Vector3 setDestination = Vector3.zero;

    NavController navCon;

    private void Awake()
    {
        navCon = GetComponent<NavController>();
    }

    private void Update()
    {
        Vector3 mpos = Input.mousePosition;
        Ray ray = Camera.main.ScreenPointToRay(mpos);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (!hit.collider.CompareTag("Player"))
            {
                Vector3 target = hit.point;
                if (Vector3.Distance(target, setDestination) > resetDestinationDistance)
                {
                    setDestination = target;
                    navCon.SetDestination(target);
                }
            }
        }
    }
}
