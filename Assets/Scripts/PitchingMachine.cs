using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PitchingMachine : MonoBehaviour
{
    /// <summary>
    /// 自身のゲームオブジェクト
    /// </summary>
    [SerializeField] private GameObject pitchingMachine;
    
    /// <summary>
    /// ターゲットとして投げるオブジェクトのプレハブ
    /// </summary>
    [SerializeField] private GameObject[] targetPrefabs;
    
    /// <summary>
    /// ショット音
    /// </summary>
    [SerializeField] private AudioClip soundShot;

    /// <summary>
    /// ターゲット保持テーブル
    /// </summary>
    private List<Target> targetPool = new List<Target>();

    /// <summary>
    /// 投げる間隔(単位：フレーム)
    /// </summary>
    private const int ShotInterval = 2000;

    /// <summary>
    /// 投げるオブジェクトの速度
    /// </summary>
    private const float ShotSpeed = 270;

    /// <summary>
    /// 投げるオブジェクトの角度
    /// </summary>
    private const float ShotAngle = 30;


    private Vector3 STRIKE_POSITION = new Vector3(0, 3, -1) / 10;

    private void Awake()
    {
        // すべてのターゲットプレハブを生成して、ターゲットプールに追加する
        CreateAllTargetPrefabs();
    }

    private void Update()
    {
        // 一定間隔で投げる
        if ((Time.frameCount % ShotInterval) == 0)
        {
            ShotTarget();
        }
    }
    
    /// <summary>
    /// ランダムでターゲットを選んで投げる
    /// </summary>
    private void ShotTarget()
    {
        // ランダムで次に投げるターゲットを選ぶ
        var target = SelectRandomTarget();
        
        if (target != null)
        {
            // 初期位置に設定
            target.Reposition(pitchingMachine.transform.position);
            target.SetDisplay(true);
            target.MoveParabola(STRIKE_POSITION, ShotAngle, ShotSpeed);

            // 投げる音を鳴らす
            AudioSource.PlayClipAtPoint(soundShot, pitchingMachine.transform.position);

            // 一定時間で消す
            StartCoroutine(target.Collect());
        }
    }

    /// <summary>
    /// すべてのターゲットプレハブを生成して、ターゲットプールに追加する
    /// </summary>
    private void CreateAllTargetPrefabs()
    {
        targetPool.Clear();

        // 登録されているすべてのターゲットプレハブに紐づけた、Targetを生成する
        for (int index = 0; index < targetPrefabs.Length; index++)
        {
            // ターゲットを生成
            var gameObject = Instantiate(targetPrefabs[index], transform.position, Quaternion.identity);
            Target target = new Target(gameObject);
            target.SetDisplay(false);

            // ターゲットプールに追加
            targetPool.Add(target);
        }
    }

    /// <summary>
    /// 非表示のターゲットの中からランダムに選ぶ
    /// </summary>
    /// <returns>ターゲット(選べなかった場合はnull)</returns>
    private Target SelectRandomTarget()
    {
        // 無限ループにならないように対処
        if (targetPool.Count == 0)
        {
            return null;
        }

        // ランダムに選ぶ
        Target target;
        do
        {
            target = targetPool[UnityEngine.Random.Range(0, targetPool.Count)];
        } while (target.IsDisplay == false);

        return target;
    }
}
