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

	public delegate void BirdThrown ();
	public event BirdThrown birdThrown;

	private Vector3 slingshotMiddleVector;

	private void Awake () {
		slingshotLineRenderer0.sortingLayerName = "foreground";
		slingshotLineRenderer1.sortingLayerName = "foreground";
		trajectoryLineRenderer.sortingLayerName = "foreground";

		state = SlingshotState.Idle;
		slingshotLineRenderer0.SetPosition (0, leftOrigin.position);
		slingshotLineRenderer0.SetPosition (0, rightOrigin.position);
		slingshotMiddleVector = new Vector3 (
											(leftOrigin.position.x + rightOrigin.position.x) / 2, 
											(leftOrigin.position.y + rightOrigin.position.y) / 2, 
											transform.position.z
		); 
	}
}
