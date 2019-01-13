using UnityEngine;
using System.Collections;

public class CannonBehavior : MonoBehaviour {

	public Transform m_cannonRot;
	public Transform m_muzzle;
	public GameObject m_shotPrefab;
	public Texture2D m_guiTexture;
    public GameObject test;

	void Update () 
	{
		if (Input.GetKey(KeyCode.LeftArrow))
		{
			m_cannonRot.transform.Rotate(Vector3.up, -Time.deltaTime * 100f);
		}
		if (Input.GetKey(KeyCode.RightArrow))
		{
			m_cannonRot.transform.Rotate(Vector3.up, Time.deltaTime * 100f);
		}
		if (Input.GetKey(KeyCode.Space))
		{
			GameObject go = GameObject.Instantiate(m_shotPrefab, m_muzzle.position, m_muzzle.rotation) as GameObject;
			GameObject.Destroy(go, 3f);
		}
        if(Input.GetKey(KeyCode.UpArrow)) {
            Instantiate(test, transform.position, transform.rotation);
        }
	}

	void OnGUI()
	{
		GUI.DrawTexture(new Rect(0f, 0f, m_guiTexture.width / 2, m_guiTexture.height / 2), m_guiTexture);
	}
}
