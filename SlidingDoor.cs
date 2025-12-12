using UnityEngine;

public class SlidingDoor : MonoBehaviour
{
    public Transform doorLeft;
    public Transform doorRight;

    public float slideDistance = 1f;
    public float speed = 2f;

    private Vector3 leftClosedPos;
    private Vector3 rightClosedPos;
    private Vector3 leftOpenPos;
    private Vector3 rightOpenPos;

    private bool isOpening = false;
    private bool wasOpening = false; // Tracks previous state to avoid spamming Debug.Log

    void Start()
    {
        leftClosedPos = doorLeft.position;
        rightClosedPos = doorRight.position;

        leftOpenPos = leftClosedPos + Vector3.right * slideDistance;
        rightOpenPos = rightClosedPos + Vector3.left * slideDistance;

    }

    void Update()
    {

        if (isOpening)
        {
            doorLeft.position = Vector3.MoveTowards(doorLeft.position, leftOpenPos, speed * Time.deltaTime);
            doorRight.position = Vector3.MoveTowards(doorRight.position, rightOpenPos, speed * Time.deltaTime);

            if (!wasOpening)
            {
                wasOpening = true;
            }
        }
        else
        {
            doorLeft.position = Vector3.MoveTowards(doorLeft.position, leftClosedPos, speed * Time.deltaTime);
            doorRight.position = Vector3.MoveTowards(doorRight.position, rightClosedPos, speed * Time.deltaTime);

            if (wasOpening)
            {
                wasOpening = false;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("NPC"))
        {
            isOpening = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("NPC"))
        {
            isOpening = false;
        }
    }
}
