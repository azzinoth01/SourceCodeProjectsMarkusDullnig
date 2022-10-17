using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.UI;

/// <summary>
/// class of the player
/// imports the controler Interface
/// </summary>
public class Player : MonoBehaviour, Controlls.IBullet_hellActions
{
    /// <summary>
    /// player max health
    /// </summary>
    public float maxBaseHealth;
    private float currentHealth;
    /// <summary>
    /// health bar
    /// </summary>
    public Image healthbar;
    /// <summary>
    /// health bar coloring above 60%
    /// </summary>
    public Color healthbarAbove60;
    /// <summary>
    /// health bar coloring above 30%
    /// </summary>
    public Color healthbarAbove30;
    /// <summary>
    /// health bar coloring below 30%
    /// </summary>
    public Color healthbarBelow30;

    /// <summary>
    /// max shield value
    /// </summary>
    public float maxschield;
    /// <summary>
    /// curent shield value
    /// </summary>
    public float currentschield;
    /// <summary>
    /// shield bar
    /// </summary>
    public Image schieldbar;
    /// <summary>
    /// shield bar step value for smooth shield increase
    /// </summary>
    public float schieldbarStepValue;

    /// <summary>
    /// shield refresh rate
    /// </summary>
    public float schieldRefreshRate;
    /// <summary>
    /// shield refresh value
    /// </summary>
    public float schieldRefreshBaseValue;

    /// <summary>
    /// physic object of player
    /// </summary>
    public Rigidbody2D body;

    /// <summary>
    /// the used force to move the player
    /// </summary>
    public float force;
    /// <summary>
    /// the maximum speed the player can have
    /// </summary>
    public float maxSpeed;

    private List<Weapon> weapons;
    /// <summary>
    /// list of weapon slot objects
    /// </summary>
    public List<GameObject> WeaponSlots;

    /// <summary>
    /// ship object
    /// </summary>
    public GameObject ship;


    private Vector2 impulse;

    private Controlls controll;
    private bool shooting;

    // private Animator anim;
    //   public Animator antrieb;

    /// <summary>
    /// additional player dmg
    /// </summary>
    public float additionalDmg;
    /// <summary>
    /// player dmg multiplier
    /// </summary>
    public float dmgModifier;

    /// <summary>
    /// immunity flicker rate
    /// </summary>
    public float immunityFlickerRate;
    /// <summary>
    /// immunity flicker visibility range
    /// </summary>
    [Range(0, 1)] public float maxFlickerRange;
    private int flickerDirection;


    /// <summary>
    /// immunity time after hit
    /// </summary>
    public float immunityTimeAfterHit;
    /// <summary>
    /// immunity time after doge
    /// </summary>
    public float immunityTimeAfterDoge;

    /// <summary>
    /// doge charges
    /// </summary>
    public int dogeCharges;
    /// <summary>
    /// doge range
    /// </summary>
    public float dogeRange;
    /// <summary>
    /// doge speed
    /// </summary>
    public float dogeSpeed;
    /// <summary>
    /// max doge duration
    /// </summary>
    public float maxDogeDuration;
    /// <summary>
    /// doge cooldown
    /// </summary>
    public float dogeCooldown;
    /// <summary>
    /// global cooldown of doge
    /// </summary>
    public float globalCooldown;

    private int maxDogeCharges;

    private bool onGlobalCooldown;
    private bool isDoging;

    // public List<GameObject> chargeBalls;

    /// <summary>
    /// charge sprites
    /// </summary>
    public List<Sprite> chargeSprites;
    /// <summary>
    /// charge UI
    /// </summary>
    public Image chargeUI;
    /// <summary>
    /// waypoint prefab
    /// </summary>
    public GameObject waypointPrefab;
    private GameObject waypoint;
    private Coroutine timer;
    private Coroutine immunityTimer;
    private Coroutine chargeFillCo;
    private Coroutine flickerCo;


    private bool isImmun;
    private SpriteRenderer sp;

    /// <summary>
    /// death effect
    /// </summary>
    public GameObject deathEffect;
    /// <summary>
    /// charge audio
    /// </summary>
    public AudioSource chargeAudio;
    /// <summary>
    /// hit audio
    /// </summary>
    public AudioSource hitAudio;

