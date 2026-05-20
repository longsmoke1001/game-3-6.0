using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalGameManager : MonoBehaviour
{
    public static GlobalGameManager instance;
    public int currentLevel = 1;
    [SerializeField] PlayerData playerData;
    public List<Card> cardList;
    public List<Card> runtimeCardList;
    public List<Card> usedCards;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
        playerData.Reset();
        foreach (var card in cardList)
            runtimeCardList.Add(Instantiate(card));
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
