using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseballBat : MonoBehaviour
{
    public AudioClip soundHit;

    /// <summary>
    /// ピッチングマシンのGameObject
    /// </summary>
    private PitchingMachine pitchingMachine;

    private readonly Vector3[] HOMERUN_POINT_LEFT = { new Vector3(-27, 4, 35), new Vector3(-25, 4, 43), new Vector3(-20, 4, 47) };

    private readonly Vector3[] HOMERUN_POINT_CENTER = { new Vector3(0, 5, 45), new Vector3(-10, 4, 40), new Vector3(7, 3, 48) };

    private readonly Vector3[] HOMERUN_POINT_RIGHT = { new Vector3(31, 3, 30), new Vector3(13, 3, 47), new Vector3(23, 4, 48) };

    private const float HIT_ANGLE = 45;

    private const float HIT_POWER = 1.0f;

    private const float BORDER_LEFT_DIRECTION = 0.2f;
    private const float BORDER_RIGHT_DIRECTION = 0.0f;

    private void Awake()
    {
        // ピッチングマシンのGameObjectを保持
        var gameObject = GameObject.Find("Pitching Machine");
        if (gameObject != null)
        {
            pitchingMachine = gameObject.GetComponent<PitchingMachine>();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // ピッチングマシンのコンポーネントが取得できなかった場合は、ログ表示
        if (pitchingMachine == null)
        {
            Debug.Log("Not found!! Pitching Machine is null!!");
            return;
        }

        // 対象のGameObjectがターゲットオブジェクトか？
        Target target = pitchingMachine.FindTarget(collision.gameObject);
        if (target != null)
        {
            // 打つ！
            hitTarget(target);
        }
    }

    private void hitTarget(Target target)
    {
        AudioSource.PlayClipAtPoint(soundHit, transform.position);
        target.ColliderOff();
        Vector3 targetPosition = selectTargetPoint(target);
        target.MoveParabola(targetPosition, HIT_ANGLE);
    }

    private Vector3 selectTargetPoint(Target target)
    {
        if (isLeftHit(target))
        {
            return selectRandomPoint(HOMERUN_POINT_LEFT);
        }

        if (isRightHit(target))
        {
            return selectRandomPoint(HOMERUN_POINT_RIGHT);
        }

        return selectRandomPoint(HOMERUN_POINT_CENTER);
    }

    private Vector3 selectRandomPoint(Vector3[] homerunPoint)
    {
        return homerunPoint[UnityEngine.Random.Range(0, homerunPoint.Length)];
    }

    private bool isLeftHit(Target target)
    {
        return target.IsLargePositionZ(BORDER_LEFT_DIRECTION);
    }

    private bool isRightHit(Target target)
    {
        return target.IsSmallPositionZ(BORDER_RIGHT_DIRECTION);
    }


}
