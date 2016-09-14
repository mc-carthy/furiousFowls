using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MenuController : MonoBehaviour {

	public void GoToMenu() {
		SceneManager.LoadScene ("menu", LoadSceneMode.Single);

	}

	public void GoToLevelSelect () {
		SceneManager.LoadScene ("levelSelect", LoadSceneMode.Single);
	}

	public void GoToLevel(int level = 1) {
		SceneManager.LoadScene ("level" + level.ToString(), LoadSceneMode.Single);
	}
}