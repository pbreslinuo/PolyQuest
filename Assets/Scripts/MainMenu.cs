using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Text bestTimeText;
    public Text bestTimeTextTutorial;
    public Text bestTimeTextMedium;
    public Text bestTimeTextHard;
    public Text currentTimeText;
    public GameObject newRecord;
    public GameObject scoreCanvas;
    public PlayerController player;
    public Transform[] children;

    private double bestMin;
    private double bestSec;
    private double currentMin;
    private double currentSec;
    private double bestMinTutorial;
    private double bestSecTutorial;
    private double bestMinMedium;
    private double bestSecMedium;
    private double bestMinHard;
    private double bestSecHard;
    private bool firstPassFinish;

    public AudioSource source;
    public AudioClip hover;
    public AudioClip click;

    private float m_Timer;
    public GameObject dieCanvas;
    private CanvasGroup dieCanvasGroup;
    public GameObject deathText; // text child of dieCanvas
    public GameObject backgroundMusic;

    void OnEnable()
    {
        DontDestroyOnLoad(scoreCanvas);
        DontDestroyOnLoad(this);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void Start()
    {
        SceneManager.LoadScene("MainMenu");
        children = gameObject.GetComponentsInChildren<Transform>();
        dieCanvasGroup = dieCanvas.GetComponent<CanvasGroup>();

        bestMinTutorial = 2;
        bestSecTutorial = 0;
        bestMinMedium = 5;
        bestSecMedium = 0;
        bestMinHard = 10;
        bestSecHard = 0;
        firstPassFinish = true;
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        if (Input.GetKey(KeyCode.M))
        {
            SceneManager.LoadScene("MainMenu");
        }
        if (player != null)
        {
            if (player.died == true)
            {
                dieCanvas.SetActive(true);
                deathText.SetActive(true);
                backgroundMusic.SetActive(false);
                PlayerDied();
            }
            else if (player.finished == true)
            {
                if (firstPassFinish)
                { // do these once on level finish
                    backgroundMusic.SetActive(false);
                    LevelFinished();
                    firstPassFinish = false;
                }
            }
        }
    }

    public void HoverSound()
    {
        source.PlayOneShot(hover, 2);
    }

    public void ClickSound()
    {
        source.PlayOneShot(click);
    }

    void UpdateTimer()
    {
        currentSec = (currentSec + 1) % 60;
        if (currentSec == 0)
        {
            currentMin += 1;
        }
        currentTimeText.text = "Current time:  " + currentMin.ToString() + "m " + (currentSec).ToString() + "s";
        if (currentMin == 60d)
        {
            currentTimeText.text = "Current time:  >1hr";
            CancelInvoke();
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        m_Timer = 0;
        newRecord.SetActive(false);
        CancelInvoke();
        ResetTime();
        InvokeRepeating("UpdateTimer", 0.0f, 1.0f);
        if (scene.name == "MainMenu")
        {
            scoreCanvas.SetActive(false);
            gameObject.SetActive(true);
            foreach (Transform child in children)
            {
                child.gameObject.SetActive(true);
            }
            bestTimeTextTutorial.text = "Best time:  " + bestMinTutorial.ToString() + "m " + bestSecTutorial.ToString() + "s";
            bestTimeTextMedium.text = "Best time:  " + bestMinMedium.ToString() + "m " + bestSecMedium.ToString() + "s";
            bestTimeTextHard.text = "Best time:  " + bestMinHard.ToString() + "m " + bestSecHard.ToString() + "s";
        }
        else if (!(scene.name == "StartingScene"))
        { // any playable level
            player = GameObject.FindWithTag("Player").GetComponent<PlayerController>(); // get player
            player.finished = false;
            firstPassFinish = true;
            backgroundMusic = GameObject.FindWithTag("BackgroundMusic");
            scoreCanvas.SetActive(true);
            foreach (Transform child in children.Skip(2))
            {
                child.gameObject.SetActive(false);
            }
            
            switch (scene.name)
            {
                case "TutorialLevel":
                    bestMin = bestMinTutorial;
                    bestSec = bestSecTutorial;
                    break;
                case "MediumLevel":
                    bestMin = bestMinMedium;
                    bestSec = bestSecMedium;
                    break;
                case "testbed":
                    bestMin = bestMinHard;
                    bestSec = bestSecHard;
                    break;
                default:
                    break;
            }
            bestTimeText.text = "Best time:       " + bestMin.ToString() + "m " + bestSec.ToString() + "s";
        }
    }

    public void LevelFinished()
    {
        if (currentMin < bestMin || (currentMin == bestMin && currentSec < bestSec))
        {
            switch (SceneManager.GetActiveScene().name)
            {
                case "TutorialLevel":
                    bestMinTutorial = currentMin;
                    bestSecTutorial = currentSec;
                    break;
                case "MediumLevel":
                    bestMinMedium = currentMin;
                    bestSecMedium = currentSec;
                    break;
                case "testbed":
                    bestMinHard = currentMin;
                    bestSecHard = currentSec;
                    break;
                default:
                    break;
            }
            bestTimeText.text = "Best time:       " + currentMin.ToString() + "m " + currentSec.ToString() + "s";
            newRecord.SetActive(true);
        }
        CancelInvoke();
    }

    public void PlayerDied()
    {
        m_Timer += Time.deltaTime;
        dieCanvasGroup.alpha = m_Timer / 3;

        if (m_Timer > 4)
        {
            player.died = false;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    public void ResetTime()
    {
        currentMin = 0;
        currentSec = 0;
    }

    public void SelectScene(string levelName)
    {
        SceneManager.LoadScene(levelName);
    }

    public void Quit()
    {
        Application.Quit();
    }
}