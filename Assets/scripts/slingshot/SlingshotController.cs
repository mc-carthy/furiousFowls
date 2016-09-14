using UnityEngine;
using System.Collections;

public class SlingshotController : MonoBehaviour {

	[HideInInspector]
	public SlingshotState state;
	[HideInInspector]
	public GameObject birdToThrow;
	[HideInInspector]
	public float timeSinceThrow;

	public Transform leftOrigin, rightOrigin, birdWaitPos;
	public LineRenderer slingshotLineRenderer0, slingshotLineRenderer1, trajectoryLineRenderer;
	public float throwSpeed;
	public float maxDrawDistance = 1.5f;
	public float minReleaseThreshold = 1f;
	public float returnToSlingshotSpeed = 0.05f;

	public delegate void BirdThrown ();
	public event BirdThrown birdThrown;

	private Vector3 slingshotMiddleVector;

	private void Awake () {
		slingshotLineRenderer0.sortingLayerName = "foreground";
		slingshotLineRenderer1.sortingLayerName = "foreground";
		trajectoryLineRenderer.sortingLayerName = "foreground";

		state = SlingshotState.Idle;
		slingshotLineRenderer0.SetPosition (0, leftOrigin.position);
		slingshotLineRenderer1.SetPosition (0, rightOrigin.position);
		slingshotLineRenderer0.enabled = false;
		slingshotLineRenderer1.enabled = false;
		slingshotMiddleVector = new Vector3 (
											(leftOrigin.position.x + rightOrigin.position.x) / 2, 
											(leftOrigin.position.y + rightOrigin.position.y) / 2, 
											transform.position.z
		); 
	}

	private void Update () {
		switch (state) {
		case SlingshotState.Idle:
			InitializeBird ();
			DisplaySlingshotLineRenderers ();

			if (Input.GetMouseButtonDown (0)) {
				Vector3 location = Camera.main.ScreenToWorldPoint (Input.mousePosition);

				if (birdToThrow.GetComponent<CircleCollider2D> () == Physics2D.OverlapPoint (location)) {
					state = SlingshotState.UserPulling;
				}
			}

			break;

		case SlingshotState.UserPulling:
			DisplaySlingshotLineRenderers ();

			if (Input.GetMouseButton (0)) {
				Vector3 location = Camera.main.ScreenToWorldPoint (Input.mousePosition); 
				location.z = 0;

				if (Vector3.Distance (location, slingshotMiddleVector) > maxDrawDistance) {
					Vector3 maxPosition = (location - slingshotMiddleVector).normalized * maxDrawDistance + slingshotMiddleVector;
					birdToThrow.transform.position = maxPosition;
				} else {
					birdToThrow.transform.position = location;
				}

				float distance = Vector3.Distance (slingshotMiddleVector, birdToThrow.transform.position);
				DisplayTrajectoryLineRenderer (distance);

			} else {

				SetTrajectoryLineRendererActive (true);
				timeSinceThrow = Time.time;

				float distance = Vector3.Distance (slingshotMiddleVector, birdToThrow.transform.position);

				if (distance > minReleaseThreshold) {
					SetSlingshotLineRendererActive (false);
					state = SlingshotState.BirdFlying;
					ThrowBird (distance);
				} else {
					birdToThrow.transform.position = Vector2.MoveTowards (birdToThrow.transform.position, birdWaitPos.position, returnToSlingshotSpeed);
					InitializeBird ();
				}

			}

			break;

		case SlingshotState.BirdFlying:

			break;
		}
	}

	private void InitializeBird () {
		birdToThrow.transform.position = birdWaitPos.position;
		state = SlingshotState.Idle;
		SetSlingshotLineRendererActive (true);
	}

	private void SetSlingshotLineRendererActive (bool active) {
		slingshotLineRenderer0.enabled = true;
		slingshotLineRenderer1.enabled = true;
		slingshotLineRenderer0.sortingLayerName = "foreground";
		slingshotLineRenderer1.sortingLayerName = "foreground";
	}

	private void DisplaySlingshotLineRenderers () {
		slingshotLineRenderer0.SetPosition (1, birdToThrow.transform.position);
		slingshotLineRenderer1.SetPosition (1, birdToThrow.transform.position);
	}

	private void SetTrajectoryLineRendererActive (bool active) {
		trajectoryLineRenderer.enabled = active;
	}

	private void DisplayTrajectoryLineRenderer (float distance) {
		SetTrajectoryLineRendererActive (true);

		Vector3 v2 = slingshotMiddleVector - birdToThrow.transform.position;
		int segmentCount = 15;
		Vector2[] segments = new Vector2[segmentCount];

		segments [0] = birdToThrow.transform.position;

		Vector2 segVel = new Vector2 (v2.x, v2.y) * throwSpeed * distance;

		for (int i = 1; i < segmentCount; i++) {
			float time = i * Time.fixedDeltaTime * 5; // Replace hardcoded value
			segments[i] = segments[0] + segVel * time + 0.5f * Physics2D.gravity * Mathf.Pow(time, 2);
		}

		trajectoryLineRenderer.SetVertexCount (segmentCount);

		for (int i = 0; i < segmentCount; i++) {
			trajectoryLineRenderer.SetPosition (i, segments [i]);
		}
	}

	private void ThrowBird (float distance) {
		Vector3 vel = slingshotMiddleVector - birdToThrow.transform.position;

		birdToThrow.GetComponent<BirdController> ().OnThrow ();

		birdToThrow.GetComponent<Rigidbody2D> ().velocity = new Vector2 (vel.x, vel.y) * throwSpeed * distance;

		if (birdThrown != null) {
			birdThrown ();
		}
	}
}
