using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


/// <summary>
/// container classe to save settings
/// </summary>
[Serializable]
public class SaveSettings
{
    [SerializeField] private bool isMute;
    [SerializeField] private float backgroundVolume;
    [SerializeField] private float sfxVolume;
    [SerializeField] private float masterVolume;
    [SerializeField] private bool isToogleOn;


    /// <summary>
    /// returns and sets mute
    /// </summary>
    public bool IsMute {
        get {
            return isMute;
        }

        set {
            isMute = value;
        }
    }
    /// <summary>
    /// returns and sets background volume
    /// </summary>
    public float BackgroundVolume {
        get {
            return backgroundVolume;
        }

        set {
            backgroundVolume = value;
        }
    }
    /// <summary>
    /// returns and sets sfx volume
    /// </summary>
    public float SfxVolume {
        get {
            return sfxVolume;
        }

        set {
            sfxVolume = value;
        }
    }
    /// <summary>
    /// returns and sets master volume
    /// </summary>
    public float MasterVolume {
        get {
            return masterVolume;
        }

        set {
            masterVolume = value;
        }
    }
    /// <summary>
    /// is toogle on
    /// </summary>
    public bool IsToogleOn {
        get {
            return isToogleOn;
        }

        set {
            isToogleOn = value;
        }
    }

    /// <summary>
    /// standard consturktor
    /// </summary>
    /// <param name="isMute"> setzt mute button</param>
    /// <param name="backgroundVolume"> setzt background Volume 0-1</param>
    /// <param name="sfxVolume"> setzt sfx Volume 0-1</param>
    public SaveSettings(bool isMute, float backgroundVolume, float sfxVolume, float masterVolume) {

        this.isMute = isMute;
        this.backgroundVolume = backgroundVolume;
        this.sfxVolume = sfxVolume;
        this.masterVolume = masterVolume;
        isToogleOn = true;
    }

    /// <summary>
    /// saves the settings
    /// </summary>
    public void savingSetting() {

        string json = JsonUtility.ToJson(this);
        using (FileStream file = File.Create(Application.persistentDataPath + "/saveSettings.json")) {
            using (StreamWriter writer = new StreamWriter(file)) {
                writer.Write(json);

            }
        }

    }

    /// <summary>
    /// loads the setting out of a saved data if it exists
    /// </summary>
    /// <returns> returns the saved settings</returns>
    public static SaveSettings loadSettings() {

        SaveSettings s = new SaveSettings(false, 1, 1, 1);


        if (System.IO.File.Exists(Application.persistentDataPath + "/saveSettings.json")) {
            string json = File.ReadAllText(Application.persistentDataPath + "/saveSettings.json");

            if (json == null || json == "") {
                return null;
            }

            s = JsonUtility.FromJson<SaveSettings>(json);
            return s;

        }
        return null;

    }
}
