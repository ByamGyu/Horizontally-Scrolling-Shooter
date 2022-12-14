using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;


    [SerializeField]
    private string[] _EnemyObjects;
    [SerializeField]
    private string[] _ObstacleObjects;
    [SerializeField]
    private Transform[] _SpawnPos;
    [SerializeField]
    private float _Spawn_Delay_Time_Next;
    [SerializeField]
    private float _Spawn_Delay_Time_Cur;
    [SerializeField]
    private float _Spawn_Delay_Time_Obstacle_Next;
    [SerializeField]
    private float _Spawn_Delay_Time_Obstacle_Cur;
    public GameObject _Player = null;
    public bool _CanSpawnEnemy = true;
    public bool _CanSpawnObstacle = true;
    public bool _CanSpawnItemShielded = true;
    public int _Enemy_Cnt;
    public bool _CanBossSpawn = false;
    public int _BossSpawnTurn = 0;
    public bool _WarningSound = false;
    public int _WarningSoundCnt = 0;


    // 게임모드 설정
    public Define.GameMode _gamemode = Define.GameMode.None;


    // 적 기체 txt 파일로 스폰하는데 사용됨
    public List<Spawn> _spawnList;
    public List<Spawn> _spawnList_obstacle;
    public int _spawnIndex; // _spawnList 인덱스 번호
    public int _spawnIndex_obstacle; //_spawnList_obstacle 인덱스 번호


    public bool _isBossSpawn = false;


    private
        void Awake()
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
            //DontDestroyOnLoad(this);
        }
        else
        {
            Debug.Log("게임 매니저가 2개 이상 존재함!");
            //Destroy(gameObject);
        }

        Init();
    }

    private void Start()
    {
        // 어떤 오브젝트가 보다 먼저 만들어졌는데 만약 게임 매니저를 참고해야 하는 상황일 경우
        // 다시 Init()을 실행시켜서 확실히 한다.
        Init();

        _Player = GameObject.Find("Player");

        // 스테이지별로 게임이 시작될 때 점수 UI 초기화
        UIManager.instance.InitScoreTextAndHighScoreTextAtGameStart();
    }

    public void Init()
    {
        _Spawn_Delay_Time_Cur = 0;
        _Spawn_Delay_Time_Obstacle_Cur = 0;
        _BossSpawnTurn = 0;
        _WarningSoundCnt = 0;
        _Enemy_Cnt = 1;
    }

    void ReadSpawnFile()
    {
        // 초기화
        _spawnList.Clear();
        _spawnIndex = 0;

        // 스폰 txt파일 읽기 (7:45)(13:32)
        // Resources 폴더 내부의 파일
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

        if(_spawnList.Count != 0)
        {
            _Spawn_Delay_Time_Next = _spawnList[0].delay;
        }
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

        if(_spawnList_obstacle.Count != 0)
        {
            _Spawn_Delay_Time_Obstacle_Next = _spawnList_obstacle[0].delay;
        }
    }

    void Update()
    {
        if (_gamemode == Define.GameMode.UI || _gamemode == Define.GameMode.None) return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_gamemode == Define.GameMode.Campaign || _gamemode == Define.GameMode.Infinite)
            {
                if (UIManager.instance.GetEscMenuActiveSelf() == false)
                {
                    UIManager.instance.Init();
                    UIManager.instance._EscMenuGroup.SetActive(true);
                    Time.timeScale = 0f;
                }
            }
        }

        if(_isBossSpawn == true) // 보스가 스폰된 상태면 보스가 죽을 때 까지 다음 스폰을 멈춘다.
        {
            _Spawn_Delay_Time_Cur = 0;
            return;
        }

        _Spawn_Delay_Time_Cur += Time.deltaTime;
        _Spawn_Delay_Time_Obstacle_Cur += Time.deltaTime;

        if (_Spawn_Delay_Time_Cur >= 20) _Spawn_Delay_Time_Cur = 20;
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
        // 캠페인 모드 장애물 생성
        if (_gamemode == Define.GameMode.Campaign && _CanSpawnObstacle == true)
        {
            SpawnObstacle_CampaignMode();
        }

        // 점수 갱신
        if(_Player != null)
        {
            // 게임 인스턴스에 점수 저장
            GameInstance.instance.SetPlayerScore(GameInstance.instance.GetPlayerScore());

            // UI 갱신
            UIManager.instance.UpdateScoreText(GameInstance.instance.GetPlayerScore());
        }
    }

    public void SetGameMode(Define.GameMode mode)
    {
        if (mode == Define.GameMode.Campaign) _gamemode = Define.GameMode.Campaign;
        else if (mode == Define.GameMode.Infinite) _gamemode = Define.GameMode.Infinite;
        else if (mode == Define.GameMode.UI) _gamemode = Define.GameMode.UI;
        else if (mode == Define.GameMode.None) _gamemode = Define.GameMode.None;
    }

    void SpawnEnemy_InfiniteMode()
    {
        // 무한모드 종료, UI 호출 필요
        if (_Enemy_Cnt >= 1001)
        {
            _gamemode = Define.GameMode.UI;

            Time.timeScale = 0f;            
            UIManager.instance.SetActiveStageClearGroup(true);

            // ui에 점수 적용
            UIManager.instance._scoretext.text = string.Format("Total Score: " + "{0:n0}", GameInstance.instance.GetPlayerScore());

            UIManager.instance.SetActiveSceneUIGroup(false);

            SoundManager.instance.PlayBGM("Stage_Clear", 0.6f, true);

            return;
        }

        if(_CanSpawnEnemy == true)
        {
            // 일반 기체 50기 잡을 때 마다 보스 스폰
            if (_Enemy_Cnt % 50 == 0 && _CanBossSpawn == false)
            {
                _CanBossSpawn = true;
                _BossSpawnTurn++;
            }

            if(_CanSpawnItemShielded == true && _Enemy_Cnt % 10 == 0)
            {
                SpawnRandomShieldedItemRandomLocation();
                _CanSpawnItemShielded = false;
            }

            if (_CanBossSpawn == true)
            {
                if (_Enemy_Cnt % 50 == 0 && _BossSpawnTurn % 2 == 1) // 보스 serpent 스폰
                {
                    // 10초 후에 스폰
                    Invoke("InifiniteModeSerpentSpawnInvoke", 10f);

                    _CanSpawnEnemy = false;
                    _CanBossSpawn = false;
                    return;
                }
                else if (_Enemy_Cnt >= 50 && _BossSpawnTurn % 2 == 0) // 보스 Claw 스폰
                {
                    if(_WarningSound == false)
                    {
                        InvokeRepeating("InfiniteModeWarningSoundInvoke", 0, 1f);

                        if(_WarningSoundCnt >= 2)
                        {
                            CancelInvoke("InfiniteModeWarningSoundInvoke");
                            _WarningSoundCnt = 0;
                            _WarningSound = true;
                        }
                        
                        return;
                    }
                    

                    GameObject enemy = ObjectManager.instance.MakeObj(_EnemyObjects[11]);
                    Warp tmp_warp = enemy.GetComponent<Warp>();

                    _CanSpawnEnemy = false;
                    _CanBossSpawn = false;
                    return;
                }
            }
            else if (_CanBossSpawn == false && _Enemy_Cnt % 50 != 0)
            {
                // 0 = cone, 1 = ring, 2 = satelite, 3 = starknife
                int randomEnemy = Random.Range(0, 4); // 적 기체 4가지
                int randomPos = Random.Range(0, 9); // 스폰 위치 8가지

                // 오른쪽에서 시작
                if (randomPos == 0 || randomPos == 1 || randomPos == 2 || randomPos == 3 || randomPos == 4)
                {
                    GameObject enemy = ObjectManager.instance.MakeObj(_EnemyObjects[randomEnemy]);
                    enemy.transform.position = _SpawnPos[randomPos].position;
                    // 스프라이트가 우측을 바라보게 만들어져 있어서 플레이어 방향으로 회전시켜줘야 함.
                    enemy.transform.rotation = Quaternion.Euler(0.0f, 180.0f, 0.0f);

                    SetEnemyCnt(1);
                    Enemy_Base enemyInfo = enemy.GetComponent<Enemy_Base>();

                    if (_EnemyObjects[randomEnemy] == "Enemy_Cone")
                    {
                        Enemy_Cone cone = enemy.GetComponent<Enemy_Cone>();
                        cone.SimpleMoveLeft();
                    }
                    else if (_EnemyObjects[randomEnemy] == "Enemy_Ring")
                    {
                        Enemy_Ring ring = enemy.GetComponent<Enemy_Ring>();
                        ring._CanMoveSin = true;
                        ring._CanMoveCos = true;
                        ring._CanMoveLeft = true;
                    }
                    else if (_EnemyObjects[randomEnemy] == "Enemy_Satelite")
                    {
                        Enemy_Satelite satelite = enemy.GetComponent<Enemy_Satelite>();
                    }
                }
                else if (randomPos == 5 || randomPos == 6) // 아래 오른쪽, 아래 왼쪽
                {
                    if (randomEnemy == 0 || randomEnemy == 1) // cone, ring
                    {
                        GameObject enemy = ObjectManager.instance.MakeObj(_EnemyObjects[randomEnemy]);
                        enemy.transform.position = _SpawnPos[randomPos].position;
                        enemy.transform.rotation = Quaternion.Euler(0.0f, 180.0f, 0.0f);

                        SetEnemyCnt(1);
                        Enemy_Base enemyInfo = enemy.GetComponent<Enemy_Base>();


                        if (_EnemyObjects[randomEnemy] == "Enemy_Cone")
                        {
                            Enemy_Cone cone = enemy.GetComponent<Enemy_Cone>();
                            cone.SimpleMoveUp();
                        }
                        else if (_EnemyObjects[randomEnemy] == "Enemy_Ring")
                        {
                            Enemy_Ring ring = enemy.GetComponent<Enemy_Ring>();
                            ring._CanMoveCos = true;
                            ring._CanMoveUp = true;
                        }
                    }
                }
                else if (randomPos == 7 || randomPos == 8) // 위 오른쪽, 위 왼쪽
                {
                    if (randomEnemy == 0 || randomEnemy == 1) // cone, ring
                    {
                        GameObject enemy = ObjectManager.instance.MakeObj(_EnemyObjects[randomEnemy]);
                        enemy.transform.position = _SpawnPos[randomPos].position;
                        enemy.transform.rotation = Quaternion.Euler(0.0f, 180.0f, 0.0f);

                        SetEnemyCnt(1);
                        Enemy_Base enemyInfo = enemy.GetComponent<Enemy_Base>();


                        if (_EnemyObjects[randomEnemy] == "Enemy_Cone")
                        {
                            Enemy_Cone cone = enemy.GetComponent<Enemy_Cone>();
                            cone.SimpleMoveDown();
                        }
                        else if (_EnemyObjects[randomEnemy] == "Enemy_Ring")
                        {
                            Enemy_Ring ring = enemy.GetComponent<Enemy_Ring>();
                            ring._CanMoveCos = true;
                            ring._CanMoveDown = true;
                        }
                    }
                }
            }
        }
        else if(_CanSpawnEnemy == false)
        {

        }
    }

    void SpawnEnemy_CampaignMode()
    {
        if (_spawnList.Count == 0) return;
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

        GameObject enemy = ObjectManager.instance.MakeObj(_EnemyObjects[enemyIndex]);
        enemy.transform.position = _SpawnPos[enemyPoint].position;


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

            // 오른쪽에서 출현
            if(enemyPoint >= 0 && enemyPoint <= 4)
            {
                if(_spawnList[_spawnIndex].type == "Enemy_Cone")
                {
                    Enemy_Cone cone = enemy.GetComponent<Enemy_Cone>();
                    cone.SimpleMoveLeft();
                }
                else if(_spawnList[_spawnIndex].type == "Enemy_Ring")
                {
                    Enemy_Ring ring = enemy.GetComponent<Enemy_Ring>();
                    ring._CanMoveSin = true;
                    ring._CanMoveCos = true;
                    ring._CanMoveLeft = true;
                }
            }

            // 아래에서 출현
            if(enemyPoint == 5 || enemyPoint == 6)
            {
                if (_spawnList[_spawnIndex].type == "Enemy_Cone")
                {
                    Enemy_Cone cone = enemy.GetComponent<Enemy_Cone>();
                    cone.SimpleMoveUp();
                }
                else if (_spawnList[_spawnIndex].type == "Enemy_Ring")
                {
                    Enemy_Ring ring = enemy.GetComponent<Enemy_Ring>();
                    ring._CanMoveCos = true;
                    ring._CanMoveUp = true;
                }
            }

            // 위에서 출현
            if (enemyPoint == 7 || enemyPoint == 8)
            {
                if (_spawnList[_spawnIndex].type == "Enemy_Cone")
                {
                    Enemy_Cone cone = enemy.GetComponent<Enemy_Cone>();
                    cone.SimpleMoveDown();
                }
                else if (_spawnList[_spawnIndex].type == "Enemy_Ring")
                {
                    Enemy_Ring ring = enemy.GetComponent<Enemy_Ring>();
                    ring._CanMoveCos = true;
                    ring._CanMoveDown = true;
                }
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
        }
        else if(_spawnList[_spawnIndex].type == "Warp")
        {
            Warp enemyInfo = enemy.GetComponent<Warp>();
        }

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
        if (_spawnList_obstacle.Count == 0) return;
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
        GameObject obstacle = ObjectManager.instance.MakeObj(_ObstacleObjects[obstacleIndex]);
        
        // 배경 오브젝트의 위치는 해당 프리팹의 위치 수치를 통해서 조절함
        //obstacle.transform.position = _SpawnPos[SpawnPoint].position;
        Rigidbody2D rigid = obstacle.GetComponent<Rigidbody2D>();
        //rigid.velocity = Vector2.left * 0.5f;
        rigid.velocity = Vector2.left * 1f;


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
        _Spawn_Delay_Time_Obstacle_Next = _spawnList_obstacle[_spawnIndex_obstacle].delay; // index out of range 문제 있음
        _Spawn_Delay_Time_Obstacle_Cur = 0f;
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
        _Player.SetActive(true);
    }

    public void GameOver()
    {
        Time.timeScale = 0f;
        UIManager.instance.Init();
        UIManager.instance.SetActiveGameOverGroup(true);
    }

    public void SetEnemyCnt(int tmp)
    {
        _Enemy_Cnt += tmp;

        // 이럴 일은 없지만...
        if (_Enemy_Cnt <= 0) _Enemy_Cnt = 0;
    }

    void InifiniteModeSerpentSpawnInvoke()
    {
        GameObject enemy = ObjectManager.instance.MakeObj(_EnemyObjects[5]);
        enemy.transform.position = _SpawnPos[2].position;

        Enemy_Serpent enemyInfo = enemy.GetComponent<Enemy_Serpent>();
    }

    void InfiniteModeWarningSoundInvoke()
    {
        SoundManager.instance.PlaySoundEffectOneShot("Warning_1sec", 0.5f);
        _WarningSoundCnt++;
    }

    void SpawnRandomShieldedItemRandomLocation()
    {
        int tmp = Random.Range(6, 11);

        GameObject go = ObjectManager.instance.MakeObj(_EnemyObjects[tmp]);
        Item_Shielded Item = go.GetComponent<Item_Shielded>();

        int tmp2 = Random.Range(1, 4);
        Item.transform.position = _SpawnPos[tmp2].position;
    }
}