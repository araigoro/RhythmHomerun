using System.Collections;
using UnityEngine;

public class BrokenObject : MonoBehaviour
{
    /// <summary>
    /// 破壊時に加える力
    /// </summary>
    private const float ExplosionForce = 200.0f;

    /// <summary>
    /// 破壊時に加える力の半径
    /// </summary>
    private const float ExplosionRadius = 5.0f;

    /// <summary>
    /// 破壊時に加える力の中心位置のZ値シフト量
    /// </summary>
    private const float ExplosionShiftZ = -1.0f;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    /// <summary>
    /// 破壊する
    /// </summary>
    public void Broken()
    {
        // サイズが大きいので調整
        gameObject.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);

        // 破片を前に飛ばすために、爆破の中心位置のZ値を調整
        var explosionPosition = gameObject.transform.position;
        explosionPosition.z += ExplosionShiftZ;

        // 破片パーツに爆破の加速度を加える
        foreach (var rigidbody in gameObject.GetComponentsInChildren<Rigidbody>())
        {
            rigidbody.AddExplosionForce(ExplosionForce, explosionPosition, ExplosionRadius);
        }

        // 一定時間で消す
        StartCoroutine(RemoveBrokenObject());
    }

    /// <summary>
    /// 一定時間後に破壊演出用オブジェクトを非表示にするコルーチン
    /// </summary>
    /// <returns>IEnumerator</returns>
    private IEnumerator RemoveBrokenObject()
    {
        yield return new WaitForSeconds(2.0f);

        // オブジェクトを破棄
        Destroy(gameObject);
    }
}
