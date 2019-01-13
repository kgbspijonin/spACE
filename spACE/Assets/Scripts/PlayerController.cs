using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {
    public float distance = 10f;
    public float offsetY = 0.0f;
    public float sensitivityX = 30.0f;
    public float sensitivityY = 30.0f;
    public float tilt = 10.0f;
    public float speedX;
    public float speedY;
    public GameObject DamageTakenParticles;
    public GameObject canvas;
    public GameObject abilityParticles;
    public bool shieldEnabled = false;
    public GameObject shieldPrefab;
    public GameObject shieldUI;
    static public float currentScore = 0;
    static public float maxHealth = 100;
    static public float currentHeatlh;
    static public bool hasDefeatedBoss = false;
    public GameObject healChargeUI;
    public GameObject wipeChargeUI;
    public GameObject shieldChargeUI;

    private new Camera camera = new Camera();
    private Transform player;
    private Transform cameraTransform;
    private float currentX = 0.0f;
    private float currentY = 0.0f;
    private float minY = -89.0f;
    private float maxY = 89.0f;
    private bool playing = true;
    private int currentWeapon = 0;
    private int cubeCount = 0;
    private Vector3 center = new Vector3();
    private float offsetZ = 0;
    private GameObject border;

    private float healCharge = 1.0f;
    private float shieldCharge = 1.0f;
    private float wipeCharge = 1.0f;
    private float maxShield = 50;
    private float currentShield = 0;
    private GameObject shield;

    void Start() {
        BasicUI.StaticPlayerLoader();
        StartCoroutine("ChargeAbilities");
        currentShield = 0;
        shieldUI.transform.localScale = new Vector3(currentShield / maxShield, 1, 1);
        Time.timeScale = 1;
        currentScore = 0;
        hasDefeatedBoss = false;
        canvas.transform.Find("Menu").gameObject.SetActive(false);
        currentHeatlh = maxHealth;
        PopulateLasers();
        Cursor.lockState = CursorLockMode.Locked;
        camera = Camera.main;
        player = transform;
        cameraTransform = camera.transform;
        border = GameObject.FindGameObjectWithTag("PlayerBorder");        if (StaticInfo.shipName != "") {
            Transform weapons = transform.Find("Weapons");
            Spaceship spaceship = Spaceship.DeSerialize("Assets/Resources/Spaceships/" + StaticInfo.shipName + ".xml");
            foreach (Cube cube in spaceship.cubes) {
                cube.position *= 0.25f;
                GameObject created = Instantiate(Resources.Load<GameObject>("Cubes/" + cube.name), cube.position, Quaternion.Euler(cube.rotation));
                created.transform.parent = transform.GetChild(0);
                created.transform.localScale = new Vector3(1, 1, 1);
                created.GetComponent<MeshRenderer>().material.color = cube.color;
                Destroy(created.GetComponent<BoxCollider>());
                created.layer = 8;
                if(offsetZ < created.transform.position.z) {
                    offsetZ = created.transform.position.z;
                }
                if(offsetY < created.transform.position.y) {
                    offsetY = created.transform.position.y;
                }
                center.x += cube.position.x;
                center.y += cube.position.y;
                center.z += cube.position.z;
                cubeCount++;
            }
            foreach (WeaponRef weapon in spaceship.weapons) {
                weapon.position *= 0.25f;
                GameObject created = Instantiate(Resources.Load<GameObject>("Cubes/" + "WeaponPylon"), weapon.position, Quaternion.identity);
                created.GetComponent<MeshRenderer>().enabled = false;
                created.tag = "Weapon";
                created.name = weapon.weapon;
                if(transform.Find("Weapons").Find(created.name) == null) {
                    GameObject newW = new GameObject();
                    newW.transform.parent = transform.Find("Weapons");
                    newW.transform.name = created.name;
                    newW.AddComponent<WeaponController>();
                }
                created.transform.parent = transform.Find("Weapons").Find(created.name);
                Destroy(created.GetComponent<BoxCollider>());
            }
            center /= cubeCount;
            GetComponent<SphereCollider>().center = center;
            GetComponent<SphereCollider>().radius = offsetZ;
            distance = offsetZ * 5f;
        }
    }
    
    void FixedUpdate() {
        float inputX = Input.GetAxis("Horizontal") * speedX;
        float inputY = Input.GetAxis("Vertical") * speedY;
        GetComponent<Rigidbody>().velocity = new Vector3(inputX, inputY, 0);
        transform.rotation = Quaternion.Euler(new Vector3(-inputY * tilt, 0, -inputX  * tilt));
        transform.GetChild(1).rotation = camera.transform.rotation;
        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, -border.transform.localScale.x, border.transform.localScale.x),
            Mathf.Clamp(transform.position.y, -border.transform.localScale.z, border.transform.localScale.z),
            0
            );
    }

    void Update() {
        if (hasDefeatedBoss && Time.timeScale != 0.25) {
            float timeScale = Time.timeScale == 1 ? 0.25f : 1;
            Time.timeScale = timeScale;
            CursorLockMode mode = timeScale == 1 ? CursorLockMode.Locked : CursorLockMode.None;
            Cursor.lockState = mode;
            bool menu = timeScale == 1 ? false : true;
            canvas.transform.Find("Menu").gameObject.SetActive(menu);
            canvas.transform.Find("Menu").Find("GameState").Find("Text").GetComponent<Text>().text = "Game won!";
            canvas.transform.Find("Menu").Find("Message").Find("Text").GetComponent<Text>().text = "Congratulations!";
        } else {
            if (Input.GetKeyDown(KeyCode.Q)) {
                float timeScale = Time.timeScale == 1 ? 0 : 1;
                Time.timeScale = timeScale;
                CursorLockMode mode = timeScale == 1 ? CursorLockMode.Locked : CursorLockMode.None;
                Cursor.lockState = mode;
                bool menu = timeScale == 1 ? false : true;
                canvas.transform.Find("Menu").gameObject.SetActive(menu);
            }
            if (Time.timeScale == 1) {
                if (Input.GetKey("mouse 0")) {
                    transform.GetChild(1).GetComponent<WeaponsController>().Fire(currentWeapon);
                }
                if (Input.GetKeyDown(KeyCode.Alpha1)) {
                    currentWeapon = 0;
                } else if (Input.GetKeyDown(KeyCode.Alpha2)) {
                    currentWeapon = 1;
                } else if (Input.GetKeyDown(KeyCode.Alpha3)) {
                    currentWeapon = 2;
                } else if (Input.GetKeyDown(KeyCode.Alpha4)) {
                    currentWeapon = 3;
                } else if (Input.GetKeyDown(KeyCode.Alpha5)) {
                    currentWeapon = 4;
                } else if (Input.GetKeyDown(KeyCode.Alpha6)) {
                    currentWeapon = 5;
                } else if (Input.GetKeyDown(KeyCode.Alpha7)) {
                    currentWeapon = 6;
                } else if (Input.GetKeyDown(KeyCode.Alpha8)) {
                    currentWeapon = 7;
                } else if (Input.GetKeyDown(KeyCode.Alpha9)) {
                    currentWeapon = 8;
                } else if (Input.GetKeyDown(KeyCode.Alpha0)) {
                    currentWeapon = 9;
                }
                if (Input.GetKey(KeyCode.Z) && healCharge >= 1.0f) {
                    HealUp();
                } else if (Input.GetKey(KeyCode.X) && wipeCharge >= 1.0f) {
                    WipeOut();
                } else if (Input.GetKey(KeyCode.C) && shieldCharge >= 1.0f) {
                    ShieldUp();
                }
                currentX += Input.GetAxis("Mouse X") * sensitivityX;
                currentY += Input.GetAxis("Mouse Y") * sensitivityY;
                currentY = Mathf.Clamp(currentY, minY, maxY);
            }
        }
    }

    void LateUpdate() {
        Vector3 direction = new Vector3(0, 0, -distance);
        Quaternion rotation = Quaternion.Euler(-currentY, +currentX, 0);
        cameraTransform.position = player.position + rotation * direction + new Vector3(0,offsetY*1.5f,0);
        cameraTransform.LookAt(player.position + new Vector3(0, offsetY * 1.5f, 0));
    }

    static public void PopulateLasers() {
        if(StaticInfo.lasers.Count == 0) {
            StaticInfo.lasers.Add("Green", new Bullet(Color.green, 2, 1000, 300));
            StaticInfo.lasers.Add("Yellow", new Bullet(Color.yellow, 3, 1500, 250));
            StaticInfo.lasers.Add("White", new Bullet(Color.white, 5, 1000, 200));
            StaticInfo.lasers.Add("Black", new Bullet(Color.black, 2, 500, 500));
            StaticInfo.lasers.Add("Magenta", new Bullet(Color.magenta, 1, 750, 400));
            StaticInfo.lasers.Add("Red", new Bullet(Color.red, 1, 400, 750));
            StaticInfo.lasers.Add("Cyan", new Bullet(Color.cyan, 1, 600, 600));
            StaticInfo.lasers.Add("Gray", new Bullet(Color.gray, 1, 1500, 350));
        }
    }

    static public void PopulateBarrels() {
        if(StaticInfo.barrels.Count == 0) {
            StaticInfo.barrels.Add("Normal");
            StaticInfo.barrels.Add("Shotgun");
            StaticInfo.barrels.Add("Sniper");
            StaticInfo.barrels.Add("Machine gun");
            StaticInfo.barrels.Add("Mortar");
            StaticInfo.barrels.Add("Missile");
        }
    }

    void WipeOut() {
        wipeCharge = 0.0f;
        GameObject[] asteroids = GameObject.FindGameObjectsWithTag("Asteroid");
        foreach (GameObject asteroid in asteroids) {
            Destroy(asteroid);
        }
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies) {
            Destroy(enemy);
        }
        GameObject particles = Instantiate(abilityParticles, transform.position, transform.rotation);
        Destroy(particles, 5.0f);
    }

    void ShieldUp() {
        currentShield = maxShield;
        shieldUI.transform.localScale = new Vector3(currentShield / maxShield, 1, 1);
        shieldCharge = 0.0f;
        shield = Instantiate(shieldPrefab, transform.position + new Vector3(0, offsetZ, 0), transform.rotation);
        shield.transform.localScale = new Vector3(offsetZ * 3, offsetZ * 3, offsetZ * 3);
        shield.transform.parent = transform;
        GameObject particles = Instantiate(abilityParticles, transform.position, transform.rotation);
        Destroy(particles, 5.0f);
    }

    void HealUp() {
        currentHeatlh = maxHealth;
        healCharge = 0.0f;
        GameObject particles = Instantiate(abilityParticles, transform.position, transform.rotation);
        Destroy(particles, 5.0f);
    }

    IEnumerator ChargeAbilities() {
        while (true) {
            if (healCharge < 1) {
                healCharge += 0.05f;
                healChargeUI.transform.localScale = new Vector3(healCharge, 1, 1);
            }
            if (shieldCharge < 1) {
                shieldCharge += 0.05f;
                shieldChargeUI.transform.localScale = new Vector3(shieldCharge, 1, 1);
            }
            if (wipeCharge < 1) {
                wipeCharge += 0.05f;
                wipeChargeUI.transform.localScale = new Vector3(wipeCharge, 1, 1);
            }
            yield return new WaitForSeconds(1);
        }
    }

    void OnTriggerEnter(Collider other) {
        if(other.transform.name == "Boss (Clone)") {
            currentHeatlh = 0;
        }
        shieldUI.transform.localScale = new Vector3(currentShield / maxShield, 1, 1);
        if (other.transform.tag == "Enemy" || other.transform.tag == "Asteroid") {
            if (currentShield > 0)
                currentShield -= 10;
            else
                currentHeatlh -= 10;
            GameObject particles = Instantiate(DamageTakenParticles, transform.position, transform.rotation);
            Destroy(particles, 2.0f);
            Destroy(other.gameObject);
        } else if (other.transform.tag == "EnemyBullet") {
            if (currentShield > 0)
                currentShield -= other.GetComponent<EnemyBulletController>().damage;
            else
                currentHeatlh -= other.GetComponent<EnemyBulletController>().damage;
            GameObject particles = Instantiate(DamageTakenParticles, transform.position, transform.rotation);
            Destroy(particles, 2.0f);
            Destroy(other.gameObject);
        }
        if (currentHeatlh < 0 && Time.timeScale != 0.25f) {
            float timeScale = Time.timeScale == 1 ? 0.25f : 1;
            Time.timeScale = timeScale;
            CursorLockMode mode = timeScale == 1 ? CursorLockMode.Locked : CursorLockMode.None;
            Cursor.lockState = mode;
            bool menu = timeScale == 1 ? false : true;
            canvas.transform.Find("Menu").gameObject.SetActive(menu);
            canvas.transform.Find("Menu").Find("GameState").Find("Text").GetComponent<Text>().text = "Game over!";
            canvas.transform.Find("Menu").Find("Message").Find("Text").GetComponent<Text>().text = "Better luck next time!";
        }
        if (currentShield <= 0) {
            Destroy(shield);
        }
        shieldUI.transform.localScale = new Vector3(currentShield / maxShield, 1, 1);
    }
}
