using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TitleScene : MonoBehaviour
{
    [SerializeField] private GameObject targetManagerObj;
    [SerializeField] private Image logoImage;
    [SerializeField] private TextMeshProUGUI tapStartText;

    private const float blinkTime = 1.0f; // �_�ł��������ݒ肷��ϐ�

    private TargetManager targetManager;

    /// <summary>
    /// �{�^�����͑҂����H
    /// </summary>
    private bool isWaitingTap = true;

    // Start is called before the first frame update
    void Start()
    {
        targetManager = targetManagerObj.GetComponent<TargetManager>();

        StartCoroutine(BlinkText()); // �_�ŃA�j���[�V���������s����
    }

    // Update is called once per frame
    void Update()
    {
        if (isWaitingTap == true)
        {
            if (Input.GetMouseButtonDown(0))
            {
                isWaitingTap = false;
                StartCoroutine(Playball());
            }
        }
    }

    /// <summary>
    /// ������_�ł�����
    /// </summary>
    IEnumerator BlinkText()
    {
        while (isWaitingTap)
        {
            // TextMeshPro�̕\���E��\����؂�ւ���
            tapStartText.enabled = !tapStartText.enabled;

            // �_�ł��鎞�Ԃ����ҋ@����
            yield return new WaitForSeconds(blinkTime);
        }
    }

    IEnumerator Playball()
    {
        const float fadeTime = 1.0f;

        // ���S�̃t�F�[�h�A�E�g�A�j���[�V�������J�n����
        StartCoroutine(FadeOutImage(logoImage, fadeTime));

        // �^�b�v���ꂽ��e�L�X�g�������I�ɔ�\���ɂ��āA�t�F�[�h�I����҂��ăQ�[���J�n
        tapStartText.enabled = false;
        yield return new WaitForSeconds(fadeTime);

        // ��ʂ��^�b�v���ꂽ��J�n
        gameObject.SetActive(false);

        targetManager.Playball();
    }

    /// <summary>
    /// Image���t�F�[�h�A�E�g����
    /// </summary>
    /// <param name="image">�C���[�W</param>
    /// <param name="fadeTime">�t�F�[�h�A�E�g����</param>
    IEnumerator FadeOutImage(Image image, float fadeTime = 1.0f)
    {
        Color originalColor = image.color;
        float elapsedTime = 0f;
        while (elapsedTime < fadeTime)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / fadeTime); // �o�ߎ��Ԃ�[0, 1]�͈̔͂ɃN�����v����
            image.color = Color.Lerp(originalColor, Color.clear, t); // �F���Ԃ���
            yield return null;
        }
    }
}
