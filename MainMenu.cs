using UnityEngine;
using UnityEngine.SceneManagement;

// Class for Main Menu (Play/Rules/Reset/Quit)
public class MainMenu : MonoBehaviour
{
    public void Start()
    {
        // If player preferences are not found, initialize them with 0
        PlayerPrefs.SetFloat("HighScore", PlayerPrefs.GetFloat("HighScore", 0));
        PlayerPrefs.SetFloat("XP", PlayerPrefs.GetFloat("XP", 0));
        PlayerPrefs.SetInt("level", PlayerPrefs.GetInt("level", 1));
    }

    // Function to start playing the game
    public void PlayGame()
    {
        // Play the success audio when player clicks on the button
        AudioManager.Instance.Play("Success");
        // Starting a new game, so game is not paused
        PauseMenu.paused = false;
        // Timescale set to 1 as time decrements normally
        Time.timeScale = 1f;
        // Load the Game scene
        SceneManager.LoadScene("Game");
    }

    // Function to display the Main Menu
    public void LoadMainMenu()
    {
        // Play the success audio when player clicks on the button
        AudioManager.Instance.Play("Success");
        // Starting a new game, so game is not paused
        PauseMenu.paused = false;
        // Timescale set to 1 as time decrements normally
        Time.timeScale = 1f;
        // Load the Main Menu scene
        SceneManager.LoadScene("MainMenu");
    }
    
    // Function to reset all player preferences to default values
    public void ResetPrefs()
    {
        // Play the success audio when player clicks on the button
        AudioManager.Instance.Play("Success");

        // Reset all player preferences to 0
        PlayerPrefs.SetFloat("CurrentScore", 0);
        PlayerPrefs.SetFloat("HighScore", 0);
        PlayerPrefs.SetFloat("XP", 0);
        PlayerPrefs.SetFloat("XPchange", 0);
        PlayerPrefs.SetInt("level", 1);
    }

    // Function to display the rules of the game
    public void LoadRules()
    {
        // Play the success audio when player clicks on the button
        AudioManager.Instance.Play("Success");
        // Load the Rules scene, where rules of the game are displayed
        SceneManager.LoadScene("Rules");
    }

    // Function to quit the game
    public void QuitGame()
    {
        // Play the success audio when player clicks on the button
        AudioManager.Instance.Play("Success");
        // Quit the game
        Application.Quit();
    }
}
