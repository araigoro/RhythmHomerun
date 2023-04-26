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

    /// <summary>
    /// バッターのオブジェクト
    /// </summary>
    [SerializeField] private GameObject batterObj;

    /// <summary>
    /// ターゲットクラス
    /// </summary>
    private Target target;

    /// <summary>
    /// バッタークラス
    /// </summary>
    private Batter batter;

    /// <summary>
    /// 投げるオブジェクトの角度
    /// </summary>
    private const float shotAngle = 30;

    /// <summary>
    /// 投げる強さ
    /// </summary>
    private const float shotPower = 1.0f;

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

    private void Awake()
    {
        animator = gameObject.GetComponent<Animator>();
        batter = batterObj.GetComponent<Batter>();
    }

    /// <summary>
    /// 所持しているターゲットを投げる
    /// </summary>
    public void OnThrowEvent()
    {
        // ターゲットを保持していない場合は処理を飛ばす
        if (target == null)
        {
            return;
        }

        target.SetActive(true);
        // 初期位置に設定
        var position = new Vector3(gameObject.transform.position.x - 0.2f,
                                    gameObject.transform.position.y + 1.0f,
                                    gameObject.transform.position.z);
        target.Respawn(position);

        // ターゲットを狙った位置へ放物線を描いて飛ばす
        target.MoveParabola(strikePosition,shotPower, shotAngle);

        // 投げる音を鳴らす
        AudioSource.PlayClipAtPoint(soundShot, gameObject.transform.position);

        // 投げ終わったのでターゲットの保持をやめる
        target = null;
    }

    /// <summary>
    /// 渡されたターゲットを保持する
    /// </summary>
    /// <param name="target">ターゲット</param>
    public void Add(Target target)
    {
        this.target = target;

        // 投球開始
        // 投球モーションに切り替える
        animator.SetTrigger(animatorTriggerPitching);
    }

    /// <summary>
    /// バッターの足上げ
    /// </summary>
    public void BatterLegUp()
    {
        batter.LegUp();
    }
}
