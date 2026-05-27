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
    AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        resetButton.onClick.AddListener(() => { GlobalGameManager.Instance.Reset(); });
        startButton.onClick.AddListener(StartGame);
        exitButton.onClick.AddListener(QuitGame);
        settingButton.onClick.AddListener(() => { frame.SetActive(!frame.activeSelf); });
        difficultySlider.value = GlobalGameManager.Instance.difficultyMultiplier;
        difficultySlider.onValueChanged.AddListener((value) => { GlobalGameManager.Instance.difficultyMultiplier = value; });
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
        
    }
    void StartGame()
    {
            UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex + 1);
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
