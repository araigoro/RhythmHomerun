using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SwitchBatPanel : MonoBehaviour
{

    /// <summary>
    /// �o�b�^�[�̃I�u�W�F�N�g
    /// </summary>
    [SerializeField] private GameObject batterObj;

    /// <summary>
    /// �o�b�g�̎�ނ�\������e�L�X�g�I�u�W�F�N�g
    /// </summary>
    [SerializeField] private GameObject batTypeTextObj;

    /// <summary>
    /// �o�b�^�[�̃N���X
    /// </summary>
    private Batter batter;

    /// <summary>
    /// �o�b�g�̎�ނ�\������e�L�X�g
    /// </summary>
    private TextMeshProUGUI batTypeText;

    /// <summary>
    /// ���ݎg�p���Ă���o�b�g�̎��
    /// </summary>
    private string usingBatType;

    // Start is called before the first frame update
    void Start()
    {
        batter = batterObj.GetComponent<Batter>();
        batTypeText = batTypeTextObj.GetComponent<TextMeshProUGUI>();
        LoadUsingBatTag();
        ShowUsingBatName();
    }

    /// <summary>
    /// ���{�^���������ꂽ��o�b�^�[�̃o�b�g��؂�ւ���
    /// </summary>
    public void SwitchBat()
    {
        batter.SwitchBat();
        LoadUsingBatTag();
        ShowUsingBatName();
    }

    /// <summary>
    /// ���ݎg�p���Ă���o�b�g���e�L�X�g�ɕ\��
    /// </summary>
    private void ShowUsingBatName()
    {
        batTypeText.text = usingBatType;
    }

    /// <summary>
    /// �o�b�^�[�����ݎg�p���Ă���o�b�g�̖��O���擾
    /// </summary>
    private void LoadUsingBatTag()
    {
        usingBatType = batter.ReturnUsingBatType();
    }
}
