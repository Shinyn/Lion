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

    public GameObject input;
    public bool leftMinotaur;
    float escapeDuration = 1.5f;
    LionTamerController lionTamer;
    Rigidbody2D rigidbody;

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
            {
                currentPosition++;
                hasAttacked = false;
            }

            else
                TrickTamer();
        }
        else if (positions[currentPosition].gameObject.tag == "RightAttackPosition")
        {
            if (hasAttacked == true)
            {
                currentPosition--;
                hasAttacked = false;
            }

            else
                TrickTamer();
        }
        else if (positions[currentPosition].gameObject.tag == "LeftDangerPosition" || positions[currentPosition].gameObject.tag == "RightDangerPosition")
        {
            if (wasStopped == true)
            {
                // gå tillbaka till mitten (ett steg i taget)
                ReturnToMiddle();
                hasAttacked = true;
            }
            else if (wasStopped == false)
            {
                // frys tamers och sätt den som failade i sin special position
                // när en minotaur flyr så resettas båda till sin orginal position
                StartCoroutine(Escape());
                wasStopped = false;
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
                wasStopped = true;
                gameManager.AddPoint();
                // Träffar raycasten en tamer så klarade vi oss (1 poäng)
            }
        }
    }

    //Efter attack ska minotauren gå tillbaka till mitten
    private void ReturnToMiddle()
    {
        if (positions[currentPosition].gameObject.tag == "LeftDangerPosition" && rayHit)
        {
            currentPosition ++;
            wasStopped = true;
            //Debug.Log("return left pos");
        }
        else if (positions[currentPosition].gameObject.tag == "RightDangerPosition" && rayHit)
        {
            currentPosition --;
            wasStopped = true;
            //Debug.Log("return right pos");
        } 

        /*
        else if (positions[currentPosition].gameObject.tag == "RightDangerPosition" && !rayHit)
        {

        }
        else if (positions[currentPosition].gameObject.tag == "LeftDangerPosition" && !rayHit)
        {

        } */
        // fixa escape här istället och frys spelet tills animationen är klar och sen gå tillbaka
    }

    IEnumerator Escape()
    {
        // Stäng av allt i Escape();

        // stäng av input
        // pausa andra minotauren
        // sätt rätt tamer i scared position
        // byt tamer sprite
        // sätt minotaur i escape position
        // flytta minotaur i rätt riktning
        // när minotaur inte syns av kameran så reset position
        // reset tamer position
        // byt tillbaka till rätt tamer sprite
        // sätt på input igen

        input.SetActive(false);
        if (lionTamer.leftTamer)
        {
            // ändra sprite
            lionTamer.currentPosition = 3;
            lionTamer.UpdateCurrentPosition();
        }
        else if (!lionTamer.leftTamer)
        {
            // ändra sprite
            lionTamer.currentPosition = 7;
            lionTamer.UpdateCurrentPosition();
        }

        if (leftMinotaur)
        {
            // KOLLA VILKEN SIDA AV 0 DEN ÄR PÅ X
            rigidbody.AddForce(Vector2.left * 100);
            
        }
        else
        {
            rigidbody.AddForce(Vector2.right * 100);
            
        }
        /* Andra sättet att stänga av input
        GameObject inpt = GameObject.Find("Input");
        inpt.SetActive(false);
        */

        yield return new WaitForSeconds(escapeDuration);
        // efter att minotauren flytt så resetta positionen
        
    }

    private void OnBecameInvisible()
    {
        // Sätt på allt igen som stängdes av i Escape();
        // all kod som resettar allt ska in här
        if (leftMinotaur)
        {
            currentPosition = 10;
        }
        else
        {
            currentPosition = 11;
        }

        input.SetActive(true);
    }
}