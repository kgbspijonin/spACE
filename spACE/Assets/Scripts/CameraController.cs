using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraController : MonoBehaviour {
    public GameObject canvas;
    public GameObject menucanvas;
    public GameObject spaceshipNameField;
    public Material transparent;
    public float sensitivityX;
    public float sensitivityY;
    public float speedForward;
    public float speedStrafe;
    public float verticalSpeed;
    private GameObject cube;
    private GameObject placer;
    private float currentX;
    private float currentY;
    public new Camera camera;
    bool building = true;
    bool isMenu = false;
    int layerMask;
    int cubeRotation;
    static public Color color = new Color();
    static public string weaponAlias = "";

    void UpdateName(string name = "") {
        if(name != "") {
            spaceshipNameField.GetComponent<Text>().text = name;
        }
        else {
            spaceshipNameField.GetComponent<Text>().text = StaticInfo.shipName;
        }
    }
    
    void Start() {
        SpawnPlacer();
        if (StaticInfo.shipName != "") {
            BasicUI.LoadSpaceship(StaticInfo.shipName);
        }
        UpdateName();
    }

    void Update() {
        if (canvas.activeSelf || menucanvas.activeSelf) {
            Cursor.lockState = CursorLockMode.None;
        } else {
            Cursor.lockState = CursorLockMode.Locked;
        }
        Color placerColor = color;
        placerColor.a = 0.5f;
        placer.GetComponent<MeshRenderer>().material = transparent;
        placer.GetComponent<MeshRenderer>().material.SetColor("_Color", placerColor);
        if (Input.GetKeyDown("q") && !isMenu) {
            building = !building;
            canvas.SetActive(!canvas.activeSelf);
        }
        if (Input.GetKeyDown("`")) {
            isMenu = !isMenu;
            building = !building;
            Cursor.lockState = building == true ? CursorLockMode.Locked : CursorLockMode.None;
            menucanvas.SetActive(!menucanvas.activeSelf);
            UpdateName();
        }   
        if (building && !isMenu) {
            RaycastHit hit;
            if (Physics.Raycast(camera.transform.position, camera.transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, layerMask)) {
                if (hit.transform.tag == "Cube" || hit.transform.tag == "BuildFloor" || hit.transform.tag == "WeaponPylon") {
                    Vector3 position = hit.point;
                    position = Round(position);
                    position.x = Mathf.Clamp(
                        position.x,
                        -24,
                        24);
                    position.y = Mathf.Clamp(position.y, 0, 49);
                    position.z = Mathf.Clamp(
                        position.z,
                        -24,
                        24);
                    placer.transform.position = position;
                    if (Input.GetKeyDown("mouse 0")) {
                        GameObject created = Instantiate(cube, placer.transform.position, placer.transform.rotation);
                        created.GetComponent<BoxCollider>().enabled = true;
                        created.GetComponent<MeshRenderer>().material.color = color;
                        created.transform.name = cube.transform.name;
                        if(created.transform.name == "WeaponPylon") {
                            created.transform.name = weaponAlias;
                            created.transform.tag = "WeaponPylon";
                        }
                    } else if (Input.GetKeyDown("mouse 1")) {
                        if (hit.transform.tag == "Cube" || hit.transform.tag == "WeaponPylon") {
                            Destroy(hit.transform.gameObject);
                        }
                    } else if (Input.GetKeyDown("mouse 2")) {
                        if(hit.transform.tag == "Cube") {
                            ChangeCube(hit.transform.name);
                        }
                    }
                    float scroll = Input.GetAxis("Mouse ScrollWheel");
                    if (scroll != 0) {
                        if (scroll > 0) {
                            cubeRotation--;
                        } else if (scroll < 0) {
                            cubeRotation++;
                        }
                        cubeRotation = cubeRotation % 8;
                        if (cubeRotation < 0) {
                            cubeRotation = 7;
                        }
                        placer.transform.rotation = Quaternion.Euler(
                            0,
                            90 * (cubeRotation % 4),
                            0
                            );
                    }
                }
            }
        }
    }

    Vector3 Round(Vector3 vector) {
        vector.x = Mathf.Round(vector.x);
        vector.y = Mathf.Round(vector.y);
        vector.z = Mathf.Round(vector.z);
        return vector;
    }

    void FixedUpdate() { 
        if (!building || isMenu) {
            GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
        if (building && !isMenu) {
            float forwardInput = Input.GetAxis("Vertical") * speedForward;
            float strafeInput = Input.GetAxis("Horizontal") * speedStrafe;
            float up = Input.GetKey(KeyCode.Space) == true ? 1 : 0;
            float down = Input.GetKey(KeyCode.LeftShift) == true ? 1 : 0;
            GetComponent<Rigidbody>().velocity = 
                transform.forward * forwardInput + 
                transform.right * strafeInput +  
                new Vector3(0, (up-down) * verticalSpeed, 0);
            currentX += Input.GetAxis("Mouse X") * sensitivityX;
            currentY += -Input.GetAxis("Mouse Y") * sensitivityY;
            camera.transform.position = transform.position;
            Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);
            transform.rotation = rotation;
        }
    }

    public void ChangeCube(string name) {
        GameObject[] placers = GameObject.FindGameObjectsWithTag("Placer");
        foreach (GameObject placer in placers) {
            Destroy(placer);
        }
        cube = Resources.Load<GameObject>("Cubes/" + name);
        Vector3 pos = placer.transform.position;
        Quaternion rot = placer.transform.rotation;
        placer = Instantiate(Resources.Load<GameObject>("Cubes/" + name), pos, rot);
        placer.GetComponent<BoxCollider>().enabled = false;
        placer.tag = "Placer";
    }

    public void ParseColor() {
        Debug.Log(transform);
    }

    void SpawnPlacer() {
        cube = Resources.Load<GameObject>("Cubes/Cube");
        cube.GetComponent<BoxCollider>().enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        layerMask = 1 << 20;
        layerMask = ~layerMask;
        camera = Camera.main;
        canvas.SetActive(false);
        menucanvas.SetActive(false);
        placer = Instantiate(Resources.Load<GameObject>("Cubes/Cube"));
        placer.GetComponent<BoxCollider>().enabled = false;
        placer.tag = "Placer";
    }
}
