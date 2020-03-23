
using System.Collections;
using UnityEngine;
using UnityEngine.UI;


namespace GGJ2019.UI
{
    public sealed class MovementIndicator : MonoBehaviour
    {
        public RectTransform maxRefTr;
        public RectTransform minRefTr;

        public RectTransform arrowTr;
        public RectTransform[] dotTrs;

        public GameObject indicatorGroup;

        //==============================================================================

        private float _minDistance;
        private float _maxDistance;

        /////////////////////////////////////////////////////////////////////////////////////

        private IEnumerator Start()
        {
            yield return null;

            _minDistance = minRefTr.rect.width * 0.5f;
            _maxDistance = maxRefTr.rect.width * 0.5f;
        }

        /////////////////////////////////////////////////////////////////////////////////////

        public void update(Vector2 direction, float speedT)
        {
            if (speedT > 0.005f) {
                arrowTr.gameObject.SetActive(true);

                arrowTr.anchoredPosition = direction 
                    * Mathf.Lerp(_minDistance, _maxDistance, speedT);
                arrowTr.rotation = Quaternion.LookRotation(Vector3.back, direction);

                int totalDots = dotTrs.Length;
                float steps = 1f / (totalDots + 1);
                int dotCount = Mathf.Min(
                        Mathf.FloorToInt(speedT / steps), totalDots);

                for (int i = 0; i < dotCount; i++) {
                    dotTrs[i].gameObject.SetActive(true);
                }
                for (int i = dotCount; i < totalDots; i++) {
                    dotTrs[i].gameObject.SetActive(false);
                }

                float dotOffset = speedT / (dotCount + 1);
                for (int i = 0; i < dotCount; i++) {
                    dotTrs[i].anchoredPosition = direction
                        * Mathf.Lerp(_minDistance, _maxDistance, dotOffset * (i + 1));
                }
            }
        }
    }
}
