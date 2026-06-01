using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{
    public Button attackButton;
    public Button skillButton;
    Player player;
    public Joystick joystick;
    [SerializeField] GlobalGameManager globalGameManager;
    [SerializeField] TextMeshProUGUI timeText;
    [SerializeField] Sprite attackCooldownImage;
    [SerializeField] Sprite skillCooldownImage;
    [SerializeField] Image attackCooldownImageComponent;
    [SerializeField] Image skillCooldownImageComponent;
    [SerializeField] Button showUsedCardsButton;
    [SerializeField] GameObject mainUI;
    //buffUI
    [SerializeField] TextMeshProUGUI usedCardsText;
    [SerializeField] GameObject Buffs;
    [SerializeField] Button closeBuffsButton;
    //tutorial UI
    [SerializeField] GameObject tutorial;
    [SerializeField] TextMeshProUGUI tutorialText;
    [SerializeField] Button okButton;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        globalGameManager = GlobalGameManager.Instance;
        player = FindAnyObjectByType<Player>();
        if (globalGameManager.currentLevel==1)
        {
            mainUI.SetActive(false);
            Time.timeScale = 0;
            tutorial.SetActive(true);
#if UNITY_ANDROID
            tutorialText.text = "Welcome to the game! Use the joystick to move around and avoid enemies. Tap the attack button to attack and the skill button to use your skill. Good luck!";
#else
            tutorialText.text = "Welcome to the game! Use WASD or arrow keys to move around and avoid enemies. Left Click to attack. Right click to use your skill. Good luck!";
#endif
            if (globalGameManager.characterSelected==0)
                tutorialText.text += "\nAs a Knight, your skill block enemy attacks and reflect damage base on your attack power.";
            else
                 tutorialText.text += "\nAs a Ranger, your attack gives you stacks. Your skill consumes your stack to deal damage";
            okButton.onClick.AddListener(() => { tutorial.SetActive(false); Time.timeScale = 1; mainUI.SetActive(true); });
        }
        attackCooldownImage = player.attackCooldownImage;
        skillCooldownImage = player.skillCooldownImage;
        showUsedCardsButton.onClick.AddListener(() =>
        {
            Buffs.SetActive(true);
            usedCardsText.text = "";
            for(int i=0;i< globalGameManager.usedCards.Count;i++)
            {
                if (globalGameManager.usedCards[i].usesRemaining != 0)
                    usedCardsText.text += $"{globalGameManager.usedCards[i].description} x{globalGameManager.usedCards[i].usesRemaining}\n";
            }
        });
        closeBuffsButton.onClick.AddListener(() => { Buffs.SetActive(false); });
        Debug.Log(attackCooldownImage);
        Debug.Log(skillCooldownImage);
        if (attackCooldownImage != null)
        {
            attackButton.GetComponent<Image>().sprite = attackCooldownImage;
            attackCooldownImageComponent.sprite = attackCooldownImage;
        }
        if (skillCooldownImage != null)
        {
            skillButton.GetComponent<Image>().sprite = skillCooldownImage;
            skillCooldownImageComponent.sprite = skillCooldownImage;
        }
    }

    // Update is called once per frame
    void Update()
    {
        attackCooldownImageComponent.fillAmount = player.attackButtonFill;
        skillCooldownImageComponent.fillAmount = player.skillButtonFill;
        timeText.text = $"Time:{(int)globalGameManager.totalPlayTime / 60}:{(int)globalGameManager.totalPlayTime % 60:D2}";
    }
}
