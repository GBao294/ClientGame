using Riptide;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{

    public Transform firePoint;
    public GameObject bulletPrefab;
    public float bulletForce = 20f;
    // Update is called once per frame
   
    public void shoot()
    {
        // Tạo đối tượng đạn
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.AddForce(firePoint.up * bulletForce, ForceMode2D.Impulse);
    }

   

   
    //[MessageHandler((ushort)ServerToClientId.shootNotification)]
    //private  void HandleShootNotification(Message message)
    //{
   
    //    // Thực hiện các hành động liên quan đến việc hiển thị hiệu ứng bắn đạn, cập nhật trạng thái game, v.v.
    //}
}
