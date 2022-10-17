using UnityEngine;
using UnityEngine.SceneManagement;


/// <summary>
/// class to handel the main menu
/// </summary>
public class Main_menu : MonoBehaviour {

    /// <summary>
    /// main menu panel
    /// </summary>
    public GameObject mainMenu;
    /// <summary>
    /// not in use
    /// </summary>
    public GameObject optionsMenu;
    /// <summary>
    /// not in use
    /// </summary>
    public GameObject soundMenu;
    /// <summary>
    /// not in use
    /// </summary>
    public GameObject rebidningMenu;
    /// <summary>
    /// credits menu panel
    /// </summary>
    public GameObject credits;

    /// <summary>
    /// load the first game Scene
    /// </summary>
    public void playGame() {

        PlayerSave s = PlayerSave.loadSettings();

        if (s == null) {
            s = new PlayerSave();
        }

        if (s.TutorialPlayed == true) {
            SceneManager.LoadScene(1);
        }
        else {
            // lade tutorial Scene
            SceneManager.LoadScene(4);
        }


    }
    /// <summary>
    /// quit the game
    /// </summary>
    public void quitGame() {
        Application.Quit();
    }

    /// <summary>
    /// go to the main menu panel
    /// </summary>
    public void onClickBackToMainMenu() {
        mainMenu.SetActive(true);

        if (optionsMenu != null) {
            optionsMenu.SetActive(false);
        }
        if (soundMenu != null) {
            soundMenu.SetActive(false);
        }
        if (rebidningMenu != null) {
            rebidningMenu.SetActive(false);
        }
        if (credits != null) {
            credits.SetActive(false);
        }

    }

    /// <summary>
    /// not in use anymore
    /// </summary>
    public void onClickControls() {
        mainMenu.SetActive(false);
        rebidningMenu.SetActive(true);
    }
    /// <summary>
    /// not in use anymore
    /// </summary>
    public void onClickOptions() {
        mainMenu.SetActive(false);
        optionsMenu.SetActive(true);
    }

    /// <summary>
    /// not in use anymore
    /// </summary>
    public void onClickSound() {
        mainMenu.SetActive(false);
        soundMenu.SetActive(true);
    }
    /// <summary>
    /// not in use anymore
    /// </summary>
    public void onClickBackToOptions() {
        rebidningMenu.SetActive(false);
        soundMenu.SetActive(false);
        optionsMenu.SetActive(true);
    }

    /// <summary>
    /// loads the options sceene and saves the last active scene for going back
    /// </summary>
    /// <param name="index"></param>
    public void onClickOptions(int index) {
        Globals.lastSceneIndex = SceneManager.GetActiveScene().buildIndex;

        SceneManager.LoadScene(index);
    }

    /// <summary>
    /// go towards the credits
    /// </summary>
    public void onClickCredits() {
        credits.SetActive(true);
        mainMenu.SetActive(false);
    }


    public void onClickResetData() {
        PlayerSave s = new PlayerSave();

        s.savingSetting();


        Rebinding_menu defaultRebinding = new Rebinding_menu();

        defaultRebinding.ResetRebinding();
    }
}
