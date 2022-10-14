using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    public static ObjectManager instance;

    public GameObject Prefab_Enemy_Cone;
    public GameObject Prefab_Enemy_Ring;
    public GameObject Prefab_Enemy_Satellite;
    public GameObject Prefab_Enemy_Starknife;
    public GameObject Prefab_Enemy_Serpent;
    public GameObject Prefab_Enemy_Claw;

    public GameObject Prefab_Item_Shileded_Power;
    public GameObject Prefab_Item_Shileded_Life;
    public GameObject Prefab_Item_Shileded_Speed;
    public GameObject Prefab_Item_Shileded_Ult;
    public GameObject Prefab_Item_Shileded_GuideAttack;

    public GameObject Prefab_Item_Power;
    public GameObject Prefab_Item_Life;
    public GameObject Prefab_Item_Speed;
    public GameObject Prefab_Item_Ult;
    public GameObject Prefab_Item_GuideAttack;

    public GameObject Prefab_Bullet_Player_Default;
    public GameObject Prefab_Bullet_Player_MaxPower;
    public GameObject Prefab_Bullet_Player_Charge;
    public GameObject Prefab_Bullet_Player_Guide;

    public GameObject Prefab_Bullet_Enemy_Blue;
    public GameObject Prefab_Bullet_Enemy_Green;
    public GameObject Prefab_Bullet_Enemy_Orange;
    public GameObject Prefab_Bullet_Enemy_Red;
    public GameObject Prefab_Bullet_Enemy_Red_Big;

    public GameObject Prefab_Obstacle_Bottom1;
    public GameObject Prefab_Obstacle_Bottom2;
    public GameObject Prefab_Obstacle_Bottom_Tile;
    public GameObject Prefab_Obstacle_Top1;
    public GameObject Prefab_Obstacle_Top2;
    public GameObject Prefab_Obstacle_Top_Tile;
    public GameObject Prefab_Obstacle_Metal_Wall;

    // 워프(포탈) 프리팹
    public GameObject Prefab_Warp;

    GameObject[] Enemy_Cone;
    GameObject[] Enemy_Ring;
    GameObject[] Enemy_Satellite;
    GameObject[] Enemy_Starknife;
    GameObject[] Enemy_Serpent;
    GameObject[] Enemy_Claw;

    // 아이템 오브젝트(쉴드 상태로)
    GameObject[] Item_Shielded_Power;
    GameObject[] Item_Shielded_Life;
    GameObject[] Item_Shielded_Speed;
    GameObject[] Item_Shielded_Ult;
    GameObject[] Item_Shielded_GuideAttack;

    GameObject[] Item_Power;
    GameObject[] Item_Life;
    GameObject[] Item_Speed;
    GameObject[] Item_Ult;
    GameObject[] Item_GuideAttack;

    GameObject[] Bullet_Player_Default;
    GameObject[] Bullet_Player_MaxPower;
    GameObject[] Bullet_Player_Charge;
    GameObject[] Bullet_Player_Guide;

    GameObject[] Bullet_Enemy_Blue;
    GameObject[] Bullet_Enemy_Green;
    GameObject[] Bullet_Enemy_Orange;
    GameObject[] Bullet_Enemy_Red;
    GameObject[] Bullet_Enemy_Red_Big;

    GameObject[] Obstacle_Bottom1;
    GameObject[] Obstacle_Bottom2;
    GameObject[] Obstacle_Bottom_Tile;
    GameObject[] Obstacle_Top1;
    GameObject[] Obstacle_Top2;
    GameObject[] Obstacle_Top_Tile;
    GameObject[] Obstacle_Metal_Wall;

    // 워프(포탈)
    GameObject[] Warp;

    // 풀에서 꺼내는데 사용되는 배열
    GameObject[] targetPool;


    private void Awake()
    {
        Enemy_Cone = new GameObject[30];
        Enemy_Ring = new GameObject[30];
        Enemy_Satellite = new GameObject[30];
        Enemy_Starknife = new GameObject[30];
        Enemy_Serpent = new GameObject[20];
        Enemy_Claw = new GameObject[20];

        Item_Shielded_Power = new GameObject[5];
        Item_Shielded_Life = new GameObject[5];
        Item_Shielded_Speed = new GameObject[5];
        Item_Shielded_Ult = new GameObject[5];
        Item_Shielded_GuideAttack = new GameObject[5];

        Item_Power = new GameObject[5];
        Item_Life = new GameObject[5];
        Item_Speed = new GameObject[5];
        Item_Ult = new GameObject[5];
        Item_GuideAttack = new GameObject[5];

        Bullet_Player_Default = new GameObject[200];
        Bullet_Player_MaxPower = new GameObject[50];
        Bullet_Player_Charge = new GameObject[10];
        Bullet_Player_Guide = new GameObject[20];

        Bullet_Enemy_Blue = new GameObject[100];
        Bullet_Enemy_Green = new GameObject[100];
        Bullet_Enemy_Orange = new GameObject[100];
        Bullet_Enemy_Red = new GameObject[300];
        Bullet_Enemy_Red_Big = new GameObject[100];

        Obstacle_Bottom1 = new GameObject[10];
        Obstacle_Bottom2 = new GameObject[10];
        Obstacle_Bottom_Tile = new GameObject[10];
        Obstacle_Top1 = new GameObject[10];
        Obstacle_Top2 = new GameObject[10];
        Obstacle_Top_Tile = new GameObject[10];
        Obstacle_Metal_Wall = new GameObject[10];

        Warp = new GameObject[20];

        Generate();

        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Generate()
    {
        // Enemy
        for(int i = 0; i < Enemy_Cone.Length; i++)
        {
            Enemy_Cone[i] = Instantiate(Prefab_Enemy_Cone);
            DontDestroyOnLoad(Enemy_Cone[i]);
            Enemy_Cone[i].SetActive(false);
        }

        for(int i = 0; i < Enemy_Ring.Length; i++)
        {
            Enemy_Ring[i] = Instantiate(Prefab_Enemy_Ring);
            DontDestroyOnLoad(Enemy_Ring[i]);
            Enemy_Ring[i].SetActive(false);
        }

        for (int i = 0; i < Enemy_Satellite.Length; i++)
        {
            Enemy_Satellite[i] = Instantiate(Prefab_Enemy_Satellite);
            DontDestroyOnLoad(Enemy_Satellite[i]);
            Enemy_Satellite[i].SetActive(false);
        }

        for (int i = 0; i < Enemy_Starknife.Length; i++)
        {
            Enemy_Starknife[i] = Instantiate(Prefab_Enemy_Starknife);
            DontDestroyOnLoad(Enemy_Starknife[i]);
            Enemy_Starknife[i].SetActive(false);
        }

        for (int i = 0; i < Enemy_Serpent.Length; i++)
        {
            Enemy_Serpent[i] = Instantiate(Prefab_Enemy_Serpent);
            DontDestroyOnLoad(Enemy_Serpent[i]);
            Enemy_Serpent[i].SetActive(false);
        }

        for (int i = 0; i < Enemy_Claw.Length; i++)
        {
            Enemy_Claw[i] = Instantiate(Prefab_Enemy_Claw);
            DontDestroyOnLoad(Enemy_Claw[i]);
            Enemy_Claw[i].SetActive(false);
        }

        // Item_Shielded
        for (int i = 0; i < Item_Shielded_Power.Length; i++)
        {
            Item_Shielded_Power[i] = Instantiate(Prefab_Item_Shileded_Power);
            DontDestroyOnLoad(Item_Shielded_Power[i]);
            Item_Shielded_Power[i].SetActive(false);
        }

        for (int i = 0; i < Item_Shielded_Life.Length; i++)
        {
            Item_Shielded_Life[i] = Instantiate(Prefab_Item_Shileded_Life);
            DontDestroyOnLoad(Item_Shielded_Life[i]);
            Item_Shielded_Life[i].SetActive(false);
        }

        for (int i = 0; i < Item_Shielded_Speed.Length; i++)
        {
            Item_Shielded_Speed[i] = Instantiate(Prefab_Item_Shileded_Speed);
            DontDestroyOnLoad(Item_Shielded_Speed[i]);
            Item_Shielded_Speed[i].SetActive(false);
        }

        for (int i = 0; i < Item_Shielded_Ult.Length; i++)
        {
            Item_Shielded_Ult[i] = Instantiate(Prefab_Item_Shileded_Ult);
            DontDestroyOnLoad(Item_Shielded_Ult[i]);
            Item_Shielded_Ult[i].SetActive(false);
        }

        for (int i = 0; i < Item_Shielded_GuideAttack.Length; i++)
        {
            Item_Shielded_GuideAttack[i] = Instantiate(Prefab_Item_Shileded_GuideAttack);
            DontDestroyOnLoad(Item_Shielded_GuideAttack[i]);
            Item_Shielded_GuideAttack[i].SetActive(false);
        }

        // Item
        for (int i = 0; i < Item_Power.Length; i++)
        {
            Item_Power[i] = Instantiate(Prefab_Item_Power);
            DontDestroyOnLoad(Item_Power[i]);
            Item_Power[i].SetActive(false);
        }

        for (int i = 0; i < Item_Life.Length; i++)
        {
            Item_Life[i] = Instantiate(Prefab_Item_Life);
            DontDestroyOnLoad(Item_Life[i]);
            Item_Life[i].SetActive(false);
        }

        for (int i = 0; i < Item_Speed.Length; i++)
        {
            Item_Speed[i] = Instantiate(Prefab_Item_Speed);
            DontDestroyOnLoad(Item_Speed[i]);
            Item_Speed[i].SetActive(false);
        }

        for (int i = 0; i < Item_Ult.Length; i++)
        {
            Item_Ult[i] = Instantiate(Prefab_Item_Ult);
            DontDestroyOnLoad(Item_Ult[i]);
            Item_Ult[i].SetActive(false);
        }

        for (int i = 0; i < Item_GuideAttack.Length; i++)
        {
            Item_GuideAttack[i] = Instantiate(Prefab_Item_GuideAttack);
            DontDestroyOnLoad(Item_GuideAttack[i]);
            Item_GuideAttack[i].SetActive(false);
        }

        // Bullet_Player
        for (int i = 0; i < Bullet_Player_Default.Length; i++)
        {
            Bullet_Player_Default[i] = Instantiate(Prefab_Bullet_Player_Default);
            DontDestroyOnLoad(Bullet_Player_Default[i]);
            Bullet_Player_Default[i].SetActive(false);
        }

        for (int i = 0; i < Bullet_Player_MaxPower.Length; i++)
        {
            Bullet_Player_MaxPower[i] = Instantiate(Prefab_Bullet_Player_MaxPower);
            DontDestroyOnLoad(Bullet_Player_MaxPower[i]);
            Bullet_Player_MaxPower[i].SetActive(false);
        }

        for (int i = 0; i < Bullet_Player_Charge.Length; i++)
        {
            Bullet_Player_Charge[i] = Instantiate(Prefab_Bullet_Player_Charge);
            DontDestroyOnLoad(Bullet_Player_Charge[i]);
            Bullet_Player_Charge[i].SetActive(false);
        }

        for (int i = 0; i < Bullet_Player_Guide.Length; i++)
        {
            Bullet_Player_Guide[i] = Instantiate(Prefab_Bullet_Player_Guide);
            DontDestroyOnLoad(Bullet_Player_Guide[i]);
            Bullet_Player_Guide[i].SetActive(false);
        }

        // Bullet_Enemy
        for (int i = 0; i < Bullet_Enemy_Blue.Length; i++)
        {
            Bullet_Enemy_Blue[i] = Instantiate(Prefab_Bullet_Enemy_Blue);
            DontDestroyOnLoad(Bullet_Enemy_Blue[i]);
            Bullet_Enemy_Blue[i].SetActive(false);
        }

        for (int i = 0; i < Bullet_Enemy_Green.Length; i++)
        {
            Bullet_Enemy_Green[i] = Instantiate(Prefab_Bullet_Enemy_Green);
            DontDestroyOnLoad(Bullet_Enemy_Green[i]);
            Bullet_Enemy_Green[i].SetActive(false);
        }

        for (int i = 0; i < Bullet_Enemy_Orange.Length; i++)
        {
            Bullet_Enemy_Orange[i] = Instantiate(Prefab_Bullet_Enemy_Orange);
            DontDestroyOnLoad(Bullet_Enemy_Orange[i]);
            Bullet_Enemy_Orange[i].SetActive(false);
        }

        for(int i = 0; i < Bullet_Enemy_Red.Length; i++)
        {
            Bullet_Enemy_Red[i] = Instantiate(Prefab_Bullet_Enemy_Red);
            DontDestroyOnLoad(Bullet_Enemy_Red[i]);
            Bullet_Enemy_Red[i].SetActive(false);
        }

        for (int i = 0; i < Bullet_Enemy_Red_Big.Length; i++)
        {
            Bullet_Enemy_Red_Big[i] = Instantiate(Prefab_Bullet_Enemy_Red_Big);
            DontDestroyOnLoad(Bullet_Enemy_Red_Big[i]);
            Bullet_Enemy_Red_Big[i].SetActive(false);
        }

        // 장애물
        for(int i = 0; i < Obstacle_Bottom1.Length; i++)
        {
            Obstacle_Bottom1[i] = Instantiate(Prefab_Obstacle_Bottom1);
            DontDestroyOnLoad(Obstacle_Bottom1[i]);
            Obstacle_Bottom1[i].SetActive(false);
        }
        for (int i = 0; i < Obstacle_Bottom2.Length; i++)
        {
            Obstacle_Bottom2[i] = Instantiate(Prefab_Obstacle_Bottom2);
            DontDestroyOnLoad(Obstacle_Bottom2[i]);
            Obstacle_Bottom2[i].SetActive(false);
        }
        for (int i = 0; i < Obstacle_Bottom_Tile.Length; i++)
        {
            Obstacle_Bottom_Tile[i] = Instantiate(Prefab_Obstacle_Bottom_Tile);
            DontDestroyOnLoad(Obstacle_Bottom_Tile[i]);
            Obstacle_Bottom_Tile[i].SetActive(false);
        }
        for (int i = 0; i < Obstacle_Top1.Length; i++)
        {
            Obstacle_Top1[i] = Instantiate(Prefab_Obstacle_Top1);
            DontDestroyOnLoad(Obstacle_Top1[i]);
            Obstacle_Top1[i].SetActive(false);
        }
        for (int i = 0; i < Obstacle_Top2.Length; i++)
        {
            Obstacle_Top2[i] = Instantiate(Prefab_Obstacle_Top2);
            DontDestroyOnLoad(Obstacle_Top2[i]);
            Obstacle_Top2[i].SetActive(false);
        }
        for (int i = 0; i < Obstacle_Top_Tile.Length; i++)
        {
            Obstacle_Top_Tile[i] = Instantiate(Prefab_Obstacle_Top_Tile);
            DontDestroyOnLoad(Obstacle_Top_Tile[i]);
            Obstacle_Top_Tile[i].SetActive(false);
        }
        for (int i = 0; i < Obstacle_Metal_Wall.Length; i++)
        {
            Obstacle_Metal_Wall[i] = Instantiate(Prefab_Obstacle_Metal_Wall);
            DontDestroyOnLoad(Obstacle_Metal_Wall[i]);
            Obstacle_Metal_Wall[i].SetActive(false);
        }

        // Warp
        for (int i = 0; i < Warp.Length; i++)
        {
            Warp[i] = Instantiate(Prefab_Warp);
            DontDestroyOnLoad(Warp[i]);
            Warp[i].SetActive(false);
        }
    }
    
    public GameObject MakeObj(string name)
    {
        switch(name)
        {
            case "Enemy_Cone":
                targetPool = Enemy_Cone;                
                break;
            case "Enemy_Ring":
                targetPool = Enemy_Ring;
                break;
            case "Enemy_Satellite":
                targetPool = Enemy_Satellite;
                break;
            case "Enemy_Starknife":
                targetPool = Enemy_Starknife;
                break;
            case "Enemy_Serpent":
                targetPool = Enemy_Serpent;
                break;
            case "Enemy_Claw":
                targetPool = Enemy_Claw;
                break;
            case "Item_Shielded_Power":
                targetPool = Item_Shielded_Power;
                break;
            case "Item_Shielded_Life":
                targetPool = Item_Shielded_Life;
                break;
            case "Item_Shielded_Speed":
                targetPool = Item_Shielded_Speed;
                break;
            case "Item_Shielded_Ult":
                targetPool = Item_Shielded_Ult;
                break;
            case "Item_Shielded_GuideAttack":
                targetPool = Item_Shielded_GuideAttack;
                break;
            case "Item_Power":
                targetPool = Item_Power;
                break;
            case "Item_Life":
                targetPool = Item_Life;
                break;
            case "Item_Speed":
                targetPool = Item_Speed;
                break;
            case "Item_Ult":
                targetPool = Item_Ult;
                break;
            case "Item_GuideAttack":
                targetPool = Item_GuideAttack;
                break;
            case "Bullet_Player_Default":
                targetPool = Bullet_Player_Default;
                break;
            case "Bullet_Player_MaxPower":
                targetPool = Bullet_Player_MaxPower;
                break;
            case "Bullet_Player_Charge":
                targetPool = Bullet_Player_Charge;
                break;
            case "Bullet_Player_Guide":
                targetPool = Bullet_Player_Guide;
                break;
            case "Bullet_Enemy_Blue":
                targetPool = Bullet_Enemy_Blue;
                break;
            case "Bullet_Enemy_Green":
                targetPool = Bullet_Enemy_Green;
                break;
            case "Bullet_Enemy_Orange":
                targetPool = Bullet_Enemy_Orange;
                break;
            case "Bullet_Enemy_Red_Big":
                targetPool = Bullet_Enemy_Red_Big;
                break;
            case "Bullet_Enemy_Red":
                targetPool = Bullet_Enemy_Red;
                break;
            case "Warp":
                targetPool = Warp;
                break;
            case "Obstacle_Bottom1":
                targetPool = Obstacle_Bottom1;
                break;
            case "Obstacle_Bottom2":
                targetPool = Obstacle_Bottom2;
                break;
            case "Obstacle_Bottom_Tile":
                targetPool = Obstacle_Bottom_Tile;
                break;
            case "Obstacle_Top1":
                targetPool = Obstacle_Top1;
                break;
            case "Obstacle_Top2":
                targetPool = Obstacle_Top2;
                break;
            case "Obstacle_Top_Tile":
                targetPool = Obstacle_Top_Tile;
                break;
            case "Obstacle_Metal_Wall":
                targetPool = Obstacle_Metal_Wall;
                break;
            default:
                Debug.Log("ObjectManager.MakeObj's switch-case is wrong");
                break;
        }

        if (targetPool == null) return null;

        for(int i = 0; i < targetPool.Length; i++)
        {
            if (!targetPool[i].activeSelf)
            {
                targetPool[i].SetActive(true);

                return targetPool[i];
            }
        }

        return null;
    }
}
