using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager0 : MonoBehaviour
{
    [SerializeField] Button startButton;
    [SerializeField] Button exitButton;
    AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        startButton.onClick.AddListener(() => StartGame());
        exitButton.onClick.AddListener(() => Application.Quit());
        audioSource = GetComponent<AudioSource>();
            if (audioSource != null)
                audioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void StartGame()
    {
            UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex + 1);
    }
}
