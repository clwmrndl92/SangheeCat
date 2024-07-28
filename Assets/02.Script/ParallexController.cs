using UnityEngine;

namespace SHProject{

    public class ParallaxController : MonoBehaviour
    {

        [System.Serializable]
        public class ParallaxLayer
        {
            public GameObject layerObject;
            public float parallaxFactor;
            [HideInInspector]
            public float length;
            [HideInInspector]
            public float startPosX;
        }

        public ParallaxLayer[] layers;
        
        private GameObject _cam;

        void Start()
        {
            _cam = Camera.main.gameObject;
            
            foreach (var layer in layers)
            {
                layer.startPosX = layer.layerObject.transform.position.x - (_cam.transform.position.x * layer.parallaxFactor);
                layer.length = layer.layerObject.GetComponent<SpriteRenderer>().bounds.size.x;
                
            }
        }

        void FixedUpdate () {
            foreach (var layer in layers)
            {
                float temp = (_cam.transform.position.x * (1-layer.parallaxFactor));
                float dist = (_cam.transform.position.x * layer.parallaxFactor);
                
                layer.layerObject.transform.position = new Vector3(layer.startPosX + dist, layer.layerObject.transform.position.y, layer.layerObject.transform.position.z);
                if (temp > layer.startPosX + layer.length) layer.startPosX += 2*layer.length;
                else if (temp < layer.startPosX - layer.length) layer.startPosX -= 2*layer.length;
            }
        }
    }

}