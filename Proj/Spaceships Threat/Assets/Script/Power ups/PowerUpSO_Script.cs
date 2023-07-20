using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Power Up (S.O.)", fileName = "PowerUp_SO")]
public class PowerUpSO_Script : ScriptableObject
{
    #region Tooltip()
    [Tooltip("If the power-up is \"grayed out\" \nand cannot be bought")]
    #endregion
    [SerializeField] bool isUnlocked = true;
    #region Tooltip()
    [Tooltip("If the power-up is unlocked \nbut hasn't been bought yet")]
    #endregion
    [SerializeField] bool isActive = true;

    [Space(20)]
    [SerializeField] int basePrice;
    [SerializeField] int priceNow;
    #region Tooltip()
    [Tooltip("The curve used to calculate the prize \nto ADD to the base price" +
             "\n\n(Time axis = upgrade stage's percentage) \n(Value axis = the new price to add)")]
    #endregion
    [SerializeField] AnimationCurve priceAddedCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    [Space(20)]
    #region Tooltip()
    [Tooltip("The stage at which the power-up is right now" +
             "\n(0 = not active yet)")]
    #endregion
    [SerializeField] int upgradeStage;
    [Range(1, 8)]
    [SerializeField] int maxUpgradeStages = 0;



    /// <summary>
    /// Updates the price to the new one (the Base price + the Upgrade Stage)
    /// </summary>
    public void UpdatePrice()
    {
        float _priceToAdd = priceAddedCurve.Evaluate(upgradeStage / maxUpgradeStages);
        
        float _newPrice = basePrice + _priceToAdd;
        
        priceNow = Mathf.RoundToInt(_newPrice);
    }


    #region EXTRA - Changing the Inspector

    private void OnValidate()
    {
        //Clamps the prices and upgrade's stages to be always positive
        basePrice = Mathf.Clamp(basePrice, 0, basePrice);
        priceNow = Mathf.Clamp(priceNow, 0, priceNow);
        upgradeStage = Mathf.Clamp(upgradeStage, 0, maxUpgradeStages);   // & below or equal to "maxUpgradeStages"
    }

    #endregion
}
