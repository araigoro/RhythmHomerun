using System;
using System.Collections.Generic;
using UnityEngine;
using UniRx;


public class PitchingMachine : MonoBehaviour
{
    /// <summary>
    /// 自身のゲームオブジェクト
    /// </summary>
    [SerializeField] private GameObject pitchingMachine;

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

    private Animator animator;

    private const string TriggerPitching = "TriggerPitching";


    private void Awake()
    {
        animator = pitchingMachine.GetComponent<Animator>();
    }

    private void Update()
    {
        // 一定間隔で投げる
        if ((Time.frameCount % shotInterval) == 0)
        {
            Debug.Log("マシーン内のtarget:" + target);
            animator.SetTrigger(TriggerPitching);


            ShotTarget();
        }
    }

    /// <summary>
    /// 所持しているターゲットを投げる
    /// </summary>
    private void ShotTarget()
    {
        if (target != null)
        {
            // 初期位置に設定
            target.SetActive(true);
            target.Respawn(pitchingMachine.transform.position);
            target.MoveParabola(strikePosition, shotAngle);

            // 投げる音を鳴らす
            AudioSource.PlayClipAtPoint(soundShot, pitchingMachine.transform.position);

            LoseTarget();

            Debug.Log("SHOT");
        }
    }

    private void LoseTarget()
    {
        target = null;
    }

    public void Add(Target target)
    {
        this.target = target;
    }
}
