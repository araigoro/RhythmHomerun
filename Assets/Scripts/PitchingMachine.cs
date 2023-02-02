using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PitchingMachine : MonoBehaviour
{
    public GameObject pitchingMachine;
    public GameObject[] targetPrefabs;
    public AudioClip soundShot;

    private List<Target> targetPool = new List<Target>();

    private const int MAX_TARGET_COUNT = 10;
    private const int SHOT_INTERVAL = 2000;

    private const float SHOT_SPEED = 270;

    private const float SHOT_ANGLE = 30;



    private Vector3 INITIAL_POSITION = new Vector3(0, -10, 0);

    private Vector3 STRIKE_POSITION = new Vector3(0, 3, -1) / 10;

    private void Awake()
    {
        for (int i = 0; i <= MAX_TARGET_COUNT; i++)
        {
            GameObject obj = Instantiate(selectRandomTargetPrefab(), INITIAL_POSITION, Quaternion.identity);
            Target target = new Target(obj);
            target.hide();
            targetPool.Add(target);
        }
    }

    private void Update()
    {
        int frameCount = Time.frameCount;
        if (isShotTarget(frameCount))
        {
            shotTarget();
        }
    }

    private void shotTarget()
    {
        Target target = selectRandomTarget();
        target.reposition(pitchingMachine.transform.position);
        target.display();
        target.moveParabola(STRIKE_POSITION, SHOT_ANGLE, SHOT_SPEED);
        AudioSource.PlayClipAtPoint(soundShot, pitchingMachine.transform.position);
        StartCoroutine(target.collect());
    }

    private GameObject selectRandomTargetPrefab()
    {
        return targetPrefabs[UnityEngine.Random.Range(0, targetPrefabs.Length)];
    }

    private Target selectRandomTarget()
    {
        return targetPool[UnityEngine.Random.Range(0, targetPool.Count)];
    }

    private bool isShotTarget(int frameCount)
    {
        return frameCount % SHOT_INTERVAL == 0;
    }
}
