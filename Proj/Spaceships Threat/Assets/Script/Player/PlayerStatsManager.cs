using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class PlayerStatsManager : MonoBehaviour, IPlayer, IDamageable
{
    [SerializeField] MainGameManager mainGameManag;
    
    [Space(20)]
    [SerializeField] PlayerStatsSO_Script stats_SO;
    PlayerMovemRB playerMovScript;
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

    [Space(20)]
    [SerializeField] Animator gameOverUI;
    [Range(0, 10)]
    #region Tooltip()
    [Tooltip("The seconds to wait before the Game Over screen shows")] 
    #endregion
    [SerializeField] float secToWaitGameOver = .75f;
    bool doOnce_gameOver = true;

    [Space(20)]
    [SerializeField] GameObject playerModel;
    [Range(0, 100)]
    [SerializeField] int invAlphaModel = 25;
    bool isInvincible = false;
    CustomTimer invTimer = new CustomTimer();

    [Header("—— Feedback ——")]
    [SerializeField] MusicManager musicManager;
    [SerializeField] AudioSource damageSfx;
    [SerializeField] AudioSource deathSfx;
    [SerializeField] ParticleSystem death_part;



    private void Awake()
    {
        playerMovScript = FindObjectOfType<PlayerMovemRB>();


        //Setting up the Icons
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


        //Setting up the InvTimer

        invTimer.maxTime = stats_SO.GetInvSec();
        invTimer.LoopTimer(true);

        invTimer.OnTimerDone_event
                .AddListener(() => { isInvincible = false; });
        invTimer.OnTimerDone_event
                .AddListener(() => ChangeSpaceshipColor(Color.white));
    }


    void Update()
    {
        //Checks if the player is dead
        CheckDeath();

        //---Invincibility Timer---//
        if (isInvincible)
        {
            invTimer.AddTimeToTimer();
        }


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


        //Hides the icons that are not bought yet
        icons[0].enabled = sonicBoom_powerUp_SO.GetIsActive();
        icons[1].enabled = dash_powerUp_SO.GetIsActive();

        //Changes the fill amount of the icons
        //to the time remaining on each timer
        icons[0].fillAmount = RecieveElapsedTimeOnTimer(sonicBoom_powerUp_SO.GetTimer());
        icons[1].fillAmount = RecieveElapsedTimeOnTimer(dash_powerUp_SO.GetTimer());

        //Changes the charge of all icons
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
    /// </summary>
    /// <param name="_timer"></param>
    /// <returns>The time elapsed in percentage, if the timer's over, it returns 1</returns>
    float RecieveElapsedTimeOnTimer(CustomTimer _timer)
    {
        //Returns the time elapsed as a percentage,
        //or else returns 1 if the timer's over
        return _timer.CheckIsOver()
                ? 1
                : _timer.PercentElapsedTime();
    }


    void ChangeSpaceshipColor(Color newColor)
    {
        playerModel.GetComponent<MeshRenderer>().material.color = newColor;
    }



    #region Damage & Death

    public void TakeDamage(int amount)
    {
        if (!isInvincible)    //If the player CAN take damage...
        {
            stats_SO.RemoveHealth();

            isInvincible = true;
            ChangeSpaceshipColor(new Color(1, 1, 1, invAlphaModel));


            #region Feedback

            //Plays the audio
            damageSfx.PlayOneShot(damageSfx.clip);

            #endregion
        }
    }


    public void CheckDeath()
    {
        stats_SO.CheckDeath();


        if (stats_SO.GetIsDead())
        {
            if (doOnce_gameOver)
            {
                #region Feedback

                //Shows the player's death particles
                death_part.transform.position = playerMovScript.GetRB().position;
                death_part.Play();

                //Shows the Game Over screen
                gameOverUI.gameObject.SetActive(true);
                StartCoroutine(ShowGameOverScreen());

                //Stops the music
                musicManager.StopCurrentMusic();

                //Plays the death audio
                deathSfx.Play();

                #endregion


                //Removes the spaceship & control from the player
                mainGameManag.ActivatePlayer(false);


                doOnce_gameOver = false;
            }
        }
        else
        {
            doOnce_gameOver = true;
        }
    }

    IEnumerator ShowGameOverScreen()
    {
        yield return new WaitForSeconds(secToWaitGameOver);

        gameOverUI.SetTrigger("Show");
    }

    #endregion


    #region EXTRA - Changing the Inspector

    private void OnValidate()
    {
        //---
    }

    #endregion
}
