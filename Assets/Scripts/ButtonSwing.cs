using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSwing : MonoBehaviour
{
    public GameObject batter;
    private Animator animator;

    void Start()
    {
        animator = batter.GetComponent<Animator>();
    }

    public void OnClick()
    {
        animator.SetTrigger("SwingTrigger");
    }

}
