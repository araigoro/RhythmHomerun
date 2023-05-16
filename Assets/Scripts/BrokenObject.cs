using System.Collections;
using UnityEngine;

public class BrokenObject : MonoBehaviour
{
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
        gameObject.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);

        foreach (var rigidbody in gameObject.GetComponentsInChildren<Rigidbody>())
        {
            rigidbody.AddExplosionForce(200.0f, gameObject.transform.position, 5.0f);
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
