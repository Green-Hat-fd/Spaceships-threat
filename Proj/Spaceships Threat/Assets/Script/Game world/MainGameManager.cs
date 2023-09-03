using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;

public class MainGameManager : MonoBehaviour
{
    [SerializeField] PlayerStatsSO_Script stats_SO;
    ObjectPoolingScript poolingScr;

    [Header("—— Managers & UIs ——")]
    [SerializeField] SaveManager saveMng;
    [SerializeField] ChangeOptionsScript changeOptScr;
    [SerializeField] EnemiesManager enemyMng;
    [SerializeField] NumberedEventsManager playCountMng;
    [SerializeField] PauseManager pauseMng;

    [SerializeField] Canvas mainGameUI;
    [SerializeField] Canvas gameOverUI,
                            mainMenuUI,
                            powerUpsUI,
                            optionsUI;
    [SerializeField] GameObject mainMenuObjects;
    [SerializeField] GameObject newGameWarningObj;

    [Header("—— Objects and Scripts to activate/enable ——")]
    [SerializeField] PlayerStatsManager statsMng;
    [SerializeField] List<MonoBehaviour> scriptToEnable;
    [SerializeField] List<GameObject> objectsToActivate;

    [Header("—— Other ——")]
    [Range(0, 1)]
    [SerializeField] float scrapsFromPausePercent = 0.25f;

    [Header("—— Feedback ——")]
    [SerializeField] MusicManager musicMng;
    [Min(0)]
    [SerializeField] int mainMenuMusicIndex = 0,
                         gameMusicIndex = 1;
    [Space(10)]
    [SerializeField] ParticleSystem menuStars_part;
    [SerializeField] ParticleSystem movingStarts_part;
    [SerializeField] ParticleSystem playerDeathPart;

    bool isPlayerPlaying = false; 




    void Awake()
    {
        musicMng = FindObjectOfType<MusicManager>();
        poolingScr = FindObjectOfType<ObjectPoolingScript>();

        saveMng.LoadGame();             //Loads the game from the save file

        _ReturnToMainMenu();
        changeOptScr.UpdateOptions();
    }



    #region Utility functions

    public void ActivatePlayer(bool value)
    {
        foreach (var script in scriptToEnable)
        {
            script.enabled = value;
        }
        foreach (GameObject obj in objectsToActivate)
        {
            obj.SetActive(value);
        }

        statsMng.transform.localPosition = Vector3.zero;    //Reset the player's position

        isPlayerPlaying = value;    //Set the playing state
    }


    public bool GetIsPlayerPlaying() => isPlayerPlaying;

    #endregion



    #region Starting up a new game

    public void FirstSetup()
    {
        stats_SO.ResetHealth();
        statsMng.CheckPowerUp();
        stats_SO.ResetPlayerStats();

        enemyMng.ResetEnemiesOnScreen();
        enemyMng.SpawnEnemiesAtStart();

        pauseMng.ChangeIsPaused(false);            //The game is not paused
        gameOverUI.gameObject.SetActive(false);    //Hides the Game Over screen
    }
    public void StartGame()
    {
        //Causes to "start" moving the stars on the screen
        menuStars_part.Stop();
        menuStars_part.gameObject.SetActive(false);      //Hides the static stars
        movingStarts_part.gameObject.SetActive(true);    //Shows the moving stars
        movingStarts_part.Play();

        //Chages music to the Game one
        musicMng.ChangeMusicPlaylist(gameMusicIndex);


        //Shows the player
        ActivatePlayer(true);
    }

    #endregion



    #region Returning to the Main Menu

    void _ReturnToMainMenu()
    {
        //Hides the player & blocks it from moving
        ActivatePlayer(false);


        //"Stops" the stars on the screen from moving
        menuStars_part.Play();
        menuStars_part.gameObject.SetActive(true);        //Shows the static stars
        movingStarts_part.gameObject.SetActive(false);    //Hides the moving stars
        playerDeathPart.Clear();

        //Chages music to the MainMenu one
        musicMng.ChangeMusicPlaylist(mainMenuMusicIndex);

        //Activates all the menus and objects
        powerUpsUI.gameObject.SetActive(true);
        mainMenuObjects.SetActive(true);
        mainMenuUI.gameObject.SetActive(true);
        mainMenuUI.GetComponent<Animator>().SetTrigger("Visible");
        optionsUI.gameObject.SetActive(true);
        optionsUI.GetComponent<Animator>().SetTrigger("Visible");
        newGameWarningObj.SetActive(false);

        //Increases to the play count
        playCountMng.IncreaseCount();
    }

    public void ReturnToMainMenuFromDeath()
    {
        //Hides the Game Over screen & the game HUD
        mainGameUI.gameObject.SetActive(false);
        gameOverUI.gameObject.SetActive(false);
        gameOverUI.GetComponent<Animator>().SetTrigger("Hidden");

        //Hides the death particles
        playerDeathPart.Stop();

        //Adds the temporary Scraps
        //to the max collected Scraps
        stats_SO.ResetPlayerStats();

        //Saves the game
        saveMng.SaveGame();



        _ReturnToMainMenu();
    }

    public void ReturnToMainMenuFromPause()
    {
        //Adds only a percentage of the temporary Scraps
        //to the max collected Scraps
        stats_SO.ResetPlayerStats(scrapsFromPausePercent);

        //Saves the game
        saveMng.SaveGame();


        //Removes all the entities
        poolingScr.HideEveryPool();

        //Deactivates all game menus
        mainGameUI.gameObject.SetActive(false);

        //Unfreezes the time
        Time.timeScale = 1;



        _ReturnToMainMenu();
    }

    #endregion
}
