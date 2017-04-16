using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float speed = 0.05f;//how fast the wolf can move
    public float walkSpeed = 0.05f;//walk
    public float runSpeed = 0.1f;//run
    public float chaseThreshold = 5.0f;//how close the wolf has to be to run after target
    public GameObject markerObject;//the object used to mark the wolf's current target

    public AudioSource woof;
    public AudioSource breathing;
    public AudioSource howl;

    public Vector2 targetLocation;
    public GameObject targetObject;//the object it moves towards

    public bool enroute = false;

    private SpriteRenderer sr;

	// Use this for initialization
	void Start () {
        setTargetLocation(transform.position);
        sr = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        speed = walkSpeed;
        if (targetObject != null)
        {
            setTargetLocation(targetObject.transform.position);
            if (Vector2.Distance(transform.position, targetObject.transform.position) <= chaseThreshold)
            {
                speed = runSpeed;
            }
        }
        if ((Vector2)transform.position != targetLocation)
        {
            if (!woof.isPlaying && !breathing.isPlaying)
            {
                breathing.Play();
            }
            float terrainMultiplier = LevelManager.getTile(transform.position).terrainSpeedMultiplier;
            transform.position = Vector2.MoveTowards(transform.position, targetLocation, speed* terrainMultiplier);
            enroute = true;
        }
        else if (enroute == true)
        {
            enroute = false;
            breathing.Stop();
            howl.Play();
            targetObject = null;
        }
        sr.sortingOrder = LevelManager.getDisplaySortingOrder(transform.position);
	}

    public void setTargetLocation(Vector2 pos)
    {
        targetLocation = pos;
        markerObject.transform.position = pos;
    }

    public void processTapGesture(Vector3 gpos)
    {
        RaycastHit2D rch2d = Physics2D.Raycast(gpos, Vector3.zero);
        if (rch2d && rch2d.collider.gameObject.GetComponent<DeerAI>() != null)
        {
            targetObject = rch2d.collider.gameObject;
        }
        else
        {
            targetObject = null;
        }
        setTargetLocation(gpos);
        woof.Play();
        howl.Stop();
    }
}
