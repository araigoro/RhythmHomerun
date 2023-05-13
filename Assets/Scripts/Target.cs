using System.Collections;
using UnityEngine;

public class Target : MonoBehaviour
{
    /// <summary>
    /// 地面のタグ
    /// </summary>
    private const string ground = "ground";

    /// <summary>
    /// スタンドのタグ
    /// </summary>
    private const string stand = "stand";

    /// <summary>
    /// ターゲットのRigitbody
    /// </summary>
    public Rigidbody targetRigitbody { get;  private set; }

    /// <summary>
    /// ターゲットのTrailRenderer
    /// </summary>
    private TrailRenderer trailRenderer;

    /// <summary>
    /// 破壊用のオブジェクト
    /// </summary>
    [SerializeField] private GameObject brokenObject;

    /// <summary>
    /// ターゲットのステータス
    /// </summary>
    public enum State
    {
        Stay,           //非アクティブで隠れている
        WaitingShot,    //投手が保持していて、投げる前
        Hit,            //バットに打たれた瞬間
        Fly,            //打たれて飛んでいる
        StandIn         //スタンドイン
    }

    /// <summary>
    /// 初期状態
    /// </summary>
    private State status = State.Stay;

    /// <summary>
    /// 現在のターゲットの状態
    /// </summary>
    public State NowStatus
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

    /// <summary>
    /// 破壊可能か？
    /// </summary>
    public bool IsBreakable
    {
        get
        {
            return (brokenObject == null) ? false : true;
        }
    }

    private void Awake()
    {
        targetRigitbody = this.gameObject.GetComponent<Rigidbody>();
        trailRenderer = this.gameObject.GetComponent<TrailRenderer>();
        trailRenderer.enabled = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        // 地面と衝突したら
        if (collision.gameObject.tag == ground)
        {
            Stay();
            ResetVelocity();
        }

        // スタンドと衝突したら
        if (collision.gameObject.tag == stand)
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
        ResetVelocity();
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
    /// 打たれた状態に変更
    /// </summary>
    public void Hit()
    {
        NowStatus = State.Hit;
    }

    /// <summary>
    /// ターゲットのオブジェクト同一かどうか
    /// </summary>
    /// <param name="gameObject">オブジェクト</param>
    /// <returns></returns>
    public bool IsSameObj(GameObject gameObject)
    {
        return this.gameObject == gameObject;
    }

    /// <summary>
    /// 打たれているかどうか
    /// </summary>
    /// <returns></returns>
    public bool IsHit()
    {
        return NowStatus == State.Hit;
    }

    /// <summary>
    /// ターゲットのオブジェクトを返す
    /// </summary>
    /// <returns></returns>
    public GameObject GetObj()
    {
        return this.gameObject;
    }

    /// <summary>
    /// スタンドインしているかどうか
    /// </summary>
    /// <returns></returns>
    public bool IsStandIn()
    {
        return NowStatus == State.StandIn;
    }

    /// <summary>
    /// 投げられかどうか
    /// </summary>
    /// <returns></returns>
    public bool IsWaitingShot()
    {
        return NowStatus == State.WaitingShot;
    }

    /// <summary>
    /// 状態を投げられ待ちに変更
    /// </summary>
    public void WaitingShot()
    {
        NowStatus = State.WaitingShot;
    }

    /// <summary>
    /// 状態をホームランに変更
    /// </summary>
    public void Homerun()
    {
        NowStatus = State.StandIn;
    }

    /// <summary>
    /// 状態を待機中に変更
    /// </summary>
    public void Stay()
    {
        NowStatus = State.Stay;
    }

    /// <summary>
    /// 待機中かどうか
    /// </summary>
    /// <returns></returns>
    public bool IsStay()
    {
        return NowStatus == State.Stay;
    }

    /// <summary>
    /// 速度をリセット
    /// </summary>
    public void ResetVelocity()
    {
        targetRigitbody.velocity = Vector3.zero;
        targetRigitbody.angularVelocity = Vector3.zero;
    }

    /// <summary>
    /// 破壊する
    /// </summary>
    public void Broken()
    {
        if (IsBreakable)
        {
            // 破壊演出用オブジェクトを生成
            var go = Instantiate(brokenObject, gameObject.transform.position, Quaternion.identity);
            var script = go.GetComponent<BrokenObject>();

            // 破壊
            script.Broken();

            // 待機に戻す
            Stay();
        }
    }
}
