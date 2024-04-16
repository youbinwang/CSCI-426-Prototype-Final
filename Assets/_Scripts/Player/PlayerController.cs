using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    public float dashDistance = 10.0f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 1.0f;
    public Image cooldownRing; // UI圆环，用于显示冷却状态
    public Image healthBar;

    private Vector3 moveDirection = Vector3.zero;
    private bool isDashing = false;
    private float dashCooldownTimer = 0f;
    public float health;
    public float originalHealth;
    //public bool isInvisible;

    void Update()
    {
        
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

        
        if (Input.GetMouseButtonDown(0) && dashCooldownTimer <= 0f) 
        {
            StartCoroutine(Dash());
        }

        
        if (dashCooldownTimer > 0f)
        {
            dashCooldownTimer -= Time.deltaTime;
            cooldownRing.fillAmount = dashCooldownTimer / dashCooldown; 
        }

        if(health <= 0)
        {
            Destroy(gameObject);
        }

        healthBar.fillAmount = health / originalHealth;
    }

    System.Collections.IEnumerator Dash()
    {
        isDashing = true;
        float startTime = Time.time;

        while (Time.time < startTime + dashDuration)
        {
            transform.position += moveDirection * (dashDistance / dashDuration) * Time.deltaTime;
            yield return null;
        }

        dashCooldownTimer = dashCooldown; 
        isDashing = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "EnemyAttack")
        {
            health--;
        }
    }
}
