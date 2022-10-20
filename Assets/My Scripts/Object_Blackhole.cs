using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object_Blackhole : MonoBehaviour
{
    [SerializeField]
    GameObject[] _enemies;
    [SerializeField]
    GameObject[] _enemiesbullet;

    [SerializeField]
    float _time = 0;
    [SerializeField]
    float _totaltime = 0;

    Vector3 _dir;


    void Start()
    {
        PlaySound();
    }

    void FixedUpdate()
    {
        _time += Time.deltaTime;

        searchenemies();
        searchbullets();

        simulategravity();

        stopthis(_totaltime);
    }

    void simulategravity()
    {
        // 탐색된 적 관련 물체가 하나라도 없으면 통과
        if (_enemies.Length == 0 && _enemiesbullet.Length == 0) return;

        // 적 기체 끌어당김
        foreach (GameObject enemy in _enemies)
        {
            // 블랙홀 중심부와 적 사이의 거리 계산
            float dis = Vector3.Distance(transform.position, enemy.transform.position);

            if (_time > 0f) // 0초 후에 작동
            {
                _dir = transform.position - enemy.transform.position;
                enemy.transform.position += _dir * 3f * Time.deltaTime;
            }
            else if (dis <= 5.0f)
            {
                enemy.transform.position += _dir * 3f * Time.deltaTime;
            }
            else if (dis <= 2.5f)
            {
                enemy.transform.position += _dir * 5f * Time.deltaTime;
            }
            else if (dis <= 1.0f)
            {
                enemy.transform.position += _dir * 7f * Time.deltaTime;
            }
            
            if((_totaltime - _time) < 1.0f) // 사라지기 직전에
            {
                
            }
        }

        // 적 탄환들 처리
        foreach (GameObject bullet in _enemiesbullet)
        {
            bullet.SetActive(false);
        }
    }

    void PlaySound()
    {
        SoundManager.instance.PlaySoundEffectOneShot("BlackHole");
    }

    void stopthis(float time)
    {
        if (_time >= time)
        {
            Destroy(gameObject);
        }
    }

    void searchenemies()
    {
        _enemies = GameObject.FindGameObjectsWithTag("Enemy");
    }

    void searchbullets()
    {
        _enemiesbullet = GameObject.FindGameObjectsWithTag("EnemyBullet");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "EnemyBullet")
        {
            GameObject _Player = GameObject.FindGameObjectWithTag("Player");
            PlayerController playerinfo = _Player.GetComponent<PlayerController>();

            Enemy_Base enemyinfo = collision.GetComponent<Enemy_Base>();

            playerinfo.AddScore(enemyinfo._score);

            collision.gameObject.SetActive(false);
        }
    }
}
