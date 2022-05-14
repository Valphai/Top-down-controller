using UnityEngine;

namespace TopDownController
{
    public class XRay : MonoBehaviour
    {
        private SkinnedMeshRenderer mr;
        private Camera cam;
        private Material[] entityMaterials;
        [SerializeField] private Material[] XRayMaterials;
        [SerializeField] private LayerMask useLayer;

        private void Start()
        {
            cam = Camera.main;
        }
        private void OnEnable()
        {
            mr = GetComponentInChildren<SkinnedMeshRenderer>();
            entityMaterials = mr.materials;
        }

        private void Update()
        {
            if (mr.materials != XRayMaterials)
            {
                Ray ray = new Ray(
                    transform.position, cam.transform.position - transform.position
                );
                if (Physics.Raycast(ray, out RaycastHit hitInfo, Mathf.Infinity, useLayer))
                {
                    if (hitInfo.collider)
                    {
                        mr.materials = XRayMaterials;
                        return;
                    }
                }
            }
            else if (mr.materials != entityMaterials)
                mr.materials = entityMaterials;
        }
    }
}
