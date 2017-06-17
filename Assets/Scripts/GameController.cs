using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]
public class GameController : MonoBehaviour {

    public float offscreenPadding = 0.1f;
    public int startAsteroidsNumber = 2;
    public int asteroidsPerLevel = 1;
    public Rock asteroidObject;
    public string levelString = "Level: ";
    public float timeToStart = 2f;
    public float timeToRestart = 2f;
    public float startMusicTempo = 2f;
    public float finalMusicTempo = 0.5f;
    public float musicTempoChange = 0.05f;
    public static int startScore = 0;
    public static int startLifes = 3;
    public static int startLevel = 1;

    private static int score = startScore;
    private static int lifes = startLifes;
    private static int level = startLevel;

    private int asteroidsNumber;
    private int currentBeat = 0;
    private float tempo;
    private bool isGameRunning = false;

    private Text scoreText;
    private Text lifesText;
    private Text levelText;
    private AudioSource[] beats;
    private GameObject[] obstacles;
/*--------------------------------------------------------*/

    void Start () {
        InitVariables();
        InitComponents();
        StartCoroutine(CreateAsteroids());
        StartCoroutine(PlayMusic());
	}
	
	void Update () {
        CheckIfBeaten();
        UpdateText();
	}
/*--------------------------------------------------------*/

    void InitVariables() {
        asteroidsNumber = startAsteroidsNumber + (level - 1) * asteroidsPerLevel;
        tempo = startMusicTempo;
    }

    void InitComponents() {
        scoreText = (Text) GameObject.Find("Canvas/Score").GetComponent<Text>();
        lifesText = (Text) GameObject.Find("Canvas/Lifes").GetComponent<Text>();
        levelText = (Text) GameObject.Find("Canvas/Level").GetComponent<Text>();
        beats = gameObject.GetComponents<AudioSource>();
        for (int i = beats.Length; i < 2; i++) {
            beats[i] = null;
        }
    }

    void CheckIfBeaten() {
        if (isGameRunning) {
            obstacles = GameObject.FindGameObjectsWithTag("Obstacle");
            if (obstacles.Length == 0) {
                level++;
                isGameRunning = false;
                StartCoroutine(GameRestart());
            }
        }
    }

    void UpdateText() {
        if (scoreText != null) { 
            scoreText.text = score.ToString();
        }
        if (lifesText != null) {
            lifesText.text = "x" + lifes.ToString();
        }
        if (levelText != null) {
            levelText.text = levelString + level.ToString();
        }
    }

    void CreateAsteroid() {      
            float posX = 0.0f;
            float posY = 0.0f;
            int startingSide = Random.Range(0, 4);
            switch (startingSide) {
                case 0: // top
                    posX = Random.value;
                    posY = 0.0f;
                    posY -= offscreenPadding;
                    break;
                case 1: // bottom
                    posX = Random.value;
                    posY = 1.0f;
                    posY += offscreenPadding;
                break;     
                case 2: // left
                    posX = 0.0f;
                    posY = Random.value;
                    posX -= offscreenPadding;
                break;
                case 3: // right
                    posX = 1.0f;
                    posY = Random.value;
                    posX += offscreenPadding;
                break;
            }
            Vector2 vector = Camera.main.ViewportToWorldPoint(new Vector2(posX, posY));
            if (asteroidObject != null) {
                Instantiate(asteroidObject, vector, Quaternion.identity);
            }
    }

    IEnumerator CreateAsteroids() {
        yield return new WaitForSeconds(timeToStart);
        for (int i = 0; i < asteroidsNumber; i++) {
            CreateAsteroid();
        }
        isGameRunning = true;
    }

    IEnumerator PlayMusic() {
        yield return new WaitForSeconds(tempo);
        if (beats[currentBeat] != null) {
            beats[currentBeat].Play();
        }
        StartCoroutine(PlayMusic());
        currentBeat = currentBeat == 0 ? 1 : 0;
        if (tempo > finalMusicTempo) {
            tempo -= musicTempoChange;
        }
    }
/*--------------------------------------------------------*/

    public static void PlaySoundOnDestroy(AudioSource audio)  {
        AudioSource.PlayClipAtPoint(audio.clip, Camera.main.transform.position);
    }

    public static void PlayParticlesOnDestroy(ParticleSystem system) {
        system.transform.parent = null;
        system.Play();
        Destroy(system.gameObject, system.main.duration);
    }

    public static void ResetState() {
        score = startScore;
        lifes = startLifes;
        level = startLevel;
    }

    public static int GetScore() {
        return score;
    }

    public static void AddScore(int toAdd) {
        score += toAdd;
    }

    public static void LoseLife() {
        lifes--;
    }

    public IEnumerator GameRestart() {
        yield return new WaitForSeconds(2f);
        if (lifes > 0) {
            SceneManager.LoadScene("Main");
        }
        else {
            SceneManager.LoadScene("GameOver");
        }
    }
}
