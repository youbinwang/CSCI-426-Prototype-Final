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

    // Start is called before the first frame update
    void Start()
    {
        Koreographer.Instance.RegisterForEvents(trackName.EventID, GroupAction);
        beatNumber = 0; // Enemy Movement Count

        moveList = new Direction[4];
        RerollMove();
    }

    // Update is called once per frame
    void Update()
    {
        CheckEnemies();
    }

    void GroupAction(KoreographyEvent koreoEvent)//When On the Beat
    {
        if (this != null)
        {
            DoMove(beatNumber);

            if (beatNumber == 3)
            {
                beatNumber = 0;
            }
            else
            {
                beatNumber++;
            }
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
        Debug.Log("Rerolling !");

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
