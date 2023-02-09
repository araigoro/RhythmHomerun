using System.Collections;
using UnityEngine;

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

    /// <summary>
    /// イニシャライザ
    /// </summary>
    /// <param name="gameObject">Follow Cameraのオブジェクト</param>
    public FollowCamera(GameObject gameObject)
    {
        followCamera = gameObject;
    }

    /// <summary>
    /// ターゲットをカメラで追う
    /// </summary>
    /// <param name="target">対象のターゲット</param>
    public void FollowTarget(Target target)
    {
        // targetObj=target.getObj();
        // var relativePos = targetObj.transform.position - this.transform.position;
        // Quaternion rotation = Quaternion.LookRotation(relativePos);
        // transform.rotation = Quaternion.Slerp(this.transform.rotation, rotation, followSpeed);
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
