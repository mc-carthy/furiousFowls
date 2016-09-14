using UnityEngine;
using System.Collections;

public class BrickController : MonoBehaviour {

	public float health = 70f;

	private AudioSource source;
	private float damageMultiplier = 10f;

	private void Awake () {
		source = GetComponent<AudioSource> ();
	}

	private void OnCollisionEnter2D (Collision2D col) {
		if (col.gameObject.GetComponent<Rigidbody2D> () != null) {
			float projectileVelocity = col.gameObject.GetComponent<Rigidbody2D> ().velocity.magnitude;
			float damage = projectileVelocity * damageMultiplier;
			if (damage > 5f) {
				source.Play ();
			}
			health -= damage;

			if (health < 0) {
				Destroy (gameObject);
			}
		}
	}
}
