// http://man.hubwiz.com/docset/Unity_3D.docset/Contents/Resources/Documents/docs.unity3d.com/Manual/editor-CustomEditors.html

#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using Playniax.Ignition.Framework;

namespace Playniax.Pyro.Framework
{
    [AddComponentMenu("Playniax/Pyro/Framework/EnemyAI")]
    public class EnemyAI : MonoBehaviour
    {
#if UNITY_EDITOR
        [CanEditMultipleObjects]
        [CustomEditor(typeof(EnemyAI))]
        public class Inspector : Editor
        {
            SerializedProperty cruiserSettings;
            SerializedProperty homingMissileSettings;
            SerializedProperty magnetSettings;
            SerializedProperty tailerSettings;

            void OnEnable()
            {
                cruiserSettings = serializedObject.FindProperty("cruiserSettings");
                homingMissileSettings = serializedObject.FindProperty("homingMissileSettings");
                magnetSettings = serializedObject.FindProperty("magnetSettings");
                tailerSettings = serializedObject.FindProperty("tailerSettings");
            }
            override public void OnInspectorGUI()
            {
                EditorGUI.BeginChangeCheck();

                serializedObject.Update();

                var myScript = target as EnemyAI;

                EditorGUI.BeginDisabledGroup(true);
                EditorGUILayout.ObjectField("Script", MonoScript.FromMonoBehaviour(myScript), typeof(EnemyAI), false);
                EditorGUI.EndDisabledGroup();

                myScript.mode = (Mode)EditorGUILayout.EnumPopup("Mode", myScript.mode);

                if (myScript.mode == Mode.Cruiser)
                {
                    EditorGUILayout.PropertyField(cruiserSettings, new GUIContent("Cruiser Settings"));
                }
                else if (myScript.mode == Mode.HomingMissile)
                {
                    EditorGUILayout.PropertyField(homingMissileSettings, new GUIContent("Homing Missile Settings"));
                }
                else if (myScript.mode == Mode.Magnet)
                {
                    EditorGUILayout.PropertyField(magnetSettings, new GUIContent("Magnet Settings"));
                }
                else if (myScript.mode == Mode.Tailer)
                {
                    EditorGUILayout.PropertyField(tailerSettings, new GUIContent("Tailer Settings"));
                }

                myScript.target = (GameObject)EditorGUILayout.ObjectField("Target", myScript.target, typeof(GameObject), true);

                if (EditorGUI.EndChangeCheck())
                {
                    EditorUtility.SetDirty(myScript);

                    serializedObject.ApplyModifiedProperties();
                }
            }

        }
#endif
        [System.Serializable]
        public class CruiserSettings
        {
            public int minSpeed = 2;
            public int maxSpeed = 4;
            public float minReflex = 1;
            public float maxReflex = 3;
            public float timer;
            public void Update(EnemyAI instance)
            {
                if (instance == null) return;
                if (instance.gameObject == null) return;

                if (timer > 0)
                {
                    timer -= 1 * Time.deltaTime;
                }
                else
                {
                    timer = Random.Range(minReflex, maxReflex);

                    float angle;

                    if (instance.target)
                    {
                        angle = Math2DHelpers.GetAngle(instance.target, instance.gameObject);
                    }
                    else
                    {
                        angle = Random.Range(0, 359) * Mathf.Deg2Rad;
                    }

                    _speed += Random.Range(minSpeed, maxSpeed) * new Vector3(Mathf.Cos(angle), Mathf.Sin(angle));
                }

                instance.transform.position += _speed * Time.deltaTime;

                _speed.x *= .99f;
                _speed.y *= .99f;
            }

            Vector3 _speed;
        }

        [System.Serializable]
        public class HomingMissileSettings
        {
            public float intro = 0;
            public float introSpeed = 8;
            public float speed = 8;
            public float rotationSpeed = 250f;
            public float friction;

