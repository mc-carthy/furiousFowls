using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	[HideInInspector]
	public Vector3 startPos;
	[HideInInspector]
	public bool isFollowing;
	[HideInInspector]
	public Transform birdToFollow;

	public SlingshotController slingshot;

	private float minX = 0, maxX = 20;
	private float minY = 0, maxY = 2.7f;

	private float dragSpeed = 0.01f;
	private float timeDragStart;
	private Vector3 previousPosition;

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

		if (slingshot.state == SlingshotState.Idle && GameController.state == GameState.Playing) {
			if (Input.GetMouseButtonDown (0)) {
				timeDragStart = Time.time;
				dragSpeed = 0f;
				previousPosition = Input.mousePosition;
			}
		// TODO - Remove hardcoded value
		} else if (Input.GetMouseButton (0) && Time.time - timeDragStart > 0.005f) {
			Vector3 input = Input.mousePosition;
			float deltaX = (previousPosition.x - input.x) * dragSpeed;
			float deltaY = (previousPosition.y - input.y) * dragSpeed;

			float newX = Mathf.Clamp (transform.position.x + deltaX, minX, maxX);
			float newY = Mathf.Clamp (transform.position.y + deltaY, minY, maxY);

			transform.position = new Vector3 (newX, newY, transform.position.z);

			previousPosition = input;

			// TODO - Remove hardcoded values
			if (dragSpeed < 0.1f) {
				dragSpeed += 0.002f;
			}
		}
	}
}
