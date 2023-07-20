using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerStatsManager : MonoBehaviour
{
    [SerializeField] PlayerStatsSO_Script stats_SO;

    [Header("—— User Interface (UI) ——")]
    [SerializeField] List<Image> healthImages;
    [SerializeField] Sprite normalHeart,
                            bonusHeart,
                            grayHeart;
    #region Tooltip()
    [Tooltip("The alpha the Bonus Hearts have when you've lost them all")]
    #endregion
    [Range(0, 1)]
    [SerializeField] float alphaLostBonusHeart = 0.4f;


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

            //Hides the extra hearts when they are lost
            healthImages[i].color = stats_SO.GetHealth() <= 3
                                     ? i >= 3 ? new Color(1, 1, 1, alphaLostBonusHeart) : Color.white
                                     : Color.white;
        }

        #endregion
    }


    #region EXTRA - Changing the Inspector

    private void OnValidate()
    {
        //---
    }

    #endregion
}
