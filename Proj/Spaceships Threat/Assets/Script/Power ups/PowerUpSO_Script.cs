using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Power Up (S.O.)", fileName = "PowerUp_SO")]
public class PowerUpSO_Script : ScriptableObject
{
    [SerializeField] bool isUnlocked;
    [SerializeField] bool isActive;

    [Space(20)]
    [SerializeField] int basePrice;
    [SerializeField] int price;
    [SerializeField] AnimationCurve priceCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    [Space(20)]
    [SerializeField] int upgradeStage;
    [Range(1, 10)]
    [SerializeField] int maxUpgradeStages;



    /// <summary>
    /// Changes the price to the new one (the Base price + the Upgrade Stage
    /// </summary>
    public void ChangePrice()
    {
        float _newPrice =  basePrice + priceCurve.Evaluate(upgradeStage / maxUpgradeStages);
        price = Mathf.RoundToInt(_newPrice);
    }
}
