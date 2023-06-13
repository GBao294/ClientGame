using Cainos.PixelArtTopDown_Basic;
using Riptide;
using Riptide.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public static Dictionary<ushort, Player> list = new Dictionary<ushort, Player>();
    [SerializeField] public GameObject model;
    public ushort Id { get; private set; }
    public bool IsLocal { get; private set; }
    private string username;
    // [SerializeField] private PlayerAnimationManager animationManager;
   
    [SerializeField] private Transform firePoint;
    [SerializeField] private Interpolator interpolator;
    private Shooting getss;
    [SerializeField] private Sprite newSprite;
    [SerializeField] private Sprite oldSprite;
    private void Start()
    {
       
    }

    



    private void OnDestroy()
    {
        list.Remove(Id);
    }
    private void Move(Vector2 newPosition)
    {
        transform.position = newPosition;

    }
    
    private void Move(ushort tick ,Vector3 newPosition, Quaternion rotate)
    {

        interpolator.NewUpdate(tick, newPosition);
        firePoint.rotation = rotate;

    }
    public void Respawned(Vector2 position)
    {
        Debug.Log("Nhan duoc thong bao hoi sinh" + position);
        GetComponent<SpriteRenderer>().sprite = oldSprite;



        transform.position = Vector3.zero;
        model.SetActive(true);
        GetComponent<Animator>().enabled = true;
    }
    public ushort GetPlayerId()
    {
        return Id;
    }

    public static void Spawn(ushort id, string username, Vector2 position)
    {
        Player player;
        if (id == NetworkManager.Singleton.Client.Id)
        {
            player = Instantiate(GameLogic.Singleton.LocalPlayerPrefab, position, Quaternion.identity).GetComponent<Player>();
            player.IsLocal = true;

            // Gán player.transform vào biến target trong PlayerCameraController
            CameraFollow cameraController = GameObject.Find("PlayerCamera").GetComponent<CameraFollow>();
            cameraController.target = player.transform;

        }
        else
        {
            player = Instantiate(GameLogic.Singleton.PlayerPrefab, position, Quaternion.identity).GetComponent<Player>();
            player.IsLocal = false;
        }

        player.name = $"Player {id} ({username})";
        player.Id = id;
        player.username = username;
        list.Add(id, player);
        player.getss = player.GetComponent<Shooting>();
    }

    public void Died()

    {
       
        oldSprite = GetComponent<SpriteRenderer>().sprite;
        GetComponent<Animator>().enabled = false;
        GetComponent<SpriteRenderer>().sprite = newSprite;
        if (IsLocal)
            GetComponent<CountDown>().RestartCountdown();
       
        
    }


    

    [MessageHandler((ushort)ServerToClientId.playerSpawned)]
    private static void SpawnPlayer(Message message)
    {

        Spawn(message.GetUShort(), message.GetString(), message.GetVector2());
    }

    [MessageHandler((ushort)ServerToClientId.playerMovement)]
    private static void PlayerMovement(Message message)
    {
        if (list.TryGetValue(message.GetUShort(), out Player player))
        {
            ushort tick = message.GetUShort();
            Vector3 newPosition = message.GetVector2();
            Quaternion newRotation = message.GetQuaternion();

            // Kiểm tra sự thay đổi vị trí hoặc hướng mới
            if (newPosition != player.transform.position || newRotation != player.firePoint.rotation)
            {
                // Cập nhật vị trí và hướng mới
                player.Move(tick,newPosition, newRotation);
            }
        }
    }

    [MessageHandler((ushort)ServerToClientId.shootNotification)]
    private static void PlayerShoot(Message message)
    {

        if (list.TryGetValue(message.GetUShort(), out Player player))
            player.getss.shoot();
    }
    [MessageHandler((ushort)ServerToClientId.playerHealthChanged)]
    private static void PlayerHealthChanged(Message message)
    {
        ushort x = message.GetUShort();
        float y = message.GetFloat();
        Debug.Log("Player" + x + " " + y);
        if (list.TryGetValue(x, out Player player))
            player.GetComponent<Health>().UpdateHealth(y);
    }

    [MessageHandler((ushort)ServerToClientId.playerDied)]
    private static void PlayerDied(Message message)
    {

        if (list.TryGetValue(message.GetUShort(), out Player player))
            player.Died();
        
    }
    [MessageHandler((ushort)ServerToClientId.playerRespawned)]
    private static void PlayerRespawned(Message message)
    {

        if (list.TryGetValue(message.GetUShort(), out Player player))
            player.Respawned(message.GetVector2());
    }

}