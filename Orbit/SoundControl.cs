using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

/// <summary>
/// controls sound effects
/// </summary>
public class SoundControl : MonoBehaviour
{
    /// <summary>
    /// master sound group
    /// </summary>
    public AudioMixerGroup masterGroup;
    /// <summary>
    /// background sound group
    /// </summary>
    public AudioMixerGroup background;
    /// <summary>
    /// sfx sound group
    /// </summary>
    public AudioMixerGroup sfx;


    private bool isMute;
    private float backgroundVolume;
    private float sfxVolume;
    private float masterVolume;

    private SaveSettings saveSetting;

    /// <summary>
    /// mute button image
    /// </summary>
    public Image muteButton;
    /// <summary>
    /// background sound volume slider
    /// </summary>
    public Slider backgroundSlider;
    /// <summary>
    /// sfx sound volume slider
    /// </summary>
    public Slider sfxSlider;
    /// <summary>
    /// mute sprite
    /// </summary>
    public Sprite muteSp;
    /// <summary>
    /// unmute sprite
    /// </summary>
    public Sprite unMuteSp;
    /// <summary>
    /// master sound volume slider
    /// </summary>
    public Slider masterSlider;




    /// <summary>
    /// loads the start soundsettings if they exists else the standard settings will be used
    /// </summary>
    private void Start() {

        saveSetting = SaveSettings.loadSettings();
        if (saveSetting == null) {

            saveSetting = new SaveSettings(false, 1, 1, 1);
            saveSetting.savingSetting();

        }

        isMute = saveSetting.IsMute;
        backgroundVolume = saveSetting.BackgroundVolume;
        sfxVolume = saveSetting.SfxVolume;
        masterVolume = saveSetting.MasterVolume;


        setStartSettings();
    }




    /// <summary>
    /// sets the loaded setting on the UI sets the soundgroups
    /// </summary>
    public void setStartSettings() {
        if (isMute == true) {

            muteButton.sprite = muteSp;

        }
        else {

            muteButton.sprite = unMuteSp;

        }

        float volume;

        if (backgroundVolume == 0) {
            volume = -80;
        }
        else {
            volume = Mathf.Log10(backgroundVolume) * 20;
        }
        background.audioMixer.SetFloat("backgroundVolume", volume);

        backgroundSlider.value = backgroundVolume;

        if (sfxVolume == 0) {
            volume = -80;
        }
        else {
            volume = Mathf.Log10(sfxVolume) * 20;
        }

        //Debug.LogError(volume);

        sfx.audioMixer.SetFloat("sfxVolume", volume);

        sfxSlider.value = sfxVolume;



        if (masterVolume == 0 || isMute == true) {
            volume = -80;
        }
        else {
            volume = Mathf.Log10(masterVolume) * 20;
        }
        masterGroup.audioMixer.SetFloat("masterVolume", volume);

        if (masterSlider != null) {
            masterSlider.value = masterVolume;
        }



    }


    /// <summary>
    /// toggel the mute button
    /// </summary>
    public void toggleMute() {

        if (isMute == false) {

            muteButton.sprite = muteSp;
            isMute = true;
        }
        else {

            muteButton.sprite = unMuteSp;
            isMute = false;
        }
        saveSettingChanges();
    }

    /// <summary>
    /// sets the sfx sound after changing the slider
    /// </summary>
    public void sfxChanged() {
        sfxVolume = sfxSlider.value;

        float volume;

        if (sfxVolume == 0) {
            volume = -80;
        }
        else {
            volume = Mathf.Log10(sfxVolume) * 20;
        }
        //Debug.LogError(volume);

        sfx.audioMixer.SetFloat("sfxVolume", volume);
        saveSettingChanges();
    }

    /// <summary>
    /// sets the backgroundsound after changing the slider
    /// </summary>
    public void backgroundSoundChange() {

        backgroundVolume = backgroundSlider.value;

        //Debug.Log("changed");
        float volume;

        if (backgroundVolume == 0) {
            volume = -80;
        }
        else {
            volume = Mathf.Log10(backgroundVolume) * 20;
        }
        background.audioMixer.SetFloat("backgroundVolume", volume);

        saveSettingChanges();
    }

    /// <summary>
    /// sets the master sound after changing the slider
    /// </summary>
    public void masterSoundChange() {
        masterVolume = masterSlider.value;

        //Debug.Log("changed");
        float volume;

        if (masterVolume == 0) {
            volume = -80;
        }
        else {
            volume = Mathf.Log10(backgroundVolume) * 20;
        }
        masterGroup.audioMixer.SetFloat("masterVolume", volume);

        saveSettingChanges();
    }

    /// <summary>
    /// saves the sound settings
    /// </summary>
    public void saveSettingChanges() {
        saveSetting.IsMute = isMute;
        saveSetting.BackgroundVolume = backgroundVolume;
        saveSetting.SfxVolume = sfxVolume;
        saveSetting.MasterVolume = masterVolume;

        saveSetting.savingSetting();

        setStartSettings();
    }
}
