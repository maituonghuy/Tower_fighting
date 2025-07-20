using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
	// Start is called once before the first execution of Update after the MonoBehaviour is created
	public void PlayGame()
	{
		SceneManager.LoadScene("CharacterSelect");
	}

	public void OpenSettings()
	{
		SceneManager.LoadScene("Settings");
	}

	public void OpenAboutUs()
	{
		SceneManager.LoadScene("AboutUs");
	}

	public void ExitGame()
	{
		Application.Quit();
	}
}
