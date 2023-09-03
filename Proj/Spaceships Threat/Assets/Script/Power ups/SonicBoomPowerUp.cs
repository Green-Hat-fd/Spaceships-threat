using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SonicBoomPowerUp : MonoBehaviour
{
    MouseCursorManager cursorMng;
    ObjectPoolingScript poolingScr;
    
    InputManager.PlayerActions PlayerInput;

    [SerializeField] Transform playerCharacter;
    [SerializeField] PowerUpSO_Script sonicBoom_SO;
    [SerializeField] string sonicBoomPoolTag = "SonicBoom Bullets";

    [Space(20)]
    [SerializeField] float sonicBoomDamage;

    [Header("—— Feedback ——")]
    [SerializeField] AudioSource sonicBoomSfx;



    private void Awake()
    {
        cursorMng = FindObjectOfType<MouseCursorManager>();
        poolingScr = FindObjectOfType<ObjectPoolingScript>();

        //Changes the cursor when fully charged
        sonicBoom_SO.GetTimer().OnTimerDone_event
                               .AddListener(
                                () => cursorMng.ChangeCursor_Crosshair());
    }

    void Update()
    {
        PlayerInput = GameManager.inst.inputManager.Player;

        //---Timer---//
        sonicBoom_SO.UpdateTimer();
        if (sonicBoom_SO.GetIsUnlocked())
        {
            //Makes the timer run only
            //if the power-up it has been bought
            sonicBoom_SO.GetTimer().AddTimeToTimer();
        }



        //All checks happening before the power-up get used
        bool isSonicBoomUnlocked = sonicBoom_SO.GetIsUnlocked();
        bool isFullyCharged = sonicBoom_SO.GetTimer().CheckIsOver();
        bool isActionButtonPressed = PlayerInput.SonicBoom.triggered;

        //Checks if the Sonic Boom power is ready to use
        //(has been bought and it's charged)
        // + if the player has pressed the action button
        if (isActionButtonPressed && isSonicBoomUnlocked && isFullyCharged)
        {
            ActivateSonicBoomPowerUp();

            sonicBoom_SO.GetTimer().Restart();   //Restart the timer
        }
    }

    void ActivateSonicBoomPowerUp()
    {
        //Shoots the Sonic Boom bullet
        GameObject sonicBoomBullet;
        sonicBoomBullet = poolingScr
                          .TakeObjectFromPool(sonicBoomPoolTag,
                                              playerCharacter.position,
                                              playerCharacter.rotation);



        //Revert the cursor to default when used
        cursorMng.ChangeCursor_Default();


        #region Feedback

        //Plays the sound
        sonicBoomSfx.Play();

        #endregion
    }
}
