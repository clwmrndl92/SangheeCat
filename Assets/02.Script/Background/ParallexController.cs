using UnityEngine;

namespace SangheeCat{

    public class ParallaxController : MonoBehaviour
    {
        Player player;

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
            player = GameObject.Find("Player").GetComponent<Player>();
            _cam = Camera.main;
            startZoomSize = Camera.main.orthographicSize;
            
            foreach (var layer in layers)
            {
                layer.startPosX = layer.layerObject.transform.position.x;
                layer.length = layer.layerObject.GetComponent<SpriteRenderer>().bounds.size.x;

                layer.startSize = layer.layerObject.transform.localScale;
                
            }
        }

        void LateUpdate () {
            float zoom = _cam.orthographicSize / startZoomSize;
            foreach (var layer in layers)
            {
                float speed = layer.parallaxFactor * player.velocity.x * Time.deltaTime;
                
                // Disable movement
                layer.layerObject.transform.position = new Vector3(layer.layerObject.transform.position.x - speed, layer.layerObject.transform.position.y, layer.layerObject.transform.position.z);
                if (layer.layerObject.transform.position.x > layer.startPosX + layer.length || layer.layerObject.transform.position.x < layer.startPosX - layer.length) 
                    layer.layerObject.transform.position = new Vector3(layer.startPosX, layer.layerObject.transform.position.y, layer.layerObject.transform.position.z);

                
                layer.layerObject.transform.localScale =  layer.startSize + layer.startSize * (zoom-1) * layer.zoomFactor;
            }
        }
    }

}