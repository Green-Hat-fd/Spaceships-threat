using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    InputManager.GeneralActions GeneralInput;

    bool isPaused = false;

    [Header("—— Managers & UIs ——")]
    [SerializeField] MainGameManager mainGameMng;
    [SerializeField] MusicManager musicMng;
    [SerializeField] GameObject pauseWarning;
    [SerializeField] Canvas optionsUI;
    [SerializeField] ChangeOptionsScript changeOptionsScr;
    AudioSource playlistNow_source;
    [Range(0, 1)]
    [SerializeField] float musVolumeWhenPaused = 0.45f;

    [Space(10)]
    [SerializeField] Canvas pauseMenuUI;



    private void Awake()
    {
        musicMng = FindObjectOfType<MusicManager>();

        pauseMenuUI.gameObject.SetActive(false);    //De-activates the pause menu
    }

    void Update()
    {
        GeneralInput = GameManager.inst.inputManager.General;



        //Gets the current playlist
        playlistNow_source = musicMng.GetAudioSourceCurrentMusic();



        if (GeneralInput.Pause.triggered && mainGameMng.GetIsPlayerPlaying())
        {
            //Inverts the pause state when
            //the Pause button is triggered
            //(and when the player is in game)
            ChangeIsPaused(!isPaused);
        }
    }

    public void ChangeIsPaused(bool value)
    {
        isPaused = value;

        PauseGame(isPaused);
    }

    void PauseGame(bool value)
    {
        //(De)Activates the pause menu (and the warning)
        pauseMenuUI.gameObject.SetActive(value);
        optionsUI.gameObject.SetActive(value);
        optionsUI.GetComponent<Animator>().SetTrigger(value ? "Visible" : "Hidden");
        pauseWarning.SetActive(false);

        //Decreases/Increases the volume on the music
        //(see tooltip of the variable "musVolumeWhenPaused")
        playlistNow_source.volume = value
                                     ?
                                    playlistNow_source.volume * musVolumeWhenPaused    //Decreases
                                     :
                                    playlistNow_source.volume / musVolumeWhenPaused;   //Increases

        //(Un)Freezes the time
        Time.timeScale = value ? 0 : 1;


        //Updates the options' menu
        changeOptionsScr.UpdateOptions();
    }
}
