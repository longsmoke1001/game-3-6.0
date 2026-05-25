using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [field: SerializeField] public List<Enemy> Enemies { get; private set; } = new List<Enemy>();
    [SerializeField] GameObject winning;
    [SerializeField] Button nextStageButton;
    [SerializeField] Button exitButton;
    [SerializeField] Button resume;
    [SerializeField] Button exitButton2;
    [SerializeField] GameObject cards;
    [SerializeField] List<Button> cardButtons;
    [SerializeField] Button setting;
    [SerializeField] AudioSource audioSource;
    [SerializeField] List<Card> cardList;
    [SerializeField] List<Card> usedList;
    [SerializeField] TextMeshProUGUI gameOverText;
    [SerializeField] TextMeshProUGUI stageText;
    [field: SerializeField] public List<GameObject> Levels { get; private set; }
    [SerializeField] bool notLosing = false;
    [SerializeField] int enemyTotal;
    [SerializeField] int enemiesKilled;
    [SerializeField] TextMeshProUGUI enemyText;
    [SerializeField] GameObject portal;
    [SerializeField] AudioClip bossClip;
    [SerializeField] GlobalGameManager globalGameManager;
    [SerializeField] AudioClip boss2Clip;
    [SerializeField] GameObject lastLevel;
    [SerializeField] GameObject levelTen;
    [SerializeField] GameObject pause;
    [SerializeField] GameObject mobileUI;
    public List<Tuple<string, bool>> cardsList = new List<Tuple<string, bool>>();

    public void AddEnemy(Enemy enemy)
    {
        Enemies.Add(enemy);
        enemyTotal++;
        UpdateText();
    }

    public void RemoveEnemy(Enemy enemy)
    {
        Enemies.Remove(enemy);
        enemiesKilled++;
        UpdateText();
        if ((float)enemiesKilled / (float)enemyTotal > 0.799999f)
        {
            enemyText.color = new Color(0, 255, 0, 255);
            if (portal != null)
                portal.SetActive(true);
        }
    }


    void Start()
    {
#if UNITY_ANDROID || UNITY_IOS
        mobileUI.SetActive(true);
        Input.simulateMouseWithTouches = false;
#else
        mobileUI.SetActive(false);
#endif
        setting.onClick.AddListener(() =>
        {
            Time.timeScale = 1-Time.timeScale;
            pause.SetActive(!pause.activeSelf);
        });
        globalGameManager = GlobalGameManager.Instance;
        if (globalGameManager.currentLevel == 15)
            Instantiate(lastLevel);
        else if (globalGameManager.currentLevel == 10)
            Instantiate(levelTen);
        else
            Instantiate(Levels[(globalGameManager.currentLevel - 1) % Levels.Count]);
        foreach (var e in FindObjectsByType<Enemy>(FindObjectsSortMode.None))
        {
            e.Init();
        }

        if ((globalGameManager.currentLevel - 1) % Levels.Count == 3)
            audioSource.clip = bossClip;
        else if ((globalGameManager.currentLevel - 1) % Levels.Count == 4|| globalGameManager.currentLevel==15)
            audioSource.clip = boss2Clip;
        portal = FindAnyObjectByType<Portal>().gameObject;
        portal.SetActive(false);
        audioSource.Play();
        audioSource.volume = 0.2f;
        stageText.text = "Stage " + globalGameManager.currentLevel;

        Time.timeScale = 1;
        nextStageButton.onClick.AddListener(() => UnityEngine.SceneManagement.SceneManager.LoadScene(1));
        exitButton.onClick.AddListener(() => UnityEngine.SceneManagement.SceneManager.LoadScene(0));
        exitButton2.onClick.AddListener(() => UnityEngine.SceneManagement.SceneManager.LoadScene(0));
        resume.onClick.AddListener(() =>
        {
            Time.timeScale = 1;
            pause.SetActive(false);
        });
        cardList = globalGameManager.runtimeCardList;
        List<int> indexList = new List<int> { -1, -1, -1 };
        for (int i = 0; i < Mathf.Min(cardButtons.Count, cardList.Count); i++)
        {
            indexList[i] = UnityEngine.Random.Range(0, cardList.Count - i);
            for (int j = 0; j < i; j++)
                indexList[i] += indexList[i] >= indexList[j] ? 1 : 0;
        }
        Debug.Log("Card index: " + indexList[0] + " " + indexList[1] + " " + indexList[2]);
        for (int i = 0; i < Mathf.Min(cardButtons.Count, cardList.Count); i++)
        {
            Card card = cardList[indexList[i]];
            cardButtons[i].onClick.AddListener(() =>
            {
                card.Apply();
                card.usesRemaining--;
                if (card.usesRemaining <= 0)
                {
                    cardList.Remove(card);
                    globalGameManager.usedCards.Add(card);
                }
                winning.SetActive(true);
                cards.SetActive(false);
            });
            cardButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = cardList[indexList[i]].description;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Time.timeScale = 1-Time.timeScale;
            pause.SetActive(!pause.activeSelf);
        }
    }
    public void Losing()
    {
        if (!notLosing)
        {
            Debug.Log("You lose!");
            Time.timeScale = 0;
            winning.SetActive(true);
            nextStageButton.GetComponentInChildren<TextMeshProUGUI>().text = "Retry";
            gameOverText.text = "Game Over";
        }
    }
    public void Winning()
    {
        if (globalGameManager.currentLevel >= 15)
        {
            Debug.Log("You win the game!");
            Time.timeScale = 0;
            winning.SetActive(true);
            nextStageButton.gameObject.SetActive(false);
            gameOverText.text = "You win!";
            return;
        }
        Debug.Log("You win!");
        globalGameManager.currentLevel++;
        Time.timeScale = 0;
        SpawnCards();
    }

    void SpawnCards()
    {
        cards.SetActive(true);
    }
    // Update is called once per frame
    void UpdateText()
    {
        enemyText.text = "enemies killed:" + enemiesKilled + "/" + enemyTotal + "\ntarget:" + Mathf.CeilToInt((float)enemyTotal * 4 / 5);
    }
}
