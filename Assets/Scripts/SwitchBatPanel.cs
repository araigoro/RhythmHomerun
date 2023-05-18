using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SwitchBatPanel : MonoBehaviour
{

    /// <summary>
    /// バッターのオブジェクト
    /// </summary>
    [SerializeField] private GameObject batterObj;

    /// <summary>
    /// バットの種類を表示するテキストオブジェクト
    /// </summary>
    [SerializeField] private GameObject batTypeTextObj;

    /// <summary>
    /// バッターのクラス
    /// </summary>
    private Batter batter;

    /// <summary>
    /// バットの種類を表示するテキスト
    /// </summary>
    private TextMeshProUGUI batTypeText;

    /// <summary>
    /// 現在使用しているバットの種類
    /// </summary>
    private string usingBatType;

    // Start is called before the first frame update
    void Start()
    {
        batter = batterObj.GetComponent<Batter>();
        batTypeText = batTypeTextObj.GetComponent<TextMeshProUGUI>();
        LoadUsingBatTag();
        ShowUsingBatName();
    }

    /// <summary>
    /// 矢印ボタンが押されたらバッターのバットを切り替える
    /// </summary>
    public void SwitchBat()
    {
        batter.SwitchBat();
        LoadUsingBatTag();
        ShowUsingBatName();
    }

    /// <summary>
    /// 現在使用しているバットをテキストに表示
    /// </summary>
    private void ShowUsingBatName()
    {
        batTypeText.text = usingBatType;
    }

    /// <summary>
    /// バッターが現在使用しているバットの名前を取得
    /// </summary>
    private void LoadUsingBatTag()
    {
        usingBatType = batter.ReturnUsingBatType();
    }
}
