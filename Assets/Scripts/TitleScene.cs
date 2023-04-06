using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TitleScene : MonoBehaviour
{
    [SerializeField] private GameObject targetManagerObj;
    [SerializeField] private Image logoImage;
    [SerializeField] private TextMeshProUGUI tapStartText;

    private const float blinkTime = 1.0f; // 点滅する周期を設定する変数

    private TargetManager targetManager;

    /// <summary>
    /// ボタン入力待ちか？
    /// </summary>
    private bool isWaitingTap = true;

    // Start is called before the first frame update
    void Start()
    {
        targetManager = targetManagerObj.GetComponent<TargetManager>();

        StartCoroutine(BlinkText()); // 点滅アニメーションを実行する
    }

    // Update is called once per frame
    void Update()
    {
        if (isWaitingTap == true)
        {
            if (Input.GetMouseButtonDown(0))
            {
                isWaitingTap = false;
                StartCoroutine(Playball());
            }
        }
    }

    /// <summary>
    /// 文字を点滅させる
    /// </summary>
    IEnumerator BlinkText()
    {
        while (isWaitingTap)
        {
            // TextMeshProの表示・非表示を切り替える
            tapStartText.enabled = !tapStartText.enabled;

            // 点滅する時間だけ待機する
            yield return new WaitForSeconds(blinkTime);
        }
    }

    IEnumerator Playball()
    {
        const float fadeTime = 1.0f;

        // ロゴのフェードアウトアニメーションを開始する
        StartCoroutine(FadeOutImage(logoImage, fadeTime));

        // タップされたらテキストを強制的に非表示にして、フェード終了を待ってゲーム開始
        tapStartText.enabled = false;
        yield return new WaitForSeconds(fadeTime);

        // 画面がタップされたら開始
        gameObject.SetActive(false);

        targetManager.Playball();
    }

    /// <summary>
    /// Imageをフェードアウトする
    /// </summary>
    /// <param name="image">イメージ</param>
    /// <param name="fadeTime">フェードアウト時間</param>
    IEnumerator FadeOutImage(Image image, float fadeTime = 1.0f)
    {
        Color originalColor = image.color;
        float elapsedTime = 0f;
        while (elapsedTime < fadeTime)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / fadeTime); // 経過時間を[0, 1]の範囲にクランプする
            image.color = Color.Lerp(originalColor, Color.clear, t); // 色を補間する
            yield return null;
        }
    }
}
