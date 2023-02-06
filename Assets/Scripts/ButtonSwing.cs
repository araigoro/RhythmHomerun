using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSwing : MonoBehaviour
{
    /// <summary>
    /// バッターのゲームオブジェクト
    /// </summary>
    [SerializeField] private GameObject batter;

    /// <summary>
    /// バッターに設定されているアニメーション
    /// </summary>
    private Animator animator;

    /// <summary>
    /// スイングアニメーションに遷移するためのトリガー名
    /// </summary>
    private const string triggerSwing = "TriggerSwing";

    void Start()
    {
        //コンポーネントを取得して保持しておく
        animator = batter.GetComponent<Animator>();
    }

    /// <summary>
    /// 画面上の「スイング」ボタンを押下した際に、バッターにスイングさせる
    /// </summary>
    public void OnClick()
    {
        //スイングアニメーションに遷移するためのトリガーをオンにする
        animator.SetTrigger(triggerSwing);
    }

}
