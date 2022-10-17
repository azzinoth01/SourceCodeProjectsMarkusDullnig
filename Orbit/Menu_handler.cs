using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// menu class to handle all menus except the main menu
/// </summary>
public class Menu_handler : MonoBehaviour
{
    /// <summary>
    /// level select UI panel
    /// </summary>
    public GameObject levelSelectUI;
    /// <summary>
    /// ship menu UI panel
    /// </summary>
    public GameObject shipMenuUI;
    /// <summary>
    /// level finished UI panel
    /// </summary>
    public GameObject levelFinishedUI;
    /// <summary>
    /// game over UI panel
    /// </summary>
    public GameObject gameOverUI;
    /// <summary>
    /// pause menu UI panel
    /// </summary>
    public GameObject pauseUI;

    /// <summary>
    /// boss hp bar
    /// </summary>
    public Image bossHpBar;
    /// <summary>
    /// boss UI panel
    /// </summary>
    public GameObject bossUI;

    /// <summary>
    /// not in use anymore
    /// </summary>
    public List<Text> playtimeText;
    /// <summary>
    /// not in use anymore
    /// </summary>
    public List<Text> enemyKilledCounterText;

    /// <summary>
    /// shows score or wave information
    /// </summary>
    public Text score;

    private int currentScore;

    private float playtime;

    /// <summary>
    /// money earned info text
    /// </summary>
    public List<Text> moneyEarnedText;
    /// <summary>
    /// totall money possesed
    /// </summary>
    public List<Text> toalMoneyText;

    /// <summary>
    /// all selecte able buttons
    /// </summary>
    public List<Image> selectedButtonImages;

    /// <summary>
    /// all menu panels
    /// </summary>
    public List<GameObject> menuList;

    /// <summary>
    /// not in use
    /// </summary>
    public Text menuName;

    public Toggle tooltipToogle;

    /// <summary>
    /// not in use anymore
    /// </summary>
    public float Playtime {
        get {
            return playtime;
        }

        set {
            playtime = value;
        }
    }


    /// <summary>
    /// not in use anymore
    /// </summary>
    /// <param name="points"> points to add to score</param>
    public void addScore(int points) {

        if (Globals.waveControler != null) {

        }
        else {
            currentScore = currentScore + points;
            onChangedScore();
        }

    }

    /// <summary>
    /// shows the player Score (not in use anymore)
    /// shows the current wace and enemies to kill in the endless mode
    /// </summary>
    public void onChangedScore() {
        if (Globals.waveControler != null) {
            if (Globals.currentWinCondition != null && score != null) {
                score.text = "Wave: " + Globals.waveControler.CurrentWave + "\r\n" + "Enemies: " + Globals.currentWinCondition.enemysToKill.ToString();
            }

        }
        else {
            score.text = "Score: " + currentScore.ToString();
        }

    }



    //private void Awake() {
    //    Globals.menuHandler = this;
    //    Globals.bossHpBar = bossHpBar;
    //    Globals.bossUI = bossUI;
    //    currentScore = 0;


    //    SaveSettings s = SaveSettings.loadSettings();

    //    if (tooltipToogle != null) {
    //        tooltipToogle.isOn = s.IsToogleOn;
    //    }

    //}

    /// <summary>
    /// sets the menuHandler in the global variables
    /// sets basevalues for the menuHandler
    /// </summary>
    private void OnEnable() {
        Globals.menuHandler = this;
        Globals.bossHpBar = bossHpBar;
        Globals.bossUI = bossUI;
        currentScore = 0;


        SaveSettings s = SaveSettings.loadSettings();

        if (tooltipToogle != null) {
            tooltipToogle.isOn = s.IsToogleOn;
        }
    }

    /// <summary>
    /// ship editor button clicked
    /// </summary>
    /// <param name="sceneIndex"> index of ship editor</param>
    public void onClickShipEditor(int sceneIndex) {
        //Debug.Log("Durch klicken dieses Buttons haben Sie sich verpflichtet Markus Dullnig mit Süßigkeiten zu füttern");

        SceneManager.LoadScene(sceneIndex);
    }
    /// <summary>
    /// ship editor exit button clicked
    /// </summary>
    /// <param name="sceneIndex"> index of the new sceene</param>
    public void onClickExitShipEditor(int sceneIndex) {
        SceneManager.LoadScene(sceneIndex);
    }

