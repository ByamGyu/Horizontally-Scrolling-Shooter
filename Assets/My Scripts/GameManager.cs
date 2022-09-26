using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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

    // UI
    [SerializeReference]
    public Text _scoreText;
    [SerializeReference]
    public Image[] _lifeImage;
    [SerializeReference]
    public GameObject _GameOverGroup;

    private void Update()
    {
        _Spawn_Delay_Time_Cur += Time.deltaTime;

        if(_Spawn_Delay_Time_Cur > _Spawn_Delay_Time_Max)
        {
            SpawnEnemy();
            _Spawn_Delay_Time_Max = Random.Range(0.5f, 3.0f); // ���� 0.5f, 3.0f
            _Spawn_Delay_Time_Cur = 0.0f;
        }

        // ���� ����
        PlayerController playerinfo = _Player.GetComponent<PlayerController>();
        _scoreText.text = string.Format("Score: " + "{0:n0}", playerinfo.GetScore());
    }

    void SpawnEnemy()
    {
        // 0 = cone, 1 = ring, 2 = satelite, 3 = starknife
        int randomEnemy = Random.Range(0, 4); // �� ��ü 4����
        int randomPos = Random.Range(0, 9); // ���� ��ġ 8����
                

        if(randomPos == 0 || randomPos == 1 || randomPos == 2 || randomPos == 3 || randomPos == 4)
        {
            // ��������Ʈ�� ������ �ٶ󺸰� ������� �־ �÷��̾� �������� ȸ��������� ��.
            GameObject enemy = Instantiate(_EnemyObjects[randomEnemy], _SpawnPos[randomPos].position, Quaternion.Euler(0.0f, 180.0f, 0.0f));

            Enemy_Base enemyInfo = enemy.GetComponent<Enemy_Base>();
            enemyInfo.SimpleMoveLeft();
        }
        else if (randomPos == 5 || randomPos == 6) // �Ʒ� ������, �Ʒ� ����
        {
            if (randomEnemy == 0 || randomEnemy == 1) // cone, ring
            {
                GameObject enemy = Instantiate(_EnemyObjects[randomEnemy], _SpawnPos[randomPos].position, Quaternion.Euler(0.0f, 180.0f, 0.0f));
                
                Enemy_Base enemyInfo = enemy.GetComponent<Enemy_Base>();
                enemyInfo.SimpleMoveUp();
            }
        }
        else if(randomPos == 7 || randomPos == 8) // �� ������, �� ����
        {
            if (randomEnemy == 0 || randomEnemy == 1) // cone, ring
            {
                GameObject enemy = Instantiate(_EnemyObjects[randomEnemy], _SpawnPos[randomPos].position, Quaternion.Euler(0.0f, 180.0f, 0.0f));
                
                Enemy_Base enemyInfo = enemy.GetComponent<Enemy_Base>();
                enemyInfo.SimpleMoveDown();
            }
        }
    }

    public void UpdateLifeIcon(int life)
    {
        // ������ ������ �̹��� ���� �ʱ�ȭ
        for (int i = 0; i < 3; i++)
        {
            _lifeImage[i].color = new Color(1, 1, 1, 0); // ���İ��� 0���� �ؼ� �Ⱥ��̰� �Ѵ�
        }

        // ������ ������ �̹��� ���� �ݿ�
        for (int i = 0; i < life; i++)
        {
            _lifeImage[i].color = new Color(1, 1, 1, 1); // ���İ��� 1�� �ؼ� ���̰� �Ѵ�
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
        // �÷��̾��� �������� -1�̸� ������ �Լ��� �������� �ʴ´�.
        if (playerinfo.GetLife() == -1) return;

        _Player.transform.position = new Vector3(-8, 0, 0);
        playerinfo.SetInvincibleTime(2.5f); // �����ð� ����
        playerinfo.SetIsHit(false);
        _Player.SetActive(true);
    }

    public void GameOver()
    {
        _GameOverGroup.SetActive(true);

        // EditorApplication.isPaused = true; // ���� �Ͻ� ����
    }

    public void GameRetry()
    {
        SceneManager.LoadScene(0); // �� ��ȣ(���� -> ���� ���� -> ������ ������ Ȯ�� ����)
    }

    public void GameQuit()
    {
        Application.Quit();
    }
}