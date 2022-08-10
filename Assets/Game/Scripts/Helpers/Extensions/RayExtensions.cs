using UnityEngine;

namespace Game.Scripts.Helpers.Extensions
{

    public static class RayExtensions
    {

        public static Vector3 GetRayHitPointToObject(this Ray ray, string _tag, Transform target)
        {
            var mainCamera = Camera.main;
            var hitPoint = Vector3.zero;
            
            ray = new Ray(mainCamera!.transform.position, target.position - mainCamera.transform.position);

            if (!Physics.Raycast(ray, out var hit, 1000f)) return hitPoint;
            
            if (hit.collider.CompareTag(_tag))
            {
                hitPoint = hit.point;
            }

            return hitPoint;
        }

        public static Vector3 GetRayHitPointOfMouse(this Ray ray, string _tag)
        {
            RaycastHit hit;
            Vector3 v = Vector3.zero;

            ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, 1000f))
            {
                if (hit.collider != null)
                {
                    if (hit.collider.CompareTag(_tag))
                    {
                        v = hit.point;
                        Debug.Log("HIT FOUND :" + hit.collider.gameObject.name + " ====> " + v);
                    }
                    else
                        Debug.Log("Tag Not Matched");
                }
                else
                {
                    Debug.Log("No Hit Collision");
                }
            }
            else
                Debug.Log("No Hit Collision 2");


            return v;
        }

        public static Vector3 GetRayHitPoint(this Ray ray, string _tag, GameObject go, Camera cam)
        {
            RaycastHit hit;
            Vector3 v = Vector3.zero;

            ray = new Ray(cam.transform.position, cam.transform.forward);

            if (Physics.Raycast(ray, out hit, 1000f))
            {
                if (!string.IsNullOrEmpty(_tag))
                {
                    if (hit.collider.CompareTag(_tag))
                    {
                        if (go == null)
                            return hit.point;
                        else if (go == hit.collider.gameObject)
                            return hit.point;
                    }
                }
                else
                {
                    if (go == null)
                        return hit.point;
                    else if (go == hit.collider.gameObject)
                        return hit.point;
                }
            }


            return v;
        }

        public static bool IsMouseOverObject(this Ray ray, string _tag, GameObject go, Camera cam)
        {
            bool onObj = false;
            RaycastHit hit;

            ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, 100f))
            {
                if (!string.IsNullOrEmpty(_tag))
                {
                    if (hit.collider.CompareTag(_tag))
                    {
                        if (go == null)
                            onObj = true;
                        else if (go == hit.collider.gameObject)
                            onObj = true;
                    }
                }
                else
                {
                    if (go == null)
                        onObj = true;
                    else if (go == hit.collider.gameObject)
                        onObj = true;
                }
            }


            return onObj;
        }

        public static bool IsMouseOverObject(this Ray ray, string _tag) => IsMouseOverObject(ray, _tag, null, Camera.main);

        public static bool IsMouseOverObject(this Ray ray, string _tag, GameObject go) => IsMouseOverObject(ray, _tag, go, Camera.main);

        public static bool IsMouseOverObject(this Ray ray, string _tag, Camera cam) => IsMouseOverObject(ray, _tag, null, cam);

        public static bool IsMouseOverObject(this Ray ray, GameObject go) => IsMouseOverObject(ray, "", go, Camera.main);

        public static bool IsMouseOverObject(this Ray ray, GameObject go, Camera cam) => IsMouseOverObject(ray, "", go, cam);
        
        public class HitInfo
        {
            public bool isHit;
            public Vector3 hitPoint;
            public GameObject go;
        }

        public static HitInfo GetRayInfoToObject(this Ray ray, string _tag, Transform target, LayerMask layer)
        {
            HitInfo hi = new HitInfo();

            RaycastHit hit;
            ray = new Ray(Camera.main.transform.position, target.position - Camera.main.transform.position);

            if (Physics.Raycast(ray, out hit, 1000f, layer))
            {
                if (hit.collider.CompareTag(_tag))
                {
                    hi.isHit = true;
                    hi.hitPoint = hit.point;
                    hi.go = hit.collider.gameObject;
                }
                else
                    hi.isHit = false;
            }

            return hi;
        }

        public static HitInfo GetRayHitInfo(this Ray ray, string _tag, LayerMask mask)
        {
            RaycastHit hit;
            HitInfo hi = new HitInfo();
            hi.hitPoint = Vector3.zero;
            hi.isHit = false;
            Camera activeCam = Camera.main;

            ray = activeCam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, 1000f, mask))
            {
                if (hit.collider.CompareTag(_tag) || string.IsNullOrEmpty(_tag))
                {
                    hi.go = hit.collider.gameObject;
                    hi.isHit = true;
                    hi.hitPoint = hit.point;
                }
            }

            return hi;
        }
    }
}

