public class MetalBat : BaseballBat
{
    /// <summary>
    /// �I�[�o�[���C�h
    /// �o�b�g�̎��
    /// </summary>
    protected override string BatType()
    {
        return "����";
    }

    /// <summary>
    /// �I�[�o�[���C�h
    /// �^�[�Q�b�g��j��ł��邩�ǂ���
    /// </summary>
    /// <returns></returns>
    protected override bool IsBreakableTarget()
    {
        return false;
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