    /// <summary>
    /// cracked screen overlay
    /// </summary>
    public Image crackedScreenOverlay;
    /// <summary>
    /// cracked screen overlay
    /// </summary>
    public Image crackedScreenOverlay2;

    /// <summary>
    /// cracked sprite
    /// </summary>
    public Sprite crackedLevel1;
    /// <summary>
    /// cracked sprite
    /// </summary>
    public Sprite crackedLevel2;


    private float timestamp;

    private Parts shieldPart;

    /// <summary>
    /// trail object
    /// </summary>
    public TrailRenderer trail;

    /// <summary>
    /// dash audio
    /// </summary>
    public AudioSource dashAudio;

    private LoadAssets loader;
    /// <summary>
    /// cracked sound 
    /// </summary>
    public AudioSource crackedSound;
    /// <summary>
    /// player bullet sound prefab
    /// </summary>
    public GameObject playerBulletSoundPrefab;

    private bool firstCrackedSoundPlayed;
    private bool secondCrackedSoundPlayed;

    private bool schieldRefreshStarted;

    /// <summary>
    /// returns the impulse direction
    /// </summary>
    public Vector2 Impulse {
        get {
            return impulse;
        }


    }

    /// <summary>
    /// not in use anymore
    /// </summary>
    public float Timestamp {
        get {
            return timestamp;
        }

        set {
            timestamp = value;
        }
    }
    /// <summary>
    /// returns and sets the shield part
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
    /// returns and sets the current health
    /// </summary>
    public float CurrentHealth {
        get {
            return currentHealth;
        }

        set {
            currentHealth = value;
        }
    }


    /// <summary>
    /// sets the player in the global variables
    /// </summary>
    private void Awake() {
        Globals.player = gameObject;


    }

    /// <summary>
    /// controler action of the doge input
    /// </summary>
    /// <param name="context"></param>
    public void OnDoge(InputAction.CallbackContext context) {
        //throw new System.NotImplementedException();

        if (context.started) {
            doge();

        }
    }

    /// <summary>
    /// controler action of the move down input
    /// </summary>
    /// <param name="context"></param>
    public void OnMove_down(InputAction.CallbackContext context) {



        if (context.started) {
            impulse = impulse + (Vector2.down * force);
            //    anim.SetInteger("IntY", anim.GetInteger("IntY") - 1);
            // antrieb.SetInteger("IntY", antrieb.GetInteger("IntY") - 1);
        }
        else if (context.canceled) {
            impulse = impulse - (Vector2.down * force);
            //  anim.SetInteger("IntY", anim.GetInteger("IntY") + 1);
            //antrieb.SetInteger("IntY", antrieb.GetInteger("IntY") + 1);
        }
        //Debug.Log("move down");
    }
    /// <summary>
    /// controler action of the move left input
    /// </summary>
    /// <param name="context"></param>
    public void OnMove_left(InputAction.CallbackContext context) {

        if (context.started) {
            impulse = impulse + (Vector2.left * force);
            //       anim.SetInteger("IntX", anim.GetInteger("IntX") - 1);
            //  antrieb.SetInteger("IntX", antrieb.GetInteger("IntX") - 1);
        }
        else if (context.canceled) {
            impulse = impulse - (Vector2.left * force);
            //    anim.SetInteger("IntX", anim.GetInteger("IntX") + 1);
            // antrieb.SetInteger("IntX", antrieb.GetInteger("IntX") + 1);
        }
    }
    /// <summary>
    /// controler action of the move right input
    /// </summary>
    /// <param name="context"></param>
    public void OnMove_rigth(InputAction.CallbackContext context) {

        if (context.started) {
            impulse = impulse + (Vector2.right * force);
            //      anim.SetInteger("IntX", anim.GetInteger("IntX") + 1);
            //  antrieb.SetInteger("IntX", antrieb.GetInteger("IntX") + 1);
        }
        else if (context.canceled) {
            impulse = impulse - (Vector2.right * force);
            //    anim.SetInteger("IntX", anim.GetInteger("IntX") - 1);
            //  antrieb.SetInteger("IntX", antrieb.GetInteger("IntX") - 1);


        }
    }
    /// <summary>
    /// controler action of the move up input
    /// </summary>
    /// <param name="context"></param>
    public void OnMove_up(InputAction.CallbackContext context) {

        if (context.started) {
            impulse = impulse + (Vector2.up * force);
            //     anim.SetInteger("IntY", anim.GetInteger("IntY") + 1);
            //  antrieb.SetInteger("IntY", antrieb.GetInteger("IntY") + 1);

        }
        else if (context.canceled) {
            impulse = impulse - (Vector2.up * force);
            //   anim.SetInteger("IntY", anim.GetInteger("IntY") - 1);
            //  antrieb.SetInteger("IntY", antrieb.GetInteger("IntY") - 1);
        }
    }

