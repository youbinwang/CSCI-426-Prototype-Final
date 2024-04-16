using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SonicBloom.Koreo;
using Unity.VisualScripting;

public class Basic_Enemy : MonoBehaviour
{
    public enum Action
    {
        Dash,
        Attack,
        Aim,
        Shoot,
        Stay,
        Movetogether,
    }

    [Header("Beat Track")]
    [SerializeField] public KoreographyTrack trackName;

    [Header("Enemy HP")]
    [SerializeField] public int enemyHp = 1;
    private bool isEnemyDie;
    [SerializeField] public bool isTriangle;

    [Header("Enemy Action List")]
    [SerializeField] public Action[] actions = new Action[4];
    private int beatNumber;

    [Header("Enemy Dash Parameters")]
    [SerializeField] public float moveDistance = 1.0f;
    [SerializeField] public float moveDuration = 0.25f;

    [Header("Enemy Attack Parameters")]
    [SerializeField] public Transform attackTransform;
    [SerializeField] public float explosionScaleValue = 5f;
    private Vector3 explosionScale;
    [SerializeField] public float explosionDuration = 0.25f;

    [Header("Enemy Aiming Parameters")]
    [SerializeField] public LineRenderer aimLine;
    private bool isAiming;
    [SerializeField] public float aimingTime = 0.4f;

    //[SerializeField] public float aimingMoveDistance = 1.0f;
    //[SerializeField] public float aimingMoveDuration = 0.25f;
    private Transform player;

    [Header("Enemy Shoot Parameters")]
    [SerializeField] public GameObject laserPrefab;
    [SerializeField] public float laserLength = 100f;
    private Vector3 playerLastPosition;

    //Wall Detect
    [Header("Wall Detect")]
    public List<Vector3> possibleDirections = new List<Vector3>() { Vector3.forward, Vector3.back, Vector3.left, Vector3.right };
    private List<Vector3> initialDirections = new List<Vector3>() { Vector3.forward, Vector3.back, Vector3.left, Vector3.right };

    public List<Vector3> possibleDirections_T = new List<Vector3>() { (Vector3.back + Vector3.left).normalized, (Vector3.back + Vector3.right).normalized, Vector3.forward };
    private List<Vector3> initialDirections_T = new List<Vector3>() { (Vector3.back + Vector3.left).normalized, (Vector3.back + Vector3.right).normalized, Vector3.forward };

    public LayerMask wallLayer;
    public GradeSystem gradeSystem;

    // Start is called before the first frame update
    void Start()
    {
        Koreographer.Instance.RegisterForEvents(trackName.EventID, EnemyAction);

        player = GameObject.FindWithTag("Player").transform;

        beatNumber = 1; // Enemy Attack Count
        explosionScale = new Vector3(explosionScaleValue, explosionScaleValue, explosionScaleValue);
        CancelAimLine();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (enemyHp <= 0)
        {
            DestroyEnemy();
        }

        if (isAiming)
        {
            DrawAimLine();
        }

    }

