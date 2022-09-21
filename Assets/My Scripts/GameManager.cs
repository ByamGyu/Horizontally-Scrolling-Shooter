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

    private void Update()
    {
        _Spawn_Delay_Time_Cur += Time.deltaTime;

        if(_Spawn_Delay_Time_Cur > _Spawn_Delay_Time_Max)
        {
            SpawnEnemy();
            _Spawn_Delay_Time_Max = Random.Range(0.5f, 3.0f);
            _Spawn_Delay_Time_Cur = 0.0f;
        }
    }

    void SpawnEnemy()
    {
        int randomEnemy = Random.Range(0, 4); // 4가지
        int randomPos = Random.Range(0, 5);

        // 스프라이트가 우측을 바라보게 만들어져 있어서 플레이어 방향으로 회전시켜줘야 함.
        Instantiate(_EnemyObjects[randomEnemy], _SpawnPos[randomPos].position, Quaternion.Euler(0.0f, 180.0f, 0.0f));
    }
}
