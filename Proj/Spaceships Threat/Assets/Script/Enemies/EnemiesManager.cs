using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesManager : MonoBehaviour
{
    [SerializeField] PlayerMovemRB playerMovScr;
    [SerializeField] NumberedEventsManager enemiesEvMng;
    ObjectPoolingScript poolingScr;

    [Header("—— Enemies' spawn-points ——")]
    [SerializeField] List<Transform> TypeA_spawnPoints;
    [SerializeField] List<Transform> TypeDelta_spawnPoints;
    List<Transform> spawnPointsUsed;


    #region All enemies' pool tags

    [Header("—— Enemies' pools ——")]
    [SerializeField] string typeA_pool = "Type A Enemy";
    [SerializeField] string typeB_pool = "Type B Enemy";
    [SerializeField] string typeC_pool = "Type C Enemy";
    [SerializeField] string typeDelta_pool = "Type Delta Enemy";
    [SerializeField] string typeOmega_pool = "Type Omega Enemy";
    [SerializeField] string asteroids_pool = "Asteroids";
    #endregion


    #region Enemy counting variables

    int animatedEnemiesOnScreen,    //En.s with predefined movement
        inAreaEnemiesOnScreen;      //En.s which move inside the player's bounding box

    int typeA_onScreen,
        typeB_onScreen,
        typeC_onScreen,
        typeDelta_onScreen,
        typeOmega_onScreen,
        maxEnemiesOnScreen;

    int typeA_killed,
        typeB_killed,
        typeC_killed,
        typeDelta_killed,
        typeOmega_killed,
        maxKilled;

    #endregion


    [Space(20)]
    #region Tooltip()
    [Tooltip("The min (X) and max (Y) distance which the \"in area\" enemies \n(Type B, C and Omega) will spawn \n(In the Z axis)")]
    #endregion
    [SerializeField] Vector2 spawnArea_InAreaEnem = new Vector2(25, 50);

    [Space(10)]
    [SerializeField] Vector2 secRangeAsteroidTimer = new Vector2(90, 210);
    CustomTimer asteroidsTimer = new CustomTimer();
    bool canSpawnAsteroid = false;




    private void Awake()
    {
        poolingScr = FindObjectOfType<ObjectPoolingScript>();

        spawnArea_InAreaEnem.x += playerMovScr.transform.position.z;
        spawnArea_InAreaEnem.y += playerMovScr.transform.position.z;

        asteroidsTimer.OnTimerDone_event.AddListener(() => SpawnAsteroid());
        asteroidsTimer.LoopTimer(true);
    }

    void Update()
    {
        //---Management of the Asteroid Timer---//
        if (canSpawnAsteroid)
        {
            asteroidsTimer.AddTimeToTimer();
        }


        #region Max counts

        //Updates the "max animated enemies" counter
        inAreaEnemiesOnScreen = typeA_onScreen
                                + typeDelta_onScreen;
        //Updates the "max enemies in area" counter
        animatedEnemiesOnScreen = typeB_onScreen
                                  + typeC_onScreen
                                  + typeOmega_onScreen;
        //Updates the "max enemies on screen" counter
        maxEnemiesOnScreen = typeA_onScreen
                             + typeB_onScreen
                             + typeC_onScreen
                             + typeDelta_onScreen
                             + typeOmega_onScreen;


        //Updates the "max enemy kill" counter
        maxKilled = typeA_killed
                    + typeB_killed
                    + typeC_killed
                    + typeDelta_killed
                    + typeOmega_killed;

        #endregion
    }


    /// <summary>
    /// Spawn a defined number of enemies in the predefined spawnpoints
    /// <br></br>(used for the Type A or Type Delta enemies)
    /// </summary>
    /// <param name="spawnpoints">The designated spawn points</param>
    /// <param name="enPoolTag">The enemy pool tag</param>
    /// <param name="howMany">How many enemy to spawn?</param>
    void SpawnRandomEnemies(List<Transform> spawnpoints, string enPoolTag, int howMany)
    {
            //Transfers all Type A/Type Delta spawn points
            //into the temporary list
        List<Transform> points_temp = spawnpoints,
                        chosenPoints = new List<Transform>();

        //Selects as many random spawnpoints
        //as many requested
        for (int i = 0; i < howMany; i++)
        {
            int i_point = Random.Range(0, points_temp.Count);
            Transform p = points_temp[i_point];

            //Checks if the screen is filled
            //with Type A or Type Delta enemies
            if (spawnPointsUsed.Count == animatedEnemiesOnScreen)
                return;

            while (spawnPointsUsed.Contains(p))
            {
                i_point = Random.Range(0, points_temp.Count);
                p = points_temp[i_point];
            }

            chosenPoints.Add(p);       //Adds the chosen point in the final list
            points_temp.Remove(p);     //Removes the chosen point from the temp list
            spawnPointsUsed.Add(p);    //Add the chosen point to the one altready taken
        }

        //Spawns all the Type A/Type Delta enemy
        foreach (Transform p in chosenPoints)
        {
            poolingScr.TakeObjectFromPool(enPoolTag, p.position, p.rotation);
        }
    }


    /// <summary>
    /// Spawn a defined number of enemies in the player's bounding box
    /// <br></br>(used for the Type B, Type C, or Type Omega enemies)
    /// </summary>
    /// <param name="spawnpoints">The designated spawn points</param>
    /// <param name="enemyPoolTag">The enemy pool tag</param>
    /// <param name="howMany">How many enemy to spawn?</param>
    void SpawnInAreaEnemies(string enemyPoolTag, int howMany)
    {
        //Selects as many random spawnpoints
        //as many requested
        for (int i = 0; i < howMany; i++)
        {
            Vector3 randomPos;
            Vector2 playerBox = playerMovScr.GetBoundaryBox();

            #region UNUSED
            //Checks if the screen is filled
            //with Type A or Type Delta enemies
            /*if (spawnPointsUsed.Count == inAreaEnemiesOnScreen)
                return;//*/
            #endregion

            randomPos.x = Random.Range(-playerBox.x, playerBox.x);
            randomPos.y = Random.Range(-playerBox.y, playerBox.y);
            randomPos.z = Random.Range(spawnArea_InAreaEnem.x,
                                       spawnArea_InAreaEnem.y);

            //Spawns all the Type B/Type C/Type Omega enemy
            poolingScr.TakeObjectFromPool(enemyPoolTag, randomPos, Quaternion.identity);
        }
    }


    public void SpawnEnemiesAtStart()
    {
        #region Lots of Resets

        //Reset the asteroids
        canSpawnAsteroid = false;
        asteroidsTimer.Restart();

        //Resets the enemies' events count
        enemiesEvMng.ResetCount();

        #endregion


        //Sets the enemies to spawn...
        typeA_onScreen = 4;
        typeB_onScreen = 2;

        //...and spawns them
        SpawnRandomEnemies(TypeA_spawnPoints, typeA_pool, typeA_onScreen);
    }

    void SpawnAsteroid()
    {
        SpawnInAreaEnemies(asteroids_pool, 1);
    }

    void AllEnemiesDead()
    {

    }

    public void ResetEnemiesOnScreen()
    {
        typeA_onScreen = 0;
        typeB_onScreen = 0;
        typeC_onScreen = 0;
        typeDelta_onScreen = 0;
        typeOmega_onScreen = 0;
    }


    #region Custom Set Functions

    public void SetMaxEnemiesOnScreen(int newMax)
    {
        maxEnemiesOnScreen = newMax;
    }

    public void AddKillTypeA()
    {
        typeA_killed++;
    }
    public void AddKillTypeB()
    {
        typeB_killed++;
    }
    public void AddKillTypeDelta()
    {
        typeDelta_killed++;
    }
    public void AddKillTypeOmega()
    {
        typeOmega_killed++;
    }

    #endregion


    #region EXTRA - Gizmos

    private void OnDrawGizmosSelected()
    {
        //Calculates the distances and draws the cube
        Gizmos.color = new Color(1f, 0.25f, 0.25f, 1);
        

        float dist = spawnArea_InAreaEnem.y - spawnArea_InAreaEnem.x;
        
        Vector3 cubePos = new Vector3(playerMovScr.transform.position.x,
                                      playerMovScr.transform.position.y,
                                      spawnArea_InAreaEnem.x + dist / 2);

            //Adds the player's position only if it's not playing
        cubePos.z += !Application.isPlaying ? playerMovScr.transform.position.z : 0;
        
        Vector3 cubeVol = new Vector3(playerMovScr.GetBoundaryBox().x,
                                      playerMovScr.GetBoundaryBox().y,
                                      dist);

        Gizmos.DrawWireCube(cubePos, cubeVol);
    }

    #endregion


    #region EXTRA - Changing the Inspector

    private void OnValidate()
    {
        //Makes the the X always the min and Y the max
        spawnArea_InAreaEnem.x = Mathf.Clamp(spawnArea_InAreaEnem.x, 0, spawnArea_InAreaEnem.y);
    }

    #endregion
}
