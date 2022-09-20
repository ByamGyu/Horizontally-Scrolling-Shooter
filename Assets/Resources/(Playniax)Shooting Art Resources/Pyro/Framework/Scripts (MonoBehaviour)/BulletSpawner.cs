#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using Playniax.Ignition.Framework;

namespace Playniax.Pyro.Framework
{
    public class BulletSpawner : BulletSpawnerBase
    {
#if UNITY_EDITOR
        [CanEditMultipleObjects]
        [CustomEditor(typeof(BulletSpawner))]
        public class Inspector : Editor
        {
            SerializedProperty directionSettings;
            SerializedProperty pointInSpaceSettings;
            SerializedProperty targetEnemySettings;

            SerializedProperty timer;
            void OnEnable()
            {
                directionSettings = serializedObject.FindProperty("directionSettings");
                pointInSpaceSettings = serializedObject.FindProperty("pointInSpaceSettings");
                targetEnemySettings = serializedObject.FindProperty("targetEnemySettings");
            }
            override public void OnInspectorGUI()
            {
                EditorGUI.BeginChangeCheck();

                serializedObject.Update();

                timer = serializedObject.FindProperty("timer");

                var myScript = target as BulletSpawner;

                EditorGUI.BeginDisabledGroup(true);
                EditorGUILayout.ObjectField("Script", MonoScript.FromMonoBehaviour(myScript), typeof(BulletSpawner), false);
                EditorGUI.EndDisabledGroup();

                myScript.id = EditorGUILayout.TextField("Id", myScript.id);
                myScript.triggerId = EditorGUILayout.TextField("Trigger Id", myScript.triggerId);
                myScript.trigger = (Trigger)EditorGUILayout.ObjectField("Trigger", myScript.trigger, typeof(Trigger), true);
                myScript.automatically = EditorGUILayout.Toggle("Automatically", myScript.automatically);

                myScript.prefab = (GameObject)EditorGUILayout.ObjectField("Prefab", myScript.prefab, typeof(GameObject), true);
                myScript.prefabSmartOverrides = EditorGUILayout.Toggle("Prefab Smart Overrides", myScript.prefabSmartOverrides);

                EditorGUILayout.PropertyField(timer, new GUIContent("Timer"));

                myScript.position = EditorGUILayout.Vector3Field("Position", myScript.position);
                myScript.scale = EditorGUILayout.FloatField("Scale", myScript.scale);
                myScript.speed = EditorGUILayout.TextField("Speed", myScript.speed);

                myScript.mode = (Mode)EditorGUILayout.EnumPopup("Mode", myScript.mode);

                if (myScript.mode == Mode.Direction)
                {
                    EditorGUILayout.PropertyField(directionSettings, new GUIContent("Direction Settings"));
                }
                else if (myScript.mode == Mode.PointInSpace)
                {
                    EditorGUILayout.PropertyField(pointInSpaceSettings, new GUIContent("Point In Space Settings"));
                }
                else if (myScript.mode == Mode.TargetEnemy)
                {
                    EditorGUILayout.PropertyField(targetEnemySettings, new GUIContent("Target Enenmy Settings"));
                }

                if (EditorGUI.EndChangeCheck())
                {
                    EditorUtility.SetDirty(myScript);

                    serializedObject.ApplyModifiedProperties();
                }
            }
        }
#endif

        [System.Serializable]
        public class DirectionSettings
        {
            public bool smartSpawn;
            public Vector3 rotation;
        }

        [System.Serializable]
        public class PointInSpaceSettings
        {
            public Vector3 position;
        }

        [System.Serializable]
        public class TargetEnemySettings
        {
            public bool toughestFirst;
            public float targetRange;
        }
        public enum Mode { TargetPlayer, TargetEnemy, Random, Direction, PointInSpace };

        public GameObject prefab;
        public bool prefabSmartOverrides = true;
        public Mode mode = Mode.TargetPlayer;
        public Vector3 position;
        public float scale = 1;
        public string speed = "8";
        public TargetEnemySettings targetEnemySettings;
        public DirectionSettings directionSettings;
        public PointInSpaceSettings pointInSpaceSettings;

