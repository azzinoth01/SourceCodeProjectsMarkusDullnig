using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;


#if UNITY_EDITOR
public class ItemCreator : MonoBehaviour
{
    private string iD;
    public List<Button> buttons;
    public GameObject viewObject;

    public Text Info;


    public string weaponName;
    public int value;
    public Sprite icon;
    public Sprite patternIcon;
    public Sprite sprite;
    public bool weapon;
    public int healthBoost;
    public float shildRefreshBoost;
    public bool mainWeapon;
    public GameObject skill;
    public float reloadTime;
    public int shootsToCreate;
    public int additionalDmg;
    public float dmgModifier;

    private SpriteRenderer sp;
    private Weapon w;


    // Start is called before the first frame update
    void Start() {
        sp = viewObject.GetComponent<SpriteRenderer>();
        w = viewObject.GetComponent<Weapon>();

    }

    // Update is called once per frame
    void Update() {


        if (sp.sprite != sprite) {
            sp.sprite = sprite;
        }


        if (w.skill != skill) {
            w.skill = skill;
        }
        if (w.reloadTime != reloadTime) {
            w.reloadTime = reloadTime;
        }
        if (w.shootsToCreate != shootsToCreate) {
            w.shootsToCreate = shootsToCreate;
        }
        if (w.additionalDmg != additionalDmg) {
            w.additionalDmg = additionalDmg;
        }
        if (w.dmgModifier != dmgModifier) {
            w.dmgModifier = dmgModifier;
        }


    }

    public void onchangedID(string ID) {
        iD = ID;
        if (iD != null && iD != "") {
            foreach (Button b in buttons) {
                b.interactable = true;
            }
        }
        else {
            foreach (Button b in buttons) {
                b.interactable = false;
            }
        }
    }

    public void saveWeapon() {
        AssetDatabase.Refresh();
        ItemCatalog cat = ItemCatalog.loadSettings();
        if (cat == null) {
            cat = new ItemCatalog();
        }
        //Debug.Log(iD);
        Item i = cat.ItemList.Find(x => x.ID == iD);

        if (i != null) {
            cat.ItemList.Remove(i);
        }



        if (weapon == true) {
            WeaponInfo wap = new WeaponInfo();
            wap.ID = iD;
            wap.Name = weaponName;
            wap.Value = value;
            wap.Icon = AssetDatabase.GetAssetPath(icon);
            wap.PatternIcon = AssetDatabase.GetAssetPath(patternIcon);
            wap.Sprite = AssetDatabase.GetAssetPath(sprite);
            wap.skill = AssetDatabase.GetAssetPath(skill);
            wap.reloadTime = reloadTime;
            wap.shootsToCreate = shootsToCreate;
            wap.additionalDmg = additionalDmg;
            wap.dmgModifier = dmgModifier;
            wap.mainWeapon = mainWeapon;
            //Debug.Log(wap);
            //Debug.Log(wap.ID);
            cat.ItemList.Add(wap);
        }
        else {
            Parts part = new Parts();

            part.ID = iD;
            part.Name = weaponName;
            part.Value = value;
            part.Icon = AssetDatabase.GetAssetPath(icon);
            part.PatternIcon = AssetDatabase.GetAssetPath(patternIcon);
            part.Sprite = AssetDatabase.GetAssetPath(sprite);
            part.HealthBoost = healthBoost;
            part.ShieldRefreshValueBoost = shildRefreshBoost;

            cat.ItemList.Add(part);

        }
        //Debug.Log(cat.ItemList);

        Info.text = "saving";
        cat.savingSetting();
        AssetDatabase.Refresh();
        Debug.Log("saved");

        Info.text = "saved";
    }

    public void deleteWeapon() {
        AssetDatabase.Refresh();
        ItemCatalog cat = ItemCatalog.loadSettings();
        if (cat == null) {
            cat = new ItemCatalog();
        }

        Item i = cat.ItemList.Find(x => x.ID == iD);

        cat.ItemList.Remove(i);
        Info.text = "deleting";
        cat.savingSetting();
        AssetDatabase.Refresh();
        Info.text = "deleted";
    }

    public void loadWeapon() {
        Info.text = "loading";
        AssetDatabase.Refresh();
        ItemCatalog cat = ItemCatalog.loadSettings();

        if (cat == null) {
            cat = new ItemCatalog();
        }

        Item i = cat.ItemList.Find(x => x.ID == iD);

        if (i is WeaponInfo) {
            WeaponInfo wap = (WeaponInfo)i;
            iD = wap.ID;
            weaponName = wap.Name;
            value = wap.Value;
            icon = AssetDatabase.LoadAssetAtPath<Sprite>(wap.Icon);
            sprite = AssetDatabase.LoadAssetAtPath<Sprite>(wap.Sprite);
            skill = AssetDatabase.LoadAssetAtPath<GameObject>(wap.skill);
            reloadTime = wap.reloadTime;
            shootsToCreate = wap.shootsToCreate;
            additionalDmg = wap.additionalDmg;
            dmgModifier = wap.dmgModifier;

        }
        else if (i is Parts) {
            Parts part = (Parts)i;

            iD = part.ID;
            weaponName = part.Name;
            value = part.Value;
            icon = AssetDatabase.LoadAssetAtPath<Sprite>(part.Icon);
            sprite = AssetDatabase.LoadAssetAtPath<Sprite>(part.Sprite);
            healthBoost = part.HealthBoost;
            shildRefreshBoost = part.ShieldRefreshValueBoost;
        }

        AssetDatabase.Refresh();
        Info.text = "loaded";
    }
}
#endif
