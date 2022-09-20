using UnityEngine;
using Playniax.Ignition.Framework;

namespace Playniax.Pyro.Framework
{
    // Collision properties.
    public class CollisionState : CollisionBase2D, IScoreBase
    {
        [System.Serializable]
        // Cargo is released when an object is destroyed.
        public class CargoSettings
        {
            public enum Mode { all, random };
            // The list of cargo objects.
            public GameObject[] prefab;
            // Determines if all cargo objects are released or just one (random).
            public Mode mode = Mode.all;
            // Determines the scale of cargo objects.
            public float scale = 1;
            public void Add(GameObject obj)
            {
                var length = prefab.Length;
                System.Array.Resize(ref prefab, length + 1);
                prefab[length] = obj;
            }
            public void Clear()
            {
                System.Array.Resize(ref prefab, 0);
            }
            public void Init()
            {
                if (prefab == null) return;

                for (int i = 0; i < prefab.Length; i++)
                {
                    if (prefab[i]) prefab[i].SetActive(false);
                }
            }
            public void Release(CollisionState sprite)
            {
                if (mode == Mode.all)
                {
                    _AllAtOnce(sprite);
                }
                else if (mode == Mode.random)
                {
                    _Random(sprite);
                }
            }
            void _AllAtOnce(CollisionState sprite)
            {
                for (int i = 0; i < prefab.Length; i++)
                {
                    if (prefab[i])
                    {
                        var cargo = Instantiate(prefab[i]);
                        cargo.transform.position = sprite.transform.position;
                        cargo.transform.localScale *= scale;
                        cargo.SetActive(true);
                    }
                }
            }
            void _Random(CollisionState sprite)
            {
                var i = Random.Range(0, prefab.Length);

                if (i >= prefab.Length) return;

                if (prefab[i] == null) return;

                var cargo = Instantiate(prefab[i]);
                cargo.transform.position = sprite.transform.position;
                cargo.transform.localScale *= scale;
                cargo.SetActive(true);
            }

            int _index;
        }

        [System.Serializable]
        // Outro Settings determine what effect to play when an object is destroyed.
        public class OutroSettings
        {
            [System.Serializable]
            // Messenger Settings determine what messenger to use and if text or rewards are to be displayed.
            public class MessengerSettings
            {
                // Determines what messenger to use.
                public string messengerId = "Score";
                // Display text.
                //
                // Displays score points when left blank.
                public string text;
                // Determines if messages are enabled or not.
                public bool enabled = true;
                public static void Message(CollisionState collisionState)
                {
                    if (Messenger.instance == null || collisionState.outroSettings.messengerSettings.enabled == false) return;

                    if (collisionState.outroSettings.messengerSettings.text == "" && collisionState.points > 0)
                    {
                        Messenger.instance.Create(collisionState.outroSettings.messengerSettings.messengerId, collisionState.points.ToString() + "+", collisionState.transform.position);
                    }
                    else if (collisionState.outroSettings.messengerSettings.text != "")
                    {
                        Messenger.instance.Create(collisionState.outroSettings.messengerSettings.messengerId, collisionState.outroSettings.messengerSettings.text, collisionState.transform.position);
                    }
                }
            }

            // Determines what emitter to call.
            public string emitterId = "Explosion Red";
            // Determines to emitter scale.
            public float emitterScale = 1;
            // Messenger Settings.
            public MessengerSettings messengerSettings;
            // Audio Settings.
            public AudioProperties audioSettings;
            // Determines if outro is used.
            public bool enabled = true;
            public void Play(CollisionState collisionState)
            {
                if (enabled == false) return;

                var group = TaskBase.Get(emitterId);
                if (group == null) return;

                var scale = emitterScale;

                if (collisionState.spriteRenderer)
                {
                    if (group.size > 0) scale *= Mathf.Max(collisionState.spriteRenderer.sprite.rect.size.x, collisionState.spriteRenderer.sprite.rect.size.y) / group.size;

                    scale *= Mathf.Max(collisionState.transform.localScale.x, collisionState.transform.localScale.y);

                    group.Play(collisionState.transform.position, collisionState.transform.parent, scale, collisionState.spriteRenderer.sortingOrder);
                }
                else
                {
                    group.Play(collisionState.transform.position, collisionState.transform.parent, scale);
                }

                audioSettings.Play();
            }
        }

        public static int autoPointsMultiplier = 10;

        public string id;
        [SerializeField] string _material = "Metal";
        [SerializeField] int _structuralIntegrity = 1;
        [SerializeField] int _points;
        // Determines what player is rewarded.
        //
        // -1 does nothing but anything above -1 adds the points of an object destroyed to the player the index is set to.
        //
        // For example if playerIndex = 0 the points will be rewarded to player 1. playerIndex = 1 and the points will be rewarded to player 2 etc.
        //
        // To get the points you can do something like: PlayerData.Get(0).score
        public int playerIndex = -1;
        // Determines if points are automatically set by multiplying structuralIntegrity by 10 (default value 10 can be changed by setting 'autoPointsMultiplier')
        public bool autoPoints = true;
        [SerializeField] bool _indestructible;
        [SerializeField] GameObject _friend;

