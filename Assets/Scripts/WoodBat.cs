using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodBat : BaseballBat
{
    /// <summary>
    /// �I�[�o�[���C�h
    /// �o�b�g�̎��
    /// </summary>
    protected override string BatType()
    {
        return "�ؐ�";
    }

    /// <summary>
    /// �I�[�o�[���C�h
    /// �^�[�Q�b�g��j��ł��邩�ǂ���
    /// </summary>
    /// <returns></returns>
    protected override bool IsBreakableTarget()
    {
        return true;
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

}