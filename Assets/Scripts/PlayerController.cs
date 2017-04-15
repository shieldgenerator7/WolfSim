using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float speed = 0.05f;//how fast the wolf can move

    public AudioSource woof;
    public AudioSource breathing;
    public AudioSource howl;

    public Vector2 targetLocation;
    public GameObject targetObject;//the object it moves towards

    public bool enroute = false;

	// Use this for initialization
	void Start () {
        targetLocation = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        if (targetObject != null)
        {
            targetLocation = targetObject.transform.position;
        }
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
        RaycastHit2D rch2d = Physics2D.Raycast(gpos, Vector3.zero);
        if (rch2d && rch2d.collider.gameObject.GetComponent<DeerAI>() != null)
        {
            targetObject = rch2d.collider.gameObject;
        }
        targetLocation = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        woof.Play();
        howl.Stop();
    }
}