    /// <summary>
    /// not in use anymore
    /// </summary>
    public void onClickWorldSelect() {
        shipMenuUI.SetActive(false);
        levelSelectUI.SetActive(true);
    }
    /// <summary>
    /// not in use anymore
    /// </summary>
    public void onClickBack() {
        shipMenuUI.SetActive(true);
        levelSelectUI.SetActive(false);
    }

    /// <summary>
    /// to main menu scene
    /// </summary>
    /// <param name="sceneIndex"> main menu scene index</param>
    public void onClickMainMenu(int sceneIndex) {

        if (Globals.pause == true) {
            setResume();
        }

        SceneManager.LoadScene(sceneIndex);

    }

    /// <summary>
    /// level button clicked
    /// </summary>
    /// <param name="levelSceneIndex">level scene index</param>
    public void onClickLevelSelect(int levelSceneIndex) {
        SceneManager.LoadScene(levelSceneIndex);
    }

    /// <summary>
    /// next level button clicked
    /// </summary>
    /// <param name="backupSeneIndex"> backup scene index if no next level exists</param>
    public void onClickNextLeve(int backupSeneIndex) {

        int nextLevelIndex = SceneManager.GetActiveScene().buildIndex + 1;

        if (SceneManager.sceneCountInBuildSettings <= nextLevelIndex) {
            levelChanges();
            SceneManager.LoadScene(backupSeneIndex);

        }
        else {
            levelChanges();
            SceneManager.LoadScene(nextLevelIndex);

        }


    }

    /// <summary>
    /// ship menu button clicked
    /// </summary>
    /// <param name="sceneIndex"> ship menu scene index</param>
    public void onClickShipMenu(int sceneIndex) {
        levelChanges();
        SceneManager.LoadScene(sceneIndex);

    }

    /// <summary>
    /// load current scene again
    /// </summary>
    public void onClickTryAgain() {

        levelChanges();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    }


    /// <summary>
    /// reset variables on level change
    /// </summary>
    private void levelChanges() {
        if (Globals.pause == true) {
            Debug.Log("pause stopped");
            setResume();
        }

        Globals.bulletPool.Clear();
        //Debug.Log("clear");
        //Debug.Log(Globals.bulletPool.Count);
        //try {
        try {
            Player p = Globals.player.GetComponent<Player>();
            p.clearControlls();
        }
        catch {

        }

        Globals.infityWaveSpawner.Clear();

    }

    /// <summary>
    /// activates the pause menu
    /// </summary>
    public void setPause() {
        Globals.pause = true;
        Time.timeScale = 0;
        pauseUI.SetActive(true);
    }
    /// <summary>
    /// deactivates the pause menu
    /// </summary>
    public void setResume() {
        Globals.pause = false;
        Time.timeScale = 1;
        if (pauseUI != null) {
            pauseUI.SetActive(false);
        }

    }

    /// <summary>
    /// activates the game over menu
    /// </summary>
    public void setGameOver() {
        Globals.pause = false;
        Time.timeScale = 1;
        gameOverUI.SetActive(true);

        PlayerSave s = PlayerSave.loadSettings();

        if (s == null) {
            s = new PlayerSave();
        }

        int moneyEarned = 0;

        moneyEarned = Globals.money - s.Money;

        s.Money = Globals.money;

        foreach (Text t in moneyEarnedText) {
            t.text = "Scraps earned " + moneyEarned.ToString();
        }
        foreach (Text t in toalMoneyText) {
            t.text = "Total Scraps " + Globals.money;
        }



        //Debug.Log(s.Money);
        s.savingSetting();
        //Debug.Log(Globals.money);

        foreach (Text t in playtimeText) {
            t.text = "Playtime: " + playtime.ToString() + " Seconds";
        }
        foreach (Text t in enemyKilledCounterText) {
            t.text = "Enemies killed: " + (currentScore / 100).ToString();
        }
        levelChanges();
    }

