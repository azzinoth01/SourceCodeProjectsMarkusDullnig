using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RessourcesDisplay : MonoBehaviour {
    public RessourcesDisplayManager manager;
    public RessourcesDisplayInfo info;
    public TMP_Text text;
    public Image render;



    public void UpdateDisplay() {

        string setText = info.Name + ": ";

        RessourcesValue ressources = manager.PlayerBilanz.Player.GetRessourcesValue(info.Ressources);

        if (ressources == null) {
            text.SetText(setText);
            return;
        }
        RessourceInfo ressourceInfo = EconemySystemInfo.Instanz.GetRessourceDescription(ressources.Ressources);

        if (ressourceInfo.Level != 0) {
            gameObject.SetActive(false);
        }

        if (ressourceInfo.Type == RessourceTyp.limitType && ressourceInfo.CanGoNegativ == false) {
            text.SetText(setText + ressources.Value.ToString("F0") + "/" + ressources.MaxValue.ToString("F0"));
        }
        else if (ressourceInfo.Type == RessourceTyp.limitType) {
            //text.SetText(setText + (ressources.MaxValue - manager.PlayerBilanz.Player.GetTotalConsumption(ressources.Ressources).Value).ToString("F0") + "/" + ressources.MaxValue.ToString("F0"));
        }
        else {
            text.SetText(setText + ressources.Value.ToString("F0"));
        }

    }
}
