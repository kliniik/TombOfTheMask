using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;

public class GameManager : Singleton<GameManager>
{
    private TextMeshProUGUI textUI;

    [SerializeField]
    private AudioSource musicSource;

    [SerializeField]
    private AudioClip backgroundMusic;

    private int totalPoints = 0;
    private int maxPoints = 0; // max in etween games
    private int heightReached = 0;
    private int maxHeight = 0; // max in between games


    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this);

        textUI = GetComponentInChildren<TextMeshProUGUI>();
        UpdateUI();

        if (musicSource == null)
        {
            Debug.LogError("ERROR: Music AudioSource not found.");
        }
        else
        {
            musicSource.clip = backgroundMusic;
            musicSource.loop = true; // we loop the music
            musicSource.Play();
        }

    }

    public void AddPoints(int points)
    {
        totalPoints += points;
        //text.text = totalPoints.ToString();

        if (totalPoints > maxPoints)
        {
            maxPoints = totalPoints;
            //UpdateUI();
        }

        UpdateUI();
    }

    public void AddHeighReached(int currentHeight)
    {
        heightReached = currentHeight; 

        if (currentHeight > maxHeight)
        {
            maxHeight = currentHeight; 
        }

        UpdateUI();
    }

    // if the player collides with spikes or water, we restart the game
    public void GameOver()
    {
        //Debug.Log($"GAME OVER!\nYour earned {totalPoints} points.\nYou reached {heightReached} height.");
        Debug.Log($"Your earned {totalPoints} points.");
        Debug.Log($"You reached {heightReached} height.");   

        // restart the game
        StartCoroutine(WaitForRestart());
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        totalPoints = 0;
        heightReached = 0;
        UpdateUI();
    }

    // function to update the displayed text
    public void UpdateUI()
    {
        if (textUI != null)
        {
            textUI.text = "Points: " + totalPoints + "\nHeight: " + heightReached + "\n\nMax Points: " + maxPoints + "\nMax Height: " + maxHeight ;
        }
    }

    // coroutine to wait a few seconds before restarting the game
    private IEnumerator WaitForRestart()
    {
        Debug.Log("The game will restart in 5 seconds...");
        yield return new WaitForSeconds(5);
    }

    // not working
    public void QuitGame()
    {
        Debug.Log("QuitGame method called!");

        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit(); 
        #endif
    }
}
