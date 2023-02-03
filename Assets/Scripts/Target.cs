using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    private const string HIT_TARGET = "HitTarget";

    private const float WAIT_HIDE_TIME = 4.0f;

    private GameObject obj;

    public Target(GameObject obj)
    {
        this.obj = obj;
    }

    public void moveParabola(Vector3 targetPosition, float angle, float speed)
    {
        Vector3 startPosition = this.obj.transform.position;
        Vector3 velocity = calcVelocity(startPosition, targetPosition, angle);
        Rigidbody rb = this.obj.GetComponent<Rigidbody>();
        rb.AddForce(velocity * rb.mass, ForceMode.Impulse);
    }

    public void display()
    {
        this.obj.SetActive(true);
        this.obj.GetComponent<Collider>().enabled = true;
    }

    public void hide()
    {
        this.obj.SetActive(false);
    }

    public IEnumerator collect()
    {
        yield return new WaitForSeconds(WAIT_HIDE_TIME);
        this.obj.GetComponent<Rigidbody>().velocity = Vector3.zero;
        hide();
    }

    public void reposition(Vector3 targetPosition)
    {
        this.obj.transform.position = targetPosition;
    }

    public bool isHitTarget()
    {
        return LayerMask.LayerToName(this.obj.layer) == HIT_TARGET;
    }

    private Vector3 calcVelocity(Vector3 startPosition, Vector3 endPosition, float angle)
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

    public bool isLargePositionZ(float targetPositionZ)
    {
        return this.obj.transform.position.z > targetPositionZ;
    }

    public void colliderOff()
    {
        this.obj.GetComponent<Collider>().enabled = false;
    }

    internal bool isSmallPositionZ(float targetPositionZ)
    {
        return this.obj.transform.position.z < targetPositionZ;
    }
}
