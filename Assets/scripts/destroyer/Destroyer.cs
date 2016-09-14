using UnityEngine;
using System.Collections;

public class Destroyer : MonoBehaviour {

	private void OnTriggerEnter2D (Collider2D trig) {
		if (trig.tag == "bird" || trig.tag == "pig" || trig.tag == "brick") {
			Destroy (trig.gameObject);
		}
	}
}