    /// <summary>
    /// controler action of the shoot input
    /// </summary>
    /// <param name="context"></param>
    public void OnShoot(InputAction.CallbackContext context) {

        if (context.started) {
            shooting = true;
        }
        else if (context.canceled) {
            shooting = false;
        }
    }
    /// <summary>
    /// controler action of the pause input
    /// </summary>
    /// <param name="context"></param>
    public void OnPause_menu(InputAction.CallbackContext context) {
        //  Debug.Log("pause called" + context);

        if (context.started) {
            //    Debug.Log(Globals.pause);
            if (Globals.pause == true) {
                Globals.menuHandler.setResume();
            }
            else {
                Globals.menuHandler.setPause();
            }
        }
    }


    /// <summary>
    /// creates the controler object and loads all rebindings
    /// </summary>
    void Start() {

        if (controll == null) {
            controll = new Controlls();

            Rebinding_menu rebind = new Rebinding_menu();
            controll = rebind.loadRebinding(controll);

            controll.bullet_hell.Enable();
            controll.bullet_hell.SetCallbacks(this);



        }
        impulse = new Vector2(0, 0);

        //  anim = GetComponent<Animator>();

        timestamp = Time.time;

    }

    /// <summary>
    /// checks if game is paused
    /// </summary>
    void Update() {
        if (Globals.pause == true) {
            return;
        }
        else {







        }

    }
    /// <summary>
    /// sets base values and starts every corutine
    /// loads equiped weapons and creates weapon objects
    /// </summary>
    private void OnEnable() {
        onGlobalCooldown = false;
        shooting = false;
        isDoging = false;
        isImmun = false;


        PlayerSave save = PlayerSave.loadSettings();
        if (save == null) {
            save = new PlayerSave();
        }
        weapons = new List<Weapon>();

        shieldPart = save.ShieldPart;

        loader = new LoadAssets();

        if (save.MainWeapon != null) {
            WeaponSlots[0].AddComponent<Weapon>();
            WeaponSlots[0].GetComponent<SpriteRenderer>().sprite = loader.loadSprite(save.MainWeapon.Sprite);
            Weapon wep = WeaponSlots[0].GetComponent<Weapon>();

            wep.skill = loader.loadGameObject(save.MainWeapon.skill);
            wep.reloadTime = save.MainWeapon.reloadTime;
            wep.shootsToCreate = save.MainWeapon.shootsToCreate;
            wep.additionalDmg = save.MainWeapon.additionalDmg;
            wep.dmgModifier = save.MainWeapon.dmgModifier;

            wep.sound = playerBulletSoundPrefab;


            weapons.Add(wep);

        }
        if (save.SecondaryWeapon != null) {
            WeaponSlots[1].AddComponent<Weapon>();
            WeaponSlots[1].GetComponent<SpriteRenderer>().sprite = loader.loadSprite(save.SecondaryWeapon.Sprite);
            Weapon wep = WeaponSlots[1].GetComponent<Weapon>();

            wep.skill = loader.loadGameObject(save.SecondaryWeapon.skill);
            wep.reloadTime = save.SecondaryWeapon.reloadTime;
            wep.shootsToCreate = save.SecondaryWeapon.shootsToCreate;
            wep.additionalDmg = save.SecondaryWeapon.additionalDmg;
            wep.dmgModifier = save.SecondaryWeapon.dmgModifier;

            wep.sound = playerBulletSoundPrefab;


            weapons.Add(wep);
        }
        if (save.SecondaryWeapon1 != null) {
            WeaponSlots[2].AddComponent<Weapon>();
            WeaponSlots[2].GetComponent<SpriteRenderer>().sprite = loader.loadSprite(save.SecondaryWeapon1.Sprite);
            Weapon wep = WeaponSlots[2].GetComponent<Weapon>();

            wep.skill = loader.loadGameObject(save.SecondaryWeapon1.skill);
            wep.reloadTime = save.SecondaryWeapon1.reloadTime;
            wep.shootsToCreate = save.SecondaryWeapon1.shootsToCreate;
            wep.additionalDmg = save.SecondaryWeapon1.additionalDmg;
            wep.dmgModifier = save.SecondaryWeapon1.dmgModifier;

            wep.sound = playerBulletSoundPrefab;

            weapons.Add(wep);
        }


        if (shieldPart != null) {
            maxBaseHealth = maxBaseHealth + shieldPart.HealthBoost;
            schieldRefreshBaseValue = schieldRefreshBaseValue + shieldPart.ShieldRefreshValueBoost;
        }

        currentHealth = maxBaseHealth;
        //currentschield = maxschield;
        StartCoroutine(shootingHandler());
        StartCoroutine(moveHandler());
        StartCoroutine(smoothHealthDrop());

        schieldRefreshStarted = true;
        StartCoroutine(schieldRefresh(schieldRefreshRate));
        StartCoroutine(smoothSchieldDrop());


        maxDogeCharges = dogeCharges;
        flickerDirection = -1;
        sp = ship.GetComponent<SpriteRenderer>();


    }


