using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBullet : MonoBehaviour {

    public float fireRate = 0.25f;
    public GameObject bullet;

    private bool isReady;

    private AudioSource fireSound;
/*--------------------------------------------------------*/

    void Start () {
        InitVariables();
        InitComponents();
	}

    void Update() {
        CheckForFire();
    }
/*--------------------------------------------------------*/

    void InitVariables() {
        isReady = true;
    }

    void InitComponents() {
        fireSound = gameObject.GetComponent<AudioSource>();
    }

    void CheckForFire() {
        if (isReady && Input.GetButtonDown("Fire")) {
            if (bullet != null) {
                Instantiate(bullet, transform.position, transform.rotation);
            }
            if (fireSound != null) {
                fireSound.Play();
            }
            isReady = false;
            StartCoroutine(Reload());
        }
    }

    IEnumerator Reload() {
        yield return new WaitForSeconds(fireRate);
        isReady = true;
    }
}
