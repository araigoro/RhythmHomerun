using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSwing : MonoBehaviour
{
    public GameObject bat;

    HingeJoint hjBat;
    JointSpring spBat;

    private const float spring = 40000;
    private const float damper = 1000;

    private const float positionBeforeSwing = -45;
    private const float positionAfterSwing = 60;

    void Start()
    {
        hjBat = bat.GetComponent<HingeJoint>();
        spBat = hjBat.spring;
        spBat.spring = spring;
        spBat.damper = damper;        
    }

    public void OnClick()
    {
        swingBat();

        // returnBat();
    }


    private void swingBat()
    {
        spBat.targetPosition = positionAfterSwing;
        hjBat.spring = spBat;
    }

    private void returnBat()
    {
        spBat.targetPosition = positionBeforeSwing;
        hjBat.spring = spBat;
    }

}
