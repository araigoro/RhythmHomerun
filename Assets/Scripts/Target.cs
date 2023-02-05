using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target
{
    private const string HIT_TARGET = "HitTarget";

    private const float WAIT_HIDE_TIME = 4.0f;

    /// <summary>
    /// ターゲットプレハブのGameObject
    /// </summary>
    private GameObject targetGameObject;

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
        get { return targetGameObject.activeSelf ? false : true; }
    }

    public Target(GameObject gameObject)
    {
        targetGameObject = gameObject;

        // GetComponentは重いので、コンポーネントを取得して保持しておく
        targetCollider = targetGameObject.GetComponent<Collider>();
        targetRigitbody = targetGameObject.GetComponent<Rigidbody>();
    }

    public void MoveParabola(Vector3 targetPosition, float angle, float speed)
    {
        Vector3 startPosition = targetGameObject.transform.position;
        Vector3 velocity = CalcVelocity(startPosition, targetPosition, angle);
        targetRigitbody.AddForce(velocity * targetRigitbody.mass, ForceMode.Impulse);
    }

    /// <summary>
    /// 表示／非表示を設定する
    /// </summary>
    /// <param name="isDisplay">true:表示 / false:非表示</param>
    public void SetDisplay(bool isDisplay)
    {
        targetGameObject.SetActive(isDisplay);

        if (isDisplay == true)
        {
            // 表示
            targetCollider.enabled = true;
        }
    }

    public IEnumerator Collect()
    {
        yield return new WaitForSeconds(WAIT_HIDE_TIME);

        targetRigitbody.velocity = Vector3.zero;
        SetDisplay(false);
    }

    public void Reposition(Vector3 targetPosition)
    {
        targetGameObject.transform.position = targetPosition;
    }

    public bool IsHitTarget()
    {
        return LayerMask.LayerToName(targetGameObject.layer) == HIT_TARGET;
    }

    private Vector3 CalcVelocity(Vector3 startPosition, Vector3 endPosition, float angle)
    {
        float rad = angle * Mathf.PI / 180;
        float diffX = Vector2.Distance(new Vector2(startPosition.x, startPosition.z), new Vector2(endPosition.x, endPosition.z));
        float diffY = startPosition.y - endPosition.y;
        float initVelocity = Mathf.Sqrt(-Physics.gravity.y * Mathf.Pow(diffX, 2) / (2 * Mathf.Pow(Mathf.Cos(rad), 2) * (diffX * Mathf.Tan(rad) + diffY)));

        if (float.IsNaN(initVelocity))
        {
            return Vector3.zero;
        }

        return (new Vector3(endPosition.x - startPosition.x, diffX * Mathf.Tan(rad), endPosition.z - startPosition.z).normalized * initVelocity);
    }

    public void ColliderOff()
    {
        targetCollider.enabled = false;
    }

    public bool IsLargePositionZ(float targetPositionZ)
    {
        return targetGameObject.transform.position.z > targetPositionZ;
    }

    internal bool IsSmallPositionZ(float targetPositionZ)
    {
        Target target = this;
        return target.targetGameObject.transform.position.z < targetPositionZ;
    }
}
