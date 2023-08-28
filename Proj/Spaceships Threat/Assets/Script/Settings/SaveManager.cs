using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SaveManager : MonoBehaviour
{
    [SerializeField] TextAsset saveTxt;

    [Header("—— Information variables ——")]
    [SerializeField] PlayerStatsSO_Script statsSO;
    [SerializeField] OptionsSO_Script optionsSO;
    [SerializeField] List<PowerUpSO_Script> all_powerUpsSO;

    [Space(20)]
    [SerializeField] string fileName = "unityutilityasset";
    string file_path;

    const string STATS_TITLE = "# STATS #",
                 POWERUPS_TITLE = "# POWER-UPS #",
                 OPTIONS_TITLE = "# OPTIONS #";
    
    
    
    private void Awake()
    {
        //Takes the file path
        file_path = Application.dataPath + "/" + fileName + ".txt";
    }



    public void SaveGame()
    {
        string saveString = "";


        #region -- Stats --

        saveString += STATS_TITLE + "\n";

        saveString += statsSO.GetAllScraps() + "\n";   //Adds the collected scraps

        #endregion


        #region -- Power-Ups --

        saveString += "\n" + POWERUPS_TITLE + "\n";

        foreach (var powerUp in all_powerUpsSO)
        {
            saveString += powerUp.name + "\n"
                          + powerUp.GetIsUnlocked() + "\n"
                          + powerUp.GetIsActive() + "\n"
                          + powerUp.GetUpgradeStage() + "\n";
        }

        #endregion


        #region -- Options --

        saveString += "\n" + OPTIONS_TITLE + "\n";

        //Adds all the chosen options
        saveString += (int)optionsSO.GetChosenLanguage() + "\n";
        saveString += optionsSO.GetMusicVolume_Percent() + "\n";
        saveString += optionsSO.GetSoundVolume_Percent() + "\n";
        saveString += optionsSO.GetIsFullscreen() + "\n";

        #endregion


        //Overwrites the file
        //(if it doesn't exist, it creates a new one and writes on it)
        File.WriteAllText(file_path, saveString);



        #region Final save file
        //  0:  ### STATS ###
        //  1:  Tot Scraps
        //  2:  
        //  3:  ### POWER-UPS ###
        //  4:  Name (sonic boom)
        //  5:  - Is Unlocked
        //  6:  - Is Active
        //  7:  - Upgrade stage
        //  8:  Name (shielding)
        //  9:  - Is Unlocked
        // 10:  - Is Active
        // 11:  - Upgrade stage
        // 12:  Name (dash)
        // 13:  - Is Unlocked
        // 14:  - Is Active
        // 15:  - Upgrade stage
        // 16:  Name (stopwatch)
        // 17:  - Is Unlocked
        // 18:  - Is Active
        // 19:  - Upgrade stage
        // 20:  
        // 21:  ### OPTIONS ###
        // 22:  Language
        // 23:  Music volume
        // 24:  Sound volume
        // 25:  Is Fullscreen
        // 26:  
        #endregion
    }


    public void LoadGame()
    {
        string[] fileReading = new string[0];

        int i_stats = 0, i_powerups = 0, i_options = 0;


        //Read the save file
        if (File.Exists(file_path))
        {
            fileReading = File.ReadAllLines(file_path);
        }
        else
        {
            print("[!] Error message");
        }

        #region Finding the starts points

        //Search in the array the start points of the various "regions"
        for (int i = 0; i < fileReading.Length; i++)
        {
            switch (fileReading[i])
            {
                case STATS_TITLE:
                    i_stats = i;
                    break;

                case POWERUPS_TITLE:
                    i_powerups = i;
                    break;

                case OPTIONS_TITLE:
                    i_options = i;
                    break;
            }
        }

        #endregion


        #region -- Stats --

        //Turns from string to int
        int scraps_load = int.Parse(fileReading[i_stats + 1]);

        //Loads the collected scraps number
        statsSO.LoadAllScraps(scraps_load);

        #endregion


        #region -- Power-Ups --

        //Load all power-ups variables
        for (int i = 0; i < 4; i++)
        {
            //Turns from string to int for each power-up's name
            int i_powerupName = i_powerups + (4 * i) + 1;

            //Takes the corresponding numbers
            bool unlocked_load = bool.Parse(fileReading[i_powerupName + 1]),
                 active_load = bool.Parse(fileReading[i_powerupName + 2]);
            int upgradeStage_load = int.Parse(fileReading[i_powerupName + 3]);

            all_powerUpsSO[i].LoadIsUnlocked(unlocked_load);
            all_powerUpsSO[i].LoadIsActive(active_load);
            all_powerUpsSO[i].LoadUpgradeStage(upgradeStage_load);
        }

        #endregion


        #region -- Options --

        //Turns from string to int
        int language_load = int.Parse(fileReading[i_options + 1]);
        float musicVol_load = float.Parse(fileReading[i_options + 2]),
              soundVol_load = float.Parse(fileReading[i_options + 3]);
        bool fullscreen_load = bool.Parse(fileReading[i_options + 4]);

        //Loads all options numbers
        optionsSO.ChangeLanguage(language_load);
        optionsSO.ChangeMusicVolume(musicVol_load);
        optionsSO.ChangeSoundVolume(soundVol_load);
        optionsSO.ToggleFullscreen(fullscreen_load);

        #endregion
    }


    public void GenerateNewGame()
    {
        //Deletes the save file
        DeleteSaveFile();


        //Resets the collected scraps number
        statsSO.LoadAllScraps(0);

        //Resets all power-ups variables
        foreach (var powerUp in all_powerUpsSO)
        {
            powerUp.LoadIsUnlocked(false);
            powerUp.LoadIsActive(false);
            powerUp.LoadUpgradeStage(0);
        }


        //Saves the game in a new file
        SaveGame();
    }

    public void DeleteSaveFile()
    {
        //If it exists, deletes it
        if (File.Exists(file_path))
            File.Delete(file_path);
    }
}
