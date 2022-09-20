using UnityEngine;

namespace Playniax.Ignition.UI
{
    public class ScrollBoxAutoScroll : MonoBehaviour
    {
        public ScrollBox scrollBox;
        public float speed = 50;

        void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();

            if (scrollBox == null) scrollBox = GetComponent<ScrollBox>();
        }

        void Update()
        {
            if (_init == false && scrollBox.contentHeight > 0)
            {
                scrollBox.content.transform.localPosition = new Vector3(scrollBox.content.transform.localPosition.x, -_rectTransform.sizeDelta.y, scrollBox.content.transform.localPosition.z);

                _init = true;
            }

            scrollBox.content.transform.position += new Vector3(0, speed * Time.unscaledDeltaTime);

            if (scrollBox.content.transform.localPosition.y > scrollBox.contentHeight)
            {
                scrollBox.content.transform.localPosition = new Vector3(scrollBox.content.transform.localPosition.x, -_rectTransform.sizeDelta.y, scrollBox.content.transform.localPosition.z);
            }
        }

        bool _init;
        RectTransform _rectTransform;
    }
}