using Riptide;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.UIElements;
using UnityEngine.VFX;

public class Enemy : MonoBehaviour
{
    public static Dictionary<ushort, Enemy> list = new Dictionary<ushort, Enemy>();
    public ushort Id { get; private set; }
    [SerializeField] private Transform firePoint;
    bool isAlive = true;
    private static void Spawn (ushort id, Vector2 position)
    {
        Enemy enemy;
        enemy = Instantiate(GameLogic.Singleton.EnemyPrefab, position, Quaternion.identity).GetComponent<Enemy>();
        enemy.Id = id;
        list.Add(id, enemy);
    }


    private void Move(Vector3 newPosition)
    {

        //interpolator.NewUpdate(tick, newPosition);
        transform.position = newPosition;
       
    }
    private void Spin(Quaternion q)
    {
        Debug.Log(q);
        firePoint.rotation = q;
    }


    [MessageHandler((ushort)ServerToClientId.EnemySpawned)]
    private static void SpawnEnemy(Message message)
    {
        Spawn(message.GetUShort(), message.GetVector2());
    }
    [MessageHandler((ushort)ServerToClientId.EnemyMovement)]
    private static void EnemyPos(Message message)
    {
      
        if (list.TryGetValue(message.GetUShort(), out Enemy enemy))
        {
            Vector2 x = message.GetVector2();
            Debug.Log(x);
            enemy.Move(x);
        }

    }
    [MessageHandler((ushort)ServerToClientId.EnemyHealthChanged)]
    private static void EnemyHealthChanged(Message message)
    {
        ushort x = message.GetUShort();
        float y = message.GetFloat();
        Debug.Log("Enemy" + x + " " + y);
        if (list.TryGetValue(x, out Enemy enemy))
            enemy.GetComponent<Health>().UpdateHealth(y);

        if(enemy.GetComponent<Health>().health == 0)
        {
            Destroy(enemy.gameObject);
        }

    }

    
    [MessageHandler((ushort)ServerToClientId.EnemySpin)]
    private static void EnemySpin(Message message)
    {
        if (list.TryGetValue(message.GetUShort(), out Enemy enemy))
        {
            if(enemy.isAlive)
                enemy.Spin(message.GetQuaternion());
            
           
        }
    }
}
