using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;
public class BasicUI : MonoBehaviour {

    public string functionToCall = "";

    void Start() {
        if(functionToCall != "") {
            StartCoroutine(functionToCall);
        }
    }

    public void SetBuilderGun() {
        CameraController.weaponAlias = transform.name;
    }

    public void SetMMWeapon() {
        StaticInfo.weapon = transform.name;
    }

    public void QuitApplication() {
        Application.Quit();
    }

    public void SaveWeapon() {
        Weapon weapon = new Weapon();
        weapon.name = transform.parent.GetComponent<InputField>().text;
        weapon.bullet = StaticInfo.currentWeaponColor;
        weapon.barrel = StaticInfo.currentBarrelType;
        weapon.fireRate = 2;
        weapon.Serialize("Assets/Resources/Weapons/" + weapon.name + ".xml");
    }

    public void BackUI(string UIElementName = "Main") {
        PlayerSaver();
        PlayerLoader();
        transform.parent.parent.Find(UIElementName).gameObject.SetActive(true);
        transform.parent.gameObject.SetActive(false);
    }

    public void ChangeParentPanel(string panel) {
        transform.parent.gameObject.SetActive(false);
        transform.parent.parent.Find(panel).gameObject.SetActive(true);
    }

    public void SetShipName(string shipName = "") {
        if(shipName=="") {
            StaticInfo.shipName = transform.name;
        }else {
            StaticInfo.shipName = shipName;
        }
    }

    public void ToggleActive() {
        gameObject.SetActive(!gameObject.activeSelf);
    }

    public void LoadSpaceshipUI(string name = "") {
        if (name == "") {
            name = transform.parent.parent.GetChild(0).GetComponent<InputField>().text;
        }
        LoadSpaceship(name);
    }

    static public void ClearField() {
        GameObject[] old = GameObject.FindGameObjectsWithTag("Cube");
        foreach (GameObject cube in old) {
            Destroy(cube);
        }
    }

    static public void LoadSpaceship(string name) {
        ClearField();
        Spaceship spaceship = new Spaceship();
        spaceship = Spaceship.DeSerialize("Assets/Resources/Spaceships/" + name + ".xml");
        if(spaceship.name != "") {
            foreach (Cube cube in spaceship.cubes) {
                GameObject created = Instantiate(Resources.Load<GameObject>("Cubes/" + cube.name), cube.position, Quaternion.Euler(cube.rotation));
                created.transform.name = cube.name;
                created.GetComponent<MeshRenderer>().material.color = cube.color;
            }
            foreach (WeaponRef weapon in spaceship.weapons) {
                GameObject created = Instantiate(Resources.Load<GameObject>("Cubes/" + "WeaponPylon"), weapon.position, Quaternion.identity);
                created.transform.name = weapon.weapon;
                created.transform.tag = "WeaponPylon";
            }
            StaticInfo.shipName = name;
        }
    }

    public void SaveSpaceship(string name = "") {
        GameObject[] cubes = GameObject.FindGameObjectsWithTag("Cube");
        GameObject[] weapons = GameObject.FindGameObjectsWithTag("WeaponPylon");
        Spaceship spaceship = new Spaceship();
        float minY = 50;
        foreach(GameObject cube in cubes) {
            if(cube.transform.position.y < minY) {
                minY = cube.transform.position.y;
            }
        }
        foreach(GameObject cube in cubes) {
            Cube newC = new Cube();
            newC.color = cube.GetComponent<MeshRenderer>().material.color;
            newC.name = cube.transform.name;
            newC.position = cube.transform.position + new Vector3(0, -minY, 0);
            newC.rotation = cube.transform.rotation.eulerAngles;
            spaceship.cubes.Add(newC);
        }

        foreach(GameObject weapon in weapons) {
            WeaponRef weaponRef = new WeaponRef();
            weaponRef.position = weapon.transform.position;
            weaponRef.weapon = weapon.transform.name;
            spaceship.weapons.Add(weaponRef);
        }

        if (name == "") {
            spaceship.name = transform.parent.parent.GetChild(0).GetComponent<InputField>().text;
        } else {
            spaceship.name = name;
        }
        spaceship.Serialize("Assets/Resources/Spaceships/" + spaceship.name + ".xml");
    }

    public void ChangeScene(string scene) {
        SceneManager.LoadScene(scene);
    }

    static public List<string> GetFilesInFolder(string folder) {
        List<string> files = new List<string>();
        files = new List<string>(System.IO.Directory.GetFiles(folder));
        return files;
    }

    static public List<string> GetFoldersInFolder(string folder) {
        List<string> files = new List<string>();
        files = new List<string>(System.IO.Directory.GetDirectories(folder));
        return files;
    }

    static public List<string> SliceFolderNames(List<string> names) {
        for(int i = 0; i < names.Count; i++) {
            if(names[i].Contains("/")) {
                names[i] = names[i].Remove(0, names[i].LastIndexOf('/')+1);
            }
        }
        return names;
    }

    public void SetGlobalWeaponColor() {
        StaticInfo.currentWeaponColor = transform.name;
        Debug.Log(StaticInfo.currentWeaponColor);
        transform.parent.parent.parent.parent.Find("In Use").Find("Current").GetComponent<Text>().text = transform.name;
        transform.parent.parent.parent.parent.Find("In Use").Find("Current").GetComponent<Text>().color = transform.name.ToColor();
    }

    public void SetGlobalBarrelType() {
        StaticInfo.currentBarrelType = transform.name;
        Debug.Log(StaticInfo.currentBarrelType);
        transform.parent.parent.parent.parent.Find("In Use").Find("Current").GetComponent<Text>().text = transform.name;
    }

    public void DeleteSelectedWeapon() {
        string name = StaticInfo.weapon + ".xml";
        string path = "Assets/Resources/Weapons/";
        if (System.IO.File.Exists(path + name))
            System.IO.File.Delete(path + name);
        transform.parent.Find("Guns").Find("Scroll View").Find("Viewport").Find("Content").gameObject.SetActive(false);
        transform.parent.Find("Guns").Find("Scroll View").Find("Viewport").Find("Content").gameObject.SetActive(true);
    }

    IEnumerator ScoreUpdater() {
        while(true) {
            transform.GetComponent<Text>().text = PlayerController.currentScore.ToString();
            yield return new WaitForSeconds(0.1f);
        }
    }

    IEnumerator HealthUpdater() {
        while(true) {
            transform.localScale = new Vector3(PlayerController.currentHeatlh / PlayerController.maxHealth, 1, 1);
            yield return new WaitForSeconds(0.1f);
        }
    }

    public void ExitFromPlay() {
        PlayerLoader();
        StaticInfo.score += PlayerController.currentScore;
        PlayerSaver();
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }

    public IEnumerator BossHealthUpdater() {
        while(true) {
            transform.localScale = new Vector3(BossController.currentHealth / BossController.maxHealth, 1, 1);
            yield return new WaitForSeconds(0.25f);
        }
    }

    public void PlayerSaver(string name = "") {
        if(name == "")
            name = StaticInfo.player;
        float score = StaticInfo.score;
        Player player = new Player();
        player.name = name;
        player.score = score;
        player.Serialize("Assets/Resources/Players/" + player.name + ".xml");
    }

    public void PlayerLoader(string name = "") {
        if (name == "")
            name = StaticInfo.player;
        Player player = Player.DeSerialize("Assets/Resources/Players/" + name + ".xml");
        StaticInfo.score = player.score;
    }

    static public void StaticPlayerLoader(string name = "") {
        if (name == "")
            name = StaticInfo.player;
        Player player = Player.DeSerialize("Assets/Resources/Players/" + name + ".xml");
        StaticInfo.score = player.score;
    }

    public IEnumerator UpdateScore() {
        while(true) {
            transform.GetComponent<Text>().text = StaticInfo.score.ToString();
            yield return new WaitForSeconds(0.1f);
        }
    }

    public IEnumerator UpdateAbilityCount() {
        while(true) {
            yield return new WaitForSeconds(0.1f);
        }
    }

    public void BuyAbility() {
        if(StaticInfo.score >= 1000) {
            StaticInfo.score -= 1000;
        }
    }
}
