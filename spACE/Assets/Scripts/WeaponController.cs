using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour {
    public Weapon data = new Weapon();
    public string TagToHit = "Asteroid";
    private float time;
    public GameObject prefab;

    void Start() {
        data = Weapon.DeSerialize("Assets/Resources/Weapons/" + transform.name + ".xml");
        time = Time.time;
        prefab = Resources.Load<GameObject>("Projectiles/" + "Sphere");
        prefab.name = data.bullet;
        if(data.barrel == "Machine gun") {
            data.fireRate *= 4;
        }
    }

    void SetupBullet(out GameObject bullet, Transform child) {
        bullet = Instantiate(prefab, child.position, transform.rotation);
        bullet.GetComponent<BulletController>().info = new Bullet();
        Bullet info = bullet.GetComponent<BulletController>().info;
        bullet.name = prefab.name;
        ColorUtility.TryParseHtmlString(data.bullet, out info.color);
        bullet.GetComponent<MeshRenderer>().material.color = info.color;
        bullet.transform.tag = "Bullet";
        bullet.GetComponent<BulletController>().info.damage = 1.0f;
    }

    public void Shoot() {
        if(Time.time > time + 1 / data.fireRate) {
            time = Time.time;
            if (data.barrel == "Normal" || data.barrel == "Machine gun") {
                foreach(Transform child in transform) {
                    GameObject bullet;
                    SetupBullet(out bullet, child);
                    bullet.GetComponent<BulletController>().info.speed = 5f;
                    if(data.barrel == "Normal") {
                        bullet.transform.localScale = new Vector3(2, 2, 2);
                        bullet.GetComponent<BulletController>().info.damage = 4.0f;
                    } else {
                        bullet.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                    }
                    bullet.transform.tag = "Bullet";
                }
            } else if(data.barrel == "Shotgun") {
                foreach (Transform child in transform) {
                    for (int i = 0; i < 5; i++) {
                        Quaternion pelletRotationer = Quaternion.identity;
                        GameObject bullet;
                        SetupBullet(out bullet, child);
                        switch (i) {
                            case 0: bullet.transform.Rotate(bullet.transform.rotation.x - 1, bullet.transform.rotation.y, bullet.transform.rotation.z); break;
                            case 1: bullet.transform.Rotate(bullet.transform.rotation.x + 1, bullet.transform.rotation.y, bullet.transform.rotation.z); break;
                            case 2: bullet.transform.Rotate(bullet.transform.rotation.x, bullet.transform.rotation.y - 1, bullet.transform.rotation.z); break;
                            case 3: bullet.transform.Rotate(bullet.transform.rotation.x, bullet.transform.rotation.y + 1, bullet.transform.rotation.z); break;
                        }
                        bullet.GetComponent<BulletController>().info.speed = 5f;
                        bullet.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                    }
                }
            } else if(data.barrel == "Sniper") {
                foreach(Transform child in transform) {
                    GameObject bullet;
                    SetupBullet(out bullet, child);
                    bullet.GetComponent<BulletController>().info.speed = 15f;
                    bullet.GetComponent<BulletController>().info.damage = 3.0f;
                }
            } else if (data.barrel == "Mortar") {
                foreach (Transform child in transform) {
                    GameObject bullet;
                    SetupBullet(out bullet, child);
                    bullet.GetComponent<BulletController>().info.speed = 1.5f;
                    bullet.transform.localScale = new Vector3(3f, 3f, 3f);
                    bullet.GetComponent<BulletController>().info.damage = 10.0f;
                }
            } else if (data.barrel == "Missile") {
                foreach (Transform child in transform) {
                    GameObject bullet;
                    SetupBullet(out bullet, child);
                    bullet.GetComponent<BulletController>().info.speed = 1f;
                    bullet.transform.localScale = new Vector3(1f, 1f, 3f);
                    bullet.GetComponent<BulletController>().info.damage = 2;
                    bullet.GetComponent<BulletController>().followsEnemy = true;
                }
            }
        }   
    }
}
