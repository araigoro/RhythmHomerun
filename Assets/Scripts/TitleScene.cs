using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TitleScene : MonoBehaviour
{
    /// <summary>
    /// �^�[�Q�b�g�}�l�[�W���[�̃I�u�W�F�N�g
    /// </summary>
    [SerializeField] private GameObject targetManagerObj;

    /// <summary>
    /// �^�C�g���̃��S�摜
    /// </summary>
    [SerializeField] private Image logoImage;

    /// <summary>
    /// �J�n�e�L�X�g
    /// </summary>
    [SerializeField] private TextMeshProUGUI tapStartText;

    /// <summary>
    /// �o�b�g��؂�ւ���UI�p�l��
    /// </summary>
    [SerializeField] private GameObject switchBatPanel;

    /// <summary>
    /// �J�n�e�L�X�g���_�ł��������ݒ�
    /// </summary>
    private const float blinkTime = 1.0f;

    /// <summary>
    /// �^�[�Q�b�g�}�l�[�W���[�̃N���X
    /// </summary>
    private TargetManager targetManager;

    /// <summary>
    /// �Q�[���J�n�҂����H
    /// </summary>
    private bool isWaitingTap = true;

    // Start is called before the first frame update
    void Start()
    {
        // �^�[�Q�b�g�}�l�[�W���[�N���X���擾
        targetManager = targetManagerObj.GetComponent<TargetManager>();

        // �J�n�e�L�X�g�̓_�ŃA�j���[�V���������s����
        StartCoroutine(BlinkText());
    }

    // Update is called once per frame
    void Update()
    {
        // ���ɃQ�[�����J�n����Ă���ꍇ�A�������΂�
        if (isWaitingTap == false)
        {
            return;
        }

        // �}�E�X�����N���b�N���ꂽ�ꍇ
        if (Input.GetMouseButtonDown(0))
        {
            // �J�n�҂��t���O��false�ɂ���
            isWaitingTap = false;

            // �Q�[����ʂɐ؂�ւ���
            StartCoroutine(Playball());
        }
    }

    /// <summary>
    /// ������_�ł�����
    /// </summary>
    IEnumerator BlinkText()
    {
        // �Q�[�����J�n���Ă��Ȃ���
        while (isWaitingTap)
        {
            // TextMeshPro�̕\���E��\����؂�ւ���
            tapStartText.enabled = !tapStartText.enabled;

            // �_�ł��鎞�Ԃ����ҋ@����
            yield return new WaitForSeconds(blinkTime);
        }
    }

    /// <summary>
    /// �Q�[����ʂւƐ؂�ւ���
    /// </summary>
    IEnumerator Playball()
    {
        // ���S���t�F�[�h�A�E�g���鎞��
        const float fadeTime = 1.0f;

        // ���S�̃t�F�[�h�A�E�g�A�j���[�V�������J�n����
        StartCoroutine(FadeOutImage(logoImage, fadeTime));

        // �^�b�v���ꂽ��e�L�X�g�������I�ɔ�\���ɂ��āA�t�F�[�h�I����҂��ăQ�[���J�n
        tapStartText.enabled = false;
        yield return new WaitForSeconds(fadeTime);

        // �^�C�g���\��������
        gameObject.SetActive(false);

        // �Q�[�����J�n����
        targetManager.Playball();

        //�o�b�g�؂�ւ�UI��\��
        switchBatPanel.SetActive(true);
    }

    /// <summary>
    /// Image���t�F�[�h�A�E�g����
    /// </summary>
    /// <param name="image">�C���[�W</param>
    /// <param name="fadeTime">�t�F�[�h�A�E�g����</param>
    IEnumerator FadeOutImage(Image image, float fadeTime = 1.0f)
    {
        // �C���[�W�̐F���擾
        Color originalColor = image.color;

        // �o�ߎ���
        float elapsedTime = 0f;

        // �t�F�[�h�A�E�g���Ԃɓ��B����܂ł̊�
        while (elapsedTime < fadeTime)
        {
            // �o�ߎ��Ԃ����Z
            elapsedTime += Time.deltaTime;

            // �o�ߎ��Ԃ�[0, 1]�͈̔͂ɃN�����v����
            float t = Mathf.Clamp01(elapsedTime / fadeTime);

            // �F���Ԃ���
            image.color = Color.Lerp(originalColor, Color.clear, t);
            yield return null;
        }
    }
}
