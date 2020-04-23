using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    [Range(1, 10)]
    public float speed;
    public Vector2 jumpDistance;
    public AudioClip myAudio;
    public float gravityScale;

    private Rigidbody2D rigidbody2d;
    // private bool started;
    private Animator animator;
    private float fallTime;
    private bool isFalling;

    void Start() {
        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        isFalling = false;
        fallTime = 0;
        // started = false;
    }

    void Update() {
        // if (Input.GetTouch(0).phase == TouchPhase.Began && Input.GetTouch(0).position.y < Camera.main.pixelHeight * 0.8f && started){
        if (Input.GetKeyDown(KeyCode.Space)) { // || (Input.GetTouch(0).phase == TouchPhase.Began && Input.GetTouch(0).position.y < Camera.main.pixelHeight * 0.8f && started)) {
            Vector2 end = jumpDistance + (Vector2)rigidbody2d.position;
            StartCoroutine(SmoothMovement(end));
            AudioSource.PlayClipAtPoint(myAudio, Camera.main.transform.position);
            animator.Play("PlayerAnim", -1, 0f);
            isFalling = false;
        } else {
            // If no input, gravity affects the ball
            if (!isFalling) {
                fallTime = 0;
                isFalling = true;
            }
            fallTime += Time.deltaTime;
            rigidbody2d.MovePosition(rigidbody2d.position + Vector2.down * gravityScale * fallTime * fallTime * 0.01f);
        }
    }

    IEnumerator SmoothMovement(Vector2 end) {
        float sqrRemainingDistance = (rigidbody2d.position - end).sqrMagnitude;
        while (sqrRemainingDistance > 0.1f) {
            rigidbody2d.MovePosition(Vector2.MoveTowards(rigidbody2d.position, end, speed * Time.deltaTime));
            sqrRemainingDistance = (rigidbody2d.position - end).sqrMagnitude;
            yield return null;
        }
    }
}
