using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PowerUpManager : MonoBehaviour
{
    [System.Serializable]
    public class PUElements_Class
    {
        public PowerUpSO_Script powerUp_SO;
        public TMP_Text priceTxt;
        public Button buyButton;
        public Button upgradeButton;
        #region Tooltip()
        [Tooltip("The image that prevents clicking on this power-up")]
        #endregion
        public Image blockingImg;
        #region Tooltip()
        [Tooltip("The image that indicates \nif the power-up is bought or not")]
        #endregion
        public Image unlockedImg;
    }



    [SerializeField] PlayerStatsSO_Script stats_SO;

    [Space(20)]
    [SerializeField] TMP_Text txt_allScraps;
    float numScr_temp;

    [Space(20)]
    [SerializeField] List<PUElements_Class> elements_list;
    Dictionary<PowerUpSO_Script, PUElements_Class> powerUp_dict;

    [Space(20)]
    [SerializeField] PowerUpSO_Script firstPowerUp;
    [Range(0, 1)]
    [SerializeField] float percentageFirstPU = 0.3f;

    [Header("—— Feedback ——")]
    [SerializeField] AudioSource clickSource;
    [SerializeField] AudioClip usedClick_sfx,
                               idleClick_sfx;



    void Awake()
    {
        powerUp_dict = new Dictionary<PowerUpSO_Script,
                                      PUElements_Class>();    //Creates a new empty dict.

        //Transfers the list to a dictionary
        foreach (var pU in elements_list)
        {
            powerUp_dict.Add(pU.powerUp_SO, pU);
        }


        //Updates the price for each power-up
        foreach (PUElements_Class elem in powerUp_dict.Values)
        {
            elem.powerUp_SO.UpdatePrice();
        }


        txt_allScraps.text = stats_SO.GetAllScraps().ToString();
    }

    void Update()
    {


        #region Changing the UI

        foreach (PUElements_Class elem in powerUp_dict.Values)    //For all power-ups in the dictionary...
        {
            //Changes the prices of each power-up to the current price
            elem.priceTxt.text = elem.powerUp_SO.GetPriceNow() + "";


            //Removes the images that prevents clicks
            //if the power-up can be bought
            bool elem_isSOActive = elem.powerUp_SO.GetIsActive();

            elem.blockingImg
                .gameObject
                .SetActive(!elem_isSOActive);


            //Removes the [Buy] button and shows the [Upgrade] one
            //when the power-up is unlocked/has been bought
            bool elem_isUnlocked = elem.powerUp_SO.GetIsUnlocked();

            elem.buyButton.gameObject.SetActive(!elem_isUnlocked);
            elem.upgradeButton.gameObject.SetActive(elem_isUnlocked);


            //Removes the icon that notifies
            //if a power-up is bought
            elem.unlockedImg.gameObject.SetActive(!elem_isUnlocked && elem_isSOActive);


            //Removes the possibility to buy other
            //upgrades if the power-up is maxed
            elem.upgradeButton.interactable = !elem.powerUp_SO.IsMaxUpgraded();
        }


        //Changes smoothly the all scraps' text
        SmoothlyChangeScrapsNumber();

        txt_allScraps.text = numScr_temp.ToString("0");    //Removes the decimal part 

        #endregion
    }

    void SmoothlyChangeScrapsNumber()
    {
        int allScr = stats_SO.GetAllScraps();

        //The smooth change
        numScr_temp = Mathf.Lerp(numScr_temp, allScr, Time.deltaTime * 7.5f);
    }



    public void UnlockPowerUp(PowerUpSO_Script powerUp_SO)
    {
        PowerUpSO_Script powerUp = powerUp_dict[powerUp_SO].powerUp_SO;

        //Checks if the player can buy the power-up
        bool canPlayerPay = stats_SO.GetAllScraps() >= powerUp.GetPriceNow();


        if (canPlayerPay)
        {
            powerUp.SetIsUnlocked(true);    //Unlocks the power-up
            powerUp.AddUpgradeStage();      //Updates the update stage & price of the power-up
        
            stats_SO.RemoveAllScraps(powerUp.GetPriceNow());    //Withdraws the Scraps from the pile
        

            clickSource.PlayOneShot(usedClick_sfx);
        }
        else
        {
            clickSource.PlayOneShot(idleClick_sfx);
        }
    }


    public void UpgradePowerUp(PowerUpSO_Script powerUp_SO)
    {
        PowerUpSO_Script powerUp = powerUp_dict[powerUp_SO].powerUp_SO;

        //Checks if the player can buy the power-up
        bool canPlayerPay = stats_SO.GetAllScraps() >= powerUp.GetPriceNow();


        if (canPlayerPay)
        {
            powerUp.AddUpgradeStage();    //Upgrades the power-up stage
            
            stats_SO.RemoveAllScraps(powerUp.GetPriceNow());    //Withdraws the Scraps from the pile


            clickSource.PlayOneShot(usedClick_sfx);
        }
        else
        {
            clickSource.PlayOneShot(idleClick_sfx);
        }
    }



    public void SetupFirstPowerup(bool active)
    {
        //Sets the first power-up price to the
        //percentage after the second players playtrough
        if (stats_SO.GetAllScraps() >= (percentageFirstPU*10))
        {
            firstPowerUp.SetIsActive(active);
            firstPowerUp.SetBasePrice(active
                                      ?
                                      (int)(stats_SO.GetAllScraps() * percentageFirstPU)
                                      :
                                      1000);
        }
    }
}
