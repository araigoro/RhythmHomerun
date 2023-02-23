using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using System;

public class TargetManager : MonoBehaviour
{
    /// <summary>
    /// ターゲットオブジェクトのプレハブ
    /// </summary>
    [SerializeField] private GameObject[] targetPrefabs;

    [SerializeField] private GameObject pitchingMachineObj;

    [SerializeField] private GameObject baseballBatObj;

    [SerializeField] private GameObject stagingManagerObj;

    [SerializeField] private GameObject fieldGround;

    /// <summary>
    /// ターゲット保持テーブル
    /// </summary>
    private List<Target> targetPool = new List<Target>();

    private Target activeTarget;

    private PitchingMachine pitchingMachine;

    private BaseballBat baseballBat;

    private StagingManager stagingManager;

    private void Awake()
    {
        pitchingMachine = pitchingMachineObj.GetComponent<PitchingMachine>();
        baseballBat = baseballBatObj.GetComponent<BaseballBat>();
        stagingManager = stagingManagerObj.GetComponent<StagingManager>();

        // すべてのターゲットプレハブを生成して、ターゲットプールに追加する
        CreateAllTargetPrefabs();

        SelectRandomActiveTarget();
    }

    private void SelectRandomActiveTarget()
    {
        activeTarget = SelectRandomTarget();
        activeTarget.WaitingShot();

        Debug.Log("selectTarget");
    }

    private void Update()
    {
        if ((Time.frameCount % 1000) == 0)
        {
            Debug.Log("マネージャー内:" + activeTarget + ":" + activeTarget.Status);
        }
    }

    /// <summary>
    /// すべてのターゲットプレハブを生成して、ターゲットプールに追加する
    /// </summary>
    private void CreateAllTargetPrefabs()
    {
        targetPool.Clear();

        // 登録されているすべてのターゲットを生成する
        for (var index = 0; index < targetPrefabs.Length; index++)
        {
            //ターゲット生成
            var targetObj = Instantiate(targetPrefabs[index], transform.position, Quaternion.identity);
            var target = targetObj.GetComponent<Target>();

            target.SetActive(false);
            targetPool.Add(target);

            var changedTargetStatus = target.ObserveEveryValueChanged(target => target.Status);

            changedTargetStatus
            .Where(_ => target.IsWaitingShot())
            .Subscribe(_ =>
            {
                pitchingMachine.Add(target);
                Debug.Log("マシーンadd");
                baseballBat.RegisterTarget(target);
            });

            changedTargetStatus
            .Where(_ => target.IsHit())
            .Subscribe(_ => stagingManager.SwitchFollowCamera(target));

            changedTargetStatus
            .Where(_ => target.IsStandIn())
            .Subscribe(_ =>
            {
                stagingManager.GenerateHomerunEffect(target);
                target.Stay();
                target.SetActive(false);
                SelectRandomActiveTarget();
                Debug.Log("NextTarget");
            });

            target.OnCollisionEnterAsObservable()
            .Where(collision => collision.gameObject == fieldGround)
            .Subscribe(_ =>
            {
                target.Stay();
                target.SetActive(false);
                SelectRandomActiveTarget();
                Debug.Log("NextTarget");
            });

            // changedTargetStatus
            // .Where(_ => target.IsStay())
            // .Subscribe(_ =>
            // {
            //     target.SetActive(false);
            //     SelectRandomActiveTarget();
            //     Debug.Log("NextTarget");
            // });
        }
    }

    /// <summary>
    /// 非表示のターゲットの中からランダムに選ぶ
    /// </summary>
    /// <returns>ターゲット(選べなかった場合はnull)</returns>
    private Target SelectRandomTarget()
    {

        Debug.Log(targetPool.Count);

        // 無限ループにならないように対処
        if (targetPool.Count == 0)
        {
            return null;
        }

        Target target;

        do
        {
            target = targetPool[UnityEngine.Random.Range(0, targetPool.Count)];

        } while (activeTarget == target);

        return target;
    }
}
