using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// manages the skills of the enemy
/// </summary>
public class Enemy_skills : MonoBehaviour
{
    /// <summary>
    /// how many skill objects have to be pre created
    /// </summary>
    public int shootsToCreate;
    /// <summary>
    /// the skillsequenze list
    /// </summary>
    public List<Skillsequenze> skillsequenze;
    private GameObject nextSkill;
    private float nextSkillDelay;
    private int skillIndex;

    /// <summary>
    /// the addition dmg of the enemy
    /// </summary>
    public float additionalDmg;
    /// <summary>
    /// the dmg modifier of the enemy
    /// </summary>
    public float dmgModifier;

    private bool isRunning;
    private bool allwoDisable;

    private bool nextSkillRotate;
    /// <summary>
    /// stops the deactivation of skills if the enemy moves out of the enemy border
    /// </summary>
    public bool doNotDeactivate;

    /// <summary>
    /// precreates skill gameObjects to lessen the burden while the game is running
    /// </summary>
    private void Awake() {
        nextSkill = skillsequenze[0].Skill;
        nextSkillDelay = skillsequenze[0].Delay;
        nextSkillRotate = skillsequenze[0].ShootInRotatedDirection;
        skillIndex = 0;
        preCreateSkill();
    }

    /// <summary>
    /// sets the base values for the skills to use
    /// </summary>
    void Start() {
        nextSkill = skillsequenze[0].Skill;
        nextSkillDelay = skillsequenze[0].Delay;
        nextSkillRotate = skillsequenze[0].ShootInRotatedDirection;
        skillIndex = 0;
        isRunning = false;
    }

    /// <summary>
    /// starts the corutine to start the skill usage
    /// </summary>
    void Update() {
        if (Globals.pause == true) {
            return;
        }
        else {

            if (isRunning == false) {
                isRunning = true;
                StartCoroutine(startSkillTimer(nextSkillDelay));
            }

        }

    }

    /// <summary>
    /// timer for the activation of skills
    /// </summary>
    /// <param name="waitSeconds"> delay of the skill in seconds</param>
    /// <returns></returns>
    private IEnumerator startSkillTimer(float waitSeconds) {

        yield return new WaitForSeconds(waitSeconds);
        activateSkill(false);
        isRunning = false;
    }


    /// <summary>
    /// function to precreate Skill objects to lessen the burden while the game is running
    /// </summary>
    private void preCreateSkill() {
        bool needToCreate = false;
        // Debug.Log(Globals.bulletPool.Count);
        foreach (Skillsequenze s in skillsequenze) {
            if (Globals.bulletPool.Count(x => x.gameObject.name == s.Skill.name && x.gameObject.activeSelf == false) < (shootsToCreate / skillsequenze.Count)) {
                needToCreate = true;
                break;
            }
        }


        if (needToCreate == true) {
            for (int i = 0; i < shootsToCreate;) {

                GameObject skill = activateSkill(true);
                skill.SetActive(false);


                i = i + 1;
            }
        }

    }

    /// <summary>
    /// erzeugt Skills und setzt diese auf die Richtige position und activiert diese
    /// prüft vor erzeugung neuer Skills ob diese im bulletpool sind
    /// kann auch Skills im voraus erzeugen, dort wird die Position nicht gesetzt
    /// 
    /// creates skills and places them on the right position and activates them
    /// checks befor creating new Skill objecte if there is a suitable object in the bulletpool
    /// can also create skills ahead of time
    /// </summary>
    /// <param name="preCreation"> if this value is true, then skills will be created ahead of time</param>
    /// <returns> Gameobject of Skill</returns>
    private GameObject activateSkill(bool preCreation) {
        Skill skill;
        GameObject skillGameObject;
        if (preCreation == false) {
            skill = Globals.bulletPool.Find(x => x.gameObject.name == nextSkill.name && x.gameObject.activeSelf == false);
            if (skill == null) {
                if (nextSkillRotate == true) {
                    skillGameObject = Instantiate(nextSkill, transform.position, transform.rotation);
                }
                else {
                    skillGameObject = Instantiate(nextSkill, transform.position, Quaternion.identity);
                }

                skillGameObject.name = nextSkill.name;
                skillGameObject.layer = (int)Layer_enum.enemy_bullets; // enemy bullet layer ist immer enemy layer -1

                skillGameObject.GetComponent<Skill>().layerChange();
                skillGameObject.GetComponent<Skill>().setDmgModifiers(additionalDmg, dmgModifier);
                Debug.Log("additional skill created");
            }
            else {
                Globals.bulletPool.Remove(skill);
                skill.transform.position = transform.position;
                if (nextSkillRotate == true) {
                    skill.transform.rotation = transform.rotation;
                }
                else {
                    skill.transform.rotation = Quaternion.identity;
                }


                skill.gameObject.layer = (int)Layer_enum.enemy_bullets;
                skill.setDmgModifiers(additionalDmg, dmgModifier);
                skill.gameObject.SetActive(true);
                skillGameObject = skill.gameObject;

            }

        }
        else {
            skillGameObject = Instantiate(nextSkill);
            skillGameObject.name = nextSkill.name;
            skillGameObject.layer = (int)Layer_enum.enemy_bullets;

        }
        skillIndex = skillIndex + 1;

        if (skillIndex == skillsequenze.Count) {
            skillIndex = 0;
        }

        nextSkill = skillsequenze[skillIndex].Skill;
        nextSkillDelay = skillsequenze[skillIndex].Delay;
        nextSkillRotate = skillsequenze[skillIndex].ShootInRotatedDirection;

        return skillGameObject;
    }

    /// <summary>
    /// checks if the enemy move over the enemy border and deactivates the usage of skills
    /// </summary>
    /// <param name="collision"> collision object</param>
    private void OnTriggerEnter2D(Collider2D collision) {
        //Debug.Log(collision);
        try {
            if (collision.gameObject.tag == Tag_enum.enemy_border.ToString()) {
                if (allwoDisable == true) {

                    if (doNotDeactivate == true) {

                    }
                    else {
                        enabled = false;
                    }

                }

            }
        }
        catch {

        }
    }

    /// <summary>
    /// function to create a disable lock so the the class does not imediatly disable itself after crossing the enemy border
    /// </summary>
    private void OnEnable() {
        //Debug.Log("enable");
        allwoDisable = false;

        StartCoroutine(canDisableTimer(1));
    }


    /// <summary>
    /// disable allow timer
    /// </summary>
    /// <param name="wait"> delay in seconds</param>
    /// <returns></returns>
    private IEnumerator canDisableTimer(float wait) {
        yield return new WaitForSeconds(wait);
        allwoDisable = true;
    }

}
