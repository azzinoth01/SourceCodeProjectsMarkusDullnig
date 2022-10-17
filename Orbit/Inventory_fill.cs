using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

/// <summary>
/// class which controlls the buyable inventory items
/// </summary>
public class Inventory_fill : MonoBehaviour
{
    /// <summary>
    /// main weapon box prefab
    /// </summary>
    public GameObject mainWeaponBox;
    /// <summary>
    /// secondary weapon box prefab
    /// </summary>
    public GameObject secondaryWeaponBox;
    /// <summary>
    /// ship parts box prefab
    /// </summary>
    public GameObject partsBox;
    /// <summary>
    /// locked box prefab
    /// </summary>
    public GameObject LockedBox;
    /// <summary>
    /// drag and drop image which follows the cursor
    /// </summary>
    public Image dragAndDrop;


    /// <summary>
    /// ship item display
    /// </summary>
    public ShipEditorStatusDisplay display;

    /// <summary>
    /// click audio
    /// </summary>
    public AudioSource audios;
    /// <summary>
    /// click not working audio
    /// </summary>
    public AudioSource blockAudio;

    private ItemCatalog catalog;
    private LoadAssets assetLoader;


    /// <summary>
    /// loads the itemCatalog and adds each item into the scroll view with a clickable button
    /// </summary>
    void Start() {
        catalog = ItemCatalog.loadSettings();

        assetLoader = new LoadAssets();

        PlayerSave save = PlayerSave.loadSettings();

        if (save == null) {
            save = new PlayerSave();
        }

        //save.Money = 9999999;
        //save.savingSetting();

        //save.BoughtItems = new List<string>();
        //save.savingSetting();

        foreach (Item i in catalog.ItemList) {
            GameObject g;
            bool locked;
            if (save.BoughtItems.Contains(i.ID)) {
                locked = false;
                if (i is WeaponInfo) {
                    WeaponInfo wap = (WeaponInfo)i;

                    if (wap.mainWeapon == true) {
                        g = Instantiate(mainWeaponBox, transform);
                        Image img = g.transform.GetChild(0).gameObject.GetComponent<Image>();
                        img.sprite = assetLoader.loadSprite(wap.Icon);
                    }
                    else {
                        g = Instantiate(secondaryWeaponBox, transform);
                        Image img = g.transform.GetChild(0).gameObject.GetComponent<Image>();
                        img.sprite = assetLoader.loadSprite(wap.Icon);

                    }
                }
                else {
                    Parts part = (Parts)i;
                    g = Instantiate(partsBox, transform);
                    Image img = g.transform.GetChild(0).gameObject.GetComponent<Image>();
                    img.sprite = assetLoader.loadSprite(part.Icon);

                }
            }
            else {
                locked = true;
                g = Instantiate(LockedBox, transform);
                foreach (Transform t in g.transform.GetChild(0).transform) {
                    if (t.gameObject.name == "money") {
                        t.gameObject.GetComponent<Text>().text = i.Value.ToString();
                    }
                }

            }
            //Button b = g.GetComponent<Button>();
            //b.onClick.AddListener(delegate {
            //    inventoryButton(i, locked, g);
            //});


            Button b = g.GetComponent<Button>();
            b.gameObject.AddComponent<InventoryButton>();

            InventoryButton listener = b.gameObject.GetComponent<InventoryButton>();

            listener.Inv = this;
            listener.Item = i;
            listener.Locked = locked;
            listener.Obj = g;

        }
    }
    /// <summary>
    /// releases all asset wich are loaded at runtime
    /// </summary>
    private void OnDestroy() {
        assetLoader.releaseAllHandle();
    }


    /// <summary>
    /// replaces the locked item button with the corresponding item
    /// </summary>
    /// <param name="g"> the gameobject of the locked item </param>
    /// <param name="i"> the item with which the lock needs to be replaced</param>
    private void replaceLockedItem(GameObject g, Item i) {
        GameObject newGameObject;

        if (i is WeaponInfo) {
            WeaponInfo wap = (WeaponInfo)i;

            if (wap.mainWeapon == true) {
                newGameObject = Instantiate(mainWeaponBox, transform);
                Image img = newGameObject.transform.GetChild(0).gameObject.GetComponent<Image>();
                img.sprite = assetLoader.loadSprite(wap.Icon);

            }
            else {
                newGameObject = Instantiate(secondaryWeaponBox, transform);
                Image img = newGameObject.transform.GetChild(0).gameObject.GetComponent<Image>();
                img.sprite = assetLoader.loadSprite(wap.Icon);

            }
        }
        else {
            Parts part = (Parts)i;
            newGameObject = Instantiate(partsBox, transform);
            Image img = newGameObject.transform.GetChild(0).gameObject.GetComponent<Image>();
            img.sprite = assetLoader.loadSprite(part.Icon);

        }

        newGameObject.transform.SetSiblingIndex(g.transform.GetSiblingIndex());

        Button b = newGameObject.GetComponent<Button>();
        //b.onClick.AddListener(delegate {
        //    inventoryButton(i, false, newGameObject);
        //});


        b.gameObject.AddComponent<InventoryButton>();

        InventoryButton listener = b.gameObject.GetComponent<InventoryButton>();

        listener.Inv = this;
        listener.Item = i;
        listener.Locked = false;
        listener.Obj = g;

        Destroy(g);

    }


