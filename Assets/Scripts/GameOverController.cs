using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverController : MonoBehaviour {

    public string highscoreString = "Highscore: ";
    public string scoreString = "Your Score: ";

    private float highscore;

    private Text hsText;
    private Text scText;
/*--------------------------------------------------------*/

    void Start() {
        LoadHighscore("Score");
        InitComponents();
    }

    void Update() {
        UpdateText();
    }
/*--------------------------------------------------------*/

    void LoadHighscore(string key) {
        if (PlayerPrefs.HasKey(key)) {
            float savedScore = PlayerPrefs.GetFloat(key);
            if (GameController.GetScore() > savedScore) {
                highscore = GameController.GetScore();
                PlayerPrefs.SetFloat(key, highscore);
            }
            else {
                highscore = savedScore;
            }
        }
        else {
            highscore = GameController.GetScore();
        }
    }

    void InitComponents() {
        hsText = GameObject.Find("Canvas/Highscore").GetComponent<Text>();
        scText = GameObject.Find("Canvas/Current Score").GetComponent<Text>();
    }

    void UpdateText() {
        if (hsText != null) {
            hsText.text = highscoreString + highscore.ToString();
        }
        if (scText != null) {
            scText.text = scoreString + GameController.GetScore().ToString();
        }
    }
/*--------------------------------------------------------*/

    public void OnClick(string sceneName) {
        gameObject.GetComponent<AudioSource>().Play();
        GameController.ResetState();
        SceneManager.LoadScene(sceneName);
    }
}
