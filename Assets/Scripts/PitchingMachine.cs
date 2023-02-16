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

    public ReactiveProperty<int> i = new ReactiveProperty<int>();

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

    private void Update()
    {
        // 一定間隔で投げる
        if ((Time.frameCount % shotInterval) == 0)
        {
            ShotTarget();
        }
    }

    /// <summary>
    /// ランダムでターゲットを取得して投げる
    /// </summary>
    private void ShotTarget()
    {
        if (target != null)
        {
            // 初期位置に設定
            target.Respawn(pitchingMachine.transform.position);
            target.SetDisplay(true);
            target.MoveParabola(strikePosition, shotAngle);

            // 投げる音を鳴らす
            AudioSource.PlayClipAtPoint(soundShot, pitchingMachine.transform.position);

            // 一定時間で消す
            StartCoroutine(target.Collect());
        }
    }

    public void Add(Target target)
    {
        this.target = target;
    }
}
