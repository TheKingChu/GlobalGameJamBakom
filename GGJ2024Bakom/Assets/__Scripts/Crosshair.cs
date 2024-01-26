using UnityEngine;
using UnityEngine.UIElements;

namespace __Scripts
{
    public class Reticle : MonoBehaviour
    {
        private RectTransform reticle;

        public float restingSize;
        public float maxSize;
        public float speed;
        private float currSize;

        // Start is called before the first frame update
        void Start()
        {
            reticle = GetComponent<RectTransform>();
        }

        // Update is called once per frame
        void Update()
        {
            if (IsMoving)
            {
                currSize = Mathf.Lerp(currSize, maxSize, Time.deltaTime * speed);
            }
            else
            {
                currSize = Mathf.Lerp(currSize,restingSize,Time.deltaTime * speed);
            }
            reticle.sizeDelta = new Vector2(currSize,currSize);
        }

        private static bool IsMoving => Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0 || Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0;
    }
}