using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class GlobalGameManager : MonoBehaviour
{
    public static GlobalGameManager Instance { get; private set; }
    public int currentLevel = 1;
    [SerializeField] PlayerData playerData;
    public PlayerData runtimePlayerData;
    [SerializeField] bool save;
    public List<Card> cardList = new List<Card>();
    public List<Card> knightCardList = new List<Card>();
    public List<Card> rangerCardList = new List<Card>();
    public List<Card> runtimeCardList = new List<Card>();
    public List<Card> usedCards;
    public float difficultyMultiplier = 1f;
    public int characterSelected = 0; // 0: none, 1: knight, 2: ranger
    public float totalPlayTime = 0f;
    [System.Serializable]
    public class Data
    {
        public int currentLevel = 1;
        public int characterSelected = 0;
        public List<int> cardId = new List<int>();
        public List<int> cardUsesRemaining = new List<int>();
        public List<int> usedCardId = new List<int>();
        public List<int> usedCardUsesRemaining = new List<int>();
        public float totalPlayTime = 0f;
    }

    private void Update()
    {
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex==1)
            totalPlayTime += Time.deltaTime;
    }
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

    public void Reset()
    {
        runtimePlayerData = Instantiate(playerData);
        currentLevel = 1;
        runtimeCardList.Clear();
        totalPlayTime = 0f;
        usedCards = new List<Card>();
        foreach (var card in cardList)
            runtimeCardList.Add(Instantiate(card));
    }
    // Start is called before the first frame update
    void Start()
    {
        Data data = SaveManager.Load<Data>("save.json");
        if (data != null && save)
        {
            currentLevel = data.currentLevel;
            characterSelected = data.characterSelected;
            totalPlayTime = data.totalPlayTime;
            Dictionary<int, Card> cards = new Dictionary<int, Card>(cardList.Count);
            for (int i = 0; i < cardList.Count; i++)
            {
                cards[i] = cardList[i];
            }
            for (int i = 0; i < data.cardId.Count; i++)
            {
                Card c = Instantiate(cards[data.cardId[i]]);
                c.usesRemaining = data.cardUsesRemaining[i];
                runtimeCardList.Add(c);
            }
            for (int i = 0; i < data.usedCardId.Count; i++)
            {
                Card c = Instantiate(cards[data.usedCardId[i]]);
                c.usesRemaining = data.usedCardUsesRemaining[i];
                usedCards.Add(c);
            }
            runtimePlayerData = SaveManager.LoadScriptableObject<PlayerData>("playerData.json");
        }
        else
        {
            runtimePlayerData = Instantiate(playerData);
            foreach (var card in cardList)
                runtimeCardList.Add(Instantiate(card));
        }
    }
    private void OnApplicationQuit()
    {
        Save();
    }
    void Save()
    {
        Data data = new Data();
        data.currentLevel = currentLevel;
        data.characterSelected = characterSelected;
        data.totalPlayTime = totalPlayTime;
        Dictionary<string, int> cardId = new Dictionary<string, int>(cardList.Count);
        for (int i = 0; i < cardList.Count; i++)
        {
            cardId[cardList[i].description] = i;
        }
        foreach (var c in runtimeCardList)
        {
            data.cardId.Add(cardId[c.description]);
            data.cardUsesRemaining.Add(c.usesRemaining);
        }
        foreach (var c in usedCards)
        {
            data.usedCardId.Add(cardId[c.description]);
            data.usedCardUsesRemaining.Add(c.usesRemaining);
        }
        SaveManager.Save(data, "save.json");
        SaveManager.Save(runtimePlayerData, "playerData.json");
    }

    public void SelectingCardList(List<Card> cardList)
    {
        runtimeCardList.Clear();
        usedCards.Clear();
        foreach (var card in cardList)
        {
            runtimeCardList.Add(Instantiate(card));
            usedCards.Add(Instantiate(card));
        }
        foreach (var c in usedCards)
        {
            c.usesRemaining = 0;
        }
    }
}
