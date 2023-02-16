using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

public class TargetManager : MonoBehaviour
{
    /// <summary>
    /// ターゲットオブジェクトのプレハブ
    /// </summary>
    [SerializeField] private GameObject[] targetPrefabs;

    [SerializeField] private GameObject pitchingMachineObj;

    /// <summary>
    /// ターゲット保持テーブル
    /// </summary>
    private List<Target> targetPool = new List<Target>();

    public Target ActiveTarget;

    private PitchingMachine pitchingMachine;

    private void Awake()
    {

        pitchingMachine = pitchingMachineObj.GetComponent<PitchingMachine>();

        // すべてのターゲットプレハブを生成して、ターゲットプールに追加する
        CreateAllTargetPrefabs();

        ActiveTarget = SelectRandomTarget();

        ActiveTarget.ObserveEveryValueChanged(target => target)
        .Where(target => target != null)
        .Subscribe(target => pitchingMachine.Add(target));
    }

    void Update()
    {

    }

    /// <summary>
    /// すべてのターゲットプレハブを生成して、ターゲットプールに追加する
    /// </summary>
    private void CreateAllTargetPrefabs()
    {
        targetPool.Clear();

        // 登録されているすべてのターゲットプレハブに紐づけた、Targetを生成する
        for (var index = 0; index < targetPrefabs.Length; index++)
        {
            // ターゲットを生成
            var gameObject = Instantiate(targetPrefabs[index], transform.position, Quaternion.identity);
            var target = new Target(gameObject);
            target.SetDisplay(false);

            // ターゲットプールに追加
            targetPool.Add(target);
        }
    }

    /// <summary>
    /// 非表示のターゲットの中からランダムに選ぶ
    /// </summary>
    /// <returns>ターゲット(選べなかった場合はnull)</returns>
    public Target SelectRandomTarget()
    {
        // 無限ループにならないように対処
        if (targetPool.Count == 0)
        {
            return null;
        }

        // ランダムに選ぶ
        Target target;
        do
        {
            target = targetPool[UnityEngine.Random.Range(0, targetPool.Count)];
        } while (target.IsDisplay == false);

        return target;
    }

    /// <summary>
    /// 指定されたGameObjectのターゲットを取得
    /// </summary>
    /// <param name="gameObject">対象のGameObject</param>
    /// <returns>ターゲット(見つからない場合は null)</returns>
    public Target FindTarget(GameObject gameObject)
    {
        // ターゲットプールから、対象のGameObjectを持つターゲットを探す
        foreach (var target in targetPool)
        {
            if (target.TargetGameObject == gameObject)
            {
                return target;
            }
        }

        // 見つからなかった
        return null;
    }
}
