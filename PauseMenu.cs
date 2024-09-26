using UnityEngine;

// Class for Pause Menu (Pause/Resume/Main Menu/Quit)
public class PauseMenu : MonoBehaviour
{
    // Static variable that represents whether game is paused or not
    public static bool paused = false;
    // Reference to the gameobject of the Pause Menu's UI
    public GameObject pauseMenu;

    // Update is called once per frame
    void Update()
    {
        // If player presses Esc
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            // If game is already paused, resume the game
            if (paused)
                ResumeGame();
            // Else, pause the game and display the Pause Menu
            else
                PauseGame();
        }
    }

    // Function to pause the game
    public void PauseGame()
    {
        // Show the pause menu UI on screen
        pauseMenu.SetActive(true);
        // Set timescale to 0 so that counter doesn't decrement
        Time.timeScale = 0f;
        // Game is paused
        paused = true;
    }

    // Function to resume the game
    public void ResumeGame()
    {
        // Hide the pause menu UI on screen
        pauseMenu.SetActive(false);
        // Set timescale to 0 so that counter starts decrementing normally
        Time.timeScale = 1f;
        // Game has resumed
        paused = false;
    }
}
