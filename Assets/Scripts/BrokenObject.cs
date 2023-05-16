using System.Collections;
using UnityEngine;

public class BrokenObject : MonoBehaviour
{
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
        gameObject.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);

        foreach (var rigidbody in gameObject.GetComponentsInChildren<Rigidbody>())
        {
            rigidbody.AddExplosionForce(200.0f, gameObject.transform.position, 5.0f);
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
