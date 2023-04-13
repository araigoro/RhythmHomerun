using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using UniRx;
using System;
using Cinemachine;

public class StagingManager : MonoBehaviour
{
    /// <summary>
    /// Main Cameraのオブジェクト
    /// </summary>
    [SerializeField] private CinemachineVirtualCamera mainVCamera;

    /// <summary>
    /// Follow Cameraのオブジェクト
    /// </summary>
    [SerializeField] private CinemachineVirtualCamera[] followVCameras;

    /// <summary>
    /// Button Swingのオブジェクト
    /// </summary>
    [SerializeField] private GameObject buttonSwing;

    /// <summary>
    /// 打球を追う用カメラのシネマシーンカメラクラス
    /// </summary>
    private CinemachineVirtualCamera followVCamera;

    /// <summary>
    /// 花火
    /// </summary>
    [SerializeField] private VisualEffect normalFirework;
    [SerializeField] private VisualEffect sidareFirework;

    private void Start()
    {
        // 花火を停止
        normalFirework.SendEvent("StopPlay");
        sidareFirework.SendEvent("StopPlay");

        // スイングボタンは非表示
        buttonSwing.SetActive(false);
    }

    /// <summary>
    /// Follow Cameraに切り替える
    /// </summary>
    /// <param name="target">Follow対象のターゲット</param>
    public void SwitchFollowCamera(GameObject targetObject)
    {
        mainVCamera.gameObject.SetActive(false);
        buttonSwing.SetActive(false);

        // 打球を追う用のカメラを取得
        followVCamera = SelectFollowVCamera();
        followVCamera.gameObject.SetActive(true);

        // 渡されたターゲットを追う
        followVCamera.LookAt = targetObject.transform;
    }

    /// <summary>
    /// 打球を追う用のシネマカメラをランダムに選ぶ
    /// </summary>
    /// <returns></returns>
    private CinemachineVirtualCamera SelectFollowVCamera()
    {
        //無限ループ防止
        if (followVCameras.Length == 0)
        {
            return null;
        }

        return followVCameras[UnityEngine.Random.Range(0, followVCameras.Length)];
    }

    /// <summary>
    /// Main Cameraに切り替える
    /// </summary>
    public void SwitchMainCamera()
    {
        if (followVCamera != null)
        {
            // ターゲットを追うのをやめる
            followVCamera.LookAt = null;
            followVCamera.gameObject.SetActive(false);
        }

        mainVCamera.gameObject.SetActive(true);

        // スイングボタンを表示する
        buttonSwing.SetActive(true);
    }


    /// <summary>
    /// 花火エフェクトの生成
    /// </summary>
    /// <param name="target">ターゲット</param>
    public void GenerateHomerunEffect(Target target)
    {
        // ターゲットの着弾点に花火の演出開始
        normalFirework.transform.position = target.transform.position;
        sidareFirework.transform.position = target.transform.position;
        normalFirework.SendEvent("StartPlay");
        sidareFirework.SendEvent("StartPlay");

        // 一定時間で消す(強引…)
        StartCoroutine(ProcessingHomerunEffect(target));
    }

    /// <summary>
    /// 一定時間後にオブジェクトを非表示にするコルーチン
    /// </summary>
    /// <returns>IEnumerator</returns>
    public IEnumerator ProcessingHomerunEffect(Target target)
    {
        yield return new WaitForSeconds(2.0f);

        normalFirework.SendEvent("StopPlay");
        sidareFirework.SendEvent("StopPlay");

        yield return new WaitForSeconds(3.0f);

        // ターゲットのステータスを変更
        target.Stay();
    }

}
