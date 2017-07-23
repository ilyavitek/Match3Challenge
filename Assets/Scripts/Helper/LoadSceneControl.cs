using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneControl: MonoBehaviour {

	public void LoadScene (string name) {
		SceneManager.LoadScene (name);
	}

	public void RestartScene () {
		SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
	}
}
