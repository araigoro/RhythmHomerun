using System.Collections.Generic;
using UnityEngine;

public class TargetManager : MonoBehaviour
{
    /// <summary>
    /// ターゲットオブジェクトのプレハブ
    /// </summary>
    [SerializeField] private GameObject[] targetPrefabs;

    /// <summary>
    /// 投手のオブジェクト
    /// </summary>
    [SerializeField] private GameObject pitchingMachineObj;

    /// <summary>
    /// バットのオブジェクト
    /// </summary>
    [SerializeField] private GameObject baseballBatObj;

    /// <summary>
    /// ステージングマネージャーのオブジェクト
    /// </summary>
    [SerializeField] private GameObject stagingManagerObj;

    /// <summary>
    /// ターゲット保持テーブル
    /// </summary>
    private List<Target> targetPool = new List<Target>();

    /// <summary>
    /// 現在ゲームシーン上でアクティブになっているターゲット
    /// </summary>
    private Target activeTarget;

    /// <summary>
    /// 直近のターゲットの状態
    /// </summary>
    private Target.State latestTargetState = Target.State.Stay;

    /// <summary>
    /// 投手のクラス
    /// </summary>
    private PitchingMachine pitchingMachine;

    /// <summary>
    /// バットのクラス
    /// </summary>
    private BaseballBat baseballBat;

    /// <summary>
    /// ステージングマネージャーのクラス
    /// </summary>
    private StagingManager stagingManager;

    private void Awake()
    {
        pitchingMachine = pitchingMachineObj.GetComponent<PitchingMachine>();
        baseballBat = baseballBatObj.GetComponent<BaseballBat>();
        stagingManager = stagingManagerObj.GetComponent<StagingManager>();

        // すべてのターゲットプレハブを生成して、ターゲットプールに追加する
        CreateAllTargetPrefabs();
    }

    /// <summary>
    /// 投球開始
    /// </summary>
    public void Playball()
    {
        // メインカメラに切り替える
        stagingManager.SwitchMainCamera();

        // ランダムで1つアクティブにするターゲットを選ぶ
        SelectRandomActiveTarget();
    }

    /// <summary>
    /// ランダムで1つアクティブにするターゲットを選ぶ
    /// </summary>
    private void SelectRandomActiveTarget()
    {
        // ランダムで１つターゲットを選ぶ
        activeTarget = SelectRandomTarget();

        // ターゲットのステータスを投球待ちに変更
        activeTarget.WaitingShot();

        // 選ばれたターゲットを投手に渡す
        pitchingMachine.Add(activeTarget);

        // 選ばれたターゲットをバットに認識させる
        baseballBat.RegisterActiveTarget(activeTarget);
    }

    private void Update()
    {
        // ターゲットのステータスが変わっていない場合、処理をスキップ
        if (activeTarget.NowStatus == latestTargetState)
        {
            return;
        }

        // ステータスを更新
        latestTargetState = activeTarget.NowStatus;

        // ターゲットが打たれている時
        if (activeTarget.IsHit())
        {
            // 打球を追うカメラに切り替える
            stagingManager.SwitchFollowCamera(activeTarget.GetObj());
        }

        // ターゲットがスタンドインした時
        if (activeTarget.IsStandIn())
        {
            // ホームラン演出を生成
            stagingManager.GenerateHomerunEffect(activeTarget);

            // ターゲットの速度を0にする
            activeTarget.ResetVelocity();
        }

        // ターゲットが待機ステータスになった時
        if (activeTarget.IsStay())
        {
            // メインカメラに切り替える
            stagingManager.SwitchMainCamera();

            // アクティブになっているターゲットを初期位置に戻す
            activeTarget.Respawn(transform.position);
            activeTarget.SetActive(false);

            // 新しいターゲットを選ぶ
            SelectRandomActiveTarget();
        }
    }

    /// <summary>
    /// すべてのターゲットプレハブを生成して、ターゲットプールに追加する
    /// </summary>
    private void CreateAllTargetPrefabs()
    {
        // プールの中身をリセット
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
            // プールからランダムに１つ選ぶ
            target = targetPool[UnityEngine.Random.Range(0, targetPool.Count)];

        } while (activeTarget == target);

        return target;
    }
}
