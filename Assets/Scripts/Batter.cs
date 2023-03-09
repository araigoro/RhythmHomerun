using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Batter : MonoBehaviour
{

    [SerializeField] GameObject baseballBatObj;

    private BaseballBat baseballBat;

    // Start is called before the first frame update
    void Start()
    {
        baseballBat = baseballBatObj.GetComponent<BaseballBat>();
    }

    public void BatColliderOn()
    {
        baseballBat.ColliderOn();
    }

    public void BatColliderOff()
    {
        baseballBat.ColldierOff();
    }
}
