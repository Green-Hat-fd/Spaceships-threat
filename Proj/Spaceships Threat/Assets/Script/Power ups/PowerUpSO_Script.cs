using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Power Up (S.O.)", fileName = "PowerUp_SO")]
public class PowerUpSO_Script : ScriptableObject
{
    #region Tooltip()
    [Tooltip("If the power-up is \"grayed out\" \nand cannot be bought \n\t(when false)")]
    #endregion
    [SerializeField] bool isUnlocked = true;
    #region Tooltip()
    [Tooltip("If the power-up is unlocked \nbut hasn't been bought yet \n\t(when false)")]
    #endregion
    [SerializeField] bool isActive = true;

    [Header("—— Prices ——")]
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

    [Header("—— Recharge time ——")]
    [SerializeField] bool isRechargable;
    [Min(0)]
    [SerializeField] float rechargeTime;
    #region Tooltip()
    [Tooltip("Timer for the power-up to recharge")]
    #endregion
    [SerializeField] CustomTimer timer = new CustomTimer();



    /// <summary>
    /// Updates the price to the new one (the Base price + the Upgrade Stage)
    /// </summary>
    public void UpdatePrice()
    {
        float _priceToAdd = priceAddedCurve.Evaluate(upgradeStage / maxUpgradeStages);
        
        float _newPrice = basePrice + _priceToAdd;
        
        priceNow = Mathf.RoundToInt(_newPrice);
    }

    public void UpdateTimer()
    {
        timer.maxTime = rechargeTime;
    }


    #region Custom Get Functions

    public bool GetIsActive() => isActive;

    public float GetRechargeTime() => rechargeTime;

    public CustomTimer GetTimer() => timer;

    #endregion


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
