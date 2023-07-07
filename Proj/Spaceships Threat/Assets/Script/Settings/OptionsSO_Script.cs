using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

[CreateAssetMenu(menuName = "Scriptable Objects/Options (S.O.)", fileName = "Options_SO")]
public class OptionsSO_Script : ScriptableObject
{
    //Main Menu
    #region Change scene


    public void LoadChosenScene(int sceneNum)
    {
        SceneManager.LoadSceneAsync(sceneNum);
    }
    public void LoadAdditiveScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
    }
    public void LoadAdditiveScene(int sceneNum)
    {
        SceneManager.LoadScene(sceneNum, LoadSceneMode.Additive);
    }
    public void NextScene()
    {
        int sceneNow = SceneManager.GetActiveScene().buildIndex;

        SceneManager.LoadSceneAsync(++sceneNow);
    }
    public void PreviousScene()
    {
        int scenaNow = SceneManager.GetActiveScene().buildIndex;

        SceneManager.LoadSceneAsync(--scenaNow);
    }
    public void CaricaUltimaScena()
    {
        //int scenaDaCheckpoint = checkpointSO.LeggiLivello();
        //int numeroCheckpoint = checkpointSO.LeggiNumCheckpoint();

        ////Carica l'ultima scena se si ha giocato al gioco, se no ricomincia dalla prima
        //if (scenaDaCheckpoint <= 0 || numeroCheckpoint >= 999)
        //{
        //    ResetTutto();
        //    NextScene();
        //}
        //else
        //    ScenaScelta(scenaDaCheckpoint);

        //ascensSO.ScriviDaDoveCambioScena(CambioScena_Enum.DaMenuPrincipale);
        //ascensSO.ScriviPossoMettereInPausa(true);
    }
    void ResetTutto()
    {
        //TODO: reset livello

        //checkpointSO.CambiaCheckpoint(-1, 0, Vector3.zero);
        //rumSO.ResetNumBevute();
        //rumSO.PossoBereDiNuovo();
        //rumSO.DisattivaPoteriRum();
        //rumSO.CambiaRumRaccolto(false);
    }

    #endregion


    #region Exit / Quit

    public void ExitGame()
    {
        Application.Quit();
    }

    #endregion


    //Options
    #region Mouse Sensitivity

    [Space(15)]
    [SerializeField] float sensitivityMultip = 1f;

    public void ChangeSensitivity(float s)
    {
        sensitivityMultip = s;
    }

    public float GetSensitivity() => sensitivityMultip;

    #endregion


    #region Language Selection

    [Space(15)]
    [SerializeField] Language_Enum chosenLanguage;

    public void ChangeLanguage(Language_Enum l)
    {
        chosenLanguage = l;
    }
    public void ChangeLanguage(int i)
    {
        chosenLanguage = (Language_Enum)i;
    }

    public Language_Enum GetChosenLanguage() => chosenLanguage;

    #endregion


    #region Volume and Audio

    [Space(15)]
    [SerializeField] AudioMixer generalMixer;
    [SerializeField] AnimationCurve audioCurve;
    [Range(0, 110)]
    [SerializeField] float musicVolume = 0f;
    [Range(0, 110)]
    [SerializeField] float soundVolume = 0f;

    ///<summary></summary>
    /// <param name="vM"> new volume, in range [0; 1.1]</param>
    public void ChangeMusicVolume(float vM)
    {
        //Puts as volume in the mixer between [-80; 5] dB
        generalMixer.SetFloat("musVol", audioCurve.Evaluate(vM));

        musicVolume = vM * 100;
    }
    ///<summary></summary>
    /// <param name="vS"> new volume, in range [0; 1.1]</param>
    public void ChangeSoundVolume(float vS)
    {
        //Puts as volume in the mixer between [-80; 5] dB
        generalMixer.SetFloat("sfxVol", audioCurve.Evaluate(vS));

        soundVolume = vS * 100;
    }

    public AnimationCurve GetVolumeCurve() => audioCurve;

    public float GetMusicVolume() => audioCurve.Evaluate(musicVolume);
    public float GetMusicVolume_Percent() => musicVolume / 100;
    public float GetSoundVolume() => audioCurve.Evaluate(soundVolume);
    public float GetSoundVolume_Percent() => soundVolume / 100;

    #endregion


    #region Fullscreen

    [Space(15)]
    [SerializeField] bool fullscreen = true;

    public void ToggleFullscreen(bool yn)
    {
        Screen.fullScreen = yn;

        fullscreen = yn;
    }

    #endregion


    //Other
    #region Other functions

    //Languages' Enum
    public enum Language_Enum
    {
        English,
        Italian
    }

    #endregion
}
