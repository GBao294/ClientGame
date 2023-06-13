using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class EnemyFollowPlayer : MonoBehaviour
{

    public float speed;
    public float lineOfSite;
    public float fightingRange;
    //public float fireRate = 1;
    //private float nextFireTime;
    //public GameObject bullet;
    //public GameObject bulletParent;
    private Transform player;
    public Animator animator;
    public HealthBar healthBar;
    private bool hasTarget = false;
    private Transform currentTarget;

    void Update()
    {
        // Kiểm tra xem đã có mục tiêu hiện tại hay chưa
        if (!hasTarget)
        {
            // Tìm người chơi trong vùng phát hiện gần nhất
            float closestDistance = float.MaxValue;
            foreach (Player otherPlayer in Player.list.Values)
            {
                float distanceFromPlayer = Vector2.Distance(otherPlayer.transform.position, transform.position);
                if (distanceFromPlayer < lineOfSite && distanceFromPlayer > fightingRange)
                {
                    if (distanceFromPlayer < closestDistance)
                    {
                        closestDistance = distanceFromPlayer;
                        currentTarget = otherPlayer.transform;
                    }
                }
            }

            // Nếu tìm thấy người chơi, đặt mục tiêu và bật cờ hasTarget
            if (currentTarget != null)
            {
                hasTarget = true;
            }

        }
        else
        {
            if (currentTarget == null)
            {
                hasTarget = false;
                return;
            }
            // Kiểm tra nếu mục tiêu hiện tại ra khỏi vùng
            float distanceFromTarget = Vector2.Distance(currentTarget.position, transform.position);
            if (distanceFromTarget > lineOfSite || distanceFromTarget < fightingRange)
            {
                currentTarget = null;
                // Tìm người chơi khác trong vùng phát hiện
                foreach (Player otherPlayer in Player.list.Values)
                {
                    if (otherPlayer.transform != currentTarget)
                    {
                        float distanceFromPlayer = Vector2.Distance(otherPlayer.transform.position, transform.position);
                        if (distanceFromPlayer < lineOfSite && distanceFromPlayer > fightingRange)
                        {
                            currentTarget = otherPlayer.transform;
                            break;
                        }
                    }
                }
            }
        }

        // Nếu có mục tiêu, chạy đến mục tiêu
        if (currentTarget != null)
        {
            transform.position = Vector2.MoveTowards(transform.position, currentTarget.position, speed * Time.deltaTime);
        }
    }
    //void TakeDamage(int Damage)
    //{

    //    currentHealth -= Damage;
    //    healthBar.SetHealth(currentHealth);
    //    if (currentHealth <= 0)
    //    {
    //        OnDeath.Invoke();
    //    }
    //}
    public void Death()
    {
        Destroy(gameObject);
        Destroy(healthBar);
    }

    //show area of attack form enemy
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, lineOfSite);
        Gizmos.DrawWireSphere(transform.position, fightingRange);
    }
}
