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
        if ((Vector2)transform.position != targetLocation)
        {
            if (!woof.isPlaying && !breathing.isPlaying)
            {
                breathing.Play();
            }
            transform.position = Vector2.MoveTowards(transform.position, targetLocation, speed);
            enroute = true;
        }
        else if (enroute == true)
        {
            enroute = false;
            breathing.Stop();
            howl.Play();
        }
	}
    public void processTapGesture(Vector3 gpos)
    {
        targetLocation = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        woof.Play();
        howl.Stop();
    }
}
