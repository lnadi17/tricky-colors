using UnityEngine;
using System.Collections;

// This class is added to the LineHolder gameobject.
public class LineTransform : MonoBehaviour
{
    [HideInInspector]
    public float distanceBetweenY;
    [HideInInspector]
    public int horizontalCount;
    [HideInInspector]
    public int verticalCount;
    [HideInInspector]
    public int direction;
    [HideInInspector]
    public float speed;
    [HideInInspector]
    public float screenWidth;
    [HideInInspector]
    public float screenHeight;
    [HideInInspector]
    public float platformWidth;
    [HideInInspector]
    public Transform playerTransform;
    [HideInInspector]
    public float transformWaitTime;

    private Transform[] childTransforms;

    void Start() {
        childTransforms = new Transform[horizontalCount];

        Transform[] tempArray = GetComponentsInChildren<Transform>();
        for (int i = 0; i < horizontalCount; i++) {
            childTransforms[i] = tempArray[i + 1];
        }

        StartCoroutine(RepositionX());
        StartCoroutine(RepositionY());
    }

    void Update() {
        foreach (Transform t in childTransforms) {
            t.Translate(Vector2.right * speed * direction * Time.deltaTime);
        }
    }

    IEnumerator RepositionY() {
        // Don't start reposition until 5 seconds pass.
        // yield return new WaitForSeconds(5f);

        while (true) {
            if (transform.position.y < playerTransform.position.y - 1 - screenHeight * 0.5f) {
                transform.Translate(new Vector2(0, distanceBetweenY * verticalCount));
            }
            yield return new WaitForSeconds(5f);
        }
    }

    IEnumerator RepositionX() {
        int firstTransformIndex = 0;
        int lastTransformIndex = horizontalCount - 1;

        while (true) {
            bool repositioned = false;
            // If direction is one then it's moving right.
            if (direction == 1) {
                if (childTransforms[lastTransformIndex].position.x > screenWidth * 0.5f) {
                    childTransforms[lastTransformIndex].Translate(new Vector2(-platformWidth * horizontalCount, 0));
                    repositioned = true;
                }  
            } else {
                if (childTransforms[firstTransformIndex].position.x + platformWidth < -screenWidth * 0.5f) {
                    childTransforms[firstTransformIndex].Translate(new Vector2(platformWidth * horizontalCount, 0));
                    repositioned = true;
                }
            }

            if (repositioned && direction == 1) {
                lastTransformIndex--;
                if (lastTransformIndex == -1) {
                    lastTransformIndex = horizontalCount - 1;
                }
            }

            if (repositioned && direction == -1) {
                firstTransformIndex++;
                if (firstTransformIndex == horizontalCount) {
                    firstTransformIndex = 0;
                }
            }

            // Warning: wait shouldn't be so long that it can't keep up with the line's movement speed.
            yield return new WaitForSeconds(transformWaitTime);
        }
    }
}
