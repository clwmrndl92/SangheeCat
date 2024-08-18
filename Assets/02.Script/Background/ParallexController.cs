using UnityEngine;

namespace SangheeCat{

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

            public float zoomFactor;
            [HideInInspector]
            public Vector3 startSize;
        }

        public ParallaxLayer[] layers;
        
        private Camera _cam;
        float startZoomSize;

        void Start()
        {
            _cam = Camera.main;
            startZoomSize = Camera.main.orthographicSize;
            
            foreach (var layer in layers)
            {
                layer.startPosX = layer.layerObject.transform.position.x - (_cam.transform.position.x * layer.parallaxFactor);
                // layer.length = layer.layerObject.GetComponent<SpriteRenderer>().bounds.size.x;

                layer.startSize = layer.layerObject.transform.localScale;
                
            }
        }

        void LateUpdate () {
            float zoom = _cam.orthographicSize / startZoomSize;
            foreach (var layer in layers)
            {
                float temp = (_cam.transform.position.x * (1-layer.parallaxFactor));
                float dist = (_cam.transform.position.x * layer.parallaxFactor);
                
                layer.layerObject.transform.position = new Vector3(layer.startPosX + dist, layer.layerObject.transform.position.y, layer.layerObject.transform.position.z);
                // if (temp > layer.startPosX + layer.length) layer.startPosX += 2*layer.length;
                // else if (temp < layer.startPosX - layer.length) layer.startPosX -= 2*layer.length;

                
                layer.layerObject.transform.localScale =  layer.startSize + layer.startSize * (zoom-1) * layer.zoomFactor;
            }
        }
    }

}