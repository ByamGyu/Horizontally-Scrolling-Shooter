using UnityEditor;
using UnityEngine;
using Playniax.Ignition.Framework;
using Playniax.Pyro.Framework;
using Playniax.ParticleSystem;
using Playniax.Menu.Essentials;

namespace Playniax.Menu.Prototyping
{
    public class Menu : EditorBase
    {
        [MenuItem("GameObject/2D Object/Playniax/Prototyping/Engines/Pyro", false, 101)]
        public static void Add_Engine()
        {
            var engine = new GameObject("Engine");

            Undo.RegisterCreatedObjectUndo(engine.gameObject, "Create object");

            Selection.activeGameObject = engine.gameObject;

            GetAssetAtPath("Assets/Playniax/Prototyping/Prefabs/Engine/Pyro/Audio Channels.prefab");
            Selection.activeGameObject = engine.gameObject;
            GetAssetAtPath("Assets/Playniax/Prototyping/Prefabs/Engine/Pyro/Collision Audio.prefab");
            Selection.activeGameObject = engine.gameObject;
            GetAssetAtPath("Assets/Playniax/Prototyping/Prefabs/Engine/Pyro/Collision Monitor.prefab");
            Selection.activeGameObject = engine.gameObject;
            GetAssetAtPath("Assets/Playniax/Prototyping/Prefabs/Engine/Pyro/Messenger.prefab");
            Selection.activeGameObject = engine.gameObject;
            GetAssetAtPath("Assets/Playniax/Prototyping/Prefabs/Engine/Pyro/Particle Effects.prefab");
            Selection.activeGameObject = engine.gameObject;
        }

        public static void Add_Smart_Engine()
        {
            var audioChannels = Object.FindObjectOfType<AudioChannels>();
            var collisionAudio = Object.FindObjectOfType<CollisionAudio>();
            var collisionMonitor = Object.FindObjectOfType<CollisionMonitor2D>();
            var messenger = Object.FindObjectOfType<Messenger>();
            var emitter = Object.FindObjectOfType<EmitterGroup>();

            if (audioChannels) return;
            if (collisionAudio) return;
            if (collisionMonitor) return;
            if (messenger) return;
            if (emitter) return;

            Add_Engine();

            Selection.activeGameObject = null;
        }

        [MenuItem("GameObject/2D Object/Playniax/Prototyping/Sprites/Players/Player (Spaceship)", false, 101)]
        public static void Add_Player_01()
        {
            Prototyping.Menu.Add_Smart_Engine();

            Add("Assets/Playniax/Prototyping/Prefabs/Players/Player (Spaceship).prefab");
        }

        [MenuItem("GameObject/2D Object/Playniax/Prototyping/Sprites/Players/Player (Spaceship Weaponized)", false, 101)]
        public static void Add_Player_02()
        {
            Prototyping.Menu.Add_Smart_Engine();

            Add("Assets/Playniax/Prototyping/Prefabs/Players/Player (Spaceship Weaponized).prefab");
        }

        [MenuItem("GameObject/2D Object/Playniax/Prototyping/Sprites/Enemies/Enemy", false, 101)]
        public static void Enemey_01()
        {
            Prototyping.Menu.Add_Smart_Engine();

            Add("Assets/Playniax/Prototyping/Prefabs/Enemies/Enemy.prefab");
        }
    }
}


