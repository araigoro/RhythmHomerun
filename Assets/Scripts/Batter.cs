using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class Batter : MonoBehaviour
{
    /// <summary>
    /// バットのオブジェクト
    /// </summary>
    [SerializeField] private GameObject[] baseballBatObj;

    /// <summary>
    /// 現在アクティブになっているターゲット
    /// </summary>
    private Target activeTarget;

    /// <summary>
    /// バットのクラス
    /// </summary>
    private BaseballBat baseballBat;

    /// <summary>
    ///バットを選ぶ時の引数
    /// </summary>
    private int batIndex = 0;

    /// <summary>
    /// バッターのアニメーター
    /// </summary>
    private Animator animator;

    /// <summary>
    /// アニメーションで足をあげるタイミングでこいつをtrueにする
    /// </summary>
    private const string boolLegUp = "BoolLegUp";

    /// <summary>
    /// スイングアニメーションに遷移するためのトリガー名
    /// </summary>
    private const string triggerSwing = "TriggerSwing";

    /// <summary>
    /// 足を上げているモーションのモーション名
    /// </summary>
    private const string legUpMotion = "LegUpMotion_10";

    // Start is called before the first frame update
    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        //UIボタンが押された際は画面クリックが反応しないようにする
    #if UNITY_EDITOR
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }
    #else
        if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId)) {
            return;
        }
    #endif

        //画面クリックされたとき
        if (Input.GetMouseButtonDown(0))
        {
            //バッターが足を上げている状態のとき
            if (animator.GetCurrentAnimatorStateInfo(0).IsName(legUpMotion))
            {
                //スイングアニメーションに遷移するためのトリガーをオンにする
                animator.SetTrigger(triggerSwing);

                //足も下げる
                LegDown();
            }
        }
    }

    /// <summary>
    /// バットを読み込む
    /// </summary>
    private void LoadBaseballBat()
    {
        var bat = baseballBatObj[batIndex];
        bat.SetActive(true);
        baseballBat = bat.GetComponent<BaseballBat>();
        baseballBat.RegisterActiveTarget(activeTarget);
    }

    /// <summary>
    /// バットを切り替える
    /// </summary>
    public void SwitchBat()
    {
        baseballBatObj[batIndex].SetActive(false);
        batIndex = (batIndex + 1) % baseballBatObj.Length;
        LoadBaseballBat();
    }

    /// <summary>
    /// バットのコライダーをオン
    /// </summary>
    public void BatColliderOn()
    {
        baseballBat.ColliderOn();
    }

    /// <summary>
    /// バットのコライダーをオフ
    /// </summary>
    public void BatColliderOff()
    {
        baseballBat.ColldierOff();
    }

    /// <summary>
    /// アニメーションで足を上げる
    /// </summary>
    public void LegUp()
    {
        animator.SetBool(boolLegUp, true);
    }

    /// <summary>
    /// アニメーションで足を下げる
    /// </summary>
    public void LegDown()
    {
        animator.SetBool(boolLegUp, false);
    }

    /// <summary>
    /// 現在使用しているバットのタグ名を返す
    /// </summary>
    /// <returns></returns>
    public String usingBatTag()
    {
        return baseballBatObj[batIndex].tag;
    }

    /// <summary>
    /// アクティブになっているターゲットを認識する
    /// </summary>
    /// <param name="target">ターゲット</param>
    public void RecognizeActiveTarget(Target target)
    {
        activeTarget = target;
        LoadBaseballBat();
    }
}
