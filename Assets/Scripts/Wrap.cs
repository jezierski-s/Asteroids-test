using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wrap : MonoBehaviour {

    public float padding = 0.1f;

    private Vector3 position;
    private float top;
    private float bottom;
    private float left;
    private float right;
/*--------------------------------------------------------*/

    void Start () {
        InitVariables();
    }
	
	void Update () {
        WarpObject();
    }
/*--------------------------------------------------------*/

    void InitVariables() {
        top = 0.0f - padding;
        bottom = 1.0f + padding;
        left = 0.0f - padding;
        right = 1.0f + padding;
    }

    void WarpObject() {
        position = Camera.main.WorldToViewportPoint(transform.position);
        bool isOff = false;

        if (position.x < left) {
            position.x = right;
            isOff = true;
        }
        else if (position.x > right) {
            position.x = left;
            isOff = true;
        }

        if (position.y < top) {
            position.y = bottom;
            isOff = true;
        }
        else if (position.y > bottom) {
            position.y = top;
            isOff = true;
        }

        if (isOff){
            transform.position = Camera.main.ViewportToWorldPoint(position);
        }
    }
}
