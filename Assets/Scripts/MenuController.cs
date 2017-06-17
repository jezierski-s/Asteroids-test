using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour {

    private float highscore;
    private Text hsText;
/*--------------------------------------------------------*/

    void Start () {
        LoadHighscore("Score");
        InitVariables();
	}
	
	void Update () {
        UpdateText();
	}
/*--------------------------------------------------------*/

    void LoadHighscore(string key){
        if (PlayerPrefs.HasKey(key)) {
            highscore = PlayerPrefs.GetFloat(key);
        }
        else {
            highscore = 0;
            PlayerPrefs.SetFloat(key, 0f);
        }
    }

    void InitVariables() {
        hsText = GameObject.Find("Canvas/Highscore").GetComponent<Text>();
    }

    void UpdateText() {
        hsText.text = "Highscore: " + highscore.ToString();
    }
/*--------------------------------------------------------*/

    public void OnClick(string sceneName) {
        gameObject.GetComponent<AudioSource>().Play();
        SceneManager.LoadScene(sceneName);
    }
}
