using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    public GameObject[] _EnemyObjects;
    [SerializeField]
    public Transform[] _SpawnPos;
    [SerializeField]
    public float _Spawn_Delay_Time_Max;
    [SerializeField]
    public float _Spawn_Delay_Time_Cur;
    [SerializeReference]
    public GameObject _Player = null;

    private void Update()
    {
        _Spawn_Delay_Time_Cur += Time.deltaTime;

        if(_Spawn_Delay_Time_Cur > _Spawn_Delay_Time_Max)
        {
            SpawnEnemy();
            _Spawn_Delay_Time_Max = Random.Range(0.5f, 3.0f); // 원본 0.5f, 3.0f
            _Spawn_Delay_Time_Cur = 0.0f;
        }
    }

    void SpawnEnemy()
    {
        // 0 = cone, 1 = ring, 2 = satelite, 3 = starknife
        int randomEnemy = Random.Range(0, 4); // 적 기체 4가지
        int randomPos = Random.Range(0, 9); // 스폰 위치 8가지
                

        if(randomPos == 0 || randomPos == 1 || randomPos == 2 || randomPos == 3 || randomPos == 4)
        {
            // 스프라이트가 우측을 바라보게 만들어져 있어서 플레이어 방향으로 회전시켜줘야 함.
            GameObject enemy = Instantiate(_EnemyObjects[randomEnemy], _SpawnPos[randomPos].position, Quaternion.Euler(0.0f, 180.0f, 0.0f));

            Enemy_Base enemyInfo = enemy.GetComponent<Enemy_Base>();
            enemyInfo.SimpleMoveLeft();
        }
        else if (randomPos == 5 || randomPos == 6) // 아래 오른쪽, 아래 왼쪽
        {
            if (randomEnemy == 0 || randomEnemy == 1) // cone, ring
            {
                GameObject enemy = Instantiate(_EnemyObjects[randomEnemy], _SpawnPos[randomPos].position, Quaternion.Euler(0.0f, 180.0f, 0.0f));
                
                Enemy_Base enemyInfo = enemy.GetComponent<Enemy_Base>();
                enemyInfo.SimpleMoveUp();
            }
        }
        else if(randomPos == 7 || randomPos == 8) // 위 오른쪽, 위 왼쪽
        {
            if (randomEnemy == 0 || randomEnemy == 1) // cone, ring
            {
                GameObject enemy = Instantiate(_EnemyObjects[randomEnemy], _SpawnPos[randomPos].position, Quaternion.Euler(0.0f, 180.0f, 0.0f));
                
                Enemy_Base enemyInfo = enemy.GetComponent<Enemy_Base>();
                enemyInfo.SimpleMoveDown();
            }
        }
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
        _Player.SetActive(true);
    }
}