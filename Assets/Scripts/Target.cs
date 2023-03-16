using System;
using System.Collections;
using UnityEngine;
using UniRx;

public class Target : MonoBehaviour
{
    /// <summary>
    /// 非表示にするまでの時間(単位：秒)
    /// </summary>
    private const float aliveSeconds = 4.0f;

    /// <summary>
    /// ターゲットのCollider
    /// ※ターゲットによってコライダーの種類が異なるので、Collider型で探して保持しておく
    /// </summary>
    private Collider targetCollider;

    /// <summary>
    /// ターゲットのRigitbody
    /// </summary>
    public Rigidbody targetRigitbody { get;  private set; }

    /// <summary>
    /// ターゲットのTrailRenderer
    /// </summary>
    private TrailRenderer trailRenderer;

    public enum State
    {
        Stay,
        WaitingShot,
        Hit,
        Fly,
        StandIn
    }

    private State status = State.Stay;
    public State Status
    {
        get
        {
            return status;
        }
        private set
        {
            status = value;
            if (trailRenderer != null)
            {
                // 待機中になったら軌跡はオフ
                if (status == State.Stay)
                {
                    trailRenderer.enabled = false;
                }

                // ヒットしたら軌跡はオン
                if (status == State.Hit)
                {
                    trailRenderer.enabled = true;
                }
            }
        }
    }

    private void Awake()
    {
        targetCollider = this.gameObject.GetComponent<Collider>();
        targetRigitbody = this.gameObject.GetComponent<Rigidbody>();
        trailRenderer = this.gameObject.GetComponent<TrailRenderer>();
        trailRenderer.enabled = false;
    }

    public void OnUpdate()
    {
        //if (this.gameObject.transform.position.y <= 0)
        //{
        //    Homerun();
        //}
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "ground")
        {
            Stay();
            ResetVelocity();
            Debug.Log("打たれず");
        }

        if (collision.gameObject.tag == "stand")
        {
            Homerun();
            ResetVelocity();
        }
    }

    /// <summary>
    /// オブジェクトを目標地点に飛ばす
    /// </summary>
    /// <param name="targetPosition">目標地点</param>
    /// <param name="angle">角度</param>
    public void MoveParabola(Vector3 targetPosition,float speed,float angle)
    {
        var startPosition = this.gameObject.transform.position;
        var velocity = CalcVelocity(startPosition, targetPosition, angle)*speed;
        targetRigitbody.AddForce(velocity * targetRigitbody.mass, ForceMode.Impulse);
    }

    /// <summary>
    /// 表示／非表示を設定する
    /// </summary>
    /// <param name="isActive">true:表示 / false:非表示</param>
    public void SetActive(bool isActive)
    {
        this.gameObject.SetActive(isActive);
    }

    /// <summary>
    /// オブジェクトを初期位置に戻す(リスポーンする)
    /// </summary>
    /// <param name="targetPosition"></param>
    public void Respawn(Vector3 targetPosition)
    {
        this.transform.position = targetPosition;
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
    /// オブジェクトのZ値が、対象のZ値よりも大きいか？
    /// </summary>
    /// <param name="targetPositionZ">対象のZ値</param>
    /// <returns>true: 大きい / false: 同じか小さい</returns>
    public bool IsLargePositionZ(float targetPositionZ)
    {
        return this.gameObject.transform.position.z > targetPositionZ;
    }

    /// <summary>
    /// オブジェクトのZ値が、対象のZ値よりも小さいか？
    /// </summary>
    /// <param name="targetPositionZ">対象のZ値</param>
    /// <returns>true: 小さい / false: 同じか大きい</returns>
    public bool IsSmallPositionZ(float targetPositionZ)
    {
        return this.gameObject.transform.position.z < targetPositionZ;
    }

    public void Hit()
    {
        Status = State.Hit;
    }

    public bool IsSameObj(GameObject gameObject)
    {
        return this.gameObject == gameObject;
    }

    public bool IsHit()
    {
        return Status == State.Hit;
    }

    public GameObject GetObj()
    {
        return this.gameObject;
    }

    public bool IsStandIn()
    {
        return Status == State.StandIn;
    }

    public bool IsWaitingShot()
    {
        return Status == State.WaitingShot;
    }

    public void WaitingShot()
    {
        Status = State.WaitingShot;
    }

    public void Homerun()
    {
        Status = State.StandIn;
    }

    public void Stay()
    {
        Status = State.Stay;
    }

    public bool IsStay()
    {
        return Status == State.Stay;
    }

    public void Fly()
    {
        Status = State.Fly;
    }

    public void ResetVelocity()
    {
        targetRigitbody.velocity = Vector3.zero;
        targetRigitbody.angularVelocity = Vector3.zero;
    }
}
