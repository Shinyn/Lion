using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinotaurController : MonoBehaviour
{
    [SerializeField]
    private List<Transform> positions = new List<Transform>();
    public int currentPosition = 8;
   
    float lastMoveTime = 1.0f;
    float moveDelay; 

    public MinotaurController otherMinotaur;
    public LionTamerController lionTamer;

    LayerMask tamerLayer;
    bool leftRayHit;

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
            MoveLeft();
        }
        else if (positions[currentPosition].gameObject.tag == "RightAttackPosition")
        {
            MoveRight();
        }
        else if (positions[currentPosition].gameObject.tag == "DangerPosition")
        {
            ReturnToMiddle();
        }
        
        UpdateMinotaurPosition();
        CheckCollision();
        
        // ska kunna gå åt olika håll
        // ska ha en attack fas
        // ska inte kunna stå på samma plats som en annan minotaur
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
                // Väntar
                break;
        }
    }

    private void CheckCollision()
    {
        
        if (positions[currentPosition].gameObject.tag == "DangerPosition")
        {
            // Skjuter ut två laserstrålar åt höger och vänster för att kolla om den träffar något
            tamerLayer = LayerMask.GetMask("TamerLayer");
            RaycastHit2D hitLeft = Physics2D.Raycast(transform.position, Vector2.left, 3.0f, tamerLayer);
            RaycastHit2D hitRight = Physics2D.Raycast(transform.position, Vector2.right, 3.0f, tamerLayer);

            if (hitLeft.collider == null && hitRight.collider == null)
            {
                // Missar raycasten en tamer så flyr minotauren
                Debug.Log("Minotaur escaped");
            }
            else
            {
                // Träffar raycasten en tamer så klarade vi oss
                Debug.Log("You fend off the minotaur");
            }
        }
    }

    private void ReturnToMiddle()
    {
        if (positions[currentPosition].gameObject.tag == "DangerPosition" &&)
        {
            //Efter attack ska minotauren gå tillbaka till mitten
            
        }


        // behöver göra så att den bara får gå upp eller ner i dom 6 mittersta punkterna
        // gör if sats - Kolla om man är på dom 6 innersta positionerna, är man det kan man randomisera
        // är man inte det ska man attackera mot den närmaste tamern
    }
}