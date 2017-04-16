using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
    //2017-04-14: copied from Stoned_Unity
    public GameObject player;

    private Vector3 offset;
    private float scale = 1;//scale used to determine orthographicSize, independent of (landscape or portrait) orientation
    private Camera cam;
    private GestureManager gm;
    //private GameManager gameManager;
    private PlayerController plyrController;

    private int prevScreenWidth;
    private int prevScreenHeight;

    struct ScalePoint
    {
        private float scalePoint;
        private bool relative;//true if relative to player's range, false if absolute
        private PlayerController plyrController;
        public ScalePoint(float scale, bool relative, PlayerController plyrController)
        {
            scalePoint = scale;
            this.relative = relative;
            this.plyrController = plyrController;
        }
        public float absoluteScalePoint()
        {
            if (relative)
            {
                return scalePoint * 3;
            }
            return scalePoint;
        }
    }
    ArrayList scalePoints = new ArrayList();
    int scalePointIndex = 1;//the index of the current scalePoint in scalePoints

    // Use this for initialization
    void Start()
    {
        pinPoint();
        cam = GetComponent<Camera>();
        gm = GameObject.FindGameObjectWithTag("GestureManager").GetComponent<GestureManager>();
        //gameManager = GameObject.FindObjectOfType<GameManager>();
        plyrController = player.GetComponent<PlayerController>();
        scale = cam.orthographicSize;
        //Initialize ScalePoints
        scalePoints.Add(new ScalePoint(1, false, plyrController));
        scalePoints.Add(new ScalePoint(1, true, plyrController));
        scalePoints.Add(new ScalePoint(2, true, plyrController));
        scalePoints.Add(new ScalePoint(4, true, plyrController));
        //
        setScalePoint(1);

    }

    void Update()
    {
        if (prevScreenHeight != Screen.height || prevScreenWidth != Screen.width)
        {
            prevScreenWidth = Screen.width;
            prevScreenHeight = Screen.height;
            updateOrthographicSize();
        }
    }

    // Update is called once per frame, after all other objects have moved that frame
    void LateUpdate()
    {
        transform.position = player.transform.position + offset;

    }    

    /// <summary>
    /// Sets the camera's offset so it stays at this position relative to the player
    /// </summary>
    public void pinPoint()
    {
        offset = transform.position - player.transform.position;
    }

    /// <summary>
    /// Recenters on Merky
    /// </summary>
    public void recenter()
    {
        offset = new Vector3(0, 0, offset.z);
    }
    /// <summary>
    /// Moves the camera directly to Merky's position + offset
    /// </summary>
    public void refocus()
    {
        transform.position = player.transform.position + offset;
    }

    public void setScalePoint(int scalePointIndex)
    {
        if (scalePointIndex < 0)
        {
            scalePointIndex = 0;
        }
        else if (scalePointIndex > scalePoints.Count - 1)
        {
            scalePointIndex = scalePoints.Count - 1;
        }
        this.scalePointIndex = scalePointIndex;
        //Set the size
        scale = ((ScalePoint)scalePoints[scalePointIndex]).absoluteScalePoint();
        updateOrthographicSize();

        //Make sure player is still in view
        float width = Vector3.Distance(cam.ScreenToWorldPoint(new Vector3(cam.pixelWidth, 0)), cam.ScreenToWorldPoint(new Vector3(0, 0)));
        float height = Vector3.Distance(cam.ScreenToWorldPoint(new Vector3(0, cam.pixelHeight)), cam.ScreenToWorldPoint(new Vector3(0, 0)));
        float radius = Mathf.Min(width, height) / 2;
        float curDistance = Vector3.Distance(player.transform.position, transform.position);
        if (curDistance > radius)
        {
            float prevZ = offset.z;
            offset = new Vector2(offset.x, offset.y).normalized * radius;
            offset.z = prevZ;
            refocus();
        }
    }
    public void adjustScalePoint(int addend)
    {
        setScalePoint(scalePointIndex + addend);
    }
    public int getScalePointIndex()
    {
        return scalePointIndex;
    }
    public void updateOrthographicSize()
    {
        if (Screen.height > Screen.width)//portrait orientation
        {
            cam.orthographicSize = (scale * cam.pixelHeight) / cam.pixelWidth;
        }
        else {//landscape orientation
            cam.orthographicSize = scale;
        }
    }
}
