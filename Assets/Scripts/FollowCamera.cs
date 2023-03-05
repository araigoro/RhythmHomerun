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

    private Quaternion initialAngle;

    private GameObject targetObj;

    private Camera camera;

    /// <summary>
    /// イニシャライザ
    /// </summary>
    /// <param name="gameObject">Follow Cameraのオブジェクト</param>

    private void Start()
    {
        initialAngle = transform.rotation;
        camera = this.GetComponent<Camera>();
    }

    private void Update()
    {
        
    }

    private void LookTarget(GameObject targetObj)
    {
        var relativePos = targetObj.transform.position - this.gameObject.transform.position;
        var rotation = Quaternion.LookRotation(relativePos);
        this.gameObject.transform.rotation = Quaternion.Slerp(this.gameObject.transform.rotation, rotation, followSpeed);
        
        //メインに切り替えるときにコレを消さないと2回目以降ズームスピードが倍増するので処置必要
        camera.fieldOfView=camera.fieldOfView-0.1f;

    }

    /// <summary>
    /// ターゲットをカメラで追う
    /// </summary>
    /// <param name="target">対象のターゲット</param>
    public void FollowTarget(Target target)
    {
        targetObj= target.GetObj();
        camera.fieldOfView = 60;
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

    public void ResetAngle()
    {
        transform.rotation = initialAngle;
    }
}
