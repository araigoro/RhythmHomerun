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

        activeTarget.ObserveEveryValueChanged(target => target)
        .Where(target => target.IsWaitingShot())
        .Subscribe(target =>
        {
            pitchingMachine.Add(target);
            Debug.Log("マシーンadd");
            baseballBat.RegisterTarget(target);
        });

        var changedTargetStatus = activeTarget.ObserveEveryValueChanged(target => target.Status);

        changedTargetStatus
        .Where(_ => activeTarget.IsHit())
        .Subscribe(_ => stagingManager.SwitchFollowCamera(activeTarget));

        changedTargetStatus
        .Where(_ => activeTarget.IsStandIn())
        .Subscribe(_ =>
        {
            stagingManager.GenerateHomerunEffect(activeTarget);
            activeTarget.Stay();
        });

        activeTarget.OnCollisionEnterAsObservable()
        .Where(collision => collision.gameObject == fieldGround)
        .Subscribe(_ => activeTarget.Stay());

        // changedTargetStatus
        // .Where(_ => activeTarget.IsStay())
        // .Subscribe(_ =>
        // {
        //     activeTarget.SetActive(false);
        //     SelectRandomActiveTarget();
        //     Debug.Log("NextTarget");
        // });
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
        }
    }

    /// <summary>
    /// 非表示のターゲットの中からランダムに選ぶ
    /// </summary>
    /// <returns>ターゲット(選べなかった場合はnull)</returns>
    private Target SelectRandomTarget()
    {
        // 無限ループにならないように対処
        if (targetPool.Count == 0)
        {
            return null;
        }

        Target target;

        do
        {
            target = targetPool[1/*UnityEngine.Random.Range(0, targetPool.Count)*/];

        } while (activeTarget == target);

        return target;
    }
}
