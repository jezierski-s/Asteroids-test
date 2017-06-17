using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Ship : MonoBehaviour {

    public float maxVelocity = 5.0f;
    public float rotationSpeed = 250.0f;
    public float friction = 0.95f;
    public float acceleration = 5.0f;
    public float engineTreshold = 0.1f;
    public Sprite engineOnSprite;
    public Sprite engineOffSprite;

    private Vector3 velocity;
    private Vector3 clampedVelocity;

    private GameController controller;
    private AudioSource engineSound;
    private AudioSource explosionSound;
    private ParticleSystem particle;
/*--------------------------------------------------------*/

    void Start() {
        InitVariables();
        InitComponents();
    }

    void Update() {
        ShipMovement();
    }
/*--------------------------------------------------------*/

    void OnCollisionEnter2D(Collision2D coll) {
        if (coll.gameObject.tag == "Obstacle") {
            CrashWithAsteroid();      
        }
    }

    void InitVariables() {
        velocity = new Vector3(0, 0, 0);
        clampedVelocity = new Vector3(0, 0, 0);
    }

    void InitComponents() {
        engineSound = transform.Find("Engine").GetComponent<AudioSource>();
        explosionSound = transform.Find("Explosion").GetComponent<AudioSource>();
        particle = transform.Find("Particle System").GetComponent<ParticleSystem>();
        controller = GameObject.FindWithTag("GameController").GetComponent<GameController>();
    }

    void ShipMovement() {
        float inputX = Input.GetAxis("Horizontal");
        float inputY = Mathf.Clamp(Input.GetAxis("Vertical"), 0, 1);

        transform.Rotate(new Vector3(0, 0, -inputX), rotationSpeed * Time.deltaTime);
        velocity += (inputY * (transform.up * acceleration)) * Time.deltaTime;
        if (inputY < engineTreshold) {
            velocity *= friction;
            GetComponent<SpriteRenderer>().sprite = engineOffSprite;
        }
        else {
            GetComponent<SpriteRenderer>().sprite = engineOnSprite;
            if (engineSound != null && !engineSound.isPlaying) {
                engineSound.Play();
            }
        }
        clampedVelocity = Vector3.ClampMagnitude(velocity, maxVelocity);
        transform.Translate(clampedVelocity * Time.deltaTime, Space.World);
    }

    void CrashWithAsteroid() {
        GameController.LoseLife();
        if (controller != null) {
            controller.StartCoroutine(controller.GameRestart());
        }
        if (explosionSound != null) {
            GameController.PlaySoundOnDestroy(explosionSound);
        }
        if (particle != null) {
            GameController.PlayParticlesOnDestroy(particle);
        }
        Destroy(gameObject);
    }

}