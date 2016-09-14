using UnityEngine;
using System.Collections;

public class ParallaxController : MonoBehaviour {

	public float parallaxFactor;

	private Vector3 previousPosition;

	private void Start () {
		previousPosition = Camera.main.transform.position;
	}

	private void Update () {
		Vector3 delta = Camera.main.transform.position - previousPosition;
		delta.y = 0f;
		delta.z = 0f;
		transform.position += delta / parallaxFactor;

		previousPosition = Camera.main.transform.position;
	}
}
