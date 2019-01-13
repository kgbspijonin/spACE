using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageDealer : MonoBehaviour {
	public string TagToHit = "Asteroid";
    public float pierce;

	void OnTriggerEnter(Collider collision)
	{
		if(collision.gameObject.CompareTag(TagToHit)) {
            pierce--;
            if (pierce <= 0) {

                Destroy(gameObject);
            }
            HitReceiver hitReceiver = collision.gameObject.GetComponent<HitReceiver> ();
			if (hitReceiver) {
				hitReceiver.ReceiveHit (gameObject);

			} else {
				Destroy (collision.gameObject);
			}

		}
	}
}
