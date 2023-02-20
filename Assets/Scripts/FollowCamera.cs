using System;
using System.Collections;
using UnityEngine;
using UniRx;

public class FollowCamera : MonoBehaviour
{
    /// <summary>
    /// ターゲットを追う速さ
    /// </summary>
    private const float followSpeed = 0.02f;

    private const float followInterval = 0.01f;

    /// <summary>
    /// イニシャライザ
    /// </summary>
    /// <param name="gameObject">Follow Cameraのオブジェクト</param>

    private void LookTarget(GameObject targetObj)
    {
        var relativePos = targetObj.transform.position - this.gameObject.transform.position;
        var rotation = Quaternion.LookRotation(relativePos);
        this.gameObject.transform.rotation = Quaternion.Slerp(this.gameObject.transform.rotation, rotation, followSpeed);
    }

    /// <summary>
    /// ターゲットをカメラで追う
    /// </summary>
    /// <param name="target">対象のターゲット</param>
    public void FollowTarget(Target target)
    {
        var targetObj = target.GetObj();
        Observable.Interval(TimeSpan.FromSeconds(followInterval))
        .Subscribe(_ => LookTarget(targetObj));
    }

    /// <summary>
    /// 表示/非表示を設定する
    /// </summary>
    /// <param name="isActive">true:表示 / false:非表示</param>
    public void SetActive(bool isActive)
    {
        this.gameObject.SetActive(isActive);
    }
}
