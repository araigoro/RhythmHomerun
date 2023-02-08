using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    /// <summary>
    /// メインカメラのオブジェクト
    /// </summary>
    [SerializeField] private GameObject mainCamera;

    /// <summary>
    ///メインカメラの初期位置
    /// </summary>
    private Vector3 mainPosition;


    private void Start()
    {
        mainPosition = mainCamera.transform.position;
    }


    void Update()
    {

    }
}
