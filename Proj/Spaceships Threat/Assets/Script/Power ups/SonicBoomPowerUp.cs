using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SonicBoomPowerUp : MonoBehaviour
{
    MouseCursorManager cursorManager;
    
    InputManager.PlayerActions PlayerInput;

    [SerializeField] PowerUpSO_Script sonicBoom_SO;

    [Space(20)]
    [SerializeField] float sonicBoomDamage;



    private void Awake()
    {
        cursorManager = FindObjectOfType<MouseCursorManager>();

        //Changes the cursor when fully charged
        sonicBoom_SO.GetTimer().OnTimerDone_event
                               .AddListener(
                                () => cursorManager.ChangeCursor_Crosshair());
    }

    void Update()
    {
        PlayerInput = GameManager.inst.inputManager.Player;

        //---Timer---//
        sonicBoom_SO.UpdateTimer();
        if (sonicBoom_SO.GetIsActive())
        {
            //Makes the timer run only                                         
            //if the power-up it's active
            sonicBoom_SO.GetTimer().AddTimeToTimer();
        }



        //All checks happening before the power-up get used
        bool isDashActive = sonicBoom_SO.GetIsActive();
        bool isFullyCharged = sonicBoom_SO.GetTimer().CheckIsOver();
        bool isActionButtonPressed = PlayerInput.SonicBoom.triggered;

        //Checks if the Sonic Boom power is ready to use
        //(has been bought and it's charged)
        // + if the player has pressed the action button
        if (isActionButtonPressed && isDashActive && isFullyCharged)
        {
            ActivateSonicBoomPowerUp();

            sonicBoom_SO.GetTimer().Restart();   //Restart the timer
        }
    }

    void ActivateSonicBoomPowerUp()
    {
        //TODO: Spara il BoatoSonico/SonicBoom


        //Revert the cursor to default when used
        cursorManager.ChangeCursor_Default();


        #region Animations

        #endregion
    }
}
