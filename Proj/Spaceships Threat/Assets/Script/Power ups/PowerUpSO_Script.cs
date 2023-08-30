using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Power Up (S.O.)", fileName = "PowerUp_SO")]
public class PowerUpSO_Script : ScriptableObject
{
    #region Tooltip()
    [Tooltip("When false - the power-up is \"grayed out\" \nand cannot be bought")]
    #endregion
    [SerializeField] bool isActive = true;
    #region Tooltip()
    [Tooltip("If the power-up has been bought or not")]
    #endregion
    [SerializeField] bool isUnlocked = true;

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

    /// <summary>
    /// Adds 1 to the "upgrade stage" & Updates the price
    /// </summary>
    public void AddUpgradeStage()
    {
        upgradeStage++;

        UpdatePrice();
    }


    #region Custom Set Functions

    public void SetIsActive(bool value)
    {
        isActive = value;
    }

    public void SetIsUnlocked(bool value)
    {
        isUnlocked = value;
    }


    //Load functions
    public void LoadIsActive(bool value) 
    { 
        isActive = value;
    }
    public void LoadIsUnlocked(bool value)
    {
        isUnlocked = value;
    }

    public void LoadUpgradeStage(int value) 
    { 
        upgradeStage = value;
    }

    #endregion

    #region Custom Get Functions

    public bool GetIsActive() => isActive;
    public bool GetIsUnlocked() => isUnlocked;

    public int GetPriceNow() => priceNow;

    public int GetUpgradeStage() => upgradeStage;

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
