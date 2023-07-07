using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovemRB : MonoBehaviour
{
    Rigidbody rb;

    [SerializeField] float playerVelocity = 7.5f;
    float xMovement, zMovement;

    Vector3 moveVect;

    [Space(20)]
    [SerializeField] Vector3 playerSize = Vector3.one;
    [SerializeField] Vector2 boundaryBox = Vector2.one * 8;
    //Vector3 dimensBoxcast = new Vector3(0.51f, 0f, 0.51f);



    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    private void Update()
    {
        //Takes the axes from the movement input
        xMovement = GameManager.inst.inputManager.Player.Movement.ReadValue<Vector2>().x;
        zMovement = GameManager.inst.inputManager.Player.Movement.ReadValue<Vector2>().y;

        moveVect = transform.forward * zMovement + transform.right * xMovement;      //Horizontal movement vector
    }

    RaycastHit hitBase;
    void FixedUpdate()
    {
        //(Simple) Horizontal movement of the player
        rb.AddForce(moveVect.normalized * playerVelocity * 10f, ForceMode.Force);


        #region Speed limit

        //Prende la velocita' orizzontale del giocatore
        Vector3 horizVel_temp = new Vector3(rb.velocity.x, 0, rb.velocity.z);

        //Controllo se si accelera troppo, cioe' si supera la velocita'
        if (horizVel_temp.magnitude >= playerVelocity)
        {
            //Limita la velocita' a quella prestabilita, riportandola al RigidBody
            Vector3 limit = horizVel_temp.normalized * playerVelocity;
            rb.velocity = new Vector3(limit.x, rb.velocity.y, limit.z);
        }

        #endregion
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0, 1, 0, 0.5f);
        Gizmos.DrawWireCube(transform.position, boundaryBox);
    }

private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawCube(transform.position, playerSize);
    }
}