    /// <summary>
    /// function to create a flcikering effect
    /// </summary>
    private void flicker() {
        Material m = trail.material;
        float deltaTime = Time.deltaTime;
        sp.color = new Color(sp.color.r, sp.color.g, sp.color.b, sp.color.a + (flickerDirection * immunityFlickerRate * deltaTime));

        m.color = new Color(m.color.r, m.color.g, m.color.b, m.color.a + ((flickerDirection * immunityFlickerRate * deltaTime) * 2));

        if (sp.color.a <= maxFlickerRange) {
            flickerDirection = 1;

        }
        else if (sp.color.a >= 1) {
            flickerDirection = -1;
        }




    }


    /// <summary>
    /// corutine to handle movement
    /// speed is normalized, so the max speed makes a perfect circle, so that a diagonal movement 
    /// is not faster than other movement directions
    /// </summary>
    /// <returns></returns>
    private IEnumerator moveHandler() {
        while (true) {
            if (Globals.pause == false && isDoging == false) {
                body.AddForce(impulse.normalized * force * Time.deltaTime, ForceMode2D.Impulse);
                Vector2 normalizedSpeed = body.velocity.normalized * maxSpeed;
                normalizedSpeed.x = Mathf.Abs(normalizedSpeed.x);
                normalizedSpeed.y = Mathf.Abs(normalizedSpeed.y);

                body.velocity = new Vector2(Mathf.Clamp(body.velocity.x, -normalizedSpeed.x, normalizedSpeed.x), Mathf.Clamp(body.velocity.y, -normalizedSpeed.y, normalizedSpeed.y));
                float angle;
                if (body.velocity.magnitude == 0) {
                    angle = 90;
                }
                else {
                    angle = Vector2.SignedAngle(Vector2.right, body.velocity);
                }


                ship.transform.eulerAngles = new Vector3(0, 0, angle - 90);
            }

            yield return null;
        }

    }

    /// <summary>
    /// corutine which checks if the shoot input is pushed and fires shots of weapons
    /// </summary>
    /// <returns></returns>
    private IEnumerator shootingHandler() {


        while (true) {
            if (shooting == true && weapons.Count != 0 && Globals.pause == false) {
                foreach (Weapon w in weapons) {
                    w.shoot(additionalDmg, dmgModifier);
                }
            }
            yield return null;
        }
    }

