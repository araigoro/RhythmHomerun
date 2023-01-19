using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotBall : MonoBehaviour
{
    public GameObject battingTarget;
    public AudioClip shotSound;

    private const int shotInterval = 2000;

    private const int shotSpeed = 270;

    private const float destroyTime = 5.0f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    private void Update()
    {
        int frameCount = Time.frameCount;
        if (isShot(frameCount))
        {
            GameObject target = Instantiate(battingTarget, transform.position, Quaternion.identity);
            Rigidbody targetRb = target.GetComponent<Rigidbody>();

            targetRb.AddForce(transform.forward * shotSpeed);
            AudioSource.PlayClipAtPoint(shotSound, transform.position);
            Destroy(target, destroyTime);
        }

    }

    private bool isShot(int frameCount)
    {
        return frameCount % shotInterval == 0;
    }
}
