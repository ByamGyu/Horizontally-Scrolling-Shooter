using UnityEngine;

namespace Playniax.Ignition.Framework
{
    // Collection of camera functions.
    public static class CameraHelpers
    {
        // Returns whether the renderer is visible or not.
        public static bool IsVisible(Renderer renderer, Camera camera = null)
        {
            if (camera == null) camera = Camera.main;

            Plane[] planes = GeometryUtility.CalculateFrustumPlanes(camera);
            return GeometryUtility.TestPlanesAABB(planes, renderer.bounds);
        }

        // Returns bounds.
        public static Bounds OrthographicBounds(Camera camera = null)
        {
            if (camera == null) camera = Camera.main;

            float aspect = (float)Screen.width / (float)Screen.height;
            float height = camera.orthographicSize * 2;
            return new Bounds(camera.transform.position, new Vector3(height * aspect, height, 0));
        }

        // Returns mouse position.
        public static Vector3 GetMousePosition(Camera camera = null)
        {
            if (camera == null) camera = Camera.main;

            var ray = camera.ScreenPointToRay(Input.mousePosition);

            return ray.GetPoint(-camera.transform.position.z);
        }

        // Sets virtual width.
        public static void SetOrthographicWidth(int virtualWidth, Camera camera = null)
        {
            if (camera == null) camera = Camera.main;

            float width = Screen.width;
            float height = Screen.height;

            var aspect = width / height;

            camera.orthographicSize = (float)virtualWidth / aspect / 200;
        }

        // Sets virtual height.
        public static void SetOrthographicHeight(int virtualHeight, Camera camera = null)
        {
            if (camera == null) camera = Camera.main;

            float width = Screen.width;
            float height = Screen.height;

            camera.orthographicSize = (float)virtualHeight / 200;
        }

        // Sets virtual width and height.
        public static void SetOrthographic(int virtualWidth, int virtualHeight, Camera camera = null)
        {
            if (camera == null) camera = Camera.main;

            float width = Screen.width;
            float height = Screen.height;

            var aspect = width / height;

            var x = (float)virtualWidth / aspect / 200;
            var y = camera.orthographicSize = (float)virtualHeight / 200;

            camera.orthographicSize = Mathf.Max(x, y);
        }
    }
}