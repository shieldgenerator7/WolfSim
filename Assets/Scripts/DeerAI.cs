using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeerAI : MonoBehaviour {

    public float speed = 0.04f;
    public float walkSpeed = 0.02f;
    public float runSpeed = 0.04f;
    public GameObject wolf;
    public float safeThreshold = 7.0f;//the deer is safe if the wolf is at least this far away
    public float panicTime = 0;//the point in time when the deer can stop panicking
    public float panicTimeAmount = 1.0f;//how long the deer will panic after it stops seeing the wolf

    private Vector2 direction;

    private SpriteRenderer sr;
    private CircleCollider2D cc2d;
    private Color baseColor;

    // Use this for initialization
    void Start () {
        sr = GetComponent<SpriteRenderer>();
        baseColor = sr.color;
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
        RaycastHit2D[] rch2ds = Physics2D.RaycastAll(transform.position, wolf.transform.position - transform.position,safeThreshold);
        bool wolfFound = false;
        for (int i = 0; i < rch2ds.Length; i++)
        {
            RaycastHit2D rch2d = rch2ds[i];
            if (rch2d.collider.gameObject != this.gameObject)
            {
                if (rch2d.collider.gameObject == wolf)
                {
                    wolfFound = true;
                    break;
                }
                else if (rch2d.collider.gameObject.name.Contains("tree"))
                {
                    break;
                }
            }
        }
        if (wolfFound || panicTime > Time.time)
        {
            speed = runSpeed;
            if (wolfFound)
            {
                direction = transform.position - wolf.transform.position;
                sr.color = Color.red;
                panicTime = Time.time + panicTimeAmount;
            }
            else
            {
                sr.color = Color.yellow;
            }
        }
        else
        {
            direction = direction + new Vector2(Random.Range(-10, 10), Random.Range(-10, 10)).normalized;
            sr.color = baseColor;
        }
        float terrainMultiplier = LevelManager.getTile(transform.position).terrainSpeedMultiplier;
        transform.position = Vector2.MoveTowards(transform.position, ((Vector2)transform.position) + direction, speed*terrainMultiplier*Time.deltaTime);
        sr.flipX = direction.x < 0;
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
