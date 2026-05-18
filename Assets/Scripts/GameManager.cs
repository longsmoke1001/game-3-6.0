using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }
    public List<Enemy> enemies { get; private set; } = new List<Enemy>();
    [SerializeField] GameObject winning;
    [SerializeField] Button nextStageButton;
    [SerializeField] Button exitButton;
    [SerializeField] GameObject cards;
    [SerializeField] List<Button> cardButtons;
    [SerializeField] AudioSource audioSource;
    [SerializeField] List<Card> cardList;
    List<Card> usedList;
    [SerializeField] TextMeshProUGUI gameOverText;
    [SerializeField] TextMeshProUGUI stageText;
    [SerializeField] List<GameObject> levels;
    public List<Tuple<string, bool>> cardsList = new List<Tuple<string, bool>>();

    public void AddEnemy(Enemy enemy)
    {
        enemies.Add(enemy);
    }

    public void RemoveEnemy(Enemy enemy)
    {
        enemies.Remove(enemy);
    }
    void Start()
    {
        Instantiate(levels[GlobalGameManager.instance.currentLevel-1]);
        audioSource.Play();
        audioSource.volume = 0.2f;
        stageText.text = "Stage " + GlobalGameManager.instance.currentLevel;
        Time.timeScale = 1;
        nextStageButton.onClick.AddListener(() => UnityEngine.SceneManagement.SceneManager.LoadScene(1));
        exitButton.onClick.AddListener(() => UnityEngine.SceneManagement.SceneManager.LoadScene(0));
        List<int> indexList = new List<int> { -1, -1, -1 };
        for (int i = 0; i < cardButtons.Count; i++) { 
            indexList[i] = UnityEngine.Random.Range(0, cardList.Count-i);
            for (int j = 0; j < i; j++)
                indexList[i] += indexList[i] >= indexList[j] ? 1 : 0;
        }
        Debug.Log("Card index: " + indexList[0] + " " + indexList[1] + " " + indexList[2]);
        for (int i=0;i<cardButtons.Count;i++)
        {
            int x = i;
            cardButtons[x].onClick.AddListener(() =>
            {
                cardList[indexList[x]].Apply();
                winning.SetActive(true);
                cards.SetActive(false);
            });
            cardButtons[x].GetComponentInChildren<TextMeshProUGUI>().text = cardList[indexList[x]].description;
        }
    }

    public void Losing()
    {
        Debug.Log("You lose!");
        Time.timeScale = 0;
        winning.SetActive(true);
        nextStageButton.GetComponentInChildren<TextMeshProUGUI>().text = "Retry";
        gameOverText.text="Game Over";
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
    void Update()
    {

    }
}
