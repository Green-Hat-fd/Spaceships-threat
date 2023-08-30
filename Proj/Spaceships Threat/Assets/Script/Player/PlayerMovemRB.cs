using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovemRB : MonoBehaviour, IPlayer
{
    InputManager.PlayerActions PlayerInput;

    GameObject spaceshipToMove;
    [SerializeField] PlayerStatsSO_Script stats_SO;
    [SerializeField] Rigidbody rb;

    [Space(10)]
    [SerializeField] float playerVelocity = 7.5f;
    float xMovement, yMovement;

    Vector3 moveVect;

    [Space(10)]
    #region Tooltip()
    [Tooltip("The size of the player \n(a custom box collider, if you will)"
             + "\n\n/!\\   The Z value is not used at the moment \n\t(Note: this could change)")] 
    #endregion
    [SerializeField] Vector3 playerSize = Vector3.one;
    [SerializeField] Vector2 boundaryBox = new Vector2(15, 11.25f);
    //Vector3 dimensBoxcast = new Vector3(0.51f, 0f, 0.51f);

    [Header("—— Animations ——")]
    [SerializeField] Transform spaceshipModel;

    [Space(10)]
    [SerializeField] float maxWavingHeight = 0.15f;
    [SerializeField] float wavingVelocity = 5;

    [Space(10)]
    #region Tooltip()
    [Tooltip("X = horizontal tilt (roll)"
             + "\nY = vertical tilt (pitch)")] 
    #endregion
    [SerializeField] Vector2 maxTilt = new Vector2(17.5f, 10f);
    [SerializeField] float tiltVelocityPercent = 3 / 2;



    private void Awake()
    {
        rb.freezeRotation = true;
        spaceshipToMove = rb.gameObject;
    }

    private void Update()
    {
        PlayerInput = GameManager.inst.inputManager.Player;

        //Takes the axes from the movement input
        xMovement = PlayerInput.Movement.ReadValue<Vector2>().x;
        yMovement = PlayerInput.Movement.ReadValue<Vector2>().y;

        moveVect = transform.up * yMovement + transform.right * xMovement;      //Horizontal movement vector

        #region Animations

        //The (right or left) tilt of the model
        float horizVel_temp  = Mathf.Clamp(rb.velocity.x, -1, 1);           //The "normalized" horizontal velocity
        float verticVel_temp = Mathf.Clamp(rb.velocity.y, -1, 1);           //The "normalized" vertical velocity
        float tiltVelocity = Time.deltaTime * tiltVelocityPercent * 50;
   
                //The final rotation which the model has to get to
        Quaternion newRot = Quaternion.Euler(transform.forward * -horizVel_temp * maxTilt.x
                                             +
                                             transform.right * -verticVel_temp * maxTilt.y);

                //The (smooth) animation itself
        spaceshipModel.transform.rotation = Quaternion.RotateTowards(spaceshipModel.transform.rotation,
                                                                     newRot,
                                                                     tiltVelocity);


        //The sine wave of the model
        Vector3 waveModel_anim = Mathf.Sin(Time.time * wavingVelocity) * maxWavingHeight * Vector3.up;
        spaceshipModel.localPosition = waveModel_anim;

        #endregion
    }

    void FixedUpdate()
    {
        Vector3 forceVect = moveVect.normalized * playerVelocity;
        forceVect *= 10f;
        forceVect *= stats_SO.GetSpeedMultiplier();

        //(Simple) Horizontal player movement
        rb.AddForce(forceVect, ForceMode.Force);


        #region Boundary box movement limit
        
        //Checks if the spaceship is moving
        if(rb.velocity.magnitude > 0)
        {
            //Takes the local position, and
            //restricts the player's movement inside the "boundaryBox"
            Vector3 pos = spaceshipToMove.transform.localPosition;

            pos = LimitInsideBoundaryBox(pos);
        
            spaceshipToMove.transform.localPosition = pos;
        }

        #endregion


        #region Speed limit

        //Takes the player velocity
        Vector3 velTemp = new Vector3(rb.velocity.x, rb.velocity.y, 0);

        //Check if there's too much acceleration, i.e. if it's over the speed limit
        if (velTemp.magnitude >= playerVelocity)
        {
            //Limits the velocity to the intended one, giving it to the RigidBody
            Vector3 limit = velTemp.normalized * playerVelocity;
            rb.velocity = new Vector3(limit.x, limit.y, rb.velocity.z);
        }

        #endregion
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="position"></param>
    /// <returns>The position calculated inside the boundary box</returns>
    public Vector3 LimitInsideBoundaryBox(Vector3 position)
    {
        position.x = Mathf.Clamp(position.x,
                                 -(boundaryBox.x - playerSize.x) / 2,
                                 (boundaryBox.x - playerSize.x) / 2);
        
        position.y = Mathf.Clamp(position.y,
                                 -(boundaryBox.y - playerSize.y) / 2,
                                 (boundaryBox.y - playerSize.y) / 2);

        return position;
    }



    #region Custom Get Functions

    public Rigidbody GetRB() => rb;

    public Vector2 GetBoundaryBox() => boundaryBox;

    #endregion


    #region EXTRA - Gizmos

    private void OnDrawGizmos()
    {
        //Draws the boundary box
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, boundaryBox);
    }

    private void OnDrawGizmosSelected()
    {
        //Draws the box around the spaceship (player's character)
        Gizmos.color = new Color(0, 1, 0, 0.45f);
        Gizmos.DrawCube(rb.transform.position, playerSize);
    }

    #endregion


    #region EXTRA - Changing the Inspector

    private void OnValidate()
    {
        //Makes the the boundary box sizes always positives
        boundaryBox = new Vector2(Mathf.Clamp(boundaryBox.x, 0, boundaryBox.x),
                                  Mathf.Clamp(boundaryBox.y, 0, boundaryBox.y));

        //Clamps the value of the player sizes inside the boundary box
        //(XYZ always positive but XY inside the bound. box)
        playerSize = new Vector3(Mathf.Clamp(playerSize.x, 0, boundaryBox.x),
                                 Mathf.Clamp(playerSize.y, 0, boundaryBox.y),
                                 Mathf.Clamp(playerSize.z, 0, playerSize.z));
    }

    #endregion
}
