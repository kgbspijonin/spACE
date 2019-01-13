using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayBorder : MonoBehaviour {
    void OnTriggerExit(Collider other) {
        if(other.gameObject.layer == 8) {
            Debug.Log("out");
            other.transform.position = new Vector3(
            Mathf.Clamp(other.transform.position.x, (-transform.localScale.x+10) / 2, (transform.localScale.x-10) / 2),
            Mathf.Clamp(other.transform.position.y, (-transform.localScale.y+10) / 2, (transform.localScale.y-10) / 2),
            0);
        }
        else {
            Debug.Log(other.transform.position);
            Destroy(other.gameObject);
            
        }
        Debug.Log("fdsa");
    }
}
