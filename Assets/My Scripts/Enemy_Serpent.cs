using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Serpent : MonoBehaviour
{
    [SerializeField]
    public float _life = 100;
    [SerializeField]
    public int _score = 2500;
    [SerializeField]
    public Rigidbody2D _rigid;
    [SerializeField]
    GameObject _player;

    // ȿ���� ����
    [SerializeField]
    public AudioClip[] _Sounds;


    // ���� ����
    [System.Serializable]
    public class BodySettings
    {
        public GameObject prefab;
        public int count;
        public float space = 0.4f;
    }

    [SerializeField]
    public enum StartPosition { Left, Right, Top, Bottom, Fixed };
    [SerializeField]
    public StartPosition startPosition = StartPosition.Right;
    [SerializeField]
    public bool tagetPlayer = true;
    [SerializeField]
    public float rotationSpeed = 1;
    [SerializeField]
    public float speed = 1;
    [SerializeField]
    public BodySettings bodySettings;

    // ���� ����
    [SerializeField]
    public GameObject tail;
    [SerializeField]
    public float tailSpace = 0.36f;

    [SerializeField]
    public float recalculatedistance = 1f;
    [SerializeField] // ī�޶� ���� ��ġ ��⿡ ����
    public float autoboundsMultiplier = 1f;
    [SerializeField]
    public Vector2 bounds;
    [SerializeField]
    Vector3 _targetPosition;

    // ���� ���� ���ӿ�����Ʈ��
    [SerializeField]
    List<GameObject> _bodyParts = new List<GameObject>();


    void Awake()
    {
        _rigid = GetComponent<Rigidbody2D>();

        // ī�޶� �������� �̵� ����(bounds ����) ���
        //if (Camera.main.orthographic && bounds == Vector2.zero) bounds = CameraHelpers.OrthographicBounds(Camera.main).extents * autoboundsMultiplier;

        // ���� ��ġ ���(�� �ڵ尡 �۵��ؾ� �۵���)
        // if (startPosition != StartPosition.Fixed) _SetStartPosition();
        if(startPosition == StartPosition.Fixed)
        {
            _SetStartPosition();
        }

        GameObject clone = default;

        Transform leader = transform;
        float space = bodySettings.space;

        // bodySettings.count ��ŭ ���� ������ ���� �ٿ��ֱ�
        for (int i = 0; i < bodySettings.count; i++)
        {
            clone = Instantiate(bodySettings.prefab);

            _bodyParts.Add(clone);

            Enemy_SerpentBody serpentBody = clone.AddComponent<Enemy_SerpentBody>();
            serpentBody.head = leader;
            serpentBody.distanceToHead = space;
            serpentBody.transform.position = transform.position;

            leader = clone.transform;
            space = bodySettings.space;
        }

        // ������ ������ ���� �޾��ֱ�
        if (tail)
        {
            _bodyParts.Add(tail); // Gameobject tail ���� �޾��ֱ�

            if (tail.transform.parent == transform) tail.transform.SetParent(null);

            Enemy_SerpentBody serpentBody = tail.AddComponent<Enemy_SerpentBody>();
            serpentBody.head = clone.transform;
            serpentBody.distanceToHead = tailSpace;
            serpentBody.transform.position = transform.position;
        }

        // ???
        if (bodySettings.prefab.scene.rootCount > 0)
        {
            bodySettings.prefab.SetActive(false);

            //Destroy(bodySettings.prefab);
        }
    }
    

    void Update()
    {
        // �÷��̾ ���ϴ� ��ġ
        Vector3 direction = _targetPosition - transform.position;

        // �÷��̾ ���ϴ� ����
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        // ������ ���� ���� �ݿ� �� �ε巴�� ȸ��
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.AngleAxis(angle, Vector3.forward), Time.deltaTime * rotationSpeed);

        // transform.Translate�� �̵��Լ�
        transform.Translate(Vector3.right * speed * Time.deltaTime);

        // ���� ����� Ÿ�� ��ġ ������ �Ÿ��� recalculatedistance(1.0f)���� ������ Ÿ�� ��ġ ����
        if (Vector3.Distance(transform.position, _targetPosition) < recalculatedistance)
        {
            _player = GameObject.FindGameObjectWithTag("Player");
            if (tagetPlayer && _player) _targetPosition = _player.transform.position;
            else _targetPosition = _RandomPosition();
        }
    }

    void OnHit(int damage) // �ǰ� ����
    {
        _life -= damage;

        if (_life <= 0)
        {
            _player = GameObject.FindGameObjectWithTag("Player");
            PlayerController playerinfo = _player.GetComponent<PlayerController>();
            playerinfo.AddScore(_score);

            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {

        }
        else if (collision.gameObject.tag == "PlayerBullet") // �÷��̾� ��ü ���ݿ� �ǰ�
        {
            // ���� �÷��̾��� źȯ ����(���ݷ�)�� �����´�
            Bullet_Base bullet = collision.gameObject.GetComponent<Bullet_Base>();
            OnHit(bullet._damage);

            // ���� �÷��̾��� źȯ ����
            Destroy(collision.gameObject);
        }
    }

    void _SetStartPosition()
    {
        //Vector2 size = Vector2.one;

        //SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

        // ???
        //if (spriteRenderer) size = spriteRenderer.bounds.size;

        //Vector3 position = _RandomPosition();

        //if (startPosition == StartPosition.Left)
        //{
        //    position.x = -bounds.x - size.x / 2;
        //}
        //else if (startPosition == StartPosition.Right)
        //{
        //    position.x = bounds.x + size.x / 2;
        //}
        //else if (startPosition == StartPosition.Top)
        //{
        //    position.y = bounds.y + size.y / 2;
        //}
        //else if (startPosition == StartPosition.Bottom)
        //{
        //    position.y = -bounds.y - size.y / 2;
        //}

        //transform.position = position;

        transform.position = new Vector3(12f, 0, 0);
    }

    Vector3 _RandomPosition()
    {
        float x = Random.Range(-bounds.x, bounds.x);
        float y = Random.Range(-bounds.y, bounds.y);

        return new Vector3(x, y);
    }

    public int GetScore() { return _score; }

    void OnDestroy()
    {
        for (int i = 0; i < _bodyParts.Count; i++)
        {
            if (_bodyParts[i]) Destroy(_bodyParts[i]);
        }
    }

    void OnDisable()
    {
        for (int i = 0; i < _bodyParts.Count; i++)
        {
            if (_bodyParts[i]) _bodyParts[i].SetActive(false);
        }
    }

    void OnEnable()
    {
        for (int i = 0; i < _bodyParts.Count; i++)
        {
            if (_bodyParts[i]) _bodyParts[i].SetActive(true);
        }
    }
}