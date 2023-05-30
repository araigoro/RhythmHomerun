using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TitleScene : MonoBehaviour
{
    /// <summary>
    /// ターゲットマネージャーのオブジェクト
    /// </summary>
    [SerializeField] private GameObject targetManagerObj;

    /// <summary>
    /// タイトルのロゴ画像
    /// </summary>
    [SerializeField] private Image logoImage;

    /// <summary>
    /// 開始テキスト
    /// </summary>
    [SerializeField] private TextMeshProUGUI tapStartText;

    /// <summary>
    /// バットを切り替えるUIパネル
    /// </summary>
    [SerializeField] private GameObject switchBatPanel;

    /// <summary>
    /// 開始テキストが点滅する周期を設定
    /// </summary>
    private const float blinkTime = 1.0f;

    /// <summary>
    /// ターゲットマネージャーのクラス
    /// </summary>
    private TargetManager targetManager;

    /// <summary>
    /// ゲーム開始待ちか？
    /// </summary>
    private bool isWaitingTap = true;

    // Start is called before the first frame update
    void Start()
    {
        // ターゲットマネージャークラスを取得
        targetManager = targetManagerObj.GetComponent<TargetManager>();

        // 開始テキストの点滅アニメーションを実行する
        StartCoroutine(BlinkText());
    }

    // Update is called once per frame
    void Update()
    {
        // 既にゲームが開始されている場合、処理を飛ばす
        if (isWaitingTap == false)
        {
            return;
        }

        // マウスが左クリックされた場合
        if (Input.GetMouseButtonDown(0))
        {
            // 開始待ちフラグをfalseにする
            isWaitingTap = false;

            // ゲーム画面に切り替える
            StartCoroutine(Playball());
        }
    }

    /// <summary>
    /// 文字を点滅させる
    /// </summary>
    IEnumerator BlinkText()
    {
        // ゲームを開始していない間
        while (isWaitingTap)
        {
            // TextMeshProの表示・非表示を切り替える
            tapStartText.enabled = !tapStartText.enabled;

            // 点滅する時間だけ待機する
            yield return new WaitForSeconds(blinkTime);
        }
    }

    /// <summary>
    /// ゲーム画面へと切り替える
    /// </summary>
    IEnumerator Playball()
    {
        // ロゴがフェードアウトする時間
        const float fadeTime = 1.0f;

        // ロゴのフェードアウトアニメーションを開始する
        StartCoroutine(FadeOutImage(logoImage, fadeTime));

        // タップされたらテキストを強制的に非表示にして、フェード終了を待ってゲーム開始
        tapStartText.enabled = false;
        yield return new WaitForSeconds(fadeTime);

        // タイトル表示を消す
        gameObject.SetActive(false);

        // ゲームを開始する
        targetManager.Playball();

        //バット切り替えUIを表示
        switchBatPanel.SetActive(true);
    }

    /// <summary>
    /// Imageをフェードアウトする
    /// </summary>
    /// <param name="image">イメージ</param>
    /// <param name="fadeTime">フェードアウト時間</param>
    IEnumerator FadeOutImage(Image image, float fadeTime = 1.0f)
    {
        // イメージの色を取得
        Color originalColor = image.color;

        // 経過時間
        float elapsedTime = 0f;

        // フェードアウト時間に到達するまでの間
        while (elapsedTime < fadeTime)
        {
            // 経過時間を加算
            elapsedTime += Time.deltaTime;

            // 経過時間を[0, 1]の範囲にクランプする
            float t = Mathf.Clamp01(elapsedTime / fadeTime);

            // 色を補間する
            image.color = Color.Lerp(originalColor, Color.clear, t);
            yield return null;
        }
    }
}
