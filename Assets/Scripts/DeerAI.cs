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

    private SpriteRenderer sr;
    private CircleCollider2D cc2d;

    // Use this for initialization
    void Start () {
        sr = GetComponent<SpriteRenderer>();
        cc2d = GetComponent<CircleCollider2D>();
        relocate();
    }

    // Update is called once per frame
    void Update()
    {
        if (!LevelManager.inBounds(transform.position))
        {
            relocate();
        }
        speed = walkSpeed;
        RaycastHit2D[] rch2ds = new RaycastHit2D[1];
        cc2d.Cast(wolf.transform.position - transform.position,rch2ds,safeThreshold);
        if (rch2ds[0] && rch2ds[0].collider.gameObject == wolf)
        {
            direction = transform.position - wolf.transform.position;
            speed = runSpeed;
        }
        else
        {
            direction = direction + new Vector2(Random.Range(-10, 10), Random.Range(-10, 10)).normalized;
        }
        float terrainMultiplier = LevelManager.getTile(transform.position).terrainSpeedMultiplier;
        transform.position = Vector2.MoveTowards(transform.position, ((Vector2)transform.position) + direction, speed*terrainMultiplier*Time.deltaTime);
        sr.sortingOrder = LevelManager.getDisplaySortingOrder(transform.position);

    }

    /// <summary>
    /// Randomly teleports the deer after being eaten
    /// </summary>
    public void relocate()
    {
        transform.position = LevelManager.randomPosition();
    }
}
