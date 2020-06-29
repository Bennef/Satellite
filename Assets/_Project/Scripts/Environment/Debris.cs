using System.Collections;
using UnityEngine;

namespace Scripts.Environment
{
    public class Debris : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            StartCoroutine(ScaleOverTime(1));
        }

        IEnumerator ScaleOverTime(float time)
        {
            Vector3 originalScale = transform.localScale;
            Vector3 destinationScale = new Vector3(2.0f, 2.0f, 2.0f);

            float currentTime = 0.0f;

            do
            {
                transform.localScale = Vector3.Lerp(originalScale, destinationScale, currentTime / time);
                currentTime += Time.deltaTime;
                yield return null;
            } while (currentTime <= time);

            Destroy(gameObject);
        }
    }
}