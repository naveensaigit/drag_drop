using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

// Class to manage the game (scoring, timer, XP)
public class GameManager : MonoBehaviour
{
    // Creating a static instance so that multiple copies of GameManager don't exist
    public static GameManager Instance;
    // Names of all candies
    string[] candies = { "alpenliebe", "cadbury", "eclairs", "cane", "lollipop"};
    // References to the texts displaying messages on screen
    public TextMeshProUGUI scoreText, timerText, levelText;
    // A map from candy names to its corresponding index
    Dictionary<string, int> candymap = new Dictionary<string, int>();
    // A counts array that keeps a track of the number of candies of each type
    int[] cts = { 0, 0, 0, 0, 0 };
    // Variable to store the player's level
    int level;
    // Various other parameters to calculate score and XP
    double score = 0, xpchange = 0, total = 0, k = 5, basePoints, mxBonus, mxXP;
    // No. of seconds left to play
    float timer = 31;

    private void Awake()
    {
        // Set the static instance to the current object
        Instance = this;
        // Set time scale back to normal
        Time.timeScale = 1f;
        // Game is resumed, so set paused to false
        PauseMenu.paused = false;

        // Get the player's current level
        level = PlayerPrefs.GetInt("level");
        // According to the level, vary the parameters used to calculate the score
        basePoints = (level/3 + 1) * 5;
        mxBonus = (level/5 + 1) * 100;
        mxXP = (level/3 + 1) * 3.5;

        // Display the level on screen
        levelText.text = "Level: " + level.ToString();
    }

    // Update is called once per frame
    public void Update()
    {
        // If more than 1 second is left, decrease the time and update the text on screen
        if (timer > 1)
        {
            timer -= Time.deltaTime;
            UpdateTimer();
        }
        // Time is up. Show the game over screen
        else
        {
            SceneManager.LoadScene("GameOverMenu");
        }
    }

    private void Start()
    {
        // If current score or XP is not present previously, set them to 0
        PlayerPrefs.SetFloat("CurrentScore", 0);
        PlayerPrefs.SetFloat("XPchange", 0);
        // Add each candy and index to the map
        for (int i = 0; i < candies.Length; i++)
            candymap.Add(candies[i], i);
    }

    // Function to add a particular candy to the total count
    public void AddCandy(GameObject candy)
    {
        // Increase total count
        total++;
        // Increase the count of the particular candy
        cts[candymap[candy.name.Split('_')[0]]]++;
        // Update the score
        Score();
    }

    // Function to add XP points when new candy is placed inside the box
    public void AddXP(float time)
    {
        // XP points exponentially reduce as a function of how
        // long the player takes to add a candy inside the box
        xpchange += mxXP * Mathf.Exp(-0.135f * time);
        // Store it in the player preferences
        PlayerPrefs.SetFloat("XPchange", (float)xpchange);
    }

    // Function to update the score
    private void Score()
    {
        // d represents the variance of the count of the candies
        double d=0;
        score = 0;
        for (int i = 0; i < cts.Length; i++)
        {
            // Add the variance term
            d += Math.Pow(cts[i] - total / cts.Length, 2);
            // Add base points for each candy
            score += cts[i] * basePoints;
        }
        // Update the score according to the scoring function
        score += mxBonus * (1 - d * k / (total * total * (k - 1)));
        // Display the score on screen
        scoreText.text = "Score: " + score.ToString("0");
        // Set the current score in player preferences
        PlayerPrefs.SetFloat("CurrentScore", (float)score);
    }

    // Function to update the timer
    private void UpdateTimer()
    {
        // Based on number of seconds left, calculate the time left in the format mm:ss
        double mins = Mathf.FloorToInt(timer/60), secs = Mathf.FloorToInt(timer%60);
        // Format the time in mm:ss and display it on screen
        timerText.text = string.Format("{0:00}:{1:00}", mins, secs);
    }
}
