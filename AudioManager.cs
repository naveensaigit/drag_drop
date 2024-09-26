using System.Collections.Generic;
using UnityEngine;

// Class to manage audio in the game
public class AudioManager : MonoBehaviour
{
    // Creating a static instance so that multiple copies of AudioManager don't exist
    public static AudioManager Instance;
    // A map from audio names to its corresponding audio source
    public Dictionary<string, AudioSource> sources = new Dictionary<string, AudioSource>();
    // Names of all audios being used in the game
    public string[] songs = { "Theme", "Success", "Error" };

    void Awake()
    {
        // If an AudioManager instance doesn't exist, make current object the instance
        if (Instance == null)
            Instance = this;
        // If an AudioManager already exists, delete the current instance.
        else
        {
            Destroy(gameObject);
            return;
        }
        // Don't remove the AudioManager when scenes change.
        // Used so that music can be played without abrupt switches when scenes change.
        DontDestroyOnLoad(gameObject);

        // For each audio, create an audio source and set its properties
        foreach(string name in songs)
        {
            AudioSource s = gameObject.AddComponent<AudioSource>();
            s.clip = Resources.Load(name) as AudioClip;
            s.volume = 1;
            s.pitch = 1;
            s.loop = false;
            sources.Add(name, s);
        }
        // Play the theme song continuously
        sources["Theme"].loop = true;
        // Set the theme volume slightly low
        sources["Theme"].volume = 0.7f;
        // Start by playing the theme song
        Play("Theme");
    }

    // Function to play an audio given its name
    public void Play(string name)
    {
        // If the audio name is valid, play the audio
        if (sources.ContainsKey(name))
            sources[name].Play();
        // Else, log a warning
        else
            Debug.Log("Invalid song name!");
    }

    // Update is called once per frame
    void Update()
    {
        // If user presses M, mute all playing audios
        if(Input.GetKeyDown(KeyCode.M))
        {
            foreach (var s in sources.Values)
            {
                s.volume = 0;
                s.volume = s.volume;
            }
        }
        // If user presses Up Arrow, increase volume by 5%
        else if(Input.GetKeyDown(KeyCode.UpArrow))
        {
            foreach (var s in sources.Values)
            {
                if(s.volume + 0.05f <= 1f)
                    s.volume += 0.05f;
                s.volume = s.volume;
            }
        }
        // If user presses Up Arrow, decrease volume by 5%
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            foreach (var s in sources.Values)
            {
                if (s.volume - 0.05f >= 0f)
                    s.volume -= 0.05f;
                s.volume = s.volume;
            }
        }
    }
}
