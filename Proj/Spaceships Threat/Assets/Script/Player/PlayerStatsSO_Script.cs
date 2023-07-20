using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Player Stats (S.O.)", fileName = "PlStats_SO")]
public class PlayerStatsSO_Script : ScriptableObject
{
    [Header("—— Player ——")]
    [SerializeField] bool isDead = false;

    [Space(10)]
    [Range(0, 10)]
    [SerializeField] int health;
    [Range(0, 10)]
    [SerializeField] int maxHealth = 3;

    [Space(20)]
    #region Tooltip()
    [Tooltip("The scraps collected in one game")]
    #endregion
    [SerializeField] int scraps_temp = 0;
    #region Tooltip()
    [Tooltip("All the scraps the player has collected throughout the entire playthough")]
    #endregion
    [SerializeField] int allScraps = 0;

    [Header("—— Power-ups ——")]
    [SerializeField] int DEBUG;

    [Header("—— Power-downs ——")]
    #region Tooltip()
    [Tooltip("The duration of each power-down")]
    #endregion
    [SerializeField] float powerDownTime = 15f;
    [SerializeField] List<int> powerDownGet;        //SISTEMA

    [Header("—— Modifiers ——")]
    #region Tooltip()
    [Tooltip("The speed multiplier (in percentage)")]
    #endregion
    [SerializeField] float speedMultiplier = 1;



    #region Custom Set functions

    public void RemoveHealth()
    {
        health--;
    }
    public void AddHealth()
    {
        health++;
    }

    public void AddTempScraps(int amountToAdd)
    {
        scraps_temp += amountToAdd;
    }

    public void SetSpeedMultiplier(float newValue)
    {
        speedMultiplier = newValue;
    }
    public void ResetSpeedMultiplier()
    {
        speedMultiplier = 1;
    }

    #endregion


    #region Custom Get functions

    public int GetHealth()
    {
        return health;
    }
    public int GetMaxHealth()
    {
        return maxHealth;
    }

    public bool GetIsDead()
    {
        return isDead;
    }

    public int GetTempScraps()
    {
        return scraps_temp;
    }
    public int GetAllScraps()
    {
        return allScraps;
    }

    public float GetSpeedMultiplier()
    {
        return speedMultiplier;
    }

    #endregion


    public void CheckDeath()
    {
        //The player is dead when
        //health reaches (or is less than) 0
        isDead = health <= 0;
    }

    /// <summary>
    /// Used when the player starts a new game
    /// </summary>
    public void ResetPlayerStats()
    {
        health = maxHealth;   //Restores all the health

        allScraps += scraps_temp;   //Adds the collected scraps into the pile...
        scraps_temp = 0;            //...and resets the temporary variable

        ResetSpeedMultiplier();   //Brings back the speed multiplier to 1 (--> 100%)
    }


    #region EXTRA - Changing the Inspector

    private void OnValidate()
    {
        //Clamps all values of all the player stats
        //to always be positive
        health = Mathf.Clamp(health, 0, maxHealth);         // & below or equal to "maxHealth"
        maxHealth = Mathf.Clamp(maxHealth, 0, maxHealth);
        speedMultiplier = Mathf.Clamp(speedMultiplier, 0, speedMultiplier);
    }

    #endregion
}