        public bool bodyCount;
        // Outro Settings.
        public OutroSettings outroSettings;
        // Cargo Settings.
        public CargoSettings cargoSettings;
        // SpriteRenderer to use.
        public SpriteRenderer spriteRenderer;
        // Determines if a BoxCollider should be created automatically when a collider is missing.
        public bool generateBoxCollider = true;
        public Material ghostMaterial;
        public int ghostSustain = 3;
        public GameObject friend
        {
            get { return _friend; }
            set { _friend = value; }
        }
        public bool indestructible
        {
            get { return _indestructible; }
            set { _indestructible = value; }
        }
        public GameObject isTargeted
        {
            get;
            set;
        }
        public bool isVisible
        {
            get { return true; }
        }
        public string material
        {
            get { return _material; }
            set { _material = value; }
        }
        public int points
        {
            get { return _points; }
            set { _points = value; }
        }
        public int structuralIntegrity
        {
            get { return _structuralIntegrity; }
            set { _structuralIntegrity = value; }
        }

        public override void Awake()
        {
            base.Awake();

            cargoSettings.Init();

            if (autoPoints)
            {
                if (structuralIntegrity == 0) points = autoPointsMultiplier;

                if (points == 0) points = structuralIntegrity * autoPointsMultiplier;
            }

            if (spriteRenderer == null) spriteRenderer = GetComponent<SpriteRenderer>();

            if (spriteRenderer) _defaultMaterial = spriteRenderer.material;

            if (spriteRenderer && generateBoxCollider && colliders.Length == 0)
            {
                colliders = new BoxCollider2D[] { gameObject.AddComponent<BoxCollider2D>() };

                (colliders[0] as BoxCollider2D).size = spriteRenderer.sprite.bounds.size;
            }
        }
        public virtual void DoDamage(int damage)
        {
            structuralIntegrity -= damage;

            if (structuralIntegrity <= 0)
            {
                structuralIntegrity = 0;

                Kill();
            }
        }
        public void Ghost()
        {
            if (structuralIntegrity > 0 && spriteRenderer && ghostMaterial && spriteRenderer.material != ghostMaterial)
            {
                spriteRenderer.material = ghostMaterial;

                _frameCount = Time.frameCount + ghostSustain;
            }
        }
        public virtual void Kill()
        {
            GameData.bodyCount += bodyCount ? 1 : 0;

            OnOutro();

            cargoSettings.Release(this);

            Destroy(gameObject);
        }
        public override void OnCollision(CollisionBase2D collision)
        {
            var collisionState = collision as CollisionState;

            if (collisionState)
            {
                //if (playerIndex >= 0 && sprite.playerIndex >= 0 && playerIndex == sprite.playerIndex) return;

                if (collisionState.friend != null && collisionState.friend == gameObject) return;
                if (collisionState.friend != null && friend != null && collisionState.friend == friend) return;

                if (indestructible == true && collisionState.indestructible == true) return;

                if (indestructible)
                {
                    if (playerIndex > -1)
                    {
                        PlayerData.Get(playerIndex).scoreboard += collisionState.points;

                        OutroSettings.MessengerSettings.Message(collisionState);
                    }

                    collisionState.Kill();
                }
                else if (collisionState.indestructible)
                {
                    if (collisionState.playerIndex > -1)
                    {
                        PlayerData.Get(collisionState.playerIndex).scoreboard += points;

                        OutroSettings.MessengerSettings.Message(this);
                    }

                    Kill();
                }
                else
                {
                    var damage1 = structuralIntegrity;
                    var damage2 = collisionState.structuralIntegrity;

                    DoDamage(damage2);
                    collisionState.DoDamage(damage1);

                    if (structuralIntegrity > 0) Ghost();
                    if (collisionState.structuralIntegrity > 0) collisionState.Ghost();

                    if (structuralIntegrity > 0 || collisionState.structuralIntegrity > 0) CollisionAudio.Play(material, collisionState.material);

                    if (playerIndex > -1 && collisionState.structuralIntegrity == 0)
                    {
                        PlayerData.Get(playerIndex).scoreboard += collisionState.points;

                        OutroSettings.MessengerSettings.Message(collisionState);
                    }

                    if (collisionState.playerIndex > -1 && structuralIntegrity == 0)
                    {
                        PlayerData.Get(collisionState.playerIndex).scoreboard += points;

                        OutroSettings.MessengerSettings.Message(this);
                    }
                }
            }
        }
        public virtual void OnOutro()
        {
            outroSettings.Play(this);
        }
        void Update()
        {
            if (Time.frameCount >= _frameCount && spriteRenderer && _defaultMaterial && spriteRenderer.material != _defaultMaterial) spriteRenderer.material = _defaultMaterial;
        }

        Material _defaultMaterial;
        int _frameCount;
    }
}