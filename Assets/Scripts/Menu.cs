/*
* Author Walrys
* https://walrys.com
*
*/

using UnityEngine;
using UnityEngine.SceneManagement;


public class Menu : MonoBehaviour {
	public void BackToMenu() {
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
	}
	public void PlayGame() {
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);	
	}
	public void QuitGame() {
		Application.Quit();
	}
}
