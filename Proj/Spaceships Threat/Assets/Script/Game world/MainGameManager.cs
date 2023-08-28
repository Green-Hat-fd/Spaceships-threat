using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;

public class MainGameManager : MonoBehaviour
{
    [SerializeField] PlayerStatsSO_Script statsSO;

    [Header("—— Managers & UIs ——")]
    [SerializeField] SaveManager saveManager;
    [SerializeField] ChangeOptionsScript changeOptScript;
    [SerializeField] EnemiesManager enemyManager;

    [SerializeField] Canvas _mainGameUI;
    [SerializeField] Canvas _gameOverUI,
                            _mainMenuUI,
                            _optionsUI;
    GameObject mainGameUI_obj,
               gameOverUI_obj,
               mainMenuUI_obj,
               optionsUI_obj;
    [SerializeField] GameObject mainMenuObjects;

    [Header("—— Objects and Scripts to activate/enable ——")]
    [SerializeField] PlayerStatsManager statsManager;
    [SerializeField] List<MonoBehaviour> scriptToEnable;
    [SerializeField] List<GameObject> objectsToActivate;

    [Header("—— Other ——")]
    [Range(0, 1)]
    [SerializeField] float scrapsFromPausePercent = 0.25f;

    [Header("—— Feedback ——")]
    [SerializeField] MusicManager musicManager;
    [Min(0)]
    [SerializeField] int mainMenuMusicIndex = 0,
                         gameMusicIndex = 1;
    [Space(10)]
    [SerializeField] ParticleSystem menuStars_part;
    [SerializeField] ParticleSystem movingStarts_part;
    [SerializeField] ParticleSystem playerDeathPart;



    void Awake()
    {
        musicManager = FindObjectOfType<MusicManager>();
        mainMenuUI_obj = _mainMenuUI.gameObject;
        mainGameUI_obj = _mainGameUI.gameObject;
        optionsUI_obj = _optionsUI.gameObject;
        gameOverUI_obj = _gameOverUI.gameObject;

        saveManager.LoadGame();             //Loads the game from the save file

        _ReturnToMainMenu();
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

        statsManager.transform.localPosition = Vector3.zero;    //Reset the player's position
    }

    #endregion



    #region Starting up a new game

    public void FirstSetup()
    {
        statsSO.ResetPlayerStats();
        

        gameOverUI_obj.SetActive(false);    //Hides the Game Over screen
    }
    public void StartGame()
    {
        //Causes to "start" moving the stars on the screen
        menuStars_part.Stop();
        menuStars_part.gameObject.SetActive(false);      //Hides the static stars
        movingStarts_part.gameObject.SetActive(true);    //Shows the moving stars
        movingStarts_part.Play();

        //Chages music to the Game one
        musicManager.ChangeMusicPlaylist(gameMusicIndex);


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
        musicManager.ChangeMusicPlaylist(mainMenuMusicIndex);

        //Activates all the menus and objects
        optionsUI_obj.SetActive(true);
        mainMenuObjects.SetActive(true);
        mainMenuUI_obj.SetActive(true);
        mainMenuUI_obj.GetComponent<Animator>().SetTrigger("Show");
    }

    public void ReturnToMainMenuFromDeath()
    {
        //Hides the Game Over screen & the game HUD
        mainGameUI_obj.SetActive(false);
        gameOverUI_obj.SetActive(false);
        gameOverUI_obj.GetComponent<Animator>().SetTrigger("Hide");

        //Hides the death particles
        playerDeathPart.Stop();

        //Adds the temporary Scraps
        //to the max collected Scraps
        statsSO.ResetPlayerStats();

        //Saves the game
        saveManager.SaveGame();



        _ReturnToMainMenu();
    }

    public void ReturnToMainMenuFromPause()
    {
        //Adds only a percentage of the temporary Scraps
        //to the max collected Scraps
        statsSO.ResetPlayerStats(scrapsFromPausePercent);

        //Saves the game
        saveManager.SaveGame();



        _ReturnToMainMenu();
    }

    #endregion
}
