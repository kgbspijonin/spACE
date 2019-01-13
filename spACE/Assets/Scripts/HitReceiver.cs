using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitReceiver : MonoBehaviour {
	public GameObject ObjectToSpawnOnDeath;
	public GameObject DestructionFX;
    public AudioSource destroySound;
	public float SpawnDistance = 2;
	public float DeflectionAngle = 45;
	public float DestructionFXDuration = 0.5f;
	public bool DebugDraw = false;
	
	public void ReceiveHit(GameObject damageDealer) {
        if (ObjectToSpawnOnDeath != null)
		{
			Vector3 hitDirection = transform.position - damageDealer.transform.position;
			hitDirection.Normalize();
			if (DebugDraw) {
				Debug.DrawLine (damageDealer.transform.position, transform.position, Color.red, 2.0f);
			}
			SpawnDeathObject (hitDirection, -DeflectionAngle);
			SpawnDeathObject (hitDirection, DeflectionAngle);
		}
		if (DestructionFX != null) {
			GameObject spawnedFX = Instantiate (DestructionFX, transform.position, Random.rotation);
			Destroy (spawnedFX, DestructionFXDuration);
		}
        Destroy(gameObject);
	}

	private void SpawnDeathObject(Vector3 hitDirection, float angle)
	{
		Vector3 spawnDirection = Quaternion.AngleAxis (angle, Vector3.up) * hitDirection;
		Vector3 spawnPosition = transform.position + spawnDirection * SpawnDistance;
		if (DebugDraw) {
			Debug.DrawLine (transform.position, spawnPosition, Color.green, 2.0f);
		}

		GameObject spawnedObject = Instantiate(ObjectToSpawnOnDeath, spawnPosition, Random.rotation);
        spawnedObject.transform.localScale = new Vector3(transform.localScale.x/1.5f, transform.localScale.y/1.5f, transform.localScale.z/1.5f);
        spawnedObject.transform.position = new Vector3(spawnedObject.transform.position.x, 0, spawnedObject.transform.position.z);
        if(spawnedObject.transform.localScale.x < 0.4f) {
            Destroy(spawnedObject);
            return;
        }
		var asteroidMovementController = spawnedObject.GetComponent<AsteroidMovementController> ();
		if (asteroidMovementController) {
			asteroidMovementController.InitialDirection = spawnDirection;
		}
	}
}
