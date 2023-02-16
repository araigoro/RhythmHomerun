using System;
using System.Collections;
using UnityEngine;
using UniRx;

public class FollowCamera
{
    /// <summary>
    /// Follow Cameraのオブジェクト
    /// </summary>
    private GameObject followCamera;

    /// <summary>
    /// ターゲットのオブジェクト
    /// </summary>
    private GameObject targetObj;

    /// <summary>
    /// ターゲットを追う速さ
    /// </summary>
    private const float followSpeed = 0.02f;

    private const float followInterval = 0.01f;

    /// <summary>
    /// イニシャライザ
    /// </summary>
    /// <param name="gameObject">Follow Cameraのオブジェクト</param>
    public FollowCamera(GameObject gameObject)
    {
        followCamera = gameObject;
    }

    private void lookTarget()
    {
        var relativePos = targetObj.transform.position - followCamera.transform.position;
        var rotation = Quaternion.LookRotation(relativePos);
        followCamera.transform.rotation = Quaternion.Slerp(followCamera.transform.rotation, rotation, followSpeed);
    }

    /// <summary>
    /// ターゲットをカメラで追う
    /// </summary>
    /// <param name="target">対象のターゲット</param>
    public void FollowTarget(Target target)
    {
        targetObj = target.GetObj();
        Observable.Interval(TimeSpan.FromSeconds(followInterval))
        .Subscribe(_ => lookTarget());
    }

    /// <summary>
    /// 表示/非表示を設定する
    /// </summary>
    /// <param name="isDisplay">true:表示 / false:非表示</param>
    public void SetDisplay(bool isDisplay)
    {
        followCamera.SetActive(isDisplay);
    }
}