    /// <summary>
    /// button function for every inventory button
    /// </summary>
    /// <param name="i"> the item which the button corresponds to</param>
    /// <param name="locked"> if the current item is locked or not</param>
    /// <param name="g"> the gameobject of the button</param>
    public void inventoryButton(Item i, bool locked, GameObject g) {

        if (locked == true) {
            if (Globals.money >= i.Value) {
                Globals.money = Globals.money - i.Value;
                PlayerSave save = PlayerSave.loadSettings();

                if (save == null) {
                    save = new PlayerSave();
                }

                save.Money = Globals.money;
                save.BoughtItems.Add(i.ID);
                save.savingSetting();

                replaceLockedItem(g, i);
                display.MoneyChanged();
                display.changeInfoDispaly(i);
            }
        }
        else {
            dragAndDrop.enabled = true;

            dragAndDrop.sprite = assetLoader.loadSprite(i.Icon);
            Globals.currentItem = i;
            display.changeInfoDispaly(i);

            dragAndDrop.GetComponent<FollowMouse>().MouseIsPressed = true;
        }


    }

    /// <summary>
    /// main weapon equip function
    /// </summary>
    /// <param name="button"> Image of the button</param>
    public void mainWeaponSlotClicked(Image button) {
        if (Globals.currentItem is WeaponInfo) {
            WeaponInfo wep = (WeaponInfo)Globals.currentItem;
            if (wep.mainWeapon == true) {
                button.sprite = assetLoader.loadSprite(wep.Icon);
                button.enabled = true;
                Globals.currentItem = null;
                dragAndDrop.enabled = false;

                PlayerSave save = PlayerSave.loadSettings();
                if (save == null) {
                    save = new PlayerSave();
                }
                save.MainWeapon = wep;

                save.savingSetting();

                audios.Play();
            }
            else {
                blockAudio.Play();
            }

        }
        else {
            blockAudio.Play();
        }
        if (Globals.currentItem == null) {
            PlayerSave save = PlayerSave.loadSettings();
            if (save == null) {
                save = new PlayerSave();
            }

            display.changeInfoDispaly(save.MainWeapon);
        }

    }

    /// <summary>
    /// secondary weapon equip function 1
    /// </summary>
    /// <param name="button"> Image of the button</param>
    public void secondaryWeaponSlotClicked(Image button) {
        if (Globals.currentItem is WeaponInfo) {
            WeaponInfo wep = (WeaponInfo)Globals.currentItem;
            if (wep.mainWeapon == false) {
                button.sprite = assetLoader.loadSprite(wep.Icon);
                button.enabled = true;
                Globals.currentItem = null;
                dragAndDrop.enabled = false;

                PlayerSave save = PlayerSave.loadSettings();
                if (save == null) {
                    save = new PlayerSave();
                }
                save.SecondaryWeapon = wep;

                save.savingSetting();
                audios.Play();
            }
            else {
                blockAudio.Play();
            }
        }
        else {
            blockAudio.Play();
        }
        if (Globals.currentItem == null) {
            PlayerSave save = PlayerSave.loadSettings();
            if (save == null) {
                save = new PlayerSave();
            }

            display.changeInfoDispaly(save.SecondaryWeapon);
        }
    }
    /// <summary>
    /// secondary weapon equip function 2
    /// </summary>
    /// <param name="button"> Image of the button</param>
    public void secondaryWeaponSlotTwoClicked(Image button) {
        if (Globals.currentItem is WeaponInfo) {
            WeaponInfo wep = (WeaponInfo)Globals.currentItem;
            if (wep.mainWeapon == false) {
                button.sprite = assetLoader.loadSprite(wep.Icon);
                button.enabled = true;
                Globals.currentItem = null;
                dragAndDrop.enabled = false;

                PlayerSave save = PlayerSave.loadSettings();
                if (save == null) {
                    save = new PlayerSave();
                }
                save.SecondaryWeapon1 = wep;

                save.savingSetting();
                audios.Play();
            }
            else {
                blockAudio.Play();
            }
        }
        else {
            blockAudio.Play();
        }
        if (Globals.currentItem == null) {
            PlayerSave save = PlayerSave.loadSettings();
            if (save == null) {
                save = new PlayerSave();
            }

            display.changeInfoDispaly(save.SecondaryWeapon1);
        }
    }

    /// <summary>
    /// ship parts equip function
    /// </summary>
    /// <param name="button"> Image of the button</param>
    public void shieldSlotClicked(Image button) {
        if (Globals.currentItem is Parts) {
            Parts part = (Parts)Globals.currentItem;

            button.sprite = assetLoader.loadSprite(part.Icon);
            button.enabled = true;
            Globals.currentItem = null;
            dragAndDrop.enabled = false;

            PlayerSave save = PlayerSave.loadSettings();
            if (save == null) {
                save = new PlayerSave();
            }
            save.ShieldPart = part;

            save.savingSetting();
            audios.Play();

        }
        else {
            blockAudio.Play();
        }
        if (Globals.currentItem == null) {
            PlayerSave save = PlayerSave.loadSettings();
            if (save == null) {
                save = new PlayerSave();
            }

            display.changeInfoDispaly(save.ShieldPart);
        }
    }


    /// <summary>
    /// checks if a right click happend and stops drag dan drop and removes the selected item from the cursor
    /// </summary>
    void Update() {
        if (Globals.virtualMouse.VirtualMouseProperty.rightButton.wasPressedThisFrame) {
            dragAndDrop.enabled = false;
            Globals.currentItem = null;
        }
    }
}
