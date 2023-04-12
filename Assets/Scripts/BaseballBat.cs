using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class BaseballBat : MonoBehaviour
{
    /// <summary>
    /// 打撃音
    /// </summary>
    [SerializeField] private AudioClip soundHit;

    [SerializeField] GameObject targetManagerObj;

    /// <summary>
    /// 打ったオブジェクトを飛ばす目標地点（レフト方向）
    /// </summary>
    private readonly Vector3[] homerunPointLeft = { new Vector3(-27, 0, 35), new Vector3(-25, 0, 35), new Vector3(-20, 0, 35) };

    /// <summary>
    /// 打ったオブジェクトを飛ばす目標地点（センター方向）
    /// </summary>
    private readonly Vector3[] homerunPointCenter = { new Vector3(0, 0, 35), new Vector3(-10, 0, 35), new Vector3(7, 0, 35) };

    /// <summary>
    /// 打ったオブジェクトを飛ばす目標地点（ライト方向）
    /// </summary>
    private readonly Vector3[] homerunPointRight = { new Vector3(31, 0, 35), new Vector3(13, 0, 35), new Vector3(23, 0, 35) };

    /// <summary>
    /// オブジェクトの打ち出し角度
    /// </summary>
    private const float hitAngle = 30;

    private const float hitPower = 1.0f;

    /// <summary>
    ///　打ったオブジェクトをレフトに飛ばすかどうかの基準値
    /// </summary>
    private const float borderLeftDirection = 0.2f;

    /// <summary>
    /// 打ったオブジェクトをライトに飛ばすかどうかの基準値
    /// </summary>
    private const float borderRightDirection = 0.0f;

    private Target activeTarget;

    private TargetManager targetManager;

    private Collider collider;

    private void Awake()
    {
        targetManager = targetManagerObj.GetComponent<TargetManager>();
        collider = this.GetComponent<Collider>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        // 対象のGameObjectがターゲットオブジェクトか？
        // 違和感
        Target collisionTarget = collision.gameObject.GetComponent<Target>();
        if (activeTarget == collisionTarget)
        {
            // 打つ！
            HitTarget(collisionTarget);
        }
    }

    public void RegisterTarget(Target target)
    {
        activeTarget = target;
    }

    public void ColliderOn()
    {
        collider.enabled = true;
    }

    public void ColldierOff()
    {
        collider.enabled = false;
    }

    /// <summary>
    /// ターゲットオブジェクトを打つ
    /// </summary>
    /// <param name="target">対象のターゲットオブジェクト</param>
    private void HitTarget(Target target)
    {
        //予期せぬ衝突を防ぐためにコライダーを無効にする
        ColldierOff();

        //ターゲットオブジェクトを飛ばす先を取得
        var targetPosition = SelectTargetPoint(target);

        target.Hit();

        //打撃音を鳴らす
        AudioSource.PlayClipAtPoint(soundHit, transform.position);

        //ターゲットオブジェクトを放物線状に飛ばす
        target.MoveParabola(targetPosition,hitPower, hitAngle);
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
