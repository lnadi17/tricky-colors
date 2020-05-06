using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    [Range(1, 10)]
    public float speed;
    public Vector2 jumpDistance;
    public AudioClip myAudio;
    public float gravityScale;
    public Vector2 playerOffset;

    private Rigidbody2D rigidbody2d;
    private bool isGravity;
    private Animator animator;
    private float fallTime;
    private bool isFalling;
    private bool isMovingUp;
    private bool isMovingDown;
    private Vector2 endVector;

    void Start() {
        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        //isFalling = false;
        fallTime = 0;
        //isGravity = false;
        isMovingUp = false;
        isMovingDown = false;

        rigidbody2d.MovePosition(playerOffset);
    }

    void Update() { 
        // If paused, don't move.
        if (Time.timeScale == 0) {
            return;
        }

        //if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began && Input.GetTouch(0).position.y < 0.75f) {
        if (Input.GetKeyDown(KeyCode.Space)) {
            AudioSource.PlayClipAtPoint(myAudio, Camera.main.transform.position);
            animator.Play("PlayerAnim", -1, 0f);
            endVector = (Vector2)rigidbody2d.position + jumpDistance;
            isMovingUp = true;
            isMovingDown = false;
        }
    }

    void FixedUpdate() {
        if (isMovingUp) {
            float sqrRemainingDistance = (rigidbody2d.position - endVector).sqrMagnitude;
            rigidbody2d.MovePosition(Vector2.MoveTowards(rigidbody2d.position, endVector, speed * 0.01f));
            if (sqrRemainingDistance < 0.05f) {
                isMovingUp = false;
                isMovingDown = true;
                fallTime = 0;
            }
        }

        if (isMovingDown) {
            fallTime += Time.fixedDeltaTime;
            rigidbody2d.MovePosition(rigidbody2d.position + Vector2.down * fallTime * gravityScale * 0.01f);
        }
    }
}
