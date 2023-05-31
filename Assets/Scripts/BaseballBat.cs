using UnityEngine;

public class BaseballBat : MonoBehaviour
{
    /// <summary>
    /// 打撃音
    /// </summary>
    [SerializeField] private AudioClip soundHit;

    /// <summary>
    /// ターゲットマネージャーのオブジェクト
    /// </summary>
    [SerializeField] GameObject targetManagerObj;

    /// <summary>
    /// オブジェクトの打ち出し角度
    /// </summary>
    private const float hitAngle = 30;

    /// <summary>
    /// 打ち返す強さ
    /// </summary>
    private const float hitPower = 1.0f;

    /// <summary>
    /// 現在アクティブになっているターゲット
    /// </summary>
    private Target activeTarget;

    /// <summary>
    /// バットのコライダー
    /// </summary>
    private Collider batCollider;

    /// <summary>
    /// 打撃音の大きさ
    /// </summary>
    private const float hitTargetVolume = 0.3f;

    /// <summary>
    /// 飛距離
    /// </summary>
    private const float maxDistance = 35f;

    /// <summary>
    /// ターゲットとのZ距離の最大値
    /// </summary>
    private const float maxDiffZ = 0.6f;

    /// <summary>
    /// ターゲットとのZ距離の最小値
    /// </summary>
    private const float minDiffZ = 0.25f;

    /// <summary>
    /// 打撃音再生用
    /// </summary>
    private AudioSource audioSource;

    /// <summary>
    /// サブクラスでのオーバライド用
    /// バットの種類
    /// </summary>
    /// <returns></returns>
    protected virtual string BatType()
    {
        return null;
    }

    /// <summary>
    /// サブクラスでのオーバライド用
    /// それぞれのバットがターゲットを破壊できるかどうか
    /// </summary>
    /// <returns></returns>
    protected virtual bool IsBreakableTarget()
    {
        return false;
    }

    private void Awake()
    {
        batCollider = this.GetComponent<Collider>();
        audioSource = this.GetComponent<AudioSource>();

        //ボリューム設定
        audioSource.volume = hitTargetVolume;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Target collisionTarget = collision.gameObject.GetComponent<Target>();

        // 衝突したのがアクティブになっているターゲットであれば
        if (activeTarget == collisionTarget)
        {
            // 打つ！
            HitTarget(collisionTarget, IsBreakableTarget());
        }
    }

    /// <summary>
    /// ターゲットを打つ
    /// </summary>
    /// <param name="target"> 打つ対象のターゲット</param>
    /// <param name="isBreakableTarget"> バットがターゲットを破壊できるかどうか</param>
    private void HitTarget(Target target,bool isBreakableTarget)
    {
        //予期せぬ衝突を防ぐためにコライダーを無効にする
        ColldierOff();

        //打撃音を鳴らす
        audioSource.PlayOneShot(soundHit);

        //ターゲットを破壊できるとき
        if (isBreakableTarget == true)
        {
            // ターゲットを破壊する
            target.Broken();
            return;
        }

        //ターゲットオブジェクトを飛ばす先を取得
        var targetPosition = SelectTargetPoint(target);

        // ターゲットのステータスを変更
        target.Hit();

        //ターゲットオブジェクトを放物線状に飛ばす
        target.MoveParabola(targetPosition, hitPower, hitAngle);
    }

    /// <summary>
    /// ターゲットオブジェクトを飛ばす先の座標を選ぶ
    /// </summary>
    /// <param name="target">対象のターゲットオブジェクト</param>
    /// <returns>飛ばす先の座標（x,y,z）</returns>
    private Vector3 SelectTargetPoint(Target target)
    {
        // ターゲットオブジェクトの現在位置
        var currentPosition = target.gameObject.transform.position;

        // ターゲットのZ座標を取得
        var diffZ = target.gameObject.transform.position.z;
        // min-max内に補正
        diffZ = Mathf.Min(Mathf.Max(minDiffZ, diffZ), maxDiffZ);

        // 距離から角度を求める
        var angle = (((diffZ - minDiffZ) / (maxDiffZ - minDiffZ) * -80f) + 40f);

        // 角度をラジアンに変換
        float radian = angle * Mathf.Deg2Rad;

        // 新しい座標の計算
        float offsetX = Mathf.Sin(radian) * maxDistance;
        float offsetZ = Mathf.Cos(radian) * maxDistance;

        // ボールの着弾座標を返す
        return currentPosition + new Vector3(offsetX, 0f, offsetZ);
    }

    /// <summary>
    /// アクティブになっているターゲットを保持
    /// </summary>
    /// <param name="target">ターゲット</param>
    public void RegisterActiveTarget(Target target)
    {
        if(target == null)
        {
            return;
        }

        activeTarget = target;
    }

    /// <summary>
    /// バットのコライダーをオンにする
    /// </summary>
    public void ColliderOn()
    {
        batCollider.enabled = true;
    }

    /// <summary>
    /// バットのコライダーをオフにする
    /// </summary>
    public void ColldierOff()
    {
        batCollider.enabled = false;
    }

    /// <summary>
    /// バットの種類を返す
    /// </summary>
    /// <returns></returns>
    public string ReturnBatType()
    {
        return BatType();
    }
}