    void EnemyAction(KoreographyEvent koreoEvent)//When On the Beat
    {
        if (this != null)
        {
            DoAction(beatNumber);

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

    void DoAction(int index)
    {
        Action action = actions[index];

        switch (action)
        {
            case Action.Dash:
                Dash();
                break;
            case Action.Attack:
                Attack();
                break;
            case Action.Aim:
                Aim();
                break;
            case Action.Shoot:
                Shoot();
                break;
            case Action.Stay:
                Stay();
                break;
            case Action.Movetogether:
                Movetogether();
                break;
        }
    }

    //------------------------------------------ Stay ---------------------------------------
    void Stay()
    {
        Debug.Log("Enemy Stay");
    }

    //------------------------------------------ Movetogether -------------------------------
    void Movetogether()
    {
        Debug.Log("Enemy Move Together");
    }

    public void MoveDirction(Vector3 dir)
    {
        if (!RaycastForWall(dir))
        {
            Vector3 targetPosition = transform.position + dir * moveDistance;
            StartCoroutine(MoveToPosition(transform.position, targetPosition, moveDuration));
            //Pass here to move in a group
        }
    }

    //------------------------------------------- Dash/Move ----------------------------------
    void Dash()
    {
        if (isTriangle)
        {
            Vector3 targetPosition = transform.position + RandomTriangleDirection() * moveDistance;
            StartCoroutine(MoveToPosition(transform.position, targetPosition, moveDuration));
            ResetDirections();
        }
        else
        {
            Vector3 targetPosition = transform.position + RandomSquareDirection() * moveDistance;
            StartCoroutine(MoveToPosition(transform.position, targetPosition, moveDuration));
            ResetDirections();
        }

        Debug.Log("Enemy Dash");
    }

    Vector3 RandomSquareDirection()
    {
        //List<Vector3> possibleDirections = new List<Vector3>() { Vector3.forward, Vector3.back, Vector3.left, Vector3.right };


        for (int i = possibleDirections.Count - 1; i >= 0; i--)
        {
            if (RaycastForWall(possibleDirections[i]))
            {
                possibleDirections.RemoveAt(i);

            }
        }

        if (possibleDirections.Count > 0)
        {
            int randomIndex = Random.Range(0, possibleDirections.Count);
            return possibleDirections[randomIndex];

        }
        else
        {
            return Vector3.zero;
        }
    }

    Vector3 RandomTriangleDirection()
    {
        for (int i = possibleDirections_T.Count - 1; i >= 0; i--)
        {
            if (RaycastForWall(possibleDirections_T[i]))
            {
                possibleDirections_T.RemoveAt(i);

            }
        }

        if (possibleDirections_T.Count > 0)
        {
            int randomIndex = Random.Range(0, possibleDirections_T.Count);
            return possibleDirections_T[randomIndex];

        }
        else
        {
            return Vector3.zero;
        }
    }

    bool RaycastForWall(Vector3 direction)
    {
        RaycastHit hit;

        Vector3 rayEndPoint = transform.position + direction * moveDistance;


        if (Physics.Raycast(transform.position, direction, out hit, moveDistance + 0.1f, wallLayer))
        {

            //Debug.Log("111");
            return true;
        }
        else
        {

            Debug.DrawLine(transform.position, rayEndPoint, Color.green, 2f);
        }

        return false;

    }

    void ResetDirections()
    {
        possibleDirections = new List<Vector3>(initialDirections);
        possibleDirections_T = new List<Vector3>(initialDirections_T);
    }

    IEnumerator MoveToPosition(Vector3 fromPosition, Vector3 toPosition, float duration)
    {
        float elapsed = 0.0f;
        while (elapsed < duration)
        {
            transform.position = Vector3.Lerp(fromPosition, toPosition, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.position = toPosition;
    }

    //------------------------------------------- Attack ----------------------------------
    void Attack()
    {
        if (attackTransform != null)
        {
            attackTransform.localScale = explosionScale;
            StartCoroutine(ResetAndHideAttack());
        }

        //Debug.Log("Enemy Attack");
    }

    IEnumerator ResetAndHideAttack()
    {

        yield return new WaitForSeconds(explosionDuration);

        attackTransform.localScale = Vector3.zero;
    }

    //------------------------------------------- Aiming ----------------------------------
    void Aim()
    {
        if (!isAiming)
        {
            isAiming = true;
            playerLastPosition = player.position;
        }

        //isAiming = true;
        StartCoroutine(AimingFinish());
        //Move When Aiming
        //Vector3 targetPosition = transform.position + RandomSquareDirection() * aimingMoveDistance;
        //StartCoroutine(MoveToPosition(transform.position, targetPosition, aimingMoveDuration));

        Debug.Log("Enemy ReadyToShoot");
    }

    IEnumerator AimingFinish()
    {

        yield return new WaitForSeconds(aimingTime);

        isAiming = false;
        CancelAimLine();
    }

    void DrawAimLine()
    {
        if (player == null || aimLine == null)
        {
            Debug.LogWarning("Player or Aim Line not assigned.");
            return;
        }

        aimLine.positionCount = 2;
        aimLine.SetPosition(0, transform.position);
        aimLine.SetPosition(1, playerLastPosition);
    }

    void CancelAimLine()
    {
        if (aimLine != null)
        {
            aimLine.positionCount = 0; // 清空瞄准线
        }

        //playerLastPosition = player.position;
    }

    //------------------------------------------- Shoot ----------------------------------
    void Shoot()
    {
        FireLaser();

        Debug.Log("Enemy Shoot");
    }

    void FireLaser()
    {
        if (player == null || laserPrefab == null)
        {
            Debug.LogWarning("Player or Laser Prefab not assigned.");
            return;
        }

        Vector3 direction = (playerLastPosition - transform.position).normalized;

        //Vector3 startPosition = (transform.position + playerLastPosition)/2f;

        Vector3 startPosition = transform.position + direction.normalized * (laserLength / 2);

        Vector3 endPosition = playerLastPosition;

        CreateLaser(startPosition, endPosition);
    }

    void CreateLaser(Vector3 startPosition, Vector3 endPosition)
    {
        //float laserLength = Vector3.Distance(transform.position, endPosition);

        GameObject laser = Instantiate(laserPrefab, startPosition, Quaternion.identity);
        laser.transform.LookAt(endPosition);

        laser.transform.localScale = new Vector3(laser.transform.localScale.x, laser.transform.localScale.y, laserLength);
    }

    //------------------------------------------- Die ----------------------------------
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            //Debug.Log("111");
            enemyHp--;
        }
    }

    void DestroyEnemy()
    {
        GradeSystem.TriggerEnemDestroyed();
        AudioManager.TriggerEnemDestroyed();
        Destroy(gameObject);
    }
}
