using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

public class StagingManager : MonoBehaviour
{
    /// <summary>
    /// Main Cameraのオブジェクト
    /// </summary>
    [SerializeField] private GameObject mainCamera;

    /// <summary>
    /// Follow Cameraのオブジェクト
    /// </summary>
    [SerializeField] private GameObject[] followCameras;

    /// <summary>
    /// Button Swingのオブジェクト
    /// </summary>
    [SerializeField] private GameObject buttonSwing;

    /// <summary>
    /// Follow Cameraクラス
    /// </summary>
    private FollowCamera followCamera;

    /// <summary>
    /// Follow Camera保持テーブル
    /// </summary>
    private List<FollowCamera> followCameraPool = new List<FollowCamera>();

    private void Start()
    {
        //すべてのFollow Cameraをプールに保持する
        foreach (var followCameraObj in followCameras)
        {
            followCameraObj.SetActive(false);
            followCamera = followCameraObj.GetComponent<FollowCamera>();
            followCameraPool.Add(followCamera);
        }
    }

    /// <summary>
    /// Follow Cameraに切り替える
    /// </summary>
    /// <param name="target">Follow対象のターゲット</param>
    public void SwitchFollowCamera(Target target)
    {
        mainCamera.SetActive(false);
        buttonSwing.SetActive(false);
        followCamera = SelectRandomFollowCamera();
        followCamera.SetActive(true);
        followCamera.ResetAngle();
        followCamera.FollowTarget(target);
    }

    /// <summary>
    /// Main Cameraに切り替える
    /// </summary>
    public void SwitchMainCamera()
    {
        followCamera.CancelFollowTarget();
        followCamera.SetActive(false);
        buttonSwing.SetActive(true);
        mainCamera.SetActive(true);
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
        SwitchMainCamera();
    }
}