    /// <summary>
    /// activates the level finish menu
    /// </summary>
    public void setLevelFinish() {


        levelChanges();
        levelFinishedUI.SetActive(true);

        PlayerSave s = PlayerSave.loadSettings();

        if (s == null) {
            s = new PlayerSave();
        }

        // 3 ist die Tutorial scene
        if (SceneManager.GetActiveScene().buildIndex == 4) {
            s.TutorialPlayed = true;
        }

        if (SceneManager.GetActiveScene().buildIndex == 5) {
            s.Level1Played = true;
        }
        int moneyEarned = 0;

        moneyEarned = Globals.money - s.Money;

        s.Money = Globals.money;

        foreach (Text t in moneyEarnedText) {
            t.text = "Scraps earned " + moneyEarned.ToString();
        }
        foreach (Text t in toalMoneyText) {
            t.text = "Total Scraps " + Globals.money;
        }

        //Debug.Log(s.Money);
        s.savingSetting();

        foreach (Text t in playtimeText) {
            t.text = "Playtime: " + playtime.ToString() + " Seconds";
        }
        foreach (Text t in enemyKilledCounterText) {
            int i = currentScore - 1000;

            t.text = "Enemies killed: " + (i / 100).ToString() + " und Bosse killed: 1";
        }

    }


    /// <summary>
    /// options button clieckd
    /// saves the current scene index for going back to this scene
    /// </summary>
    /// <param name="index"> index of options menu</param>
    public void onClickOptions(int index) {
        Globals.lastSceneIndex = SceneManager.GetActiveScene().buildIndex;

        SceneManager.LoadScene(index);
    }

    /// <summary>
    /// goes back to the last saved scene index
    /// </summary>
    public void onClickGoBackToPreviousScene() {
        int index = Globals.lastSceneIndex;
        Globals.lastSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(index);
    }

    /// <summary>
    /// button select function
    /// </summary>
    /// <param name="button"> image to be enabled when the button is selected</param>
    public void onSelectButton(Image button) {
        foreach (Image img in selectedButtonImages) {
            img.enabled = false;

        }

        button.enabled = true;
    }

    /// <summary>
    /// shows the set menu
    /// </summary>
    /// <param name="game"> menu to show</param>
    public void onClickMenuItem(GameObject game) {
        foreach (GameObject gobj in menuList) {
            gobj.SetActive(false);
        }
        game.SetActive(true);
    }

    /// <summary>
    /// activates the set gameobject on click
    /// </summary>
    /// <param name="game"> the game object to set active</param>
    public void onClickActivateGameobnect(GameObject game) {
        game.SetActive(true);
    }

    /// <summary>
    /// not in use anymore
    /// </summary>
    /// <param name="name"></param>
    public void onClickSetMenuName(string name) {
        menuName.text = name;
    }

    /// <summary>
    /// on click tooltip button
    /// saves the tooltip on/off state
    /// </summary>
    /// <param name="game"> the tooltip button</param>
    public void onclickSetToogle(GameObject game) {
        SaveSettings s = SaveSettings.loadSettings();

        if (s.IsToogleOn == true) {
            Globals.tooltip.tooltipToogled = false;
            s.IsToogleOn = false;
            TooltipSystem.Hide();

        }
        else {
            s.IsToogleOn = true;
            Globals.tooltip.tooltipToogled = true;

            TooltipTrigger triger = game.GetComponent<TooltipTrigger>();


            TooltipSystem.Show(triger.content, triger.header);
        }

        Debug.Log("tooltip is " + s.IsToogleOn.ToString());
        s.savingSetting();
    }

    /// <summary>
    /// on changes of the tooltip toogle
    /// saves the tooltip on/off state
    /// </summary>
    /// <param name="tog"> the toogle which cause the change</param>
    public void onTooltipToogleChanged(Toggle tog) {
        SaveSettings s = SaveSettings.loadSettings();

        s.IsToogleOn = tog.isOn;


        Globals.tooltip.tooltipToogled = tog.isOn;

        s.savingSetting();

        Debug.Log("tooltip is " + tog.isOn.ToString());
    }

}
