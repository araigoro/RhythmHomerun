using UnityEngine;
using GoogleMobileAds.Api;
using System;

public class Advertisement : MonoBehaviour
{
    /// <summary>
    /// �e�X�g�p�L�����j�b�gID
    /// </summary>
    #if UNITY_ANDROID
        private string adUnitId = "ca-app-pub-3940256099942544/1033173712";
    #elif UNITY_IPHONE
            private string adUnitId = "ca-app-pub-3940256099942544/4411468910";
    #else
          private string adUnitId = "unused";
    #endif

    /// <summary>
    /// �C���^�[�X�e�B�V�����L��
    /// </summary>
    private InterstitialAd interstitialAd;

    /// <summary>
    /// �A�v�����N��������
    /// </summary>
    private int appStartCount;

    /// <summary>
    /// �A�v��������N���ōL����\�����邩
    /// </summary>
    private const int showAdCount = 2;


    // Start is called before the first frame update
    void Start()
    {
        // �A�v���N���񐔂��擾
        appStartCount = PlayerPrefs.GetInt("AppStartCount", 0);

        // �A�v���N���񐔂�1���₷
        appStartCount++;
        PlayerPrefs.SetInt("AppStartCount", appStartCount);

        // AdMob SDK��������
        MobileAds.Initialize(initStatus => { });

        //�L����\��
        RequestInterstitial();
    }

    /// <summary>
    /// �A�v�����Q��N�����邲�ƂɍL���P��\��
    /// </summary>
    private void RequestInterstitial()
    {
        //�L����\������^�C�~���O�łȂ���Ώ����X�L�b�v
        if(appStartCount % showAdCount != 0)
        {
            return;
        }

        //�L����ǂݍ���
        LoadInterstitialAd();

        //�L����\������
        ShowAd();
    }

    /// <summary>
    /// �L���̓ǂݍ���
    /// </summary>
    private void LoadInterstitialAd()
    {
        //������
        if (interstitialAd != null)
        {
            interstitialAd.Destroy();
            interstitialAd = null;
        }

        Debug.Log("Loading the interstitial ad.");

        //���N�G�X�g�̐���
        var adRequest = new AdRequest.Builder()
                .AddKeyword("unity-admob-sample")
                .Build();

        //���N�G�X�g�𑗂��čL����ǂݍ���
        InterstitialAd.Load(adUnitId, adRequest,
            (InterstitialAd ad, LoadAdError error) =>
            {
                //���[�h���s��
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
    /// �L���̕\��
    /// </summary>
    private void ShowAd()
    {
        //�L���̓ǂݍ��݂��������Ă���ꍇ�ɕ\������
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
