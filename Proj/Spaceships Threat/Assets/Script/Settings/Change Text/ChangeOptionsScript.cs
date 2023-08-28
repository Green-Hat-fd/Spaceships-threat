using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChangeOptionsScript : MonoBehaviour
{
    [SerializeField] OptionsSO_Script opt_SO;

    [Header("—— UI elements ——")]
    [SerializeField] Slider sl_musVolume;
    [SerializeField] Slider sl_sfxVolume;
    [SerializeField] TMP_Dropdown dr_language;
    [SerializeField] Toggle tg_fullscreen;



    public void UpdateOptions()
    {
        sl_musVolume.value = opt_SO.GetMusicVolume_Percent() * 10;
        sl_sfxVolume.value = opt_SO.GetSoundVolume_Percent() * 10;
        opt_SO.ChangeMusicVolumeTen(sl_musVolume.value);
        opt_SO.ChangeSoundVolumeTen(sl_sfxVolume.value);

        dr_language.value = (int)opt_SO.GetChosenLanguage();

        tg_fullscreen.isOn = Screen.fullScreen;
    }
}
