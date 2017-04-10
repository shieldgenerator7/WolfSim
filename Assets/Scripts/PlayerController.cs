using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float speed = 0.5f;//how fast the wolf can move

    public AudioSource woof;
    public AudioSource breathing;
    public AudioSource howl;

    public Vector2 targetLocation;

    public bool enroute = false;

	// Use this for initialization
	void Start () {
        targetLocation = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began))
        {
            targetLocation = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            woof.Play();
            howl.Stop();
        }
        if ((Vector2)transform.position != targetLocation)
        {
            if (!woof.isPlaying && !breathing.isPlaying)
            {
                breathing.Play();
            }
            transform.position = Vector2.MoveTowards(transform.position, targetLocation, speed);
            //if (Vector2.Distance(transform.position, targetLocation) < 0.1)
            //{
            //    transform.position = targetLocation;
            //    //Debug.Log("Equal: " + (transform.position == targetLocation));
            //}
            //else {
            enroute = true;
            //}
        }
        else if (enroute == true)
        {
            enroute = false;
            breathing.Stop();
            howl.Play();
        }
	}
}
