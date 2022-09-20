using System.Collections.Generic;
using UnityEngine;

namespace Playniax.Ignition.Framework
{
    [AddComponentMenu("Playniax/Ignition/Framework/PlayersGroup")]
    // Whether the GameObject is 'marked' as a player or not. Some of the AI and bullet spawners depend on it.
    public class PlayersGroup : MonoBehaviour
    {
        // Player id.
        public string id = "Player 1";
        // Returns the total of active players from the group.
        public static int Count()
        {
            int count = 0;

            var list = GetList();

            for (int i = 0; i < list.Count; i++)
            {
                if (list[i] && list[i].isActiveAndEnabled) count += 1;
            }

            return count;
        }
        /*
        public static int CountVisible(Camera camera = null)
        {
            if (camera == null) camera = Camera.main;

            int count = 0;

            var list = GetList();

            for (int i = 0; i < list.Count; i++)
            {
                if (list[i] && list[i].isActiveAndEnabled)
                {
                    var position = list[i].gameObject.transform.position;

                    var min = camera.ViewportToWorldPoint(new Vector3(0, 1, position.z - camera.transform.position.z));
                    var max = camera.ViewportToWorldPoint(new Vector3(1, 0, position.z - camera.transform.position.z));

                    if (position.x > min.x && position.x < max.x && position.y > max.y && position.y < min.y) count += 1;
                }
            }

            return count;
        }
        */
        // Returns the player by id.
        public static GameObject Get(string id)
        {
            var list = GetList();

            for (int i = 0; i < list.Count; i++)
            {
                if (list[i] && list[i].isActiveAndEnabled && list[i].id == id) return list[i].gameObject;
            }

            return null;
        }
        // Returns the first player from the group.
        public static GameObject GetFirstAvailable(GameObject locked = null)
        {
            if (locked && locked.activeInHierarchy) return locked;

            var list = GetList();

            for (int i = 0; i < list.Count; i++)
            {
                if (list[i] && list[i].gameObject && list[i].isActiveAndEnabled == false) return list[i].gameObject;
            }

            return null;
        }
        public static GameObject GetFirstAvailableAndLock()
        {
            return GetFirstAvailable(_firstAvailableLocked);
        }
        // Returns a list of all players in the group.
        public static List<PlayersGroup> GetList()
        {
            if (_update == false) return _list;

            _update = false;

            var list = FindObjectsOfType<PlayersGroup>();

            _list.Clear();

            for (int i = 0; i < list.Length; i++)
            {
                _list.Add(list[i]);
            }

            return _list;
        }
        // Returns a random player from the group.
        public static GameObject GetRandom(GameObject locked = null)
        {
            if (locked && locked.activeInHierarchy) return locked;

            var list = GetList();

            if (list == null) return null;
            if (list.Count == 0) return null;

            int index = Random.Range(0, list.Count);

            if (list[index] == null) return null;

            return list[index].gameObject;
        }
        public static GameObject GetRandomAndLock()
        {
            return GetRandom(_randomLocked);
        }
        // Returns whether player is a member of the group or not.
        public static bool IsMember(GameObject gameObject)
        {
            if (gameObject == null) return false;

            var group = gameObject.GetComponent<PlayersGroup>();
            if (group == null) return false;

            var list = GetList();

            for (int i = 0; i < list.Count; i++)
            {
                if (list[i] != null && list[i] == group) return true;
            }

            return false;
        }
        void OnEnable()
        {
            _update = true;
        }
        void OnDisable()
        {
            _update = true;
        }

        static List<PlayersGroup> _list = new List<PlayersGroup>();
        static GameObject _firstAvailableLocked;
        static GameObject _randomLocked;
        static bool _update = true;
    }
}