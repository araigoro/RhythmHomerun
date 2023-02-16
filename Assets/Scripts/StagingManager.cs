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

    [SerializeField] private GameObject targetManagerObj;

    /// <summary>
    /// Follow Cameraクラス
    /// </summary>
    private FollowCamera followCamera;

    /// <summary>
    /// Follow Camera保持テーブル
    /// </summary>
    private List<FollowCamera> followCameraPool = new List<FollowCamera>();

    private TargetManager targetManager;

    private Target activeTarget;

    private void Start()
    {
        //すべてのFollow Cameraをプールに保持する
        foreach (var gameObject in followCameras)
        {
            gameObject.SetActive(false);
            var followCamera = new FollowCamera(gameObject);
            followCameraPool.Add(followCamera);
        }

        targetManager = targetManagerObj.GetComponent<TargetManager>();

        targetManager.ObserveEveryValueChanged(manager => manager.ActiveTarget)
        .Where(target => target != null)
        .Subscribe(target => activeTarget = target);

        activeTarget.ObserveEveryValueChanged(target => target.Status)
        .Where(_ => activeTarget.IsHit())
        .Subscribe(_ => SwitchFollowCamera(activeTarget));

        activeTarget.ObserveEveryValueChanged(target => target.Status)
        .Where(_ => activeTarget.IsStandIn())
        .Subscribe(_ => generateHomerunEffect());
    }

    private void generateHomerunEffect()
    {
        Debug.Log("HOME RUN!");
    }

    void Update()
    {

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
        followCamera.SetDisplay(true);
        followCamera.FollowTarget(target);
    }

    /// <summary>
    /// Main Cameraに切り替える
    /// </summary>
    public void SwitchMainCamera()
    {
        followCamera.SetDisplay(false);
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
}
