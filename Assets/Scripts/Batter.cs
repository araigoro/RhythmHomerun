using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Batter : MonoBehaviour
{
    /// <summary>
    /// バットのオブジェクト
    /// </summary>
    [SerializeField] GameObject baseballBatObj;

    /// <summary>
    /// バットのクラス
    /// </summary>
    private BaseballBat baseballBat;

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
    void Start()
    {
        baseballBat = baseballBatObj.GetComponent<BaseballBat>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
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

                // FIXME: 振り遅れの場合に、次の投球時にスイングアニメーションが再生されてしまう
            }
        }
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
}
