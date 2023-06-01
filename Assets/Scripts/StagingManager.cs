using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using UniRx;
using System;
using Cinemachine;

public class StagingManager : MonoBehaviour
{
    /// <summary>
    /// Main Cameraのオブジェクト
    /// </summary>
    [SerializeField] private CinemachineVirtualCamera mainVCamera;

    /// <summary>
    /// Follow Cameraのオブジェクト
    /// </summary>
    [SerializeField] private CinemachineVirtualCamera[] followVCameras;

    /// <summary>
    /// 打球を追う用カメラのシネマシーンカメラクラス
    /// </summary>
    private CinemachineVirtualCamera followVCamera;

    /// <summary>
    /// VisualEffect演出用プレハブ
    /// </summary>
    [SerializeField] private VisualEffect[] visualEffects;

    /// <summary>
    /// 生成されたアクティブなエフェクト用オブジェクト
    /// </summary>
    private List<VisualEffect> activeEffects = new List<VisualEffect>();

    /// <summary>
    /// VisualEffectを生成して表示する
    /// </summary>
    /// <param name="position">生成する座標</param>
    public void CreateVisualEffect(Vector3 position)
    {
        // 近いものを探す
        if (FindVisualEffect(position, 1.0f) == null) {
            // 近いものが無ければ、新たに生成する
            var prefab = visualEffects[UnityEngine.Random.Range(0, visualEffects.Length)];
            var visualEffect = Instantiate(prefab, position, Quaternion.identity);
            visualEffect.SendEvent("StartPlay");
            activeEffects.Add(visualEffect);
        }
    }
    
    /// <summary>
    /// 指定された位置に近い花火演出を探す
    /// </summary>
    /// <param name="position">位置</param>
    /// <param name="distance">近いと判断する距離</param>
    /// <returns>花火演出(近いもの場無い場合は null)</returns>
    private VisualEffect FindVisualEffect(Vector3 position, float distance)
    {
        foreach (var visualEffect in activeEffects)
        {
            var lx = Math.Abs(visualEffect.transform.position.x - position.x);
            if (lx <= distance)
            {
                return visualEffect;
            }
        }

        return null;
    }

    /// <summary>
    /// 花火演出をすべてリセットする
    /// </summary>
    public void ResetVisualEffects()
    {
        foreach (var visualEffect in activeEffects)
        {
            Destroy(visualEffect);
        }
        activeEffects.Clear();
    }

    /// <summary>
    /// Follow Cameraに切り替える
    /// </summary>
    /// <param name="target">Follow対象のターゲット</param>
    public void SwitchFollowCamera(GameObject targetObject)
    {
        mainVCamera.gameObject.SetActive(false);

        // 打球を追う用のカメラを取得
        followVCamera = SelectFollowVCamera();
        followVCamera.gameObject.SetActive(true);

        // 渡されたターゲットを追う
        followVCamera.LookAt = targetObject.transform;
    }

    /// <summary>
    /// 打球を追う用のシネマカメラをランダムに選ぶ
    /// </summary>
    /// <returns></returns>
    private CinemachineVirtualCamera SelectFollowVCamera()
    {
        //無限ループ防止
        if (followVCameras.Length == 0)
        {
            return null;
        }

        return followVCameras[UnityEngine.Random.Range(0, followVCameras.Length)];
    }

    /// <summary>
    /// Main Cameraに切り替える
    /// </summary>
    public void SwitchMainCamera()
    {
        if (followVCamera != null)
        {
            // ターゲットを追うのをやめる
            followVCamera.LookAt = null;
            followVCamera.gameObject.SetActive(false);
        }

        mainVCamera.gameObject.SetActive(true);
    }


    /// <summary>
    /// 花火エフェクトの生成
    /// </summary>
    /// <param name="target">ターゲット</param>
    public void GenerateHomerunEffect(Target target)
    {
        Debug.Log("HOMERUN!!");

        // VisualEffectの演出追加
        CreateVisualEffect(target.transform.position);

        // 一定時間で消す(強引…)
        StartCoroutine(ProcessingHomerunEffect(target));
    }

    /// <summary>
    /// 一定時間後にオブジェクトを非表示にするコルーチン
    /// </summary>
    /// <returns>IEnumerator</returns>
    public IEnumerator ProcessingHomerunEffect(Target target)
    {
        yield return new WaitForSeconds(0.5f);

        // ターゲットのステータスを変更
        target.Stay();
        target.ResetVelocity();
    }
}
