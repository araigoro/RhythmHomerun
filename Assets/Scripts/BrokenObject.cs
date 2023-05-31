using System.Collections;
using UnityEngine;

public class BrokenObject : MonoBehaviour
{
    /// <summary>
    /// �j�󎞂ɉ������
    /// </summary>
    private const float ExplosionForce = 200.0f;

    /// <summary>
    /// �j�󎞂ɉ�����͂̔��a
    /// </summary>
    private const float ExplosionRadius = 5.0f;

    /// <summary>
    /// �j�󎞂ɉ�����͂̒��S�ʒu��Z�l�V�t�g��
    /// </summary>
    private const float ExplosionShiftZ = -1.0f;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    /// <summary>
    /// �j�󂷂�
    /// </summary>
    public void Broken()
    {
        // �T�C�Y���傫���̂Œ���
        gameObject.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);

        // �j�Ђ�O�ɔ�΂����߂ɁA���j�̒��S�ʒu��Z�l�𒲐�
        var explosionPosition = gameObject.transform.position;
        explosionPosition.z += ExplosionShiftZ;

        // �j�Ѓp�[�c�ɔ��j�̉����x��������
        foreach (var rigidbody in gameObject.GetComponentsInChildren<Rigidbody>())
        {
            rigidbody.AddExplosionForce(ExplosionForce, explosionPosition, ExplosionRadius);
        }

        // ��莞�Ԃŏ���
        StartCoroutine(RemoveBrokenObject());
    }

    /// <summary>
    /// ��莞�Ԍ�ɔj�󉉏o�p�I�u�W�F�N�g���\���ɂ���R���[�`��
    /// </summary>
    /// <returns>IEnumerator</returns>
    private IEnumerator RemoveBrokenObject()
    {
        yield return new WaitForSeconds(2.0f);

        // �I�u�W�F�N�g��j��
        Destroy(gameObject);
    }
}
