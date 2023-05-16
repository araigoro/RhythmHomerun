using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class Batter : MonoBehaviour
{
    /// <summary>
    /// �o�b�g�̃I�u�W�F�N�g
    /// </summary>
    [SerializeField] private GameObject[] baseballBatObj;

    /// <summary>
    /// ���݃A�N�e�B�u�ɂȂ��Ă���^�[�Q�b�g
    /// </summary>
    private Target activeTarget;

    /// <summary>
    /// �o�b�g�̃N���X
    /// </summary>
    private BaseballBat baseballBat;

    /// <summary>
    ///�o�b�g��I�Ԏ��̈���
    /// </summary>
    private int batIndex = 0;

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
    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        //UI�{�^���������ꂽ�ۂ͉�ʃN���b�N���������Ȃ��悤�ɂ���
    #if UNITY_EDITOR
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }
    #else
        if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId)) {
            return;
        }
    #endif

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
            }
        }
    }

    /// <summary>
    /// �o�b�g��ǂݍ���
    /// </summary>
    private void LoadBaseballBat()
    {
        var bat = baseballBatObj[batIndex];
        bat.SetActive(true);
        baseballBat = bat.GetComponent<BaseballBat>();
        baseballBat.RegisterActiveTarget(activeTarget);
    }

    /// <summary>
    /// �o�b�g��؂�ւ���
    /// </summary>
    public void SwitchBat()
    {
        baseballBatObj[batIndex].SetActive(false);
        batIndex = (batIndex + 1) % baseballBatObj.Length;
        LoadBaseballBat();
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

    /// <summary>
    /// ���ݎg�p���Ă���o�b�g�̃^�O����Ԃ�
    /// </summary>
    /// <returns></returns>
    public String usingBatTag()
    {
        return baseballBatObj[batIndex].tag;
    }

    /// <summary>
    /// �A�N�e�B�u�ɂȂ��Ă���^�[�Q�b�g��F������
    /// </summary>
    /// <param name="target">�^�[�Q�b�g</param>
    public void RecognizeActiveTarget(Target target)
    {
        activeTarget = target;
        LoadBaseballBat();
    }
}