        public virtual void Awake()
        {
            if (prefab && prefab.scene.rootCount > 0) prefab.SetActive(false);
        }
        public override GameObject OnSpawn()
        {
            if (prefab == null) return null;

            if (mode == Mode.TargetPlayer)
            {
                var target = PlayersGroup.GetRandom();
                if (target)
                {
                    var clone = Instantiate(prefab, transform);
                    if (clone)
                    {
                        clone.transform.localScale *= scale;
                        clone.transform.position = transform.position;
                        clone.transform.rotation = transform.rotation;
                        clone.transform.Translate(position, Space.Self);

                        clone.transform.parent = transform.parent;

                        if (prefabSmartOverrides) _SmartOverrides(clone);

                        clone.SetActive(true);

                        var bulletBase = clone.GetComponent<BulletBase>();
                        if (bulletBase)
                        {
                            var angle = Mathf.Atan2(target.transform.position.y - clone.transform.position.y, target.transform.position.x - clone.transform.position.x);

                            clone.transform.eulerAngles = new Vector3(0, 0, angle * Mathf.Rad2Deg);

                            bulletBase.velocity = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle)) * _RandomFloat(speed);
                        }
                    }

                    return clone;
                }
            }
            else if (mode == Mode.TargetEnemy)
            {
                var target = Targetable.GetClosest(gameObject, targetEnemySettings.toughestFirst, targetEnemySettings.targetRange);
                if (target != null)
                {
                    var clone = Instantiate(prefab, transform);
                    if (clone)
                    {
                        clone.transform.localScale *= scale;
                        clone.transform.position = transform.position;
                        clone.transform.rotation = transform.rotation;
                        clone.transform.Translate(position, Space.Self);

                        clone.transform.parent = transform.parent;

                        if (prefabSmartOverrides) _SmartOverrides(clone);

                        clone.SetActive(true);

                        var bulletBase = clone.GetComponent<BulletBase>();
                        if (bulletBase)
                        {
                            var angle = Mathf.Atan2(target.gameObject.transform.position.y - clone.transform.position.y, target.gameObject.transform.position.x - clone.transform.position.x);

                            clone.transform.eulerAngles = new Vector3(0, 0, angle * Mathf.Rad2Deg);

                            bulletBase.velocity = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle)) * _RandomFloat(speed);
                        }
                        else
                        {
                            var rb = clone.GetComponent<Rigidbody2D>();
                            if (rb)
                            {
                                var angle = Mathf.Atan2(target.gameObject.transform.position.y - clone.transform.position.y, target.gameObject.transform.position.x - clone.transform.position.x);

                                clone.transform.eulerAngles = new Vector3(0, 0, angle * Mathf.Rad2Deg);

                                rb.velocity = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle)) * _RandomFloat(speed);
                            }
                        }
                    }

                    return clone;
                }
            }
            else if (mode == Mode.Random)
            {
                var clone = Instantiate(prefab, transform);
                if (clone)
                {
                    clone.transform.localScale *= scale;
                    clone.transform.position = transform.position;
                    clone.transform.rotation = transform.rotation;
                    clone.transform.Translate(position, Space.Self);

                    clone.transform.parent = transform.parent;

                    if (prefabSmartOverrides) _SmartOverrides(clone);

                    clone.SetActive(true);

                    var bulletBase = clone.GetComponent<BulletBase>();
                    if (bulletBase)
                    {
                        var angle = Random.Range(0, 359) * Mathf.Deg2Rad;

                        clone.transform.eulerAngles = new Vector3(0, 0, angle * Mathf.Rad2Deg);

                        bulletBase.velocity = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle)) * _RandomFloat(speed);
                    }
                }

                return clone;
            }
            else if (mode == Mode.Direction)
            {
                if (directionSettings.smartSpawn == true && ObjectCounter.objects > 0 || directionSettings.smartSpawn == false)
                {
                    var clone = Instantiate(prefab, transform);
                    if (clone)
                    {
                        clone.transform.localScale *= scale;
                        clone.transform.position = transform.position;
                        clone.transform.rotation = transform.rotation * Quaternion.Euler(directionSettings.rotation);
                        clone.transform.Translate(position, Space.Self);

                        clone.transform.parent = transform.parent;

                        if (prefabSmartOverrides) _SmartOverrides(clone);

                        clone.SetActive(true);

                        var bulletBase = clone.GetComponent<BulletBase>();
                        if (bulletBase)
                        {
                            bulletBase.velocity = clone.transform.rotation * new Vector3(_RandomFloat(speed), 0, 0);
                        }
                        else
                        {
                            var rb = clone.GetComponent<Rigidbody2D>();
                            if (rb) rb.velocity = clone.transform.rotation * new Vector3(_RandomFloat(speed), 0, 0);
                            //Physics2D.IgnoreCollision(gameObject.GetComponent<Collider2D>(), clone.GetComponent<Collider2D>());
                        }
                    }

                    return clone;
                }
            }
            else if (mode == Mode.PointInSpace)
            {
                var clone = Instantiate(prefab, transform);
                if (clone)
                {
                    clone.transform.localScale *= scale;
                    clone.transform.position = transform.position;
                    clone.transform.rotation = transform.rotation;
                    clone.transform.Translate(position, Space.Self);

                    clone.transform.parent = transform.parent;

                    if (prefabSmartOverrides) _SmartOverrides(clone);

                    clone.SetActive(true);

                    var bulletBase = clone.GetComponent<BulletBase>();
                    if (bulletBase)
                    {
                        var angle = Mathf.Atan2(pointInSpaceSettings.position.y - clone.transform.position.y, pointInSpaceSettings.position.x - clone.transform.position.x);

                        clone.transform.eulerAngles = new Vector3(0, 0, angle * Mathf.Rad2Deg);

                        bulletBase.velocity = MathHelpers.GetVelocity(clone.transform.position, pointInSpaceSettings.position) * _RandomFloat(speed);
                    }
                }

                return clone;
            }

            return null;
        }
        float _RandomFloat(string str, float defaultValue = 0)
        {
            if (str.Trim() == "") return defaultValue;
            string[] r = str.Split(',');
            if (r.Length == 1) return float.Parse(str);
            float min = float.Parse(r[0]);
            float max = float.Parse(r[1]);
            return Random.Range(min, max);
        }
        void _SmartOverrides(GameObject clone)
        {
            if (_spriteRenderer == null) _spriteRenderer = GetComponent<SpriteRenderer>();

            if (_spriteRenderer)
            {
                var orderInLayer = _spriteRenderer.sortingOrder;
                _spriteRenderer = clone.GetComponent<SpriteRenderer>();
                if (_spriteRenderer != null) _spriteRenderer.sortingOrder = orderInLayer + 1;
            }

            var scoreBase = clone.GetComponent<IScoreBase>();
            if (scoreBase != null) scoreBase.friend = gameObject;
        }

        SpriteRenderer _spriteRenderer;
    }
}
 