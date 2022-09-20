namespace Playniax.Ignition.Framework
{
    [System.Serializable]
    // PlayerData can be used to temporarily store data of the game like lives, name or score of the player. 
    //
    // lives, name of score are built-in data fields.
    //
    // Custom fields can be added to custom and supports floats, integers, booleans and strings.
    //
    // This information will not be saved and is only avaiable at runtime.
    //
    // Example(s):
    //
    // Set name of the player              : PlayerData.Get(0).name = "Tony"
    //
    // Increase the score of the player    : PlayerData.Get(0).scoreboard += 100;
    //
    // Custom variable                     : PlayerData.Get(0).custom.SetBool("invincible", true);
    public class PlayerData
    {
        public static int defaultLives = 3;

        public string name;
        public int scoreboard;
        public int lives = defaultLives;

        //public List<string> rewards;

        // Custom data storage.
        public Config custom = new Config();
        public static int CountLives()
        {
            if (_data == null) return 0;

            var lives = 0;

            for (int i = 0; i < _data.Length; i++)
            {
                lives += _data[i].lives;
            }
            
            return lives;
        }
        // Returns the PlayerData for the player by index.
        public static PlayerData Get(int index = 0)
        {
            if (index < 0) return null;
            if (_data == null) System.Array.Resize(ref _data, index + 1);
            if (index >= _data.Length) System.Array.Resize(ref _data, index + 1);
            if (_data[index] == null) _data[index] = new PlayerData();

            return _data[index];
        }
        // Returns the PlayerData for the player by name.
        public static PlayerData Get(string name)
        {
            if (_data != null)
            {
                for (int i = 0; i < _data.Length; i++)
                {
                    if (_data[i].name == name) return Get(i);
                }

            }

            return null;
        }
        // Reset to defaults
        public static void Reset(int lives)
        {
            if (_data == null) return;

            for (int i = 0; i < _data.Length; i++)
            {
                _data[i].scoreboard = 0;
                _data[i].lives = lives;

                //if (_data[i].rewards != null) _data[i].rewards.Clear();

                _data[i].custom.Clear();
            }
        }
        // Set number of lives
        public static void SetLives(int lives)
        {
            if (_data == null) return;

            for (int i = 0; i < _data.Length; i++)
            {
                _data[i].lives = lives;
            }
        }

        static PlayerData[] _data;
    }
}