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
    /// Follow Cameraクラス
    /// </summary>
    //private FollowCamera followCamera;

    private CinemachineVirtualCamera followVCamera;

    /// <summary>
    /// 花火、紙吹雪演出
    /// </summary>
    [SerializeField] private VisualEffect[] visualEffects;

    /// <summary>
    /// Follow Camera保持テーブル
    /// </summary>
    private List<FollowCamera> followCameraPool = new List<FollowCamera>();

    private void Start()
    {
        //すべてのFollow Cameraをプールに保持する
        //foreach (var followCameraObj in followCameras)
        //{
        //    followCameraObj.SetActive(false);
        //    followCamera = followCameraObj.GetComponent<FollowCamera>();
        //    followCameraPool.Add(followCamera);
        //}

        // VisualEffectを停止
        foreach (var visualEffect in visualEffects)
        {
            visualEffect.SendEvent("StopPlay");
        }

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
        followVCamera = SelectFollowVCamera();
        followVCamera.gameObject.SetActive(true);
        followVCamera.LookAt = targetObject.transform;

        //followCamera = SelectRandomFollowCamera();
        //followCamera.SetActive(true);
        //followCamera.ResetAngle();
        //followCamera.FollowTarget(target);
    }

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
        //followCamera.CancelFollowTarget();
        if (followVCamera != null)
        {
            followVCamera.LookAt = null;
            followVCamera.gameObject.SetActive(false);
        }
        buttonSwing.SetActive(true);
        mainVCamera.gameObject.SetActive(true);
    }

    /// <summary>
    /// ランダムにFollow Cameraを選択する
    /// </summary>
    /// <returns>Follow Camera（選べなかった場合はnull）</returns>
    private FollowCamera SelectRandomFollowCamera()
    {
        //無限ループ防止
        if (followCameraPool.Count == 0)
        {
            return null;
        }

        return followCameraPool[UnityEngine.Random.Range(0, followCameraPool.Count)];
    }

    public void GenerateHomerunEffect(Target target)
    {
        Debug.Log("HOMERUN!!");

        // VisualEffectの演出開始
        foreach (var visualEffect in visualEffects)
        {
            visualEffect.transform.position = target.transform.position;
            visualEffect.SendEvent("StartPlay");
        }

        // 一定時間で消す(強引…)
        StartCoroutine(ProcessingHomerunEffect(target));
    }

    /// <summary>
    /// 一定時間後にオブジェクトを非表示にするコルーチン
    /// </summary>
    /// <returns>IEnumerator</returns>
    public IEnumerator ProcessingHomerunEffect(Target target)
    {
        yield return new WaitForSeconds(0.5f);

        //// VisualEffectを停止
        //foreach (var visualEffect in visualEffects)
        //{
        //    visualEffect.SendEvent("StopPlay");
        //}

        //yield return new WaitForSeconds(1.0f);

        target.Stay();
    }

}
