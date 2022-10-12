using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField]
    public string[] _EnemyObjects;
    [SerializeField]
    public string[] _ObstacleObjects;
    [SerializeField]
    public Transform[] _SpawnPos;
    [SerializeField]
    public float _Spawn_Delay_Time_Next;
    [SerializeField]
    public float _Spawn_Delay_Time_Cur;
    [SerializeField]
    public float _Spawn_Delay_Time_Obstacle_Next;
    [SerializeField]
    public float _Spawn_Delay_Time_Obstacle_Cur;
    [SerializeReference]
    public GameObject _Player = null;
    [SerializeField]
    public bool _CanSpawnEnemy = true;
    [SerializeField]
    public bool _CanSpawnObstacle = true;

    // 게임모드 설정
    [SerializeField]
    public Define.GameMode _gamemode = Define.GameMode.Campaign;

    // UI
    [SerializeReference]
    public Text _scoreText;
    [SerializeReference]
    public Image[] _lifeImage;
    [SerializeReference]
    public Image[] _UltImage;
    [SerializeReference]
    public GameObject _GameOverGroup;

    // UI 차지 공격 바 관련
    [SerializeReference]
    public Slider _ChargeAttackBar;

    // 오브젝트 매니저(오브젝트 풀)
    public ObjectManager objectManager;

    // 적 기체 txt 파일로 스폰하는데 사용됨
    public List<Spawn> _spawnList;
    public List<Spawn> _spawnList_obstacle;
    public int _spawnIndex; // _spawnList 인덱스 번호
    public int _spawnIndex_obstacle; //_spawnList_obstacle 인덱스 번호


    private void Awake()
    {
        _EnemyObjects = new string[] {
            "Enemy_Cone", // 0
            "Enemy_Ring", // 1
            "Enemy_Satellite", // 2
            "Enemy_Starknife", // 3
            "Enemy_Claw", // 4
            "Enemy_Serpent", // 5
            "Item_Shielded_Power", // 6
            "Item_Shielded_Life", // 7
            "Item_Shielded_Speed", //8
            "Item_Shielded_Ult", //9
            "Item_Shielded_GuideAttack", //10
            "Warp", // 11
        };

        _ObstacleObjects = new string[] {
            "Obstacle_Bottom1", // 0
            "Obstacle_Bottom2", // 1
            "Obstacle_Bottom_Tile", // 2
            "Obstacle_Top1", // 3
            "Obstacle_Top2", // 4
            "Obstacle_Top_Tile", // 5
            "Obstacle_Metal_Wall", // 6
        };

        _spawnList = new List<Spawn>();
        ReadSpawnFile();
        _spawnList_obstacle = new List<Spawn>();
        ReadSpawnObstacleFile();

        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else Destroy(gameObject);
    }

    void ReadSpawnFile()
    {
        // 초기화
        _spawnList.Clear();
        _spawnIndex = 0;

        // 스폰 txt파일 읽기 (7:45)(13:32)
        // Resources 폴더 내부의 파일 (Resources 폴더는 절대 변경할 수 없다)
        TextAsset textFile = Resources.Load("Stage_01_SpawnData") as TextAsset;
        StringReader stringReader = new StringReader(textFile.text);

        while (stringReader != null)
        {
            string line = stringReader.ReadLine();
            Debug.Log(line);
            // 파일에 내용이 없거나 파일이 없으면 null
            if (line == null) break;

            // 파일에서 한 줄씩 읽는다.
            Spawn spawnData = new Spawn();

            // 시간, 적 기체 종류, 위치
            spawnData.delay = float.Parse(line.Split(',')[0]); // Parse는 형변환과 비슷
            spawnData.type = line.Split(',')[1];
            spawnData.point = int.Parse(line.Split(',')[2]);
            // 읽은 데이터들을 _spawnList 변수에 모두 넣는다.
            _spawnList.Add(spawnData);
        }

        stringReader.Close(); // 파일을 다 읽었으면 꼭 닫아줘야한다.

        // 첫 번째 스폰 딜레이 적용
        _Spawn_Delay_Time_Next = _spawnList[0].delay;
    }

    void ReadSpawnObstacleFile()
    {
        _spawnList_obstacle.Clear();
        _spawnIndex_obstacle = 0;

        TextAsset textFile = Resources.Load("Stage_01_ObstacleData") as TextAsset;
        StringReader stringReader = new StringReader(textFile.text);

        while(stringReader != null)
        {
            string line = stringReader.ReadLine();
            Debug.Log("Obstacle txt info: " + line);
            if (line == null) break;

            Spawn spawnData = new Spawn();

            // 시간, 장애물 종류, 위치
            spawnData.delay = float.Parse(line.Split(',')[0]);
            spawnData.type = line.Split(',')[1];
            spawnData.point = int.Parse(line.Split(',')[2]);

            _spawnList_obstacle.Add(spawnData);
        }

        stringReader.Close();

        _Spawn_Delay_Time_Obstacle_Next = _spawnList_obstacle[0].delay;
    }

    private void FixedUpdate()
    {
        _Spawn_Delay_Time_Cur += Time.deltaTime;
        _Spawn_Delay_Time_Obstacle_Cur += Time.deltaTime;

        if (_Spawn_Delay_Time_Cur >= 10) _Spawn_Delay_Time_Cur = 10;
        if (_Spawn_Delay_Time_Obstacle_Cur >= 120) _Spawn_Delay_Time_Obstacle_Cur = 120;


        // 무한 모드일 경우
        if (_gamemode == Define.GameMode.Infinite)
        {
            if (_CanSpawnEnemy == true) 

            if (_Spawn_Delay_Time_Cur > _Spawn_Delay_Time_Next)
            {
                SpawnEnemy_InfiniteMode();
                _Spawn_Delay_Time_Next = Random.Range(0.5f, 3.0f); // 원본 0.5f, 3.0f
                _Spawn_Delay_Time_Cur = 0.0f;
            }
        }

        // 캠페인 모드일 경우
        if (_gamemode == Define.GameMode.Campaign && _CanSpawnEnemy == true)
        {
            SpawnEnemy_CampaignMode();
        }
        if (_gamemode == Define.GameMode.Campaign && _CanSpawnObstacle == true)
        {
            SpawnObstacle_CampaignMode();
        }


        // 점수 갱신
        PlayerController playerinfo = _Player.GetComponent<PlayerController>();
        _scoreText.text = string.Format("Score: " + "{0:n0}", playerinfo.GetScore());
    }

    void SpawnEnemy_InfiniteMode()
    {
        // 0 = cone, 1 = ring, 2 = satelite, 3 = starknife
        int randomEnemy = Random.Range(0, 4); // 적 기체 4가지
        int randomPos = Random.Range(0, 9); // 스폰 위치 8가지

        if (randomPos == 0 || randomPos == 1 || randomPos == 2 || randomPos == 3 || randomPos == 4)
        {
            GameObject enemy = objectManager.MakeObj(_EnemyObjects[randomEnemy]);
            enemy.transform.position = _SpawnPos[randomPos].position;
            // 스프라이트가 우측을 바라보게 만들어져 있어서 플레이어 방향으로 회전시켜줘야 함.
            enemy.transform.rotation = Quaternion.Euler(0.0f, 180.0f, 0.0f);

            Enemy_Base enemyInfo = enemy.GetComponent<Enemy_Base>();
            enemyInfo.objectmanager = objectManager;
            enemyInfo.SimpleMoveLeft();
        }
        else if (randomPos == 5 || randomPos == 6) // 아래 오른쪽, 아래 왼쪽
        {
            if (randomEnemy == 0 || randomEnemy == 1) // cone, ring
            {
                GameObject enemy = objectManager.MakeObj(_EnemyObjects[randomEnemy]);
                enemy.transform.position = _SpawnPos[randomPos].position;
                enemy.transform.rotation = Quaternion.Euler(0.0f, 180.0f, 0.0f);

                Enemy_Base enemyInfo = enemy.GetComponent<Enemy_Base>();
                enemyInfo.objectmanager = objectManager;
                enemyInfo.SimpleMoveUp();
            }
        }
        else if (randomPos == 7 || randomPos == 8) // 위 오른쪽, 위 왼쪽
        {
            if (randomEnemy == 0 || randomEnemy == 1) // cone, ring
            {
                GameObject enemy = objectManager.MakeObj(_EnemyObjects[randomEnemy]);
                enemy.transform.position = _SpawnPos[randomPos].position;
                enemy.transform.rotation = Quaternion.Euler(0.0f, 180.0f, 0.0f);

                Enemy_Base enemyInfo = enemy.GetComponent<Enemy_Base>();
                enemyInfo.objectmanager = objectManager;
                enemyInfo.SimpleMoveDown();
            }
        }
    }

    void SpawnEnemy_CampaignMode()
    {
        if (_CanSpawnEnemy == false) return;
        if (_Spawn_Delay_Time_Cur < _Spawn_Delay_Time_Next) return;


        int enemyIndex = 0; // 적 기체 종류 번호 초기화        
        switch (_spawnList[_spawnIndex].type)
        {
            case "Enemy_Cone":
                enemyIndex = 0;
                break;
            case "Enemy_Ring":
                enemyIndex = 1;
                break;
            case "Enemy_Satellite":
                enemyIndex = 2;
                break;
            case "Enemy_Starknife":
                enemyIndex = 3;
                break;
            case "Enemy_Claw":
                enemyIndex = 4;
                break;
            case "Enemy_Serpent":
                enemyIndex = 5;
                break;
            case "Item_Shielded_Power":
                enemyIndex = 6;
                break;
            case "Item_Shielded_Life":
                enemyIndex = 7;
                break;
            case "Item_Shielded_Speed":
                enemyIndex = 8;
                break;
            case "Item_Shielded_Ult":
                enemyIndex = 9;
                break;
            case "Item_Shielded_GuideAttack":
                enemyIndex = 10;
                break;
            case "Warp":
                enemyIndex = 11;
                break;
            default:
                Debug.Log("swtich-case is wrong");
                break;
        }

        // 스폰 위치 (0 ~ 9) 9가지 (0부터 시작하니 -1 잊지말자)
        int enemyPoint = _spawnList[_spawnIndex].point;

        // 해당 오브젝트 스폰
        GameObject enemy = objectManager.MakeObj(_EnemyObjects[enemyIndex]);
        // 스폰한 오브젝트 위치 설정
        enemy.transform.position = _SpawnPos[enemyPoint].position;


        // 몬스터 종류별로 회전값 및 오브젝트 매니저 전달
        if(_spawnList[_spawnIndex].type == "Enemy_Claw" || _spawnList[_spawnIndex].type == "Enemy_Serpent")
        {
            // 없음
        }
        else if(
            _spawnList[_spawnIndex].type == "Enemy_Cone" ||
            _spawnList[_spawnIndex].type == "Enemy_Ring" ||
            _spawnList[_spawnIndex].type == "Enemy_Satellite" ||
            _spawnList[_spawnIndex].type == "Enemy_Starknife")
        {
            // 스프라이트가 우측을 바라보게 만들어져 있어서 반대 방향으로 회전시켜줘야 함.
            enemy.transform.rotation = Quaternion.Euler(0.0f, 180.0f, 0.0f);

            Enemy_Base enemyInfo = enemy.GetComponent<Enemy_Base>();
            enemyInfo.objectmanager = objectManager;

            if(_spawnList[_spawnIndex].type == "Enemy_Cone")
            {
                
            }
        }
        else if(
            _spawnList[_spawnIndex].type == "Item_Shielded_Power" ||
            _spawnList[_spawnIndex].type == "Item_Shielded_Life" ||
            _spawnList[_spawnIndex].type == "Item_Shielded_Speed" ||
            _spawnList[_spawnIndex].type == "Item_Shielded_Ult" ||
            _spawnList[_spawnIndex].type == "Item_Shielded_GuideAttack"
            )
        {
            Item_Shielded enemyInfo = enemy.GetComponent<Item_Shielded>();
            enemyInfo.objectmanager = objectManager;
        }
        else if(_spawnList[_spawnIndex].type == "Warp")
        {
            Warp enemyInfo = enemy.GetComponent<Warp>();
            enemyInfo.objectManager = objectManager;
        }

        

        //if (enemyPoint == 0 || enemyPoint == 1 || enemyPoint == 2 || enemyPoint == 3 || enemyPoint == 4)
        //{
        //    GameObject enemy = objectManager.MakeObj(_EnemyObjects[enemyIndex]);
        //    enemy.transform.position = _SpawnPos[enemyPoint].position;
        //    // 스프라이트가 우측을 바라보게 만들어져 있어서 플레이어 방향으로 회전시켜줘야 함.
        //    enemy.transform.rotation = Quaternion.Euler(0.0f, 180.0f, 0.0f);

        //    Enemy_Base enemyInfo = enemy.GetComponent<Enemy_Base>();
        //    enemyInfo.objectmanager = objectManager;
        //    enemyInfo.SimpleMoveLeft();
        //}
        //else if (enemyPoint == 5 || enemyPoint == 6) // 아래 오른쪽, 아래 왼쪽
        //{
        //    if (enemyIndex == 0 || enemyIndex == 1) // cone, ring
        //    {
        //        GameObject enemy = objectManager.MakeObj(_EnemyObjects[enemyIndex]);
        //        enemy.transform.position = _SpawnPos[enemyPoint].position;
        //        enemy.transform.rotation = Quaternion.Euler(0.0f, 180.0f, 0.0f);

        //        Enemy_Base enemyInfo = enemy.GetComponent<Enemy_Base>();
        //        enemyInfo.objectmanager = objectManager;
        //        enemyInfo.SimpleMoveUp();
        //    }
        //}
        //else if (enemyPoint == 7 || enemyPoint == 8) // 위 오른쪽, 위 왼쪽
        //{
        //    if (enemyIndex == 0 || enemyIndex == 1) // cone, ring
        //    {
        //        GameObject enemy = objectManager.MakeObj(_EnemyObjects[enemyIndex]);
        //        enemy.transform.position = _SpawnPos[enemyPoint].position;
        //        enemy.transform.rotation = Quaternion.Euler(0.0f, 180.0f, 0.0f);

        //        Enemy_Base enemyInfo = enemy.GetComponent<Enemy_Base>();
        //        enemyInfo.objectmanager = objectManager;
        //        enemyInfo.SimpleMoveDown();
        //    }
        //}

        // 스폰 순서 다음줄로
        _spawnIndex++;

        // 모든 스폰이 다 끝났으면 멈춤
        if(_spawnIndex >= _spawnList.Count)
        {
            _CanSpawnEnemy = false;
            return;
        }

        // 다음 스폰시간 갱신
        _Spawn_Delay_Time_Next = _spawnList[_spawnIndex].delay;
        _Spawn_Delay_Time_Cur = 0f;
    }

    void SpawnObstacle_CampaignMode()
    {
        if (_CanSpawnObstacle == false) return;
        if (_Spawn_Delay_Time_Obstacle_Cur < _Spawn_Delay_Time_Obstacle_Next) return;


        int obstacleIndex = 0; // 장애물 종류 번호 초기화        
        switch (_spawnList_obstacle[_spawnIndex_obstacle].type)
        {
            case "Obstacle_Bottom1":
                obstacleIndex = 0;
                break;
            case "Obstacle_Bottom2":
                obstacleIndex = 1;
                break;
            case "Obstacle_Bottom_Tile":
                obstacleIndex = 2;
                break;
            case "Obstacle_Top1":
                obstacleIndex = 3;
                break;
            case "Obstacle_Top2":
                obstacleIndex = 4;
                break;
            case "Obstacle_Top_Tile":
                obstacleIndex = 5;
                break;
            case "Obstacle_Metal_Wall":
                obstacleIndex = 6;
                break;
            default:
                Debug.Log("swtich-case is wrong");
                break;
        }

        // 16:41 // 스폰 위치 (0 ~ 9) 9가지 (0부터 시작하니 -1 잊지말자)
        int SpawnPoint = _spawnList_obstacle[_spawnIndex_obstacle].point;
        Debug.Log("Spawn Pos: " + SpawnPoint);

        // 해당 오브젝트 스폰
        GameObject obstacle = objectManager.MakeObj(_ObstacleObjects[obstacleIndex]);
        
        // 배경 오브젝트의 위치는 해당 프리팹의 위치 수치를 통해서 조절함
        //obstacle.transform.position = _SpawnPos[SpawnPoint].position;
        Rigidbody2D rigid = obstacle.GetComponent<Rigidbody2D>();
        //rigid.velocity = Vector2.left * 0.5f;
        rigid.velocity = Vector2.left * 10f;


        // 몬스터 종류별로 회전값 및 오브젝트 매니저 전달
        if (_spawnList_obstacle[_spawnIndex_obstacle].type == "Obstacle_Bottom1" ||
            _spawnList_obstacle[_spawnIndex_obstacle].type == "Obstacle_Bottom2" ||
            _spawnList_obstacle[_spawnIndex_obstacle].type == "Obstacle_Bottom_Tile" ||
            _spawnList_obstacle[_spawnIndex_obstacle].type == "Obstacle_Top1" ||
            _spawnList_obstacle[_spawnIndex_obstacle].type == "Obstacle_Top2" ||
            _spawnList_obstacle[_spawnIndex_obstacle].type == "Obstacle_Top_Tile" ||
            _spawnList_obstacle[_spawnIndex_obstacle].type == "Metal_Wall")
        {
            
        }

        // 스폰 순서 다음줄로
        _spawnIndex_obstacle++;

        // 모든 스폰이 다 끝났으면 멈춤
        if (_spawnIndex_obstacle >= _spawnList_obstacle.Count)
        {
            _CanSpawnObstacle = false;
            return;
        }

        // 다음 스폰시간 갱신
        _Spawn_Delay_Time_Obstacle_Next = _spawnList_obstacle[_spawnIndex].delay;
        _Spawn_Delay_Time_Obstacle_Cur = 0f;
    }

    public void UpdateLifeIcon(int life)
    {
        // 라이프 아이콘 이미지 상태 초기화
        for (int i = 0; i < 3; i++)
        {
            _lifeImage[i].color = new Color(1, 1, 1, 0); // 알파값을 0으로 해서 안보이게 한다
        }

        // 라이프 아이콘 이미지 상태 반영
        for (int i = 0; i < life; i++)
        {
            _lifeImage[i].color = new Color(1, 1, 1, 1); // 알파값을 1로 해서 보이게 한다
        }
    }

    public void UpdateUltIcon(int tmp)
    {
        for (int i = 0; i < 3; i++)
        {
            _UltImage[i].color = new Color(1, 1, 1, 0);
        }

        for (int i = 0; i < tmp; i++)
        {
            _UltImage[i].color = new Color(1, 1, 1, 1);
        }
    }

    public void UpdateChargeGuage(float curvalue, float maxvalue = 3.0f)
    {
        if (curvalue == 0) _ChargeAttackBar.value = 0;

        _ChargeAttackBar.value = curvalue / maxvalue;
    }

    public void RespawnPlayerInvoke(float time)
    {
        Invoke("RespawnPlayer", time);
        RespawnPlayer();
    }

    void RespawnPlayer()
    {
        PlayerController playerinfo = _Player.GetComponent<PlayerController>();
        // 플레이어의 라이프가 -1이면 리스폰 함수를 실행하지 않는다.
        if (playerinfo.GetLife() == -1) return;

        _Player.transform.position = new Vector3(-8, 0, 0);
        playerinfo.SetInvincibleTime(2.5f); // 무적시간 설정
        playerinfo.SetIsHit(false);
        _Player.SetActive(true);
    }

    public void GameOver()
    {
        _GameOverGroup.SetActive(true);

        // EditorApplication.isPaused = true; // 게임 일시 정지
    }

    public void GameRetry()
    {
        SceneManager.LoadScene(0); // 씬 번호(파일 -> 빌드 설정 -> 빌드의 씬에서 확인 가능)
    }

    public void GameQuit()
    {
        Application.Quit();
    }
}