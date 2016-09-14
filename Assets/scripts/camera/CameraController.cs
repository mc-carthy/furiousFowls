using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	[HideInInspector]
	public Vector3 startPos;
	[HideInInspector]
	public bool isFollowing;
	[HideInInspector]
	public Transform birdToFollow;

	private float minX = 0, maxX = 20;

	private void Awake () {
		startPos = transform.position;
	}

	private void Update () {
		if (isFollowing) {
			if (birdToFollow != null) {
				Vector3 birdPos = birdToFollow.position;
				float x = Mathf.Clamp (birdPos.x, minX, maxX);
				transform.position = new Vector3 (x, startPos.y, startPos.z);
			} else {
				isFollowing = false;
			}
		}
	}
}
