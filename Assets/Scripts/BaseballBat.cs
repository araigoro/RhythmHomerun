using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseballBat : MonoBehaviour
{
    public AudioClip soundHit;

    private readonly Vector3[] HOMERUN_POINT_LEFT = { new Vector3(-27, 4, 35), new Vector3(-25, 4, 43), new Vector3(-20, 4, 47) };

    private readonly Vector3[] HOMERUN_POINT_CENTER = { new Vector3(0, 5, 45), new Vector3(-10, 4, 40), new Vector3(7, 3, 48) };

    private readonly Vector3[] HOMERUN_POINT_RIGHT = { new Vector3(31, 3, 30), new Vector3(13, 3, 47), new Vector3(23, 4, 48) };

    private const float HIT_ANGLE = 45;

    private const float HIT_POWER = 1.0f;

    private const float BORDER_LEFT_DIRECTION = 0.2f;
    private const float BORDER_RIGHT_DIRECTION = 0.0f;

    private void OnCollisionEnter(Collision collision)
    {
        Target target = new Target(collision.gameObject);
        if (!target.isHitTarget())
        {
            return;
        }
        hitTarget(target);
    }

    private void hitTarget(Target target)
    {
        AudioSource.PlayClipAtPoint(soundHit, transform.position);
        target.colliderOff();
        Vector3 targetPosition = selectTargetPoint(target);
        target.moveParabola(targetPosition, HIT_ANGLE, HIT_POWER);
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
        return target.isLargePositionZ(BORDER_LEFT_DIRECTION);
    }

    private bool isRightHit(Target target)
    {
        return target.isSmallPositionZ(BORDER_RIGHT_DIRECTION);
    }


}
