using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseballBat : MonoBehaviour
{
    /// <summary>
    /// 打撃音
    /// </summary>
    [SerializeField] private AudioClip soundHit;

    /// <summary>
    /// Switch Cameraのオブジェクト
    /// </summary>
    [SerializeField] private GameObject switchCameraObj;

    /// <summary>
    /// ピッチングマシンクラス
    /// </summary>
    private PitchingMachine pitchingMachine;

    /// <summary>
    /// カメラ切替クラス
    /// </summary>
    private SwitchCamera switchCamera;

    /// <summary>
    /// 打ったオブジェクトを飛ばす目標地点（レフト方向）
    /// </summary>
    private readonly Vector3[] homerunPointLeft = { new Vector3(-27, 4, 35), new Vector3(-25, 4, 43), new Vector3(-20, 4, 47) };

    /// <summary>
    /// 打ったオブジェクトを飛ばす目標地点（センタ0方向）
    /// </summary>
    private readonly Vector3[] homerunPointCenter = { new Vector3(0, 5, 45), new Vector3(-10, 4, 40), new Vector3(7, 3, 48) };

    /// <summary>
    /// 打ったオブジェクトを飛ばす目標地点（ライト方向）
    /// </summary>
    private readonly Vector3[] homerunPointRight = { new Vector3(31, 3, 30), new Vector3(13, 3, 47), new Vector3(23, 4, 48) };

    /// <summary>
    /// オブジェクトの打ち出し角度
    /// </summary>
    private const float hitAngle = 45;

    /// <summary>
    /// オブジェクトを打つ強さ
    /// </summary>
    private const float hitPower = 1.0f;

    /// <summary>
    ///　打ったオブジェクトをレフトに飛ばすかどうかの基準値
    /// </summary>
    private const float borderLeftDirection = 0.2f;

    /// <summary>
    /// 打ったオブジェクトをライトに飛ばすかどうかの基準値
    /// </summary>
    private const float borderRightDirection = 0.0f;

    private void Awake()
    {
        //ピッチングマシンクラスを保持
        var gameObject = GameObject.Find("Pitching Machine");
        if (gameObject != null)
        {
            pitchingMachine = gameObject.GetComponent<PitchingMachine>();
        }

        //カメラ切替クラスを保持
        switchCamera = switchCameraObj.GetComponent<SwitchCamera>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        // ピッチングマシンのコンポーネントが取得できなかった場合は、ログ表示
        if (pitchingMachine == null)
        {
            Debug.Log("Not found!! Pitching Machine is null!!");
            return;
        }

        // 対象のGameObjectがターゲットオブジェクトか？
        Target target = pitchingMachine.FindTarget(collision.gameObject);
        if (target != null)
        {
            // 打つ！
            HitTarget(target);

            //サブカメラに切り替える
            switchCamera.SwitchFollowCamera(target);
        }
    }

    /// <summary>
    /// ターゲットオブジェクトを打つ
    /// </summary>
    /// <param name="target">対象のターゲットオブジェクト</param>
    private void HitTarget(Target target)
    {
        //打撃音を鳴らす
        AudioSource.PlayClipAtPoint(soundHit, transform.position);

        //予期せぬ衝突を防ぐためにコライダーを無効にする
        target.ColliderOff();

        //ターゲットオブジェクトを飛ばす先を取得
        var targetPosition = SelectTargetPoint(target);

        //ターゲットオブジェクトを放物線上に飛ばす
        target.MoveParabola(targetPosition, hitAngle);
    }

    /// <summary>
    /// ターゲットオブジェクトを飛ばす先の座標を選ぶ
    /// </summary>
    /// <param name="target">対象のターゲットオブジェクト</param>
    /// <returns>飛ばす先の座標（x,y,z）</returns>
    private Vector3 SelectTargetPoint(Target target)
    {
        //レフト方向に飛ばすか？
        if (IsLeftHit(target))
        {
            //レフト方向に設定した座標の中からランダムに選ぶ
            return SelectRandomPoint(homerunPointLeft);
        }

        //ライト方向に飛ばすか？
        if (IsRightHit(target))
        {
            //ライト方向に設定した座標の中からランダムに選ぶ
            return SelectRandomPoint(homerunPointRight);
        }

        //センター方向に設定した座標の中からランダムに選ぶ
        return SelectRandomPoint(homerunPointCenter);
    }

    /// <summary>
    /// 座標配列の中からランダムに１つ選ぶ
    /// </summary>
    /// <param name="homerunPoint">対象の座標配列</param>
    /// <returns>飛ばす先の座標（x,y,z）</returns>
    private Vector3 SelectRandomPoint(Vector3[] homerunPoint)
    {
        return homerunPoint[UnityEngine.Random.Range(0, homerunPoint.Length)];
    }

    /// <summary>
    /// 打ったターゲットオブジェクトをレフト方向に飛ばすか？
    /// </summary>
    /// <param name="target">対象のターゲットオブジェクト</param>
    /// <returns>true:飛ばす / false:飛ばさない</returns>
    private bool IsLeftHit(Target target)
    {
        return target.IsLargePositionZ(borderLeftDirection);
    }

    /// <summary>
    /// 打ったターゲットオブジェクトをライト方向に飛ばすか？
    /// </summary>
    /// <param name="target">対象のターゲットオブジェクト</param>
    /// <returns>true:飛ばす / false:飛ばさない</returns>
    private bool IsRightHit(Target target)
    {
        return target.IsSmallPositionZ(borderRightDirection);
    }


}
