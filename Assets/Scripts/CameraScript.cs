using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public GameObject player;
    [Range (0, 1)]
    public float speed;
    [Range (0, 1)]
    public float downSpeedMultiplier;
    [Space(1)]
    public Transform leftPlane;
    public Transform rightPlane;
    public Transform topPlane;
    public Transform bottomPlane;

    private Transform camTransform;
    private Vector2 playerOffset;
    private float lerpFactor;
    private Vector3 futureCamPos;
    private float displacement;

    void Awake() {
        camTransform = Camera.main.transform;
        playerOffset = player.GetComponent<PlayerMovement>().playerOffset;
        AlignCollisionPlanes();
    }

    void Update() {
        futureCamPos = player.transform.position - (Vector3)playerOffset - Vector3.forward * 10;
        displacement = futureCamPos.y - camTransform.position.y;

        if (displacement < 0) {
            lerpFactor = 0.05f * speed * Time.deltaTime * 100 * downSpeedMultiplier;
        } else {
            lerpFactor = 0.05f * speed * displacement * Time.deltaTime * 100;
        }
        camTransform.position = Vector3.Lerp(camTransform.position, futureCamPos, lerpFactor);
    }

    void AlignCollisionPlanes() {
        topPlane.localPosition = new Vector2(0, Camera.main.orthographicSize);
        bottomPlane.localPosition = new Vector2(0, -Camera.main.orthographicSize);
        leftPlane.localPosition = new Vector2(-Camera.main.orthographicSize * Camera.main.aspect, 0);
        rightPlane.localPosition = new Vector2(Camera.main.orthographicSize * Camera.main.aspect, 0);
    }
}
