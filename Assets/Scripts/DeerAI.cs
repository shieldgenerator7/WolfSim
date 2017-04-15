using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeerAI : MonoBehaviour {

    public float speed = 0.04f;
    public GameObject wolf;

    private Vector2 direction;

	// Use this for initialization
	void Start () {
		
	}

    // Update is called once per frame
    void Update()
    {
        direction = transform.position - wolf.transform.position;
        transform.position = Vector2.MoveTowards(transform.position, ((Vector2)transform.position)+direction, speed);
    }
}
