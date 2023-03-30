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
    /// 花火
    /// </summary>
    [SerializeField] private VisualEffect normalFirework;
    [SerializeField] private VisualEffect sidareFirework;

    /// <summary>
    /// 花火音
    /// </summary>
    private AudioSource soundFirework;

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

        soundFirework = GetComponent<AudioSource>();

        // 花火を停止
        normalFirework.SendEvent("StopPlay");
        sidareFirework.SendEvent("StopPlay");
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

        // 花火の演出開始
        normalFirework.transform.position = target.transform.position;
        sidareFirework.transform.position = target.transform.position;
        normalFirework.SendEvent("StartPlay");
        sidareFirework.SendEvent("StartPlay");

        // 花火音を鳴らす
        soundFirework.Play();

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

        normalFirework.SendEvent("StopPlay");
        sidareFirework.SendEvent("StopPlay");

        target.Stay();

        yield return new WaitForSeconds(1.0f);

        // 花火効果音を止める
        soundFirework.Stop();
    }

}
