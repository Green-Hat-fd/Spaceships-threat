using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class PlayerStatsManager : MonoBehaviour
{
    [SerializeField] PlayerStatsSO_Script stats_SO;
    [Space(5)]
    [SerializeField] PowerUpSO_Script dash_powerUp_SO;
    [SerializeField] PowerUpSO_Script sonicBoom_powerUp_SO;
    [SerializeField] PowerUpSO_Script shielding_powerUp_SO;
    [SerializeField] PowerUpSO_Script stopwatch_powerUp_SO;

    [Header("—— User Interface (UI) ——")]
    [SerializeField] List<Image> healthImages;
    [SerializeField] Sprite normalHeart,
                            bonusHeart,
                            grayHeart;
    
    [Space(20)]
    #region Tooltip()
    [Tooltip("The alpha the Bonus Hearts have when the player has lost them all")]
    #endregion
    [Range(0, 1)]
    [SerializeField] float alphaLostBonusHeart = 0.4f;

    [Space(20)]
    [SerializeField] TextMeshProUGUI scrapsAmountText;
    
    [Space(20)]
    [SerializeField] Image sonicBoomPowerup_icon;
    [SerializeField] Image dashPowerup_icon;
    [SerializeField] Color powerupCharging_color = new Color(0.5f, 0.5f, 0.5f, 0.75f);
    List<Image> icons = new List<Image>();



    private void Awake()
    {
        icons.Add(sonicBoomPowerup_icon);
        icons.Add(dashPowerup_icon);

        foreach (Image img in icons)
        {
            img.preserveAspect = true;

            img.type = Image.Type.Filled;
            img.fillMethod = Image.FillMethod.Radial360;
            img.fillOrigin = 2;
            img.fillClockwise = true;

            img.color = powerupCharging_color;
        }


    }

    void Update()
    {
        //Checks if the player is dead
        stats_SO.CheckDeath();


        #region Changing the UI

        //For each image in the health bar...
        for (int i=0; i < healthImages.Count; i++)
        {
            //Checks if the index is below the player HP
            //and changes the image in a "normal" or "bonus" heart accordingly,
            //if not, changes it a "grayHeart" (the player has lost it)
            healthImages[i].sprite = i < stats_SO.GetHealth()
                                        ? i < 3 ? normalHeart : bonusHeart
                                        : grayHeart;


            //If max health < 3, then show only the normal hearts
            if(stats_SO.GetMaxHealth() <= 3)
            {
                healthImages[i].color = i < 3 ? Color.white : Color.clear;
            }
            else
            {
                //Shows only the unlocked bonus hearts,
                //and hides the other ones
                if(i >= stats_SO.GetMaxHealth())
                {
                    healthImages[i].color = Color.clear;
                }
                else
                {
                    //Putting emphasis on the normal hearts
                    //(dims the bonus hearts when the player has lost all of them)
                    healthImages[i].color = stats_SO.GetHealth() <= 3
                                             ? i >= 3 ? new Color(1, 1, 1, alphaLostBonusHeart) : Color.white
                                             : Color.white;
                }
            }
        }


        //Changes the fill amount of the icons
        //to the time remaining on each timer
        icons[0].fillAmount = CocaCola(dash_powerUp_SO.GetTimer());
        icons[1].fillAmount = CocaCola(sonicBoom_powerUp_SO.GetTimer());

        //Changes all the charging icons
        //(Recharging = gray and semi-transparent)
        //(Fully charged = visible)
        foreach (Image img in icons)
        {
            img.color = img.fillAmount >= 0.99f ? Color.white : powerupCharging_color;
        }


        //Changes the temp scraps' text to the amount gained
        scrapsAmountText.text = stats_SO.GetTempScraps().ToString();

        #endregion
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="_timer"></param>
    /// <returns>The time elapsed in percentage, if the timer's over, it returns 1</returns>
    float CocaCola(CustomTimer _timer)
    {
        //Returns 
        return _timer.CheckIsOver()
                ? 1
                : _timer.PercentElapsedTime();
    }


    #region EXTRA - Changing the Inspector

    private void OnValidate()
    {
        //---
    }

    #endregion
}
