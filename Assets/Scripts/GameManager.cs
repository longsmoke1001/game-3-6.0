using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public List<Enemy> Enemies { get; private set; } = new List<Enemy>();
    [SerializeField] GameObject winning;
    [SerializeField] Button nextStageButton;
    [SerializeField] Button exitButton;
    [SerializeField] GameObject cards;
    [SerializeField] List<Button> cardButtons;
    [SerializeField] AudioSource audioSource;
    [SerializeField] List<Card> cardList;
    [SerializeField] List<Card> usedList;
    [SerializeField] TextMeshProUGUI gameOverText;
    [SerializeField] TextMeshProUGUI stageText;
    [SerializeField] List<GameObject> levels;
    [SerializeField] bool notLosing = false;
    [SerializeField] int enemyTotal;
    [SerializeField] int enemiesKilled;
    [SerializeField] TextMeshProUGUI enemyText;
    [SerializeField] GameObject portal;
    [SerializeField] AudioClip bossClip;
    [SerializeField] GlobalGameManager globalGameManager;
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
            portal.SetActive(true);
        }
    }
    void Start()
    {
        globalGameManager = GlobalGameManager.instance;
        Instantiate(levels[(globalGameManager.currentLevel-1)%4]);
        if ((globalGameManager.currentLevel - 1) % 4 == 3)
            audioSource.clip = bossClip;
        portal = FindAnyObjectByType<Portal>().gameObject;
        portal.SetActive(false);
        audioSource.Play();
        audioSource.volume = 0.2f;
        stageText.text = "Stage " + globalGameManager.currentLevel;

        Time.timeScale = 1;
        nextStageButton.onClick.AddListener(() => UnityEngine.SceneManagement.SceneManager.LoadScene(1));
        exitButton.onClick.AddListener(() => UnityEngine.SceneManagement.SceneManager.LoadScene(0));
        cardList = globalGameManager.runtimeCardList;
        List<int> indexList = new List<int> { -1, -1, -1 };
        for (int i = 0; i < Mathf.Min(cardButtons.Count,cardList.Count); i++) { 
            indexList[i] = UnityEngine.Random.Range(0, cardList.Count-i);
            for (int j = 0; j < i; j++)
                indexList[i] += indexList[i] >= indexList[j] ? 1 : 0;
        }
        Debug.Log("Card index: " + indexList[0] + " " + indexList[1] + " " + indexList[2]);
        for (int i=0;i< Mathf.Min(cardButtons.Count, cardList.Count); i++)
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
        Debug.Log("You win!");
        GlobalGameManager.instance.currentLevel++;
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
        enemyText.text = "killed enemy" + enemiesKilled + "/" + enemyTotal+"\ntarget:"+Mathf.CeilToInt((float)enemyTotal*4/5);
    }
}
