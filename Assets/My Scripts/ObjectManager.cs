using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    public ObjectManager instance;

    public GameObject Prefab_Enemy_Cone;
    public GameObject Prefab_Enemy_Ring;
    public GameObject Prefab_Enemy_Satellite;
    public GameObject Prefab_Enemy_Starknife;

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

    GameObject[] Enemy_Cone;
    GameObject[] Enemy_Ring;
    GameObject[] Enemy_Satellite;
    GameObject[] Enemy_Starknife;

    // 아이템 오브젝트(쉴드 상태로)
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

    // 풀에서 꺼내는데 사용되는 배열
    GameObject[] targetPool;


    private void Awake()
    {
        Enemy_Cone = new GameObject[30];
        Enemy_Ring = new GameObject[30];
        Enemy_Satellite = new GameObject[30];
        Enemy_Starknife = new GameObject[30];

        Item_Power = new GameObject[5];
        Item_Life = new GameObject[5];
        Item_Speed = new GameObject[5];
        Item_Ult = new GameObject[5];
        Item_GuideAttack = new GameObject[5];

        Bullet_Player_Default = new GameObject[200];
        Bullet_Player_MaxPower = new GameObject[50];
        Bullet_Player_Charge = new GameObject[10];
        Bullet_Player_Guide = new GameObject[10];

        Bullet_Enemy_Blue = new GameObject[100];
        Bullet_Enemy_Green = new GameObject[100];
        Bullet_Enemy_Orange = new GameObject[100];
        Bullet_Enemy_Red = new GameObject[300];
        Bullet_Enemy_Red_Big = new GameObject[100];

        Generate();

        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else Destroy(gameObject);
    }

    void Generate()
    {
        // Enemy
        for(int i = 0; i < Enemy_Cone.Length; i++)
        {
            Enemy_Cone[i] = Instantiate(Prefab_Enemy_Cone);
            Enemy_Cone[i].SetActive(false);
        }

        for(int i = 0; i < Enemy_Ring.Length; i++)
        {
            Enemy_Ring[i] = Instantiate(Prefab_Enemy_Ring);
            Enemy_Ring[i].SetActive(false);
        }

        for (int i = 0; i < Enemy_Satellite.Length; i++)
        {
            Enemy_Satellite[i] = Instantiate(Prefab_Enemy_Satellite);
            Enemy_Satellite[i].SetActive(false);
        }

        for (int i = 0; i < Enemy_Starknife.Length; i++)
        {
            Enemy_Starknife[i] = Instantiate(Prefab_Enemy_Starknife);
            Enemy_Starknife[i].SetActive(false);
        }

        // Item
        for (int i = 0; i < Item_Power.Length; i++)
        {
            Item_Power[i] = Instantiate(Prefab_Item_Power);
            Item_Power[i].SetActive(false);
        }

        for (int i = 0; i < Item_Life.Length; i++)
        {
            Item_Life[i] = Instantiate(Prefab_Item_Life);
            Item_Life[i].SetActive(false);
        }

        for (int i = 0; i < Item_Speed.Length; i++)
        {
            Item_Speed[i] = Instantiate(Prefab_Item_Speed);
            Item_Speed[i].SetActive(false);
        }

        for (int i = 0; i < Item_Ult.Length; i++)
        {
            Item_Ult[i] = Instantiate(Prefab_Item_Ult);
            Item_Ult[i].SetActive(false);
        }

        for (int i = 0; i < Item_GuideAttack.Length; i++)
        {
            Item_GuideAttack[i] = Instantiate(Prefab_Item_GuideAttack);
            Item_GuideAttack[i].SetActive(false);
        }

        // Bullet_Player
        for (int i = 0; i < Bullet_Player_Default.Length; i++)
        {
            Bullet_Player_Default[i] = Instantiate(Prefab_Bullet_Player_Default);
            Bullet_Player_Default[i].SetActive(false);
        }

        for (int i = 0; i < Bullet_Player_MaxPower.Length; i++)
        {
            Bullet_Player_MaxPower[i] = Instantiate(Prefab_Bullet_Player_MaxPower);
            Bullet_Player_MaxPower[i].SetActive(false);
        }

        for (int i = 0; i < Bullet_Player_Charge.Length; i++)
        {
            Bullet_Player_Charge[i] = Instantiate(Prefab_Bullet_Player_Charge);
            Bullet_Player_Charge[i].SetActive(false);
        }

        for (int i = 0; i < Bullet_Player_Guide.Length; i++)
        {
            Bullet_Player_Guide[i] = Instantiate(Prefab_Bullet_Player_Guide);
            Bullet_Player_Guide[i].SetActive(false);
        }

        // Bullet_Enemy
        for (int i = 0; i < Bullet_Enemy_Blue.Length; i++)
        {
            Bullet_Enemy_Blue[i] = Instantiate(Prefab_Bullet_Enemy_Blue);
            Bullet_Enemy_Blue[i].SetActive(false);
        }

        for (int i = 0; i < Bullet_Enemy_Green.Length; i++)
        {
            Bullet_Enemy_Green[i] = Instantiate(Prefab_Bullet_Enemy_Green);
            Bullet_Enemy_Green[i].SetActive(false);
        }

        for (int i = 0; i < Bullet_Enemy_Orange.Length; i++)
        {
            Bullet_Enemy_Orange[i] = Instantiate(Prefab_Bullet_Enemy_Orange);
            Bullet_Enemy_Orange[i].SetActive(false);
        }

        for(int i = 0; i < Bullet_Enemy_Red.Length; i++)
        {
            Bullet_Enemy_Red[i] = Instantiate(Prefab_Bullet_Enemy_Red);
            Bullet_Enemy_Red[i].SetActive(false);
        }

        for (int i = 0; i < Bullet_Enemy_Red_Big.Length; i++)
        {
            Bullet_Enemy_Red_Big[i] = Instantiate(Prefab_Bullet_Enemy_Red_Big);
            Bullet_Enemy_Red_Big[i].SetActive(false);
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
        }

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
