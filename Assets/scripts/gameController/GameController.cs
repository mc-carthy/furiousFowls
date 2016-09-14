using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class GameController : MonoBehaviour {

	[HideInInspector]
	public static GameState state;

	public CameraController camera;
	public SlingshotController slingshot;

	private int currentBirdIndex;
	private float cameraMovementSpeed = 10f;
	private List<GameObject> bricks;
	private List<GameObject> birds;
	private List<GameObject> pigs;

	private void Awake () {
		state = GameState.Start;
		slingshot.enabled = false;

		bricks = new List<GameObject>(GameObject.FindGameObjectsWithTag("brick"));
		birds = new List<GameObject>(GameObject.FindGameObjectsWithTag("bird"));
		pigs = new List<GameObject>(GameObject.FindGameObjectsWithTag("pig"));
	}

	private void Update () {
		switch (state) {
		case GameState.Start:

			if (Input.GetMouseButtonUp (0)) {
				AnimateBirdToSlingshot ();
			}

			break;

		case GameState.Playing:

			if (slingshot.state == SlingshotState.BirdFlying && (GameObjectsStoppedMoving () || Time.time - slingshot.timeSinceThrow > 5)) {
				slingshot.enabled = false;
				AnimateCameraToStartPos ();
				state = GameState.BirdMovingToSlingshot;
			}

			break;
		case GameState.Won:
		case GameState.Lost:

			if (Input.GetMouseButtonDown (0)) {
				SceneManager.LoadScene (SceneManager.GetActiveScene ().name, LoadSceneMode.Single);
			}

			break;
		}
	}

	private void OnEnable () {
		slingshot.birdThrown += SlingshotBirdThrown;
	}

	private void OnDisable () {
		slingshot.birdThrown -= SlingshotBirdThrown;
	}

	private void AnimateBirdToSlingshot () {
		state = GameState.BirdMovingToSlingshot;
		birds [currentBirdIndex].transform.position = Vector2.MoveTowards (birds [currentBirdIndex].transform.position, slingshot.birdWaitPos.position, slingshot.returnToSlingshotSpeed);
		state = GameState.Playing;
		slingshot.enabled = true;
		slingshot.birdToThrow = birds [currentBirdIndex];
	}

	// TODO - Get Rigidbody components in Awake to limit amount of GetComponent calls
	private bool GameObjectsStoppedMoving () {

		foreach (GameObject item in bricks.Union(birds).Union(pigs)) {
			if (item != null) {
				if (item.GetComponent<Rigidbody2D> ().velocity.sqrMagnitude > GameVariables.minVelocity) {
					return false;
				}
			}
		}

		return true;
	}

	private bool AllPigsAreDead () {
		return pigs.All (x => x == null);
	}

	private void AnimateCameraToStartPos () {
		float duration = Vector2.Distance (Camera.main.transform.position, CameraController.startPos) / 10f; // TODO - Remove hard coded number

		if (duration == 0.0f) {
			duration = 0.1f;
		}

		Camera.main.transform.position = Vector2.MoveTowards (Camera.main.transform.position, CameraController.startPos, cameraMovementSpeed);
		CameraController.isFollowing = false;
		if (AllPigsAreDead ()) {
			state = GameState.Won;
		} else if (currentBirdIndex == birds.Count - 1) {
			state = GameState.Lost;
		} else {
			slingshot.state = SlingshotState.Idle;
			currentBirdIndex++;
			AnimateBirdToSlingshot ();
		}
	}

	private void SlingshotBirdThrown () {
		CameraController.birdToFollow = birds [currentBirdIndex].transform;
		CameraController.isFollowing = true;
	}
}