    /// <summary>
    /// corutine which makes live drop smoothly and colorizes the health
    /// </summary>
    /// <returns></returns>
    public IEnumerator smoothHealthDrop() {

        while (true) {
            if (Globals.pause == true) {
                yield return null;
            }
            float prozentValue = currentHealth / maxBaseHealth;
            float currentFillProzent = healthbar.fillAmount;

            //Debug.Log("% Value " + prozentValue.ToString());
            //Debug.Log("Fill Value " + currentFillProzent.ToString());
            if (prozentValue <= currentFillProzent) {

                float toSet = currentFillProzent - 0.01f;
                if (toSet < prozentValue) {
                    toSet = prozentValue;
                }
                healthbar.fillAmount = toSet;
            }
            else if (prozentValue >= currentFillProzent) {
                float toSet = currentFillProzent + 0.01f;
                if (toSet > prozentValue) {
                    toSet = prozentValue;
                }
                healthbar.fillAmount = toSet;
            }

            if (healthbar.fillAmount >= 0.6f) {
                healthbar.color = healthbarAbove60;
                crackedScreenOverlay.sprite = null;
                crackedScreenOverlay.enabled = false;

                crackedScreenOverlay2.sprite = null;
                crackedScreenOverlay2.enabled = false;

                firstCrackedSoundPlayed = false;
                secondCrackedSoundPlayed = false;
            }
            else if (healthbar.fillAmount >= 0.3f) {
                healthbar.color = healthbarAbove30;
                crackedScreenOverlay.sprite = crackedLevel1;
                crackedScreenOverlay.enabled = true;

                crackedScreenOverlay2.sprite = null;
                crackedScreenOverlay2.enabled = false;

                if (crackedSound != null && firstCrackedSoundPlayed == false) {
                    crackedSound.Play();
                }


                firstCrackedSoundPlayed = true;
                secondCrackedSoundPlayed = false;



            }
            else {
                healthbar.color = healthbarBelow30;
                crackedScreenOverlay2.sprite = crackedLevel2;
                crackedScreenOverlay2.enabled = true;

                if (crackedSound != null && secondCrackedSoundPlayed == false) {
                    crackedSound.Play();
                }



                firstCrackedSoundPlayed = true;

                secondCrackedSoundPlayed = true;

            }
            if (healthbar.fillAmount <= 0) {
                Destroy(gameObject);
                Instantiate(deathEffect, transform.position, transform.rotation);
                Globals.menuHandler.Playtime = Time.time - timestamp;

                Globals.menuHandler.setGameOver();
            }


            yield return null;
        }
    }


    /// <summary>
    /// corutine which makes the shield bar drop smootly and fill smoothly
    /// </summary>
    /// <returns></returns>
    public IEnumerator smoothSchieldDrop() {

        while (true) {
            if (Globals.pause == true) {
                yield return null;
            }
            float prozentValue = currentschield / maxschield;
            float currentFillProzent = schieldbar.fillAmount;

            if (prozentValue <= currentFillProzent) {
                float toSet = currentFillProzent - schieldbarStepValue;
                if (toSet < prozentValue) {
                    toSet = prozentValue;
                }
                schieldbar.fillAmount = toSet;
            }
            else if (prozentValue >= currentFillProzent) {
                float toSet = currentFillProzent + schieldbarStepValue;
                if (toSet > prozentValue) {
                    toSet = prozentValue;
                }
                schieldbar.fillAmount = toSet;
            }

            if (schieldbar.fillAmount <= 0 && schieldRefreshStarted == false) {
                // startet das Schildauffüllen nur nachdem das schild leer ist
                schieldRefreshStarted = true;
                StartCoroutine(schieldRefresh(schieldRefreshRate));
            }

            yield return null;
        }
    }

    /// <summary>
    /// corutine which charges the shield after it was depleted
    /// </summary>
    /// <param name="wait"> delay in seconds</param>
    /// <returns></returns>
    public IEnumerator schieldRefresh(float wait) {



        yield return new WaitForSeconds(wait);

        currentschield = currentschield + schieldRefreshBaseValue;

        if (currentschield >= maxschield) {
            currentschield = maxschield;
            schieldRefreshStarted = false;
        }
        else {
            //Debug.Log("schield refreseh");
            StartCoroutine(schieldRefresh(wait));
        }


    }

