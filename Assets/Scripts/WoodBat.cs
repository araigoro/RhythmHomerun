public class WoodBat : BaseballBat
{
    /// <summary>
    /// オーバーライド
    /// バットの種類
    /// </summary>
    protected override string BatType()
    {
        return "木製";
    }

    /// <summary>
    /// オーバーライド
    /// ターゲットを破壊できるかどうか
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
