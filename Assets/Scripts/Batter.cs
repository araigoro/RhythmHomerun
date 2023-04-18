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

    // Start is called before the first frame update
    void Start()
    {
        baseballBat = baseballBatObj.GetComponent<BaseballBat>();
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
}
