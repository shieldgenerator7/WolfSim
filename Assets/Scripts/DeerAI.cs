using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeerAI : MonoBehaviour {

    public float speed = 0.04f;
    public float walkSpeed = 0.02f;
    public float runSpeed = 0.04f;
    public GameObject wolf;
    public float safeThreshold = 7.0f;//the deer is safe if the wolf is at least this far away

    private Vector2 direction;

	// Use this for initialization
	void Start () {
		
	}

    // Update is called once per frame
    void Update()
    {
        speed = walkSpeed;
        if (Vector3.Distance(transform.position, wolf.transform.position) <= safeThreshold)
        {
            direction = transform.position - wolf.transform.position;
            speed = runSpeed;
        }
        else
        {
            direction = direction + new Vector2(Random.Range(-10, 10), Random.Range(-10, 10)).normalized;
        }
        float terrainMultiplier = LevelManager.getTile(transform.position).terrainSpeedMultiplier;
        transform.position = Vector2.MoveTowards(transform.position, ((Vector2)transform.position) + direction, speed*terrainMultiplier);
    }
}
