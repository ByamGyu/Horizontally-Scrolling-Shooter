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
        // Ž���� �� ���� ��ü�� �ϳ��� ������ ���
        if (_enemies.Length == 0 && _enemiesbullet.Length == 0) return;

        // �� ��ü ������
        foreach (GameObject enemy in _enemies)
        {
            // ��Ȧ �߽ɺο� �� ������ �Ÿ� ���
            float dis = Vector3.Distance(transform.position, enemy.transform.position);

            if (_time > 0f) // 0�� �Ŀ� �۵�
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
            
            if((_totaltime - _time) < 1.0f) // ������� ������
            {
                
            }
        }

        // �� źȯ�� ó��
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
