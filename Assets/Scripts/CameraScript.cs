using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public GameObject player;
    [Range (0, 1)]
    public float speed;
    [Range (0, 1)]
    public float downSpeedMultiplier;

    private Transform camTransform;
    private Vector2 playerOffset;
    private float lerpFactor;
    private Vector3 futureCamPos;
    private float displacement;

    void Awake() {
        camTransform = Camera.main.transform;
        playerOffset = player.GetComponent<PlayerMovement>().playerOffset;
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
}
