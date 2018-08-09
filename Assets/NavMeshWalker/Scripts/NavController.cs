using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// NavMeshAgentをうまいこと動かすクラス
/// Ver0.9.0
/// MIT License
/// Copyright (C) 2018 YuTanaka
/// 
/// - NavMeshをベイクします
/// - 目的地をSetDestination()メソッドで渡してもらえれば、そこに向けて移動します
/// - 当たり判定は、NavMeshAgentと、CharacterControllerの双方に設定します。CharacterControllerの半径はNavMeshAgentのものより1周り小さくしておくと引っかかりが少なくなります
/// - 子供にAnimatorを持ったオブジェクトを設定。floatのSpeedプロパティに速度を渡すので、アニメの切り替えに使えます
/// 
/// </summary>

[RequireComponent(typeof(NavMeshAgent), typeof(CharacterController))]
public class NavController : MonoBehaviour {

    [Header("移動")]
    [TooltipAttribute("歩く速度"), SerializeField]
    float walkSpeed = 1.5f;
    [TooltipAttribute("通常の旋回速度"), SerializeField]
    float angularSpeed = 200f;
    [TooltipAttribute("ターンする時の角度差"), SerializeField]
    float turnAngle = 45f;
    [TooltipAttribute("ターン時の旋回速度"), SerializeField]
    float turnAngularSpeed = 1000f;

    [Header("アニメーション")]
    [TooltipAttribute("移動速度とアニメーション速度の変換率"), SerializeField]
    float Speed2Anim = 1f;
    [TooltipAttribute("停止とみなす速度"), SerializeField]
    float stopSpeed = 0.01f;

    NavMeshAgent agent;
    Animator anim;
    CharacterController chrController;
    Vector3 velocity;

    private void Awake() {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();
        chrController = GetComponent<CharacterController>();

        // NavMeshAgentの移動と回転を無効化しておく
        agent.speed = 0f;
        agent.angularSpeed = 0f;
        agent.acceleration = 0f;
    }

    /// <summary>
    /// 新しい目的地を設定します。
    /// </summary>
    /// <param name="pos">設定する座標です</param>
    public void SetDestination(Vector3 pos)
    {
        agent.SetDestination(pos);
    }

    void Update () {
        Vector3 move = chrController.velocity;
        float spd = 0f;

        // ルート検索中
        if (agent.pathPending)
        {
            move.Set(0, 0, 0);
        }
        else
        {
            // 次の目的座標を確認
            Vector3 target = transform.position;
            target.y = transform.position.y;    // yは無視
            spd = walkSpeed * Time.deltaTime;
            for (int i = 0; i < agent.path.corners.Length; i++)
            {
                target = agent.path.corners[i];
                if (Vector3.Distance(target, transform.position) >= spd)
                {
                    break;
                }
            }

            // 移動方向と速度を算出
            move = target - transform.position;
            move.y = 0f;
            spd = Mathf.Min(spd, move.magnitude);

            //　角度を調査
            if (spd > stopSpeed)
            {
                float angle = Vector3.SignedAngle(transform.forward, move, Vector3.up);
                float rot = angularSpeed * Time.deltaTime;
                if (Mathf.Abs(angle) > turnAngle)
                {
                    // 最高速度を越えているのでターンのみ
                    rot = turnAngularSpeed*Time.deltaTime;
                    rot = Mathf.Min(Mathf.Abs(angle), rot);
                    move = Vector3.zero;
                    transform.Rotate(0f, rot * Mathf.Sign(angle), 0f);
                }
                else
                {
                    // 移動しながらターン
                    rot = Mathf.Min(Mathf.Abs(angle), rot);
                    transform.Rotate(0f, rot * Mathf.Sign(angle), 0f);
                    // キャラクターの前方に移動
                    move = transform.forward*spd;
                }
            }
        }

        // 重力加速
        velocity += Physics.gravity;
        move.y = velocity.y * Time.deltaTime;

        CollisionFlags flags = chrController.Move(move);
        if ((flags & CollisionFlags.Below) != 0)
        {
            // 地面と接触しているので、Y速度をリセット
            move.y = 0f;
        }
        velocity = move / Time.deltaTime;
        spd = spd / Time.deltaTime;

        // アニメーション
        if (anim != null)
        {
            anim.SetFloat("Speed", spd);
            if (spd >= stopSpeed)
            {
                anim.speed = spd * Speed2Anim;
            }
            else
            {
                anim.speed = 1;
            }
        }
    }


#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        if (agent != null)
        {
            if (!agent.pathPending)
            {
                Gizmos.color = Color.blue;
                foreach (Vector3 pos in agent.path.corners)
                {
                    Gizmos.DrawSphere(pos, 0.2f);
                }
            }
        }
    }
#endif

}
