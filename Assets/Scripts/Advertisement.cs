using UnityEngine;
using GoogleMobileAds.Api;
using System;

public class Advertisement : MonoBehaviour
{
    /// <summary>
    /// テスト用広告ユニットID
    /// </summary>
    #if UNITY_ANDROID
        private string adUnitId = "ca-app-pub-3940256099942544/1033173712";
    #elif UNITY_IPHONE
            private string adUnitId = "ca-app-pub-3940256099942544/4411468910";
    #else
          private string adUnitId = "unused";
    #endif

    /// <summary>
    /// インタースティシャル広告
    /// </summary>
    private InterstitialAd interstitialAd;

    /// <summary>
    /// アプリが起動した回数
    /// </summary>
    private int appStartCount;

    /// <summary>
    /// アプリを何回起動で広告を表示するか
    /// </summary>
    private const int showAdCount = 2;


    // Start is called before the first frame update
    void Start()
    {
        // アプリ起動回数を取得
        appStartCount = PlayerPrefs.GetInt("AppStartCount", 0);

        // アプリ起動回数を1増やす
        appStartCount++;
        PlayerPrefs.SetInt("AppStartCount", appStartCount);

        // AdMob SDKを初期化
        MobileAds.Initialize(initStatus => { });

        //広告を表示
        RequestInterstitial();
    }

    /// <summary>
    /// アプリが２回起動するごとに広告１回表示
    /// </summary>
    private void RequestInterstitial()
    {
        //広告を表示するタイミングでなければ処理スキップ
        if(appStartCount % showAdCount != 0)
        {
            return;
        }

        //広告を読み込む
        LoadInterstitialAd();

        //広告を表示する
        ShowAd();
    }

    /// <summary>
    /// 広告の読み込み
    /// </summary>
    private void LoadInterstitialAd()
    {
        //初期化
        if (interstitialAd != null)
        {
            interstitialAd.Destroy();
            interstitialAd = null;
        }

        Debug.Log("Loading the interstitial ad.");

        //リクエストの生成
        var adRequest = new AdRequest.Builder()
                .AddKeyword("unity-admob-sample")
                .Build();

        //リクエストを送って広告を読み込む
        InterstitialAd.Load(adUnitId, adRequest,
            (InterstitialAd ad, LoadAdError error) =>
            {
                //ロード失敗時
                if (error != null || ad == null)
                {
                    Debug.LogError("interstitial ad failed to load an ad " +
                                   "with error : " + error);
                    return;
                }

                Debug.Log("Interstitial ad loaded with response : "
                          + ad.GetResponseInfo());

                interstitialAd = ad;
            });
    }

    /// <summary>
    /// 広告の表示
    /// </summary>
    private void ShowAd()
    {
        //広告の読み込みが完了している場合に表示する
        if (interstitialAd != null && interstitialAd.CanShowAd())
        {
            Debug.Log("Showing interstitial ad.");
            interstitialAd.Show();
        }
        else
        {
            Debug.LogError("Interstitial ad is not ready yet.");
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
