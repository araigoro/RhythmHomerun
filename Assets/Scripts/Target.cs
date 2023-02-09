using System.Collections;
using UnityEngine;

public class Target
{
    /// <summary>
    /// ヒットターゲットのレイヤー名
    /// </summary>
    private const string hitTargetLayerName = "HitTarget";

    /// <summary>
    /// 非表示にするまでの時間(単位：秒)
    /// </summary>
    private const float aliveSeconds = 4.0f;

    /// <summary>
    /// ターゲットプレハブのGameObject
    /// </summary>
    public GameObject TargetGameObject { get; private set; }

    /// <summary>
    /// ターゲットのCollider
    /// ※ターゲットによってコライダーの種類が異なるので、Collider型で探して保持しておく
    /// </summary>
    private Collider targetCollider;

    /// <summary>
    /// ターゲットのRigitbody
    /// </summary>
    private Rigidbody targetRigitbody;

    /// <summary>
    /// 表示中か？
    /// </summary>
    public bool IsDisplay
    {
        get { return TargetGameObject.activeSelf ? false : true; }
    }

    /// <summary>
    /// イニシャライザ
    /// </summary>
    /// <param name="gameObject">ターゲットのプレハブのGameObject</param>
    public Target(GameObject gameObject)
    {
        TargetGameObject = gameObject;

        // GetComponentは重いので、コンポーネントを取得して保持しておく
        targetCollider = TargetGameObject.GetComponent<Collider>();
        targetRigitbody = TargetGameObject.GetComponent<Rigidbody>();
    }

    /// <summary>
    /// オブジェクトを目標地点に飛ばす
    /// </summary>
    /// <param name="targetPosition">目標地点</param>
    /// <param name="angle">角度</param>
    public void MoveParabola(Vector3 targetPosition, float angle)
    {
        var startPosition = TargetGameObject.transform.position;
        var velocity = CalcVelocity(startPosition, targetPosition, angle);
        targetRigitbody.AddForce(velocity * targetRigitbody.mass, ForceMode.Impulse);
    }

    /// <summary>
    /// 表示／非表示を設定する
    /// </summary>
    /// <param name="isDisplay">true:表示 / false:非表示</param>
    public void SetDisplay(bool isDisplay)
    {
        TargetGameObject.SetActive(isDisplay);

        if (isDisplay == true)
        {
            // 表示
            targetCollider.enabled = true;
        }
    }

    /// <summary>
    /// 一定時間後にオブジェクトを非表示にするコルーチン
    /// </summary>
    /// <returns>IEnumerator</returns>
    public IEnumerator Collect()
    {
        yield return new WaitForSeconds(aliveSeconds);

        targetRigitbody.velocity = Vector3.zero;
        SetDisplay(false);
    }

    /// <summary>
    /// オブジェクトを初期位置に戻す(リスポーンする)
    /// </summary>
    /// <param name="targetPosition"></param>
    public void Respawn(Vector3 targetPosition)
    {
        TargetGameObject.transform.position = targetPosition;
    }

    /// <summary>
    /// オブジェクトは打てるターゲットオブジェクトか？
    /// </summary>
    /// <returns>true: ターゲット / false: 非ターゲット</returns>
    public bool IsHitTarget()
    {
        return LayerMask.LayerToName(TargetGameObject.layer) == hitTargetLayerName;
    }

    /// <summary>
    /// 始点と終点、打ち上げ角度から、速度を求める
    /// </summary>
    /// <param name="startPosition">始点</param>
    /// <param name="endPosition">終点</param>
    /// <param name="angle">角度</param>
    /// <returns>速度</returns>
    private Vector3 CalcVelocity(Vector3 startPosition, Vector3 endPosition, float angle)
    {
        var rad = angle * Mathf.PI / 180;
        var diffX = Vector2.Distance(new Vector2(startPosition.x, startPosition.z), new Vector2(endPosition.x, endPosition.z));
        var diffY = startPosition.y - endPosition.y;
        var initVelocity = Mathf.Sqrt(-Physics.gravity.y * Mathf.Pow(diffX, 2) / (2 * Mathf.Pow(Mathf.Cos(rad), 2) * (diffX * Mathf.Tan(rad) + diffY)));

        if (float.IsNaN(initVelocity))
        {
            return Vector3.zero;
        }

        return (new Vector3(endPosition.x - startPosition.x, diffX * Mathf.Tan(rad), endPosition.z - startPosition.z).normalized * initVelocity);
    }

    /// <summary>
    /// コライダーを無効にする
    /// </summary>
    public void ColliderOff()
    {
        targetCollider.enabled = false;
    }

    /// <summary>
    /// オブジェクトのZ値が、対象のZ値よりも大きいか？
    /// </summary>
    /// <param name="targetPositionZ">対象のZ値</param>
    /// <returns>true: 大きい / false: 同じか小さい</returns>
    public bool IsLargePositionZ(float targetPositionZ)
    {
        return TargetGameObject.transform.position.z > targetPositionZ;
    }

    /// <summary>
    /// オブジェクトのZ値が、対象のZ値よりも小さいか？
    /// </summary>
    /// <param name="targetPositionZ">対象のZ値</param>
    /// <returns>true: 小さい / false: 同じか大きい</returns>
    internal bool IsSmallPositionZ(float targetPositionZ)
    {
        return TargetGameObject.transform.position.z < targetPositionZ;
    }
}
