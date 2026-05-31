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
    [System.Serializable]
    public class Data
    {
        public int currentLevel = 1;
        public int characterSelected = 0;
        public List<(int, int)> cardId = new List<(int, int)>();
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
            Dictionary<int, Card> cards = new Dictionary<int, Card>(cardList.Count);
            for (int i = 0; i < cardList.Count; i++)
            {
                cards[i] = cardList[i];
            }
            for (int i = 0; i < data.cardId.Count; i++)
            {
                Card c = Instantiate(cards[data.cardId[i].Item1]);
                c.usesRemaining = data.cardId[i].Item2;
                runtimeCardList.Add(c);
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
        Dictionary<Card, int> cardId = new Dictionary<Card, int>(cardList.Count);
        for (int i = 0; i < cardList.Count; i++)
        {
            cardId[cardList[i]] = i;
        }
        foreach (var c in runtimeCardList)
        {
            data.cardId.Add((cardId[c], c.usesRemaining));
        }
        SaveManager.Save(data, "save.json");
        SaveManager.Save(runtimePlayerData, "playerData.json");
    }

    public void SelectingCardList(List<Card> cardList)
    {
        runtimeCardList.Clear();
        foreach (var card in cardList)
            runtimeCardList.Add(Instantiate(card));
    }
}
