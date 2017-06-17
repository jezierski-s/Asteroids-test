using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    public float speed = 10;
    public float lifeSpan = 2;
/*--------------------------------------------------------*/

    void Start () {
        StartCoroutine(EndMyLife());
	}
	
	void Update () {
        MoveForward();
	}
/*--------------------------------------------------------*/

    void MoveForward() {
        transform.Translate(transform.up * speed * Time.deltaTime, Space.World);
    }

    IEnumerator EndMyLife() {
        yield return new WaitForSeconds(lifeSpan);
        Destroy(gameObject);
    }
}
