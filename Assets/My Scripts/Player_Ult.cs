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
        // �� ��ü�� ���� źȯ�� ��� ã�Ƽ� �迭 ���·� ��´�.
        _enemies = GameObject.FindGameObjectsWithTag("Enemy");
        _enemybullets = GameObject.FindGameObjectsWithTag("EnemyBullet");
    }

    void Update()
    {
        _time += Time.deltaTime;

        // ����ü �迭�� ��� ���ҿ� ���ؼ�
        foreach(GameObject _enemy in _enemies)
        {
            // ����Ʈ�� �� �߾Ӱ� ��ǥ���� ������ �Ÿ��� ����Ѵ�.
            float dis = Vector3.Distance(transform.position, _enemy.transform.position);

            if(_time > 1.0f)
            {
                // ��ǥ������ ����Ʈ�� �߾����� ���ϴ� ������ ���Ѵ�.
                _dir = transform.position = _enemy.transform.position;
                // ��ǥ������ ��ġ�� �߾����� ������ 10%��? �̵���Ų��.
                _enemy.transform.position += _dir * 0.1f * Time.deltaTime;
            }

            if(_time > 2.5f)
            {
                // ��ǥ������ ��ġ�� �߾����� ������ 20%��? �̵���Ų��.
                _enemy.transform.position += _dir * 0.2f * Time.deltaTime;
            }

            if(_time > 4.0f)
            {
                // ��ǥ������ ��ġ�� �߾����� ������ 20%��? �̵���Ų��.
                _enemy.transform.position += _dir * 0.5f * Time.deltaTime;
                // ��ǥ������ ũ�⸦ �����Ӹ��� 10%�� �۾����� �Ѵ�.
                _enemy.transform.localScale *= 0.9f;
            }

            if(_time >= 5.0f)
            {
                Destroy(_enemy);

                // �۵����� 5�ʰ� �������� �ı��Ѵ�.
                Destroy(gameObject);
            }
        }
    }
}
