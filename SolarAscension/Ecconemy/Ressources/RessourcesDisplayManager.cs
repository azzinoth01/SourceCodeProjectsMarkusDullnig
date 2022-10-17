using System.Collections.Generic;
using UnityEngine;

public class RessourcesDisplayManager : MonoBehaviour {



    public GameObject DisplayPrefab;
    public PlayerBilanz PlayerBilanz;



    private List<RessourcesDisplay> _displayList;




    private void Start() {


        if (_displayList == null) {
            _displayList = new List<RessourcesDisplay>();

            foreach (RessourceInfo info in EconemySystemInfo.Instanz.RessourceDescription.Values) {
                GameObject g = Instantiate(DisplayPrefab, transform);
                RessourcesDisplay display = g.GetComponent<RessourcesDisplay>();
                display.manager = this;
                display.info = new RessourcesDisplayInfo(info.Ressources, info.Type, info.Name, info.Icon);
                _displayList.Add(display);
            }
        }


    }

    private void Update() {
        foreach (RessourcesDisplay display in _displayList) {

            display.UpdateDisplay();
        }
    }

}
