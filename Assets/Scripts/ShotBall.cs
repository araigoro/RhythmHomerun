using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotBall : MonoBehaviour
{
    [SerializeField]
    private GameObject[] battingTargets;

    [SerializeField]
    private AudioClip shotSound;

    private const int shotInterval = 2000;

    private const int shotSpeed = 270;

    private const float destroyTime = 5.0f;

    // 投げるオブジェクトのインデックス
    private int throwTargetIndex = 0;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    private void Update()
    {
        if (IsThrowsThisFrame(Time.frameCount))
        {
            ThrowObject(InstantiateNextObject());
        }
    }

    /// <summary>
    /// このフレームで次のオブジェクトを投げるか？
    /// </summary>
    /// <param name="frameCount">フレーム数</param>
    /// <returns>true:投げる / false:投げない</returns>
    private bool IsThrowsThisFrame(int frameCount)
    {
        return frameCount % shotInterval == 0;
    }

    /// <summary>
    /// 次に投げるオブジェクトをインスタンス化する
    /// </summary>
    /// <returns>プレハブのGameObject</returns>
    private GameObject InstantiateNextObject()
    {
        GameObject gameObject = Instantiate(battingTargets[throwTargetIndex], transform.position, Quaternion.identity);

        // 登録順にオブジェクトを切り替える
        throwTargetIndex = (throwTargetIndex + 1) % battingTargets.Length;

        return gameObject;
    }

    /// <summary>
    /// 指定したオブジェクトを投げる
    /// </summary>
    /// <param name="target">投げるオブジェクトのプレハブ</param>
    private void ThrowObject(GameObject target)
    {
        Rigidbody targetRb = target.GetComponent<Rigidbody>();

        // とりあえず適当に回転させる
        targetRb.angularVelocity = new Vector3(0.5f, 0.5f, 0.5f);

        targetRb.AddForce(transform.forward * shotSpeed);
        AudioSource.PlayClipAtPoint(shotSound, transform.position);
        Destroy(target, destroyTime);
    }
}
