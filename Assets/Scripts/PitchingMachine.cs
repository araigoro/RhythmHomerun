using System;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System.Collections;

public class PitchingMachine : MonoBehaviour
{
    /// <summary>
    /// ショット音
    /// </summary>
    [SerializeField] private AudioClip soundShot;

    private Target target;

    /// <summary>
    /// 投げる間隔(単位：フレーム)
    /// </summary>
    private const int shotInterval = 2000;

    /// <summary>
    /// 投げるオブジェクトの角度
    /// </summary>
    private const float shotAngle = 30;

    /// <summary>
    /// オブジェクトを飛ばす目標地点
    /// </summary>
    private Vector3 strikePosition = new Vector3(0, 3, -1) / 10;

    /// <summary>
    /// アニメーター
    /// </summary>
    private Animator animator;

    /// <summary>
    /// 投球モーション開始用のトリガー名
    /// </summary>
    private const string animatorTriggerPitching = "TriggerPitching";

    enum State
    {
        Waiting, Pitching, AfterPitching
    }
    private State pitchingState = State.Waiting;

    private void Awake()
    {
        animator = gameObject.GetComponent<Animator>();
    }

    [System.Obsolete]
    private void Update()
    {
        switch (pitchingState)
        {
            case State.Waiting:
                if (target != null)
                {
                    // 投球開始
                    // 投球モーションに切り替える
                    animator.ForceStateNormalizedTime(1.0f);
                    animator.SetTrigger(animatorTriggerPitching);

                    pitchingState = State.Pitching;
                }
                break;

            case State.Pitching:
                if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.88f)
                {
                    ShotTarget();
                    pitchingState = State.Waiting;
                }
                break;
        }
    }

    /// <summary>
    /// 所持しているターゲットを投げる
    /// </summary>
    private void ShotTarget()
    {
        if (target == null)
        {
            Debug.Log("Target is null!!");
            return;
            //yield return null;
        }

        //yield return new WaitForSeconds(1.0f);

        // 初期位置に設定
        target.SetActive(true);
        var position = new Vector3(gameObject.transform.position.x - 0.2f,
                                    gameObject.transform.position.y + 1.0f,
                                    gameObject.transform.position.z);
        target.Respawn(position);
        Debug.Log(strikePosition);
        Debug.Log(shotAngle);
        target.MoveParabola(strikePosition, shotAngle);

        // 投げる音を鳴らす
        AudioSource.PlayClipAtPoint(soundShot, gameObject.transform.position);

        LoseTarget();
    }

    private void LoseTarget()
    {
        target = null;

        if (target == null)
        {
            Debug.Log("null");
        }
    }

    public void Add(Target target)
    {
        this.target = target;
        Debug.Log(target + "in pitching machine");
    }
}