    /// <summary>
    /// take dmg funktion
    /// </summary>
    /// <param name="dmg"> the dmg the player takes</param>
    public void takeDmg(float dmg) {
        if (isImmun == true) {
            return;
        }

        hitAudio.Play();

        if (schieldbar.fillAmount >= 1) {
            currentschield = 0;

            // verschoben damit erst startet sobald anzeige wirklich auf 0 gedroped ist
            //StartCoroutine(schieldRefresh(schieldRefreshRate));

        }
        else {
            currentHealth = currentHealth - dmg;
            if (currentHealth > 0) {
                StartCoroutine(Globals.currentCamera.GetComponent<CameraScript>().startScreenShake());
            }

        }


        if (currentHealth <= 0) {
            return;
            // destroy moved to smooth health drop
            //Globals.gameoverHandler.gameOver();
        }
        isImmun = true;
        gameObject.layer = (int)Layer_enum.player_immunity; // immunity layer
        immunityTimer = StartCoroutine(immunityTime(immunityTimeAfterHit));
    }

    /// <summary>
    /// destorys the controler input because it would continue to exist if it is not disposed
    /// </summary>
    private void OnDestroy() {

        controll.Dispose();
        body.velocity = Vector2.zero;
        loader.releaseAllHandle();
    }

    /// <summary>
    /// destroys den controller input
    /// </summary>
    public void clearControlls() {

        controll.Dispose();
        body.velocity = Vector2.zero;

    }

    /// <summary>
    /// charge refill timer
    /// </summary>
    /// <param name="cooldown"> cooldown of charge in seconds </param>
    /// <returns></returns>
    private IEnumerator chargeFill(float cooldown) {
        yield return new WaitForSeconds(cooldown);

        dogeCharges = dogeCharges + 1;
        dogeVisual(false);
        if (dogeCharges != maxDogeCharges) {
            chargeFillCo = StartCoroutine(chargeFill(cooldown));
        }
        else {
            chargeFillCo = null;
        }
    }

    /// <summary>
    /// cooldown timer between charges, so charges can't be spammed
    /// </summary>
    /// <param name="cooldown"> cooldwon in seconds</param>
    /// <returns></returns>
    private IEnumerator globalCooldownTimer(float cooldown) {
        yield return new WaitForSeconds(cooldown);
        onGlobalCooldown = false;
    }

    /// <summary>
    /// doge special ability, which speeds up the player for a short duration
    /// </summary>
    private void doge() {
        if (dogeCharges > 0 && onGlobalCooldown == false && isDoging == false && impulse != Vector2.zero) {

            if (dashAudio != null) {

                dashAudio.Play();
            }

            isDoging = true;
            isImmun = true;
            gameObject.layer = (int)Layer_enum.player_immunity; // immunity layer



            // falls immunity durch hit wird diese vom doge überschrieben
            if (immunityTimer != null) {
                StopCoroutine(immunityTimer);
            }



            //transform.position = transform.position + (Vector3)(impulse.normalized * dogeRange);

            Vector3 point;

            point = transform.position + (Vector3)(impulse.normalized * dogeRange);
            Vector3 cameraPoint = Globals.currentCamera.WorldToViewportPoint(point);
            float fixedDogeRange = dogeRange;
            // check if charge punkt is outside of field
            while (cameraPoint.x < 0 || cameraPoint.x > 1 || cameraPoint.y < 0 || cameraPoint.y > 1) {
                //Debug.Log("doge outside view");

                fixedDogeRange = fixedDogeRange - 1;
                if (fixedDogeRange <= 0) {
                    //Debug.Log("doge nicht möglich");
                    isDoging = false;
                    return;
                }
                point = transform.position + (Vector3)(impulse.normalized * fixedDogeRange);
                cameraPoint = Globals.currentCamera.WorldToViewportPoint(point);

            }
            //Debug.Log("current pos " + transform.position.ToString());
            //Debug.Log("ziel pos " + point.ToString());
            waypoint = Instantiate(waypointPrefab, point, Quaternion.identity, transform.parent);

            Vector3 direction = (waypoint.transform.position - transform.position);
            //Debug.Log(direction);
            //Debug.Log(direction.normalized);
            body.velocity = direction.normalized * dogeSpeed;
            //Debug.Log(body.velocity);

            dogeCharges = dogeCharges - 1;
            dogeVisual(true);
            onGlobalCooldown = true;
            timer = StartCoroutine(maxDogeTimer(maxDogeDuration));

            if (chargeFillCo == null) {
                chargeFillCo = StartCoroutine(chargeFill(dogeCooldown));
            }

            StartCoroutine(globalCooldownTimer(globalCooldown));


        }
    }
    /// <summary>
    /// corutine which shows the flickering of the player if he is immmun to dmg
    /// </summary>
    /// <returns></returns>
    private IEnumerator immunityFlickerHandler() {


        while (isImmun == true) {
            flicker();
            yield return null;
        }
        Material m = trail.material;
        sp.color = new Color(sp.color.r, sp.color.g, sp.color.b, 1);
        m.color = new Color(m.color.r, m.color.g, m.color.b, 1);

        flickerCo = null;
    }

