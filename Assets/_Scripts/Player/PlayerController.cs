using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using MoreMountains.Feedbacks;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    public float dashDistance = 10.0f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 1.0f;
    public Image cooldownRing; // UIԲ����������ʾ��ȴ״̬
    public Image healthBar;

    private Vector3 moveDirection = Vector3.zero;
    private bool isDashing = false;
    private float dashCooldownTimer = 0f;
    public float health;
    public float originalHealth;
    /*public Transform hpTransfrom;
    public Image hpBar;*/
    //public bool isInvisible;

    [Header("Player Feedbacks")]
    [SerializeField] public MMFeedbacks playerHurt;
    [SerializeField] public MMFeedbacks killEnemy;
    [SerializeField] public MMFeedbacks playerDie;
    [SerializeField] public MMFeedbacks playerFlicker;

    [Header("HP")]
    [SerializeField] public GameObject HP_1;
    [SerializeField] public GameObject HP_2;
    [SerializeField] public GameObject HP_3;
    [SerializeField] public GameObject HP_4;

    private void Start()
    {
        ResetHP();
    }


    void Update()
    {
        //hpBar.transform.position = hpTransfrom.position;

        if (!isDashing) 
        {
            float moveHorizontal = Input.GetAxisRaw("Horizontal");
            float moveVertical = Input.GetAxisRaw("Vertical");
            moveDirection = new Vector3(moveVertical, 0.0f, -moveHorizontal).normalized;

            if (moveDirection != Vector3.zero)
            {
                transform.position += moveDirection * moveSpeed * Time.deltaTime;
            }
        }

        
        if (Input.GetKeyDown(KeyCode.Space) && dashCooldownTimer <= 0f) 
        {
            StartCoroutine(Dash());
        }

        
        if (dashCooldownTimer > 0f)
        {
            dashCooldownTimer -= Time.deltaTime;
            cooldownRing.fillAmount = dashCooldownTimer / dashCooldown; 
        }
        else if(dashCooldownTimer < 0f)
        {
            //Dash CD Finish
            playerFlicker.PlayFeedbacks(gameObject.transform.position);

            dashCooldownTimer = 0;
        }

        if(health <= 0)
        {
            PlayerDied();
        }

        //healthBar.fillAmount = health / originalHealth;

        //HP Change
        if(health == 5)
        {
            ResetHP();
        }
        else if(health == 4)
        {
            HP3();
        }
        else if (health == 3)
        {
            HP2();
        }
        else if (health == 2)
        {
            HP1();
        }
        else if (health == 1)
        {
            HP0();
        }
        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
            Debug.Log("Game Quit");
        }
    }

    void PlayerDied()
    {
        playerDie.PlayFeedbacks(gameObject.transform.position);
        Destroy(gameObject);
    }
    

    IEnumerator Dash()
    {
        isDashing = true;
        float startTime = Time.time;
        int wallLayer = LayerMask.GetMask("Wall");

        // while (Time.time < startTime + dashDuration)
        // {
        //     transform.position += moveDirection * (dashDistance / dashDuration) * Time.deltaTime;
        //     yield return null;
        // }
        
        
        if (!Physics.Raycast(transform.position, moveDirection, dashDistance, wallLayer))
        {
            while (Time.time < startTime + dashDuration)
            {
                transform.position += moveDirection * (dashDistance / dashDuration) * Time.deltaTime;
                yield return null;
            }
        }
        else
        {
            playerFlicker.PlayFeedbacks(gameObject.transform.position);
            Debug.Log("Blocked by a wall!");
        }
        
        dashCooldownTimer = dashCooldown; 
        isDashing = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "EnemyAttack")
        {
            health--;
            playerHurt.PlayFeedbacks(gameObject.transform.position);
            //palyerHurtParticle.Play(gameObject.transform.position,1);

            // Camera Shake
            Cinemachine.CinemachineImpulseSource impulseSource = Camera.main.GetComponent<Cinemachine.CinemachineImpulseSource>();
            if (impulseSource != null)
            {
                impulseSource.GenerateImpulse();
            }
        }
    }

    void ResetHP()
    {
        HP_1.SetActive(true);
        HP_2.SetActive(true);
        HP_3.SetActive(true);
        HP_4.SetActive(true);
    }

    void HP3()
    {
        HP_1.SetActive(true);
        HP_2.SetActive(true);
        HP_3.SetActive(true);
        HP_4.SetActive(false);
    }

    void HP2()
    {
        HP_1.SetActive(true);
        HP_2.SetActive(true);
        HP_3.SetActive(false);
        HP_4.SetActive(false);
    }

    void HP1()
    {
        HP_1.SetActive(true);
        HP_2.SetActive(false);
        HP_3.SetActive(false);
        HP_4.SetActive(false);
    }

    void HP0()
    {
        HP_1.SetActive(false);
        HP_2.SetActive(false);
        HP_3.SetActive(false);
        HP_4.SetActive(false);
    }
}