            public void Update(EnemyAI instance)
            {
                if (instance == null) return;
                if (instance.gameObject == null) return;

                if (intro > 0)
                {
                    speed -= friction * Time.deltaTime;
                    intro -= 1 * Time.deltaTime;
                    if (intro < 0) intro = 0;
                }
                else
                {
                    if (instance.target) _direction = instance.target.transform.position - instance.transform.position;

                    float angle = Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg;

                    var targetRotation = Quaternion.AngleAxis(angle, Vector3.forward);

                    instance.transform.rotation = Quaternion.RotateTowards(instance.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
                }

                instance.transform.position += instance.transform.right * speed * Time.deltaTime;
            }

            Vector3 _direction;
        }

        [System.Serializable]
        public class TailerSettings
        {
            public float speed = 1;

            public void Update(EnemyAI instance)
            {
                if (instance == null) return;
                if (instance.gameObject == null) return;
                if (instance.target == null) return;

                instance.transform.position = Vector3.MoveTowards(instance.transform.position, instance.target.transform.position, speed * Time.deltaTime);
            }

            Vector3 _speed;
        }

        [System.Serializable]
        public class MagnetSettings
        {
            public static int globalPolarity = 1;

            public float range = 3;
            public int polarity = 1;
            public float speed = 5;
            public float maxSpeed = 75;
            public float friction = 1;
            public void Update(EnemyAI instance)
            {
                if (instance == null) return;
                if (instance.gameObject == null) return;
                if (instance.target == null) return;

                var angle = Math2DHelpers.GetAngle(instance.target, instance.gameObject);
                int distance = (int)(Mathf.Abs(instance.target.transform.position.x - instance.transform.position.x) + Mathf.Abs(instance.target.transform.position.y - instance.transform.position.y));

                if (distance > 0)
                {
                    _speed.x += Mathf.Cos(angle) * (int)(range / distance) * speed * Time.deltaTime;
                    _speed.y += Mathf.Sin(angle) * (int)(range / distance) * speed * Time.deltaTime;
                }

                if (_speed.x > maxSpeed) _speed.x = maxSpeed;
                if (_speed.x < -maxSpeed) _speed.x = -maxSpeed;
                if (_speed.y > maxSpeed) _speed.y = maxSpeed;
                if (_speed.y < -maxSpeed) _speed.y = -maxSpeed;

                instance.transform.position += globalPolarity * polarity * _speed * Time.deltaTime;

                if (friction != 0) _speed *= 1 / (1 + (Time.deltaTime * friction));
            }

            Vector3 _speed;
        }

        public enum Mode { Cruiser, HomingMissile, Magnet, Tailer };

        public Mode mode = Mode.HomingMissile;
        public CruiserSettings cruiserSettings;
        public HomingMissileSettings homingMissileSettings;
        public MagnetSettings magnetSettings;
        public TailerSettings tailerSettings;
        [Space(8)]
        public GameObject target;

        void Awake()
        {
            if (mode == Mode.HomingMissile && homingMissileSettings.intro > 0)
            {
                homingMissileSettings.friction = (homingMissileSettings.introSpeed - homingMissileSettings.speed) / homingMissileSettings.intro;
                homingMissileSettings.speed = homingMissileSettings.introSpeed;
            }
        }
        void Update()
        {
            if (target == null || target && target.activeInHierarchy == false) target = PlayersGroup.GetRandom();

            if (mode == Mode.Cruiser)
            {
                cruiserSettings.Update(this);
            }
            else if (mode == Mode.Magnet)
            {
                magnetSettings.Update(this);
            }
            else if (mode == Mode.HomingMissile)
            {
                homingMissileSettings.Update(this);
            }
            else if (mode == Mode.Tailer)
            {
                tailerSettings.Update(this);
            }
        }

        /*
        void Test1()
        {
            Vector3 direction = target.transform.position - transform.position;

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            transform.position += transform.right * speed * Time.deltaTime;
        }
        */

    }
}