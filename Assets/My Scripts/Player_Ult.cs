using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Ult : MonoBehaviour
{
    [SerializeField]
    GameObject[] _enemies;
    [SerializeField]
    GameObject[] _enemybullets;
    [SerializeField]
    float _time = 0;
    [SerializeField]
    Vector3 _dir;

    void Start()
    {
        // 적 기체와 적의 탄환을 모두 찾아서 배열 형태로 담는다.
        _enemies = GameObject.FindGameObjectsWithTag("Enemy");
        _enemybullets = GameObject.FindGameObjectsWithTag("EnemyBullet");
    }

    void Update()
    {
        _time += Time.deltaTime;

        // 적기체 배열의 모든 원소에 대해서
        foreach(GameObject _enemy in _enemies)
        {
            // 이펙트의 정 중앙과 목표물들 사이의 거리를 계산한다.
            float dis = Vector3.Distance(transform.position, _enemy.transform.position);

            if(_time > 1.0f)
            {
                // 목표물들이 이펙트의 중앙으로 향하는 방향을 구한다.
                _dir = transform.position = _enemy.transform.position;
                // 목표물들의 위치를 중앙으로 차이의 10%씩? 이동시킨다.
                _enemy.transform.position += _dir * 0.1f * Time.deltaTime;
            }

            if(_time > 2.5f)
            {
                // 목표물들의 위치를 중앙으로 차이의 20%씩? 이동시킨다.
                _enemy.transform.position += _dir * 0.2f * Time.deltaTime;
            }

            if(_time > 4.0f)
            {
                // 목표물들의 위치를 중앙으로 차이의 20%씩? 이동시킨다.
                _enemy.transform.position += _dir * 0.5f * Time.deltaTime;
                // 목표물들의 크기를 프레임마다 10%씩 작아지게 한다.
                _enemy.transform.localScale *= 0.9f;
            }

            if(_time >= 5.0f)
            {
                Destroy(_enemy);

                // 작동한지 5초가 지났으면 파괴한다.
                Destroy(gameObject);
            }
        }
    }
}
