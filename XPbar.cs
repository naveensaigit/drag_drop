using UnityEngine;
using UnityEngine.UI;
using TMPro;

// Class to manage XP of player
public class XPbar : MonoBehaviour
{
    // Reference to the XP bar (slider)
    private Slider slider;
    // References to the texts displaying messages on screen
    private TextMeshProUGUI gameOverScoreText, highScoreText, levelUpText;

    // Variable to store the current level of the player
    int level;
    // Various parameters that affect how the XP is calculated
    float fillSpeed = 0.25f, target = 0, score, prevxp, xpchange, highscore, xpmin, xpmax, factor = 600;
    bool xprem = false, updateXP = true;

    void Awake()
    {
        // Get player's current statistics
        level = PlayerPrefs.GetInt("level", 1);
        score = PlayerPrefs.GetFloat("CurrentScore", 0);
        prevxp = PlayerPrefs.GetFloat("XP", 0);
        highscore = PlayerPrefs.GetFloat("HighScore", 0);
        xpchange = PlayerPrefs.GetFloat("XPchange", 0);

        // Current score is a high score
        if (score > highscore)
        {
            xpchange += (level / 3 + 1) * 50;
            PlayerPrefs.SetFloat("XPchange", xpchange);
        }
        // Calculate the minimum and maximum XP for the level player is at
        SetXPForLevel();

        // Set the initial XP bar value based on current XP
        GameObject sliderObject = GameObject.Find("ProgressBar");
        slider = sliderObject.GetComponent<Slider>();
        slider.value = (prevxp - xpmin) / (xpmax - xpmin);

        // Display the score on screen
        GameObject scoreText = GameObject.Find("Score");
        gameOverScoreText = scoreText.GetComponent<TextMeshProUGUI>();
        gameOverScoreText.text = "You won " + score.ToString("0") + " points!";

        scoreText = GameObject.Find("HighScore");
        highScoreText = scoreText.GetComponent<TextMeshProUGUI>();
        // If score is a high score, display the high score message on screen
        if (score >= highscore)
            PlayerPrefs.SetFloat("HighScore", score);
        // Else, don't show the high score message
        else
            highScoreText.enabled = false;

        // Initially hide the level up message
        scoreText = GameObject.Find("LevelUp");
        levelUpText = scoreText.GetComponent<TextMeshProUGUI>();
        levelUpText.enabled = false;

        // Add XP points that the player has received from the previous game
        IncrementProgress(xpchange);
    }

    // Function to add XP points to player
    void IncrementProgress(float xpchange)
    {
        // If XP points achieved are higher than current level's threshold
        if (xpchange + prevxp > xpmax)
        {
            target = 1;
            xprem = true;
        }
        // Just increase the XP points, and player doesn't level up
        else
            target = (xpchange + prevxp - xpmin) / (xpmax - xpmin);
    }

    // Function to Level Up the player
    void ChangeLevel()
    {
        // Increase the level and store it
        level++;
        PlayerPrefs.SetInt("level", level);
        
        // Calculate thresholds for new level
        SetXPForLevel();

        // Show the level up message on screen
        levelUpText.text = "LEVEL " + (level-1).ToString() + " CLEARED!";
        levelUpText.enabled = true;
    }

    // Function to calculate the min. and max. thresholds for the level
    void SetXPForLevel()
    {
        xpmin = GetXPForLevel((float)level);
        xpmax = GetXPForLevel((float)(level + 1));
    }

    // Calculate the XP threshold at the level
    float GetXPForLevel(float x)
    {
        // Source: https://oldschool.runescape.wiki/w/Experience
        return (Mathf.Pow(x, 2) - x + factor * ((Mathf.Pow(2, x / 7) - Mathf.Pow(2, 1f / 7)) / (Mathf.Pow(2, 1f / 7) - 1))) / 8;
    }

    // Update is called once per frame
    void Update()
    {
        // Slowly fill the XP bar as XP is added
        if (slider.value < target)
            slider.value += Time.deltaTime * fillSpeed;
        // XP bar reaches maximum value, so set it back to 0 and increase the level
        else if(Mathf.Abs(slider.value - 1) < 0.001 && xprem)
        {
            slider.value = 0;
            ChangeLevel();
            xpchange -= (xpmin - prevxp);
            prevxp = xpmin;
            IncrementProgress(xpchange);
        }
        // XP points are now added. Store the new XP
        else if(updateXP)
        {
            updateXP = false;
            PlayerPrefs.SetFloat("XP", PlayerPrefs.GetFloat("XP") + PlayerPrefs.GetFloat("XPchange"));
        }
    }
}
