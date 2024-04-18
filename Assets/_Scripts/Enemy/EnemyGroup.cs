using System.Collections;
using System.Collections.Generic;
using SonicBloom.Koreo;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class EnemyGroup : MonoBehaviour
{
    public enum Direction
    {
        Back,
        Left,
        Right,
        Forward,
        BackLeft,
        BackRight,
        Reroll,
    }

    [Header("Beat Track")]
    [SerializeField] public KoreographyTrack trackName;
    private int beatNumber;

    [Header("Enemies")]
    [SerializeField] public bool isTriangleMove;
    [SerializeField] public GameObject[] enemyList;

    [Header("Move Direction")]
    [SerializeField] public Direction[] moveList = new Direction[4];

    void Start()
    {
        Koreographer.Instance.RegisterForEvents(trackName.EventID, GroupAction);
        beatNumber = 1; // Enemy Movement Count

        moveList = new Direction[4];
        
        RerollMove();
    }
    
    
    void Update()
    {
        CheckEnemies();
    }

    //When On the Beat
    void GroupAction(KoreographyEvent koreoEvent)
    {
        // if (this != null)
        // {
        //     DoMove(beatNumber);
        //
        //     if (beatNumber == 3)
        //     {
        //         beatNumber = 0;
        //     }
        //     else
        //     {
        //         beatNumber++;
        //     }
        // }
        
        if (this != null)
        {
            if (beatNumber % 4 == 3)
            {
                Debug.Log("No Movement on 4th Beat");
            }
            else if (CanGroupMove())
            {
                DoMove(beatNumber % 4);
            }
            else
            {
                RerollMove();
            }
            beatNumber++;
            
            //Robin: New Group Move Logic
        }
    }
    
    //-------------------------------- Group Stop Move ---------------------------------
    
    private bool CanGroupMove()
    {
        Vector3 direction = GetDirectionVector(moveList[beatNumber % moveList.Length]);
        foreach (GameObject enemy in enemyList)
        {
            if (enemy != null)
            {
                Basic_Enemy enemyScript = enemy.GetComponent<Basic_Enemy>();
                if (enemyScript.RaycastForWall(direction))
                {
                    return false;
                }
            }
            //Robin: If an Enemy Detected the Walls
        }
        return true;
        //Robin: If no Enemy Detected the Walls
    }
    
    private Vector3 GetDirectionVector(Direction dir)
    {
        switch (dir)
        {
            case Direction.Forward:
                return Vector3.forward;
            case Direction.Back:
                return Vector3.back;
            case Direction.Left:
                return Vector3.left;
            case Direction.Right:
                return Vector3.right;
            case Direction.BackLeft:
                return (Vector3.back + Vector3.left).normalized;
            case Direction.BackRight:
                return (Vector3.back + Vector3.right).normalized;
            default:
                return Vector3.zero;
        }
    }
    
    
    void DoMove(int index)
    {
        Direction move = moveList[index];

        switch (move)
        {
            case Direction.Forward:
                MoveForward();
                break;
            case Direction.Back:
                MoveBack();
                break;
            case Direction.Left:
                MoveLeft();
                break;
            case Direction.Right:
                MoveRight();
                break;
            case Direction.Reroll:
                RerollMove();
                break;
            case Direction.BackLeft:
                MoveBackLeft();
                break;
            case Direction.BackRight:
                MoveBackRight();
                break;
        }

    }

    //------------------------------- Reroll --------------------------------
    void RerollMove()
    {
        Debug.Log("Rerolling!");

        if (isTriangleMove)
        {
            for (int i = 0; i < 3; i++)
            {
                Direction randomDirection = (Direction)Random.Range(3, 6);
                moveList[i] = randomDirection;
            }
        }
        else
        {
            for (int i = 0; i < 3; i++)
            {
                Direction randomDirection = (Direction)Random.Range(0, 4);
                moveList[i] = randomDirection;
            }
        }
        moveList[3] = Direction.Reroll;
    }

    //-------------------------------- Move ---------------------------------
    void MoveForward()
    {
        for(int i = 0; i < enemyList.Length; i++)
        {
            if (enemyList[i] != null)
            {
                enemyList[i].GetComponent<Basic_Enemy>().MoveDirction(Vector3.forward);
            }
        }

        Debug.Log("Move Forward Together");
    }

    void MoveBack()
    {
        for (int i = 0; i < enemyList.Length; i++)
        {
            if (enemyList[i] != null)
            {
                enemyList[i].GetComponent<Basic_Enemy>().MoveDirction(Vector3.back);
            }
        }

        Debug.Log("Move Back Together");
    }

    void MoveLeft()
    {
        for (int i = 0; i < enemyList.Length; i++)
        {
            if (enemyList[i] != null)
            {
                enemyList[i].GetComponent<Basic_Enemy>().MoveDirction(Vector3.left);
            }
        }

        Debug.Log("Move Left Together");
    }

    void MoveRight()
    {
        for (int i = 0; i < enemyList.Length; i++)
        {
            if (enemyList[i] != null)
            {
                enemyList[i].GetComponent<Basic_Enemy>().MoveDirction(Vector3.right);
            }
        }

        Debug.Log("Move Right Together");
    }

    void MoveBackRight()
    {
        Vector3 BR = (Vector3.back + Vector3.right).normalized;

        for (int i = 0; i < enemyList.Length; i++)
        {
            if (enemyList[i] != null)
            {
                enemyList[i].GetComponent<Basic_Enemy>().MoveDirction(BR);
            }
        }

        Debug.Log("Move Right Together");
    }

    void MoveBackLeft()
    {
        Vector3 BL = (Vector3.back + Vector3.left).normalized;

        for (int i = 0; i < enemyList.Length; i++)
        {
            if (enemyList[i] != null)
            {
                enemyList[i].GetComponent<Basic_Enemy>().MoveDirction(BL);
            }
        }

        Debug.Log("Move Right Together");
    }

    //---------------------------- Destroy --------------------------------------
    public void CheckEnemies()
    {
        bool allNull = true;

        foreach (GameObject enemy in enemyList)
        {
            if (enemy != null)
            {
                allNull = false;
                break;
            }
        }

        if (allNull)
        {
            Destroy(gameObject);
        }
    }
}
