using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public int attacksRemaining;
    public int movesRemaining;

    GameObject selectedFriendlyPiece;
    Vector2 startPos;
    Vector2 endPos;
    bool canAttack;
    bool canMove;

    PhysicalStats friendlyPieceStats;
    PhysicalStats enemyPieceStats;

    void Start ()
    {
        canAttack = false;
        canMove = false;
	}

	void Update ()
    {
        if (!canAttack)
        {
            selectedFriendlyPiece = CheckTouch("Friendly Piece");
        }

        if (selectedFriendlyPiece != null) 
        {
            if (attacksRemaining > 0)
            {
                canAttack = true;
            }
            if (movesRemaining > 0)
            {
                canMove = true;
            }
        }
        else
        {
            canAttack = false;
            canMove = false;
        }

        if (canAttack)
        {
            Attack(CheckTouch ("Enemy Piece"));
        }
        if (canMove)
        {
            Move(CheckTouch("Space"));
        }
    }

    // Check the touch input to see if any board pieces have been selected
    GameObject CheckTouch (string tag)
    {
        if (Input.touchCount == 1)
        {
            Touch selectionTouch = Input.GetTouch(0);

            // Check if the touch has begun and set the starting position
            if (selectionTouch.phase == TouchPhase.Began)
            {
                startPos = selectionTouch.position;
            }

            // Check if the touch has ended and set an ending position
            if (selectionTouch.phase == TouchPhase.Ended)
            {
                endPos = selectionTouch.position;
                // Check if the starting and ending positions are the same
                if (startPos == endPos)
                {
                    // If the start and end positions are the same cast a Ray from the ending position
                    Ray ray = Camera.main.ScreenPointToRay(endPos);
                    RaycastHit hit;

                    // If the ray hits something check what it hit
                    if (Physics.Raycast(ray, out hit))
                    {
                        // If the hit is tagged by the tag parameter return it
                        if (hit.transform.tag == tag)
                        {
                            print("Selected Object: " + hit.transform.gameObject);
                            friendlyPieceStats = hit.transform.GetComponent<PhysicalStats>();
                            return hit.transform.gameObject;
                        }
                    }
                }
            }
        }
        // If one or more of these checks come back false, return null
        return null;
    }

    void Move (GameObject spaceToMoveTo)
    {
        if (spaceToMoveTo != null)
        {
            Rigidbody rb = selectedFriendlyPiece.GetComponent<Rigidbody>();
            rb.MovePosition(spaceToMoveTo.transform.position * Time.deltaTime);
        }
    }

    // Choose an enemy to attack
    void Attack (GameObject enemyToAttack)
    {
        if (enemyToAttack != null)
        {
            float distanceBetweenPieces = ((enemyToAttack.transform.position - selectedFriendlyPiece.transform.position).magnitude) / 2f;
            print("Distance between pieces: " + distanceBetweenPieces);
            print("Selected enemy: " + enemyToAttack);
            if (friendlyPieceStats != null)
            {
                if (distanceBetweenPieces <= friendlyPieceStats._range)
                {
                    attacksRemaining--;
                    print("Attacks Remaining: " + attacksRemaining);
                    enemyPieceStats = enemyToAttack.GetComponent<PhysicalStats>();
                    if (enemyPieceStats != null)
                    {
                        enemyPieceStats._health -= friendlyPieceStats._strength;
                        selectedFriendlyPiece = null;
                        print("Enemy's Remaining Health: "+ enemyPieceStats._health);
                    }
                }
                else if (distanceBetweenPieces > friendlyPieceStats._range)
                {
                    print("Too far away");
                }
            }
        }
    }
}
