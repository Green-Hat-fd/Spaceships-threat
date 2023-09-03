using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SaveManager : MonoBehaviour
{
    [SerializeField] TextAsset saveTxt;

    [Header("—— Information variables ——")]
    [SerializeField] NumberedEventsManager playCountMng;
    [SerializeField] PlayerStatsSO_Script statsSO;
    [SerializeField] OptionsSO_Script opt_SO;
    [SerializeField] List<PowerUpSO_Script> all_powerUpsSO;

    [Space(20)]
    [SerializeField] string fileName = "unityutilityasset";
    string file_path;

    const string STATS_TITLE = "# STATS #",
                 POWERUPS_TITLE = "# POWER-UPS #",
                 PLAYCOUNT_TITLE = "# PLAY COUNT #",
                 OPTIONS_TITLE = "# OPTIONS #";
    
    
    
    private void Awake()
    {
        //Takes the file path
        file_path = Application.dataPath + "/" + fileName + ".txt";

        //Makes it the first save if there isn't a save file
        if (!File.Exists(file_path))
            playCountMng.LoadCount(0);
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
                          + powerUp.GetIsActive() + "\n"
                          + powerUp.GetIsUnlocked() + "\n"
                          + powerUp.GetBasePrice() + "\n"
                          + powerUp.GetUpgradeStage() + "\n";
        }

        #endregion


        #region -- Play count --

        saveString += "\n" + PLAYCOUNT_TITLE + "\n";

        saveString += playCountMng.GetCount() + "\n";   //Adds the play count

        #endregion


        #region -- Options --

        saveString += "\n" + OPTIONS_TITLE + "\n";

        //Adds all the chosen options
        saveString += (int)opt_SO.GetChosenLanguage() + "\n";
        saveString += opt_SO.GetMusicVolume_Percent() + "\n";
        saveString += opt_SO.GetSoundVolume_Percent() + "\n";
        saveString += opt_SO.GetIsFullscreen() + "\n";

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
        //  5:  - Is Active
        //  6:  - Is Unlocked
        //  7:  - Base price
        //  8:  - Upgrade stage
        //  9:  Name (shielding)
        // 10:  - Is Active
        // 11:  - Is Unlocked
        // 12:  - Base price
        // 13:  - Upgrade stage
        // 14:  Name (dash)
        // 15:  - Is Unlocked
        // 16:  - Is Active
        // 17:  - Base price
        // 18:  - Upgrade stage
        // 19:  Name (stopwatch)
        // 20:  - Is Unlocked
        // 21:  - Is Active
        // 22:  - Base price
        // 23:  - Upgrade stage
        // 24:  
        // 25:  # PLAY COUNT #
        // 26:  Play count
        // 27:  
        // 28:  ### OPTIONS ###
        // 29:  Language
        // 30:  Music volume
        // 31:  Sound volume
        // 32:  Is Fullscreen
        // 33:  
        #endregion
    }


    public void LoadGame()
    {
        string[] fileReading = new string[0];

        int i_stats = 0, i_powerups = 0, i_playcount = 0, i_options = 0;


        //Read the save file
        if (File.Exists(file_path))
        {
            fileReading = File.ReadAllLines(file_path);
        }
        else
        {
            print("[!] Error message");
            return;
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

                case PLAYCOUNT_TITLE:
                    i_playcount = i;
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
            int i_powerupName = i_powerups + (5 * i) + 1;
            
            /* 
             * 5 ---> the lines' skip amount (for each power-up section)
             */

            //Takes the corresponding numbers
            bool active_load = bool.Parse(fileReading[i_powerupName + 1]),
                 unlocked_load = bool.Parse(fileReading[i_powerupName + 2]);
            int basePrice_load = int.Parse(fileReading[i_powerupName + 3]),
                upgradeStage_load = int.Parse(fileReading[i_powerupName + 4]);

            all_powerUpsSO[i].LoadIsActive(active_load);
            all_powerUpsSO[i].LoadIsUnlocked(unlocked_load);
            all_powerUpsSO[i].LoadBasePrice(basePrice_load);
            all_powerUpsSO[i].LoadUpgradeStage(upgradeStage_load);
        }

        #endregion


        #region -- Play count --

        //Turns from string to int
        int playCount_load = int.Parse(fileReading[i_playcount + 1]);

        //Loads the player played games' count
        playCountMng.LoadCount(playCount_load);

        playCountMng.CustomAddCount(0);

        #endregion


        #region -- Options --

        //Turns from string to int
        int language_load = int.Parse(fileReading[i_options + 1]);
        float musicVol_load = float.Parse(fileReading[i_options + 2]),
              soundVol_load = float.Parse(fileReading[i_options + 3]);
        bool fullscreen_load = bool.Parse(fileReading[i_options + 4]);

        //Loads all options numbers
        opt_SO.ChangeLanguage(language_load);
        opt_SO.ChangeMusicVolume(musicVol_load);
        opt_SO.ChangeSoundVolume(soundVol_load);
        opt_SO.ToggleFullscreen(fullscreen_load);

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
            powerUp.LoadIsActive(false);
            powerUp.LoadIsUnlocked(false);
            powerUp.LoadUpgradeStage(0);
        }


        //Saves the game in a new file
        SaveGame();


        //Resets the plays count
        playCountMng.ResetCount();
        playCountMng.CustomAddCount(0);
    }

    public void DeleteSaveFile()
    {
        //If it exists, deletes it
        if (File.Exists(file_path))
            File.Delete(file_path);
    }
}
