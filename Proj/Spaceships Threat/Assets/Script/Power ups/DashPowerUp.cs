using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashPowerUp : MonoBehaviour
{
    InputManager.PlayerActions PlayerInput;

    [SerializeField] PowerUpSO_Script dash_SO;
    [SerializeField] PlayerMovemRB movemScript;

    [Space(20)]
    [SerializeField] float distanceToDash;
    Vector3 playerMovem;



    void Update()
    {
        PlayerInput = GameManager.inst.inputManager.Player;
        
        //---Timer---//
        dash_SO.UpdateTimer();
        if (dash_SO.GetIsActive())
        {
            //Makes the timer run only                                         
            //if the power-up it's active
            dash_SO.GetTimer().AddTimeToTimer();
        }



        playerMovem = PlayerInput.Movement.ReadValue<Vector2>();

        //All checks happening before the power-up get used
        bool isDashActive = dash_SO.GetIsActive();
        bool isFullyCharged = dash_SO.GetTimer().CheckIsOver();
        bool isActionButtonPressed = PlayerInput.Dash.triggered;
        bool isPlayerMoving = playerMovem != Vector3.zero;

        //Checks if the Dash power is ready to use
        //(has been bought and it's charged)
        // + if the player has pressed the action button
        if(isActionButtonPressed && isDashActive && isFullyCharged && isPlayerMoving)
        {
            ActivateDashPowerUp();

            dash_SO.GetTimer().Restart();   //Restart the timer
        }
    }

    void ActivateDashPowerUp()
    {
        //Calculates the next 
        Vector3 vectToMove = movemScript.GetRB().position
                              +
                             playerMovem * distanceToDash;

        //Limits the new "dash position" inside the boundary box
        vectToMove = movemScript.LimitInsideBoundaryBox(vectToMove);

        //Moves the player in the new "dash position"
        movemScript.GetRB().MovePosition(vectToMove);

        #region Animations

        #endregion
    }


    #region EXTRA - Gizmos

    private void OnDrawGizmosSelected()
    {
        Vector3 p = movemScript.GetRB().position + playerMovem * distanceToDash;
        p = movemScript.LimitInsideBoundaryBox(p);

        if (playerMovem != Vector3.zero)
        {
            Gizmos.color = new Color(0, 1, 1, 0.5f);
            Gizmos.DrawLine(movemScript.GetRB().position, p);
            //Gizmos.DrawCube(p, Vector3.one * 0.45f);
            Gizmos.DrawSphere(p, 0.25f);
        }
    }

    #endregion
}
