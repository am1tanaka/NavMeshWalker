using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// NavMeshAgentをうまいこと動かすクラス
/// Ver0.9.3
/// MIT License
/// Copyright (C) 2018 YuTanaka
/// 
/// - NavMeshをベイクします
/// - 目的地をSetDestination()メソッドで渡してもらえれば、そこに向けて移動します
/// - 当たり判定は、NavMeshAgentと、CharacterControllerの双方に設定します。CharacterControllerの半径はNavMeshAgentのものより1周り小さくしておくと引っかかりが少なくなります
/// - 子供にAnimatorを持ったオブジェクトを設定。floatのSpeedプロパティに速度を渡すので、アニメの切り替えに使えます
/// 
/// </summary>

namespace AM1.Nav
{

    [RequireComponent(typeof(NavMeshAgent), typeof(CharacterController))]
    public class NavController : MonoBehaviour
    {

        [Header("移動")]
        [TooltipAttribute("歩く速度"), SerializeField]
        float walkSpeed = 2f;
        [TooltipAttribute("通常の旋回速度"), SerializeField]
        float angularSpeed = 200f;
        [TooltipAttribute("ターンする時の角度差"), SerializeField]
        float turnAngle = 45f;
        [TooltipAttribute("ターン時の旋回速度"), SerializeField]
        float turnAngularSpeed = 1000f;
        [TooltipAttribute("スピードを落とす距離。目的地がこの距離以内になったら、旋回角度に応じた減速をする"), SerializeField]
        float speedDownDistance = 0.5f;
        [TooltipAttribute("停止距離。この距離以下は移動しない"), SerializeField]
        float stopDistance = 0.01f;

        [Header("アニメーション")]
        [TooltipAttribute("移動速度とアニメーション速度の変換率"), SerializeField]
        float Speed2Anim = 1f;
        [TooltipAttribute("アニメを停止とみなす速度"), SerializeField]
        float stopSpeed = 0.01f;
        [TooltipAttribute("アニメの平均化係数"), SerializeField]
        float averageSpeed = 0.5f;

        NavMeshAgent agent;
        Animator anim;
        CharacterController chrController;
        Vector3 destination;
        /// <summary>
        /// アニメ速度を少し慣らすための値
        /// </summary>
        float lastSpeed;

        public bool IsReached
        {
            get
            {
                return DistanceXZ(LastCorner, transform.position) <= stopDistance;
            }
        }

        /// <summary>
        /// XZのみの距離を返します。
        /// </summary>
        /// <param name="src">元座標</param>
        /// <param name="dst">先座標</param>
        /// <returns>高さの差を考慮しない距離</returns>
        public float DistanceXZ(Vector3 src, Vector3 dst)
        {
            src.y = dst.y;
            return Vector3.Distance(src, dst);
        }

        public Vector3 LastCorner
        {
            get
            {
                if (agent.pathPending || agent.path.corners.Length == 0)
                {
                    return destination;
                }
                return agent.path.corners[agent.path.corners.Length - 1];
            }
        }

        private void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
            anim = GetComponentInChildren<Animator>();
            chrController = GetComponent<CharacterController>();

            // NavMeshAgentの移動と回転を無効化しておく
            agent.speed = 0f;
            agent.angularSpeed = 0f;
            agent.acceleration = 0f;

            SetDestination(transform.position);
        }

        /// <summary>
        /// 新しい目的地を設定します。
        /// </summary>
        /// <param name="pos">設定する座標です</param>
        public void SetDestination(Vector3 pos)
        {
            destination = pos;
            agent.SetDestination(pos);
        }

        void Update()
        {
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
                Vector3 target = LastCorner;
                spd = walkSpeed * Time.deltaTime;
                for (int i = 0; i < agent.path.corners.Length; i++)
                {
                    target = agent.path.corners[i];
                    if (DistanceXZ(target, transform.position) >= spd)
                    {
                        break;
                    }
                }

                // 移動方向と速度を算出
                move = target - transform.position;
                move.y = 0f;
                float rot = angularSpeed * Time.deltaTime;

                //　移動距離が目的地までの距離より遠い場合、角度と移動設定
                if (!IsReached)
                {
                    float angle = Vector3.SignedAngle(transform.forward, move, Vector3.up);

                    // 角度がturnAngleを越えていたら速度0
                    if (Mathf.Abs(angle) > turnAngle)
                    {
                        // 最高速度を越えているのでターンのみ
                        float rotmax = turnAngularSpeed * Time.deltaTime;
                        rot = Mathf.Min(Mathf.Abs(angle), rotmax);
                        transform.Rotate(0f, rot * Mathf.Sign(angle), 0f);
                        move = Vector3.zero;
                        spd = 0f;
                    }
                    else
                    {
                        // ターンはしない

                        // ゴール距離がスピードダウンより近い場合、角度の違いの分、前進速度を比例減速する
                        if (DistanceXZ(LastCorner, transform.position) < speedDownDistance)
                        {
                            spd *= (1f - (Mathf.Abs(angle) / turnAngle));
                        }

                        // 1回分の移動をキャンセルする場合、回転速度は制限しない
                        if (move.magnitude < spd)
                        {
                            spd = move.magnitude;
                            rot = angle;
                            transform.Rotate(0f, angle, 0f);
                        }
                        else
                        {
                            // 移動しながらターン
                            rot = Mathf.Min(Mathf.Abs(angle), rot);
                            transform.Rotate(0f, rot * Mathf.Sign(angle), 0f);
                        }

                        // キャラクターの前方に移動
                        move = transform.forward * spd;
                    }
                }
                else
                {
                    spd = 0f;
                    move = LastCorner - transform.position;
                    move.y = 0f;
                }
            }

            chrController.Move(move);
            spd = spd / Time.deltaTime;

            // アニメーション
            if (anim != null)
            {
                lastSpeed = averageSpeed * spd + lastSpeed * (1f - averageSpeed);
                anim.SetFloat("Speed", lastSpeed);
                if (spd >= stopSpeed)
                {
                    anim.speed = lastSpeed * Speed2Anim;
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
}