    /// <summary>
    /// immunity timer
    /// </summary>
    /// <param name="time"> duration of immunity in seconds</param>
    /// <returns></returns>
    private IEnumerator immunityTime(float time) {
        if (flickerCo == null) {
            flickerCo = StartCoroutine(immunityFlickerHandler());
        }
        yield return new WaitForSeconds(time);
        isImmun = false;
        gameObject.layer = (int)Layer_enum.player; //player layer
    }


    /// <summary>
    /// timer which stops the doge, if target point was not reached
    /// </summary>
    /// <param name="duration"> delay in seconds</param>
    /// <returns></returns>
    private IEnumerator maxDogeTimer(float duration) {
        yield return new WaitForSeconds(duration);
        isDoging = false;
        Vector2 normalizedSpeed = body.velocity.normalized * maxSpeed;
        normalizedSpeed.x = Mathf.Abs(normalizedSpeed.x);
        normalizedSpeed.y = Mathf.Abs(normalizedSpeed.y);

        body.velocity = new Vector2(Mathf.Clamp(body.velocity.x, -normalizedSpeed.x, normalizedSpeed.x), Mathf.Clamp(body.velocity.y, -normalizedSpeed.y, normalizedSpeed.y));

        Destroy(waypoint);
        immunityTimer = StartCoroutine(immunityTime(immunityTimeAfterDoge));

    }

    /// <summary>
    /// visualize the charge display
    /// </summary>
    /// <param name="used"> true if a doge was used, fals if a charge was refilled</param>
    private void dogeVisual(bool used) {
        if (used == true) {

            chargeUI.sprite = chargeSprites[dogeCharges];
            if (dogeCharges == 0) {
                chargeUI.color = new Color(chargeUI.color.r, chargeUI.color.g, chargeUI.color.b, 0);
            }
            else {
                chargeUI.color = new Color(chargeUI.color.r, chargeUI.color.g, chargeUI.color.b, 1);
            }
            //chargeBalls[dogeCharges].GetComponent<Image>().color = Color.red;
        }
        else {
            chargeUI.sprite = chargeSprites[dogeCharges];
            chargeAudio.Play();
            if (dogeCharges == 0) {
                chargeUI.color = new Color(chargeUI.color.r, chargeUI.color.g, chargeUI.color.b, 0);
            }
            else {
                chargeUI.color = new Color(chargeUI.color.r, chargeUI.color.g, chargeUI.color.b, 1);
            }
            //chargeBalls[dogeCharges].GetComponent<Image>().color = Color.green;
        }
    }






    /// <summary>
    /// checks if the doge target point was reached
    /// </summary>
    /// <param name="collision"> collison object</param>
    private void OnTriggerEnter2D(Collider2D collision) {



        if (collision.gameObject == waypoint) {
            //Debug.Log("doge complete");
            StopCoroutine(timer);
            isDoging = false;
            Vector2 normalizedSpeed = body.velocity.normalized * maxSpeed;
            normalizedSpeed.x = Mathf.Abs(normalizedSpeed.x);
            normalizedSpeed.y = Mathf.Abs(normalizedSpeed.y);

            body.velocity = new Vector2(Mathf.Clamp(body.velocity.x, -normalizedSpeed.x, normalizedSpeed.x), Mathf.Clamp(body.velocity.y, -normalizedSpeed.y, normalizedSpeed.y));

            Destroy(waypoint);
            immunityTimer = StartCoroutine(immunityTime(immunityTimeAfterDoge));
        }
    }


}
