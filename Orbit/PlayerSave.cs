using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;



/// <summary>
/// class to save the player progress
/// </summary>
[Serializable]
public class PlayerSave {
    [SerializeField] private int money;
    [SerializeField] private WeaponInfo mainWeapon;
    [SerializeField] private WeaponInfo secondaryWeapon;
    [SerializeField] private WeaponInfo secondaryWeapon1;
    [SerializeField] private Parts shieldPart;
    [SerializeField] private List<string> boughtItems;
    [SerializeField] private bool tutorialPlayed;
    [SerializeField] private bool level1Played;


    /// <summary>
    /// player money
    /// </summary>
    public int Money {
        get {
            return money;
        }

        set {
            money = value;
        }
    }

    /// <summary>
    /// main weapon of player
    /// </summary>
    public WeaponInfo MainWeapon {
        get {
            return mainWeapon;
        }

        set {
            mainWeapon = value;
        }
    }
    /// <summary>
    /// secondary weapon of player
    /// </summary>
    public WeaponInfo SecondaryWeapon {
        get {
            return secondaryWeapon;
        }

        set {
            secondaryWeapon = value;
        }
    }
    /// <summary>
    /// second secondary weapon of player
    /// </summary>
    public WeaponInfo SecondaryWeapon1 {
        get {
            return secondaryWeapon1;
        }

        set {
            secondaryWeapon1 = value;
        }
    }
    /// <summary>
    /// ship parts of player
    /// </summary>
    public Parts ShieldPart {
        get {
            return shieldPart;
        }

        set {
            shieldPart = value;
        }
    }
    /// <summary>
    /// list of bought items in the shop
    /// </summary>
    public List<string> BoughtItems {
        get {
            return boughtItems;
        }

        set {
            boughtItems = value;
        }
    }
    /// <summary>
    /// turorial finished save
    /// </summary>
    public bool TutorialPlayed {
        get {
            return tutorialPlayed;
        }

        set {
            tutorialPlayed = value;
        }
    }
    /// <summary>
    /// level 1 finished save
    /// </summary>
    public bool Level1Played {
        get {
            return level1Played;
        }

        set {
            level1Played = value;
        }
    }

    /// <summary>
    /// base values for a new player
    /// </summary>
    public PlayerSave() {
        money = 0;
        mainWeapon = null;
        secondaryWeapon = null;
        secondaryWeapon1 = null;
        shieldPart = null;
        boughtItems = new List<string>();
        boughtItems.Add("1004");

        tutorialPlayed = true;
        level1Played = false;


        ItemCatalog cat = ItemCatalog.loadSettings();
        mainWeapon = (WeaponInfo)cat.ItemList.Find(x => x.ID == "1004");
    }


    /// <summary>
    /// saves player information
    /// </summary>
    public void savingSetting() {

        //  string json = JsonUtility.ToJson(this);
        using (FileStream file = File.Create(Application.persistentDataPath + "/savePlayer.sav")) {
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(file, this);
        }

    }

    /// <summary>
    /// loads the player information from the saved data, if exists
    /// </summary>
    /// <returns> returns the saved player information </returns>
    public static PlayerSave loadSettings() {

        PlayerSave s = new PlayerSave();


        if (System.IO.File.Exists(Application.persistentDataPath + "/savePlayer.sav")) {

            BinaryFormatter bf = new BinaryFormatter();

            using (FileStream file = File.Open(Application.persistentDataPath + "/savePlayer.sav", FileMode.Open)) {
                s = (PlayerSave)bf.Deserialize(file);
            }


            //string json = File.ReadAllText(Application.persistentDataPath + "/savePlayer.sav");

            //if (json == null || json == "") {
            //    return null;
            //}


            //s = JsonUtility.FromJson<PlayerSave>(json);

            if (s == null) {
                return null;
            }
            return s;

        }
        return null;

    }
}
