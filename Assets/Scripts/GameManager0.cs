using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class GameManager0 : MonoBehaviour
{
    [SerializeField] Button startButton;
    [SerializeField] Button exitButton;
    [SerializeField] Button resetButton;
    [SerializeField] GameObject frame;
    [SerializeField] Button settingButton;
    [SerializeField] Slider difficultySlider;
    [SerializeField] GameObject difficultySelection;
    [SerializeField] Slider difficultySlider2;
    [SerializeField] Button confirmButton;
    [SerializeField] GameObject mainUI;
    [SerializeField] Button confirmButtonAtSetting;
    [SerializeField] GameObject areYouSureUI;
    [SerializeField] Button yesButton;
    [SerializeField] Button noButton;
    [SerializeField] GameObject dataDeletedUI;
    [SerializeField] Button dataDeletedUIConFirmButton;
    AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        //mainUI
        startButton.onClick.AddListener(StartButton);
        exitButton.onClick.AddListener(QuitGame);
        settingButton.onClick.AddListener(() => { frame.SetActive(true); mainUI.SetActive(false); });
        //settingUI
        confirmButton.onClick.AddListener(StartGame);
        confirmButtonAtSetting.onClick.AddListener(() => { frame.SetActive(false); mainUI.SetActive(true); });
        resetButton.onClick.AddListener(() => { areYouSureUI.SetActive(true); });
        difficultySlider.value = GlobalGameManager.Instance.difficultyMultiplier;
        difficultySlider.onValueChanged.AddListener((value) => { GlobalGameManager.Instance.difficultyMultiplier = value; });
        //areYouSureUI
        yesButton.onClick.AddListener(() => { areYouSureUI.SetActive(false); GlobalGameManager.Instance.Reset(); dataDeletedUI.SetActive(true); });
        noButton.onClick.AddListener(() => { areYouSureUI.SetActive(false); });
        //dataDeletedUI
        dataDeletedUIConFirmButton.onClick.AddListener(() => { dataDeletedUI.SetActive(false); });
        //difficulySelectionUI
        difficultySlider2.value = GlobalGameManager.Instance.difficultyMultiplier;
        difficultySlider2.onValueChanged.AddListener((value) => { GlobalGameManager.Instance.difficultyMultiplier = value; });
        //audio
        audioSource = GetComponent<AudioSource>();
        if (audioSource != null)
            audioSource.Play();
#if UNITY_WEBGL
        exitButton.gameObject.SetActive(false);
        startButton.gameObject.transform.localPosition = new Vector3(0, startButton.gameObject.transform.localPosition.y, 0);
#endif
    }

    // Update is called once per frame
    void Update()
    {
        difficultySlider.value = GlobalGameManager.Instance.difficultyMultiplier;
        difficultySlider2.value = GlobalGameManager.Instance.difficultyMultiplier;
    }
    void StartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex + 1);
    }

    void StartButton()
    {
        if (GlobalGameManager.Instance.currentLevel == 1)
        {
            difficultySelection.SetActive(true);
            mainUI.SetActive(false);
        }
        else
            StartGame();
    }

public static void QuitGame()
    {
        Debug.Log("Quit requested");
#if UNITY_EDITOR
        // 在 Editor 停止 Play 模式，方便測試
        EditorApplication.isPlaying = false;
#else
        // 正常關閉遊戲
        Application.Quit();

        // 某些平台或特殊情況下可作為保險 fallback（Windows/mac standalone）
        // 注意：System.Environment.Exit 強制結束進程，僅在你確定要這麼做時使用
        System.Environment.Exit(0);
#endif
    }
}
