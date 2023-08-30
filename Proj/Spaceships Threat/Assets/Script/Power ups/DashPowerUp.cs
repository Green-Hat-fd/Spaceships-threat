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

    [Header("—— Feedback ——")]
    [SerializeField] ParticleSystem dashEffect_partA,
                                    dashEffect_partB,
                                    dashEffect_partC;



    void Update()
    {
        PlayerInput = GameManager.inst.inputManager.Player;
        
        //---Timer---//
        dash_SO.UpdateTimer();
        if (dash_SO.GetIsUnlocked())
        {
            //Makes the timer run only
            //if the power-up it has been bought
            dash_SO.GetTimer().AddTimeToTimer();
        }



        playerMovem = PlayerInput.Movement.ReadValue<Vector2>();

        //All checks happening before the power-up get used
        bool isDashUnlocked = dash_SO.GetIsUnlocked();
        bool isFullyCharged = dash_SO.GetTimer().CheckIsOver();
        bool isActionButtonPressed = PlayerInput.Dash.triggered;
        bool isPlayerMoving = playerMovem != Vector3.zero;

        //Checks if the Dash power is ready to use
        //(has been bought and it's charged)
        // + if the player has pressed the action button
        if(isActionButtonPressed && isDashUnlocked && isFullyCharged && isPlayerMoving)
        {
            ActivateDashPowerUp();

            dash_SO.GetTimer().Restart();   //Restart the timer
        }
    }

    void ActivateDashPowerUp()
    {
        Vector3 oldPos = movemScript.GetRB().position;    //Used for the particle later

        //Calculates the next position to move
        Vector3 posToMove = movemScript.GetRB().position
                              +
                             playerMovem * distanceToDash;

        //Limits the new "dash position" inside the boundary box
        posToMove = movemScript.LimitInsideBoundaryBox(posToMove);

        //Moves the player in the new "dash position"
        movemScript.GetRB().MovePosition(posToMove);


        #region Animations

        Vector3 halfPos = Vector3.Lerp(oldPos, posToMove, 0.5f);

        //Creates the particle on the old, new and half position
        MoveAndPlayParticleEffect(dashEffect_partA, oldPos);
        MoveAndPlayParticleEffect(dashEffect_partB, halfPos);
        MoveAndPlayParticleEffect(dashEffect_partC, posToMove);

        #endregion
    }

    void MoveAndPlayParticleEffect(ParticleSystem particle, Vector3 positionToMove)
    {
        particle.transform.position = positionToMove;
        particle.Play();
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
