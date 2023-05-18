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

    /// <summary>
    /// �o�b�^�[�̃A�j���[�^�[
    /// </summary>
    private Animator animator;

    /// <summary>
    /// �A�j���[�V�����ő���������^�C�~���O�ł�����true�ɂ���
    /// </summary>
    private const string boolLegUp = "BoolLegUp";

    /// <summary>
    /// �X�C���O�A�j���[�V�����ɑJ�ڂ��邽�߂̃g���K�[��
    /// </summary>
    private const string triggerSwing = "TriggerSwing";

    /// <summary>
    /// �����グ�Ă��郂�[�V�����̃��[�V������
    /// </summary>
    private const string legUpMotion = "LegUpMotion_10";

    // Start is called before the first frame update
    void Start()
    {
        baseballBat = baseballBatObj.GetComponent<BaseballBat>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        //��ʃN���b�N���ꂽ�Ƃ�
        if (Input.GetMouseButtonDown(0))
        {
            //�o�b�^�[�������グ�Ă����Ԃ̂Ƃ�
            if (animator.GetCurrentAnimatorStateInfo(0).IsName(legUpMotion))
            {
                //�X�C���O�A�j���[�V�����ɑJ�ڂ��邽�߂̃g���K�[���I���ɂ���
                animator.SetTrigger(triggerSwing);

                //����������
                LegDown();

                // FIXME: �U��x��̏ꍇ�ɁA���̓������ɃX�C���O�A�j���[�V�������Đ�����Ă��܂�
            }
        }
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

    /// <summary>
    /// �A�j���[�V�����ő����グ��
    /// </summary>
    public void LegUp()
    {
        animator.SetBool(boolLegUp, true);
    }

    /// <summary>
    /// �A�j���[�V�����ő���������
    /// </summary>
    public void LegDown()
    {
        animator.SetBool(boolLegUp, false);
    }
}
