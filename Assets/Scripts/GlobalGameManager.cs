using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GlobalGameManager : MonoBehaviour
{
    public static GlobalGameManager Instance { get; private set; }
    public int currentLevel = 1;
    [SerializeField] PlayerData playerData;
    public List<Card> cardList=new List<Card>();
    public List<Card> runtimeCardList=new List<Card>();
    public List<Card> usedCards;
    [System.Serializable]
    public class Data
    {
        public int currentLevel = 1;
        public List<int> cardId=new List<int>();
        public List<int> usesRemaining = new List<int>();
        public float maxHealth = 20f;
        public float movSpeed = 5f;
        public float dashCooldown = 1f;
        public float attackPower = 5f;
        public float attackRange = 2f;
        public float dashDuration = 0.1f;
        public float dashSpeed = 20f;
        public float defendDuration = 0.5f;
        public float defendCooldown = 2f;
        public float attackSpeed = 0.5f;
        public float attackdelay = 0.2f;
        public float healthOnHit = 0f;
        public float reflectedDamageMultiplier = 1f;
        public bool canMoveWhileAttacking = false;
        public bool canMoveWhileDefending = false;
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
        playerData.Reset();
        currentLevel = 1;
        runtimeCardList.Clear();
        foreach (var card in cardList)
            runtimeCardList.Add(Instantiate(card));
    }
    // Start is called before the first frame update
    void Start()
    {
        Data data = SaveManager.Load<Data>("save.json");
        if (data!=null)
        {
            currentLevel = data.currentLevel;
            Debug.Log(data.cardId[0] - 1);
            for (int i=0;i< data.cardId.Count; i++)
            {
                Card c = Instantiate(cardList[data.cardId[i] - 1]);
                c.usesRemaining = data.usesRemaining[i];
                runtimeCardList.Add(c);
            }
            playerData.maxHealth = data.maxHealth;
            playerData.movSpeed = data.movSpeed;
            playerData.dashCooldown = data.dashCooldown;
            playerData.attackPower = data.attackPower;
            playerData.attackRange = data.attackRange;
            playerData.dashDuration = data.dashDuration;
            playerData.dashSpeed = data.dashSpeed;
            playerData.defendDuration = data.defendDuration;
            playerData.defendCooldown = data.defendCooldown;
            playerData.attackSpeed = data.attackSpeed;
            playerData.attackdelay = data.attackdelay;
            playerData.healthOnHit = data.healthOnHit;
            playerData.reflectedDamageMultiplier = data.reflectedDamageMultiplier;
            playerData.canMoveWhileAttacking = data.canMoveWhileAttacking;
            playerData.canMoveWhileDefending = data.canMoveWhileDefending;
        }
        else
        {
            playerData.Reset();
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
        foreach(var c in runtimeCardList)
        {
            data.cardId.Add(c.id);
            data.usesRemaining.Add(c.usesRemaining);
        }
        data.maxHealth = playerData.maxHealth;
        data.movSpeed = playerData.movSpeed;
        data.dashCooldown = playerData.dashCooldown;
        data.attackPower = playerData.attackPower;
        data.attackRange = playerData.attackRange;
        data.dashDuration = playerData.dashDuration;
        data.dashSpeed = playerData.dashSpeed;
        data.defendDuration = playerData.defendDuration;
        data.defendCooldown = playerData.defendCooldown;
        data.attackSpeed = playerData.attackSpeed;
        data.attackdelay = playerData.attackdelay;
        data.healthOnHit = playerData.healthOnHit;
        data.reflectedDamageMultiplier = playerData.reflectedDamageMultiplier;
        data.canMoveWhileAttacking = playerData.canMoveWhileAttacking;
        data.canMoveWhileDefending = playerData.canMoveWhileDefending;
        SaveManager.Save(data, "save.json");
    }
}
