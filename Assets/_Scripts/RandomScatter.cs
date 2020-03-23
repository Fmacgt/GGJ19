
using System.Collections;
using UnityEngine;


namespace GGJ2019
{
    public sealed class RandomScatter : MonoBehaviour
    {
        public GameObject prefab;

        public Transform containerTr;
        public int amountToSpawn = 10;
        public Vector2 scaleRange;
        public Vector2 radiusRange;

        /////////////////////////////////////////////////////////////////////////////////////

        private void Awake()
        {
            for (int i = 0; i < amountToSpawn; i++) {
                var obj = Instantiate(prefab, containerTr);

                var forward = Vector3.forward * Random.Range(radiusRange.x, radiusRange.y);
                var offset = Quaternion.AngleAxis(Random.Range(0, 360), Vector3.up) * forward;
                obj.transform.localPosition = offset;
                obj.transform.localScale = new Vector3(
                        Random.Range(scaleRange.x, scaleRange.y),
                        Random.Range(scaleRange.x, scaleRange.y),
                        Random.Range(scaleRange.x, scaleRange.y));
            }
        }
    }
}
