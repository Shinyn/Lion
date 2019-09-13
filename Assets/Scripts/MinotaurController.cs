using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinotaurController : MonoBehaviour
{
    [SerializeField]
    private List<Transform> positions = new List<Transform>();
    public int currentPosition = 10;
    float lastMoveTime = 1.5f;
    SpriteRenderer spriteRenderer;
    public float moveDelay; 
    public MinotaurController otherMinotaur;
    public GameManager gameManager;
    LayerMask tamerLayer;

    [HideInInspector]
    public bool rayHit;
    bool hasAttacked = false;
    bool wasStopped = false;

    void Start()
    {
        UpdateMinotaurPosition();
        lastMoveTime = Time.time;
        moveDelay = Random.Range(0.5f, 1.0f);
    }

    private void Update()
    {
        if (Time.time > lastMoveTime + moveDelay)
        {
            MovePosition();
            FlipMinotaur();
            lastMoveTime = Time.time;
        }
    }

    private void UpdateMinotaurPosition()
    {
        if (currentPosition < positions.Count && currentPosition >= 0)
            transform.position = positions[currentPosition].position;
    }

    private void MovePosition()
    {
        if (positions[currentPosition].gameObject.tag == "FreeMovePosition")
        {
            RandomizePosition();
        }
        else if (positions[currentPosition].gameObject.tag == "LeftAttackPosition")
        {
            if (hasAttacked == true)
                currentPosition++;
            
            else
            TrickTamer();
        }
        else if (positions[currentPosition].gameObject.tag == "RightAttackPosition")
        {
            if (hasAttacked == true)
                currentPosition--;

            else
            TrickTamer();
        }
        else if (positions[currentPosition].gameObject.tag == "LeftDangerPosition" || positions[currentPosition].gameObject.tag == "RightDangerPosition")
        {
            if (hasAttacked == true && wasStopped == true)
            {
                // gå tillbaka till mitten (ett steg i taget)
                ReturnToMiddle();
            }
            else if (hasAttacked == true && wasStopped == false)
            {
                // escape
                EscapeSequence();
            }
            else if (hasAttacked == false)
            {

            }
        }

        UpdateMinotaurPosition();
        CheckCollision();
    }

    private void FlipMinotaur()
    {
        // vänder minotauren beroende på vilken sida om 0 den är på x-axeln
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (transform.position.x < 0)
            spriteRenderer.flipX = false;

        else
            spriteRenderer.flipX = true;
    }

    private void MoveUp()
    {
        if (currentPosition - 6 > 0 && currentPosition - 6 != otherMinotaur.currentPosition)
        {
            currentPosition -= 6;
        }
        else
        {
            RandomizePosition();
        }
    }

    private void MoveDown()
    {
        if (currentPosition + 6 < positions.Count - 1 && currentPosition + 6 != otherMinotaur.currentPosition)
        {
            currentPosition += 6;
        }
        else
        {
            RandomizePosition();
        }
    }

    private void MoveLeft()
    {
        // Kollar så att den inte går till en position på andra sidan planen och att platsen inte är upptagen
        if (currentPosition != 0 && currentPosition != 6 && currentPosition != 12 && currentPosition - 1 != otherMinotaur.currentPosition)
        {
            currentPosition--;
        }
        else
        {
            RandomizePosition();
        }
    }

    private void MoveRight()
    {
        // Kollar så att den inte går till en position på andra sidan planen och att platsen inte är upptagen
        if (currentPosition != 5 && currentPosition != 11 && currentPosition != 17 && currentPosition + 1 != otherMinotaur.currentPosition)
        {
            currentPosition++;
        }
        else
        {
            RandomizePosition();
        }
    }

    private void TrickTamer()
    {
        int random = Random.Range(1, 3);
        switch (random)
        {
            case 1:
                MoveLeft();
                break;
            case 2:
                MoveRight();
                break;
        }
    }

    private void RandomizePosition()
    {
        int random = Random.Range(1, 6);
        switch (random)
        {
            case 1:
                MoveUp();
                break;
            case 2:
                MoveDown();
                break;
            case 3:
                MoveLeft();
                break;
            case 4:
                MoveRight();
                break;
            case 5:
                // Väntar till nästa Randomize
                break;
        }
    }

    private void CheckCollision()
    {
        
        if (positions[currentPosition].gameObject.tag == "LeftDangerPosition" || positions[currentPosition].gameObject.tag == "RightDangerPosition")
        {
            // Skjuter ut två laserstrålar åt höger och vänster för att kolla om den träffar något
            tamerLayer = LayerMask.GetMask("TamerLayer");
            RaycastHit2D hitLeft = Physics2D.Raycast(transform.position, Vector2.left, 3.0f, tamerLayer);
            RaycastHit2D hitRight = Physics2D.Raycast(transform.position, Vector2.right, 3.0f, tamerLayer);

            if (hitLeft.collider == null && hitRight.collider == null)
            {
                // Missar raycasten en tamer så flyr minotauren (1 miss)
                gameManager.EscapedLions();
                rayHit = false;
            }
            else 
            {
                rayHit = true;
                gameManager.AddPoint();
                // Träffar raycasten en tamer så klarade vi oss (1 poäng)
            }
        }
    }

    //Efter attack ska minotauren gå tillbaka till mitten
    private void ReturnToMiddle()
    {
        if (positions[currentPosition].gameObject.tag == "LeftDangerPosition" && rayHit )
        {
            currentPosition ++;
            hasAttacked = true;
        }
        else if (positions[currentPosition].gameObject.tag == "RightDangerPosition" && rayHit )
        {
            currentPosition --;
            hasAttacked = true;
        } 
        
        // fixa escape här istället och frys spelet tills animationen är klar och sen gå tillbaka
        /*
        else if (positions[currentPosition].gameObject.tag == "LeftDangerPosition" && !rayHit)
        {
            currentPosition += 2;
        } 
        else if (positions[currentPosition].gameObject.tag == "RightDangerPosition" && !rayHit)
        {
            currentPosition -= 2;
        }
        */
    }

    private void EscapeSequence()
    {
        // pendla mellan escape positionerna
        if (positions[currentPosition].gameObject.tag == "LeftDangerPosition" && !rayHit)
        {
            MoveLeft();
            wasStopped = false;
        }
        else if (positions[currentPosition].gameObject.tag == "RightDangerPosition" && !rayHit)
        {
            MoveRight();
            wasStopped = false;
        }
    }
}