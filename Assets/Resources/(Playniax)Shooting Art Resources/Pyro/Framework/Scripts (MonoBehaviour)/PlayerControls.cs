#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using Playniax.Ignition.Framework;

namespace Playniax.Pyro.Framework
{
    [AddComponentMenu("Playniax/Pyro/Framework/PlayerControls")]
    public class PlayerControls : MonoBehaviour
    {
#if UNITY_EDITOR
        [CanEditMultipleObjects]
        [CustomEditor(typeof(PlayerControls))]
        public class Inspector : Editor
        {
            SerializedProperty swipeSettings;
            SerializedProperty zigZagSettings;
            SerializedProperty wasdSettings;
            SerializedProperty mouseSettings;
            SerializedProperty rocketSettings;
            void OnEnable()
            {
                swipeSettings = serializedObject.FindProperty("swipeSettings");
                zigZagSettings = serializedObject.FindProperty("zigZagSettings");
                wasdSettings = serializedObject.FindProperty("wasdSettings");
                mouseSettings = serializedObject.FindProperty("mouseSettings");
                rocketSettings = serializedObject.FindProperty("rocketSettings");
            }
            override public void OnInspectorGUI()
            {
                EditorGUI.BeginChangeCheck();

                serializedObject.Update();

                var myScript = target as PlayerControls;

                EditorGUI.BeginDisabledGroup(true);
                EditorGUILayout.ObjectField("Script", MonoScript.FromMonoBehaviour(myScript), typeof(PlayerControls), false);
                EditorGUI.EndDisabledGroup();

                myScript.speed = EditorGUILayout.FloatField("Speed", myScript.speed);

                myScript.mode = (Mode)EditorGUILayout.EnumPopup("Mode", myScript.mode);

                if (myScript.mode == Mode.Swipe)
                {
                    EditorGUILayout.PropertyField(swipeSettings, new GUIContent("Swipe Settings"));
                }
                else if (myScript.mode == Mode.ZigZag)
                {
                    EditorGUILayout.PropertyField(zigZagSettings, new GUIContent("Zig Zag Settings"));
                }
                else if (myScript.mode == Mode.WASD)
                {
                    EditorGUILayout.PropertyField(wasdSettings, new GUIContent("WASD Settings"));
                }
                else if (myScript.mode == Mode.Mouse)
                {
                    EditorGUILayout.PropertyField(mouseSettings, new GUIContent("Mouse Settings"));
                }
                else if (myScript.mode == Mode.Rocket)
                {
                    EditorGUILayout.PropertyField(rocketSettings, new GUIContent("Rocket Settings"));
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
        public class MouseSettings
        {
            public Vector2 steps = new Vector2(16, 16);
            public bool requireMouseButton = true;
            public bool horizontal = true;
            public bool vertical = true;
            public void Update(PlayerControls instance)
            {
                if (Input.GetMouseButton(0) || requireMouseButton == false)
                {
                    var mousePosition = Input.mousePosition;

                    mousePosition.z = instance.transform.position.z - Camera.main.transform.position.z;

                    mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

                    var position = instance.transform.position;

                    if (horizontal == true) position.x += (mousePosition.x - position.x) / steps.x;
                    if (vertical == true) position.y += (mousePosition.y - position.y) / steps.y;

                    instance.transform.position = position;
                }
            }
        }

        [System.Serializable]
        public class RocketSettings
        {
            public string horizontal = "Horizontal";
            public string vertical = "Vertical";
            public float rotationSpeed = 250;
            public float speed = 10;
            public float friction = 1f;
            public Vector3 velocity;

            public void Update(PlayerControls instance)
            {
                var x = Input.GetAxis(horizontal);
                var y = Input.GetAxis(vertical);

                instance.transform.Rotate(0, 0, -x * rotationSpeed * Time.deltaTime);

                float angle = instance.gameObject.transform.eulerAngles.z;

                velocity += Quaternion.AngleAxis(angle, Vector3.forward) * Vector3.right * y * speed * instance.speed * Time.deltaTime;

                instance.gameObject.transform.position += velocity * Time.deltaTime;

                if (friction != 0) velocity *= 1 / (1 + (Time.deltaTime * friction));
            }
        }

        [System.Serializable]
        public class SwipeSettings
        {
            public float speed = 5;
            public bool rotation = true;
            public float rotationSpeed = 250;
            public float friction = 1f;
            public Vector3 velocity;
            public bool boundaries = true;
            public bool horizontal = true;
            public bool vertical = true;
            public void Awake(PlayerControls instance)
            {
                if (instance.gameObject.activeInHierarchy && _GetSelected() == null) _SetSelected(instance.gameObject);

                velocity = instance.gameObject.transform.rotation * Vector3.right * Mathf.Epsilon;
            }
            public void Update(PlayerControls instance)
            {
                if (Input.GetMouseButton(0) && _GetSelected(instance.gameObject) == instance.gameObject)
                {
                    var mousePosition = Input.mousePosition;

                    if (_previous == Vector3.zero) _previous = mousePosition;

                    velocity += (mousePosition - _previous) * speed * instance.speed * Time.deltaTime;

                    _previous = mousePosition;
                }
                else if (Input.GetMouseButtonDown(0) && _GetSelected(instance.gameObject) != instance)
                {
                    var size = RendererHelpers.GetSize(instance.gameObject) * .5f;

                    var mousePosition = CameraHelpers.GetMousePosition();

                    if (Math2DHelpers.PointInsideRect(mousePosition.x, mousePosition.y, instance.gameObject.transform.position.x, instance.gameObject.transform.position.y, size.x, size.y))
                    {
                        _SetSelected(instance.gameObject);
                    }
                }
                else
                {
                    _previous = Vector3.zero;
                }

                float angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;

                if (rotation)
                {
                    var targetRotation = Quaternion.AngleAxis(angle, Vector3.forward);

                    instance.gameObject.transform.rotation = Quaternion.RotateTowards(instance.gameObject.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
                }

                if (horizontal == false) velocity.x = 0;
                if (vertical == false) velocity.y = 0;

                instance.gameObject.transform.position += velocity * Time.deltaTime;

                if (boundaries) _Bounds(instance);

                if (friction != 0) velocity *= 1 / (1 + (Time.deltaTime * friction));
            }

            void _Bounds(PlayerControls instance)
            {
                var min = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, instance.transform.position.z - Camera.main.transform.position.z));
                var max = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, instance.transform.position.z - Camera.main.transform.position.z));
                /*
                min.x -= additionalSettings.size.x / 2;
                max.x += additionalSettings.size.x / 2;

                min.y += additionalSettings.size.y / 2;
                max.y -= additionalSettings.size.y / 2;
                */
                var position = instance.transform.position;

                if (position.x > max.x)
                {
                    position.x = max.x;
                    velocity.x = 0;
                }
                else if (position.x < min.x)
                {
                    position.x = min.x;
                    velocity.x = 0;
                }

                if (position.y > min.y)
                {
                    position.y = min.y;
                    velocity.y = 0;
                }
                else if (position.y < max.y)
                {
                    position.y = max.y;
                    velocity.y = 0;
                }

                instance.transform.position = position;
            }
            GameObject _GetSelected(GameObject fallBack = null)
            {
                if (_selected) return _selected;

                var list = PlayersGroup.GetList();

                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i] && list[i].gameObject != null && list[i].isActiveAndEnabled)
                    {
                        _selected = list[i].gameObject;

                        return _selected;
                    }
                }

                return fallBack;
            }

            void _SetSelected(GameObject gameObject)
            {
                if (gameObject && PlayersGroup.IsMember(gameObject)) _selected = gameObject;
            }

            Vector3 _previous;
            static GameObject _selected;
        }

        [System.Serializable]
        public class WASDSettings
        {
            public string horizontal = "Horizontal";
            public string vertical = "Vertical";
            public float speed = 8;
            public bool boundaries = true;
            public void Update(PlayerControls instance)
            {
                var h = Input.GetAxis(horizontal);
                var v = Input.GetAxis(vertical);

                instance.gameObject.transform.position += new Vector3(h, v) * Time.deltaTime * speed * instance.speed;

                if (boundaries) _Bounds(instance);
            }
            void _Bounds(PlayerControls instance)
            {
                var min = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, instance.transform.position.z - Camera.main.transform.position.z));
                var max = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, instance.transform.position.z - Camera.main.transform.position.z));
                /*
                min.x -= additionalSettings.size.x / 2;
                max.x += additionalSettings.size.x / 2;

                min.y += additionalSettings.size.y / 2;
                max.y -= additionalSettings.size.y / 2;
                */
                var position = instance.transform.position;

                if (position.x > max.x)
                {
                    position.x = max.x;
                }
                else if (position.x < min.x)
                {
                    position.x = min.x;
                }

                if (position.y > min.y)
                {
                    position.y = min.y;
                }
                else if (position.y < max.y)
                {
                    position.y = max.y;
                }

                instance.transform.position = position;
            }
        }
        [System.Serializable]
        public class ZigZagSettings
        {
            public KeyCode controlKey;
            public float rotationSpeed = 100;
            public float speed = 1;
            public void Update(PlayerControls instance)
            {
                if (_previousDirection == 0 && _direction != 0) _previousDirection = _direction;

                if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(controlKey))
                {
                    _direction = 0;
                }
                else if (Input.GetMouseButtonUp(0) || Input.GetKeyUp(controlKey))
                {
                    if (_direction == 0) _direction = _previousDirection;
                    _direction = -_direction;
                    _previousDirection = _direction;
                }

                instance.gameObject.transform.Rotate(new Vector3(0, 0, _direction * rotationSpeed) * Time.deltaTime);

                instance.gameObject.transform.position += instance.gameObject.transform.right * speed * instance.speed * Time.deltaTime;
            }
            int _direction = 1;
            int _previousDirection;
        }

        public enum Mode { Swipe, ZigZag, WASD, Mouse, Rocket };

        public float speed = 1;
        public Mode mode = Mode.Swipe;
        public SwipeSettings swipeSettings;
        public ZigZagSettings zigZagSettings;
        public WASDSettings wasdSettings;
        public MouseSettings mouseSettings;
        public RocketSettings rocketSettings;
        void Awake()
        {
            if (mode == Mode.Swipe)
            {
                swipeSettings.Awake(this);
            }
        }
        void Update()
        {
            if (mode == Mode.Mouse)
            {
                mouseSettings.Update(this);
            }
            else if (mode == Mode.Swipe)
            {
                swipeSettings.Update(this);
            }
            else if (mode == Mode.WASD)
            {
                wasdSettings.Update(this);
            }
            else if (mode == Mode.ZigZag)
            {
                zigZagSettings.Update(this);
            }
            else if (mode == Mode.Rocket)
            {
                rocketSettings.Update(this);
            }
        }
    }
}