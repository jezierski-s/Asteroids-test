using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Rock : MonoBehaviour {

    public Sprite[] rockSprites;
    public float speed = 2;
    public Rock smallerRock;
    public int piecesNumber = 2;
    public int pointsNumber = 100;

    private Vector2 direction;
    private AudioSource explosionSound;
    private ParticleSystem particle;
/*--------------------------------------------------------*/

    void Start () {
        SetRandomSprite();
        InitVariables();
        InitComponents();
    }
	
	void Update () {
        MoveStraight();
    }

    void OnCollisionEnter2D(Collision2D coll) {
        if (coll.gameObject.tag == "Bullet") {
            CrashWithBullet();
            Destroy(coll.gameObject);
        }
    }
/*--------------------------------------------------------*/

    void SetRandomSprite() {
        if (rockSprites.Length > 0) {
            int spriteIndex = Random.Range(0, rockSprites.Length - 1);
            GetComponent<SpriteRenderer>().sprite = rockSprites[spriteIndex];
        }
    }

    void InitVariables() {
        direction = new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f));
    }

    void InitComponents() {
        explosionSound = gameObject.GetComponent<AudioSource>();
        particle = transform.Find("Particle System").GetComponent<ParticleSystem>();
    }

    void MoveStraight() {
        transform.Translate(direction * speed * Time.deltaTime, Space.World);
    }

    void CrashWithBullet() {
        BreakToPieces();
        GameController.AddScore(pointsNumber);
        if (explosionSound != null) { 
            GameController.PlaySoundOnDestroy(explosionSound);
        }
        if (particle != null) {
            GameController.PlayParticlesOnDestroy(particle);
        }
        Destroy(gameObject);
    }

    void BreakToPieces() {
        if (smallerRock != null) {
            for (int i = 0; i < piecesNumber; i++) {
                Instantiate(smallerRock, transform.position, transform.rotation);
            }
        }
    }
}
