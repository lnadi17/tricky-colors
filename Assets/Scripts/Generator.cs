using UnityEngine;
using System.Collections.Generic;

public class Generator : MonoBehaviour
{
    // Static fields are referred by LineTransform class.
    [Header("Line Settings")]
    public GameObject platform;
    public float distanceBetweenY = 3f;
    public float speed = 1f;
    public int horizontalCount = 10;
    [Space]
    public bool generateLinesOnly = true;
    public bool swapPlatformDirections = true;
    public float linesOffsetY = 5f;
    public int verticalCount = 5;

    private float screenHeight;
    private float screenWidth;

    private ColorScript colorScript;
    private SpriteRenderer platformSpriteRenderer;
    private Transform playerTransform;
    private float platformWidth;
    private float platformHeight;
    private int[] lineColorIndexArray;

    void Start() {
        screenHeight = Camera.main.orthographicSize * 2;
        screenWidth = screenHeight * Camera.main.aspect;

        colorScript = GetComponent<ColorScript>();
        platformSpriteRenderer = platform.GetComponent<SpriteRenderer>();
        playerTransform = GameObject.Find("Player").transform;
        platformWidth = platformSpriteRenderer.sprite.bounds.size.x * platform.transform.localScale.x;
        platformHeight = platformSpriteRenderer.sprite.bounds.size.y * platform.transform.localScale.y;

        if (generateLinesOnly) {
            GenerateLines();
        }
    }

    void GenerateLine(Vector2 position, int direction) {
        GameObject lineHolder = new GameObject("LineHolder");
        for (int i = 0; i < horizontalCount; i++) {
            Vector2 objectPosition = new Vector2(i * platformWidth, 0);
            GameObject platformClone = Instantiate<GameObject>(platform, objectPosition, Quaternion.identity, lineHolder.transform);
            platformClone.GetComponent<SpriteRenderer>().color = GetRandomLineColor(i);
        }
        float lineWidth = horizontalCount * platformWidth;
        // Move parent gameobject to passed position + calculated offset.
        if (direction == 1) {
            lineHolder.transform.position = position + new Vector2(screenWidth * 0.5f - lineWidth, 0);
        } else {
            lineHolder.transform.position = position + new Vector2(-screenWidth * 0.5f, 0);
        }
        // Set lineHolder fieds.
        LineTransform lineTransform = lineHolder.AddComponent<LineTransform>();
        lineTransform.distanceBetweenY = distanceBetweenY;
        lineTransform.horizontalCount = horizontalCount;
        lineTransform.verticalCount = verticalCount;
        lineTransform.direction = direction;
        lineTransform.speed = speed;
        lineTransform.screenWidth = screenWidth;
        lineTransform.screenHeight = screenHeight;
        lineTransform.platformWidth = platformWidth;
        lineTransform.playerTransform = playerTransform;
    }

    void GenerateLines() {
        float startX = 0;
        float startY = -screenHeight * 0.5f + linesOffsetY; // (-screenHeight / 2) is the lowest point on Y axis.
        for (int i = 0; i < verticalCount; i++) {
            Vector2 currentPosition = new Vector2(startX, startY + i * distanceBetweenY);
            int direction = i % 2 == 0 ? -1 : 1;
            if (swapPlatformDirections) {
                direction = -direction;
            }
            GenerateLine(currentPosition, direction);
        }
    }

    // Resets lineColorIndexArray.
    void ResetRandomLineColorList() {
        int[] indexArray = new int[horizontalCount];
        // The last platform color index will be chosen from this list.
        List<int> lastChoiceList = new List<int>();

        // Every color should be in this list at least once.
        for (int i = 0; i < colorScript.colorList.Count; i++) {
            indexArray[i] = i;
        }

        // Shuffle current list.
        for (int i = 0; i < colorScript.colorList.Count; i++) {
            int randomIndex = Random.Range(0, colorScript.colorList.Count);
            int temp = indexArray[i];
            indexArray[i] = indexArray[randomIndex];
            indexArray[randomIndex] = temp;
        }

        // Copy shuffled list into last choice list (it will contain every color).
        for (int i = 0; i < colorScript.colorList.Count; i++) {
            lastChoiceList.Add(indexArray[i]);
        }

        int firstIndex = indexArray[0];
        int lastIndex = indexArray[colorScript.colorList.Count - 1];
        
        // Add more colors to the list. Same colors should not be adjacent.
        for (int i = colorScript.colorList.Count; i < horizontalCount; i++) {
            if (i == horizontalCount - 1) {
                lastChoiceList.Remove(firstIndex);
                lastChoiceList.Remove(lastIndex);
                int lastChoiceIndex = Random.Range(0, lastChoiceList.Count);
                indexArray[i] = lastChoiceList[lastChoiceIndex];
            } else {
                int belowLast = Random.Range(0, lastIndex);
                int aboveLast = Random.Range(lastIndex + 1, colorScript.colorList.Count);

                if (lastIndex == 0) {
                    indexArray[i] = aboveLast;
                } else if (lastIndex == colorScript.colorList.Count - 1) {
                    indexArray[i] = belowLast;
                } else {
                    int choice = Random.Range(0, 2);
                    indexArray[i] = choice == 0 ? belowLast : aboveLast;
                }
                
                lastIndex = indexArray[i];
            }
        }
        
        // At this point random color index array is ready so we (re)assign it.
        lineColorIndexArray = indexArray;
    }

    Color GetRandomLineColor(int index) {
        if (index == 0) {
            ResetRandomLineColorList();
        }
        return colorScript.colorList[lineColorIndexArray[index]];
    }
}
