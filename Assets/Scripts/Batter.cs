using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Batter : MonoBehaviour
{
    /// <summary>
    /// �o�b�g�̃I�u�W�F�N�g
    /// </summary>
    [SerializeField] GameObject baseballBatObj;

    /// <summary>
    /// �o�b�g�̃N���X
    /// </summary>
    private BaseballBat baseballBat;

    // Start is called before the first frame update
    void Start()
    {
        baseballBat = baseballBatObj.GetComponent<BaseballBat>();
    }

    /// <summary>
    /// �o�b�g�̃R���C�_�[���I��
    /// </summary>
    public void BatColliderOn()
    {
        baseballBat.ColliderOn();
    }

    /// <summary>
    /// �o�b�g�̃R���C�_�[���I�t
    /// </summary>
    public void BatColliderOff()
    {
        baseballBat.ColldierOff();
    }
}
