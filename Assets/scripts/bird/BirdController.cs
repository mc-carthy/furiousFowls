using UnityEngine;
using System.Collections;

public class BirdController : MonoBehaviour {

	public BirdState birdstate { get; set; }

	private TrailRenderer trailRenderer;
	private Rigidbody2D rb;
	private CircleCollider2D cirCol;
	private AudioSource source;

	private void Awake () {
		InitializeVariables ();
	}

	private void FixedUpdate () {
		if (birdstate == BirdState.Thrown && rb.velocity.sqrMagnitude <= GameVariables.minVelocity) {
			StartCoroutine (DestroyAfterDelay (3f));
		}
	}
	public void OnThrow () {
		source.Play ();
		trailRenderer.enabled = true;
		rb.isKinematic = false;
		cirCol.radius = GameVariables.birdColliderRadiusNormal;
		birdstate = BirdState.Thrown;
	}

	private void InitializeVariables () {
		trailRenderer = GetComponent<TrailRenderer> ();
		rb = GetComponent<Rigidbody2D> ();
		cirCol = GetComponent<CircleCollider2D> ();
		source = GetComponent<AudioSource> ();

		trailRenderer.enabled = false;
		trailRenderer.sortingLayerName = "foreGround";

		rb.isKinematic = true;
		cirCol.radius = GameVariables.birdColliderRadiusLarge;

		birdstate = BirdState.BeforeThrown;
	}

	private IEnumerator DestroyAfterDelay (float delay) {
		yield return new WaitForSeconds (delay);
		Destroy (gameObject);
	}
}
