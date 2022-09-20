namespace Playniax.Ignition.Framework
{
    // Collection of array functions.
    public class ArrayHelpers
    {
        // Returns a new array with added value.
        public static T[] Add<T>(T[] array, T value)
        {
            if (array == null) array = new T[0];
            T[] result = new T[array.Length + 1];
            array.CopyTo(result, 0);
            result[array.Length] = value;
            return result;
        }

        // Returns a new array with inserted value.
        public static T[] Insert<T>(T[] array, T value)
        {
            if (array == null) array = new T[0];
            T[] result = new T[array.Length + 1];
            array.CopyTo(result, 1);
            result[0] = value;
            return result;
        }

        // Returns a new array with the first value removed.
        public static T[] Skim<T>(T[] array)
        {
            if (array.Length < 1) return array;
            T[] result = new T[array.Length - 1];
            System.Array.Copy(array, 1, result, 0, result.Length);
            return result;
        }
    }
}
