using UnityEngine;

namespace TopDownController.Controller
{
    public class CameraMovement : MonoBehaviour
    {
        public float StickMinZoom, StickMaxZoom;
        public float SwivelMinZoom, SwivelMaxZoom;
        public float MoveSpeedMinZoom, MoveSpeedMaxZoom;
        public float RotationSpeed;
        public BoxCollider Bounds;
        public LayerMask MouseWheelDraggable;
        
        // [HideInInspector] 
        public Transform LockedTransform;
        private Transform swivel, stick;
        private Camera cam;
        private EdgeRect screenPan;
        /// <summary>The starting position of swivel & stick</summary>
        private float zoom = 0.6f;
        private float rotationAngle;
        private float screenEdgePan = 25f;
        private const float yRot = 50f;
        private Vector3 dragStartPos;
        private Vector3 dragCurrentPos;

        private struct EdgeRect
        {
            public Rect Lft, Up, Rht, Dwn;

            public EdgeRect(Rect lft, Rect up, Rect rht, Rect dwn)
            {
                this.Lft = lft;
                this.Up = up;
                this.Rht = rht;
                this.Dwn = dwn;
            }
        }

        private void Awake() 
        {
            cam = Camera.main;
        }
        private void OnEnable()
        {
            swivel = transform.GetChild(0);
            stick = swivel.GetChild(0);

            screenPan = new EdgeRect(
                new Rect(0, 0, screenEdgePan, Screen.height),
                new Rect(0, Screen.height - screenEdgePan, Screen.width, screenEdgePan),
                new Rect(Screen.width - screenEdgePan, 0, screenEdgePan, Screen.height),
                new Rect(0, 0, Screen.width, screenEdgePan)
            );
        }

        private void Update()
        {
            float zoomDelta = Input.GetAxis("Mouse ScrollWheel");
            if (zoomDelta != 0f)
            {
                AdjustZoom(zoomDelta);
            }

            float rotationDelta = Input.GetAxis("Rotation");
            if (rotationDelta != 0f)
            {
                AdjustRotation(rotationDelta);
            }

            float xDelta = Input.GetAxis("Horizontal");
            float zDelta = Input.GetAxis("Vertical");
            if (xDelta != 0f || zDelta != 0f)
            {
                AdjustPosition(xDelta, zDelta);
            }
            AdjustPosition();
            AdjustPositionMouseWheel();
            
            LockPositionOn();
        }

        private void AdjustPositionMouseWheel()
        {
            if (Input.GetKeyDown(KeyCode.Mouse2))
            {
                Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hitInfo, Mathf.Infinity, MouseWheelDraggable))
                {
                    dragStartPos = new Vector3(hitInfo.point.x, 0, hitInfo.point.z);
                }
            }

            if (Input.GetKey(KeyCode.Mouse2))
            {
                Ray ray2 = cam.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray2, out RaycastHit hitInfo2, Mathf.Infinity, MouseWheelDraggable))
                {
                    dragCurrentPos = new Vector3(hitInfo2.point.x, 0, hitInfo2.point.z);

                    Vector3 position = transform.localPosition;
                    position += dragStartPos - dragCurrentPos;

                    transform.localPosition = Bounds ? 
                        transform.localPosition = ClampPosition(position) : position;
                }
            }
        }

        public void LockPositionOn()
        {
            if (LockedTransform != null)
            {
                Vector3 position = LockedTransform.transform.localPosition;
                transform.localPosition = ClampPosition(position);
            }
        }

        private void AdjustZoom (float delta) 
        {
            zoom = Mathf.Clamp01(zoom + delta);

            float distance = Mathf.Lerp(StickMinZoom, StickMaxZoom, zoom);
            stick.localPosition = new Vector3(0f, 0f, distance);

            float angle = Mathf.Lerp(SwivelMinZoom, SwivelMaxZoom, zoom);
            swivel.localRotation = Quaternion.Euler(angle, yRot, 0f);
        }

        private void AdjustRotation(float delta)
        {
            rotationAngle += delta * RotationSpeed * Time.deltaTime;
            if (rotationAngle < 0f)
            {
                rotationAngle += 360f;
            }
            else if (rotationAngle >= 360f)
            {
                rotationAngle -= 360f;
            }
            transform.localRotation = Quaternion.Euler(0f, rotationAngle, 0f);
        }

        private void AdjustPosition(float xDelta, float zDelta)
        {
            LockedTransform = null;

            Quaternion angle = Quaternion.AngleAxis(yRot, Vector3.up);
            Vector3 direction = 
                transform.localRotation * angle * new Vector3(xDelta, 0f, zDelta).normalized;
            float damping = Mathf.Max(Mathf.Abs(xDelta), Mathf.Abs(zDelta));

            float distance = Mathf.Lerp(MoveSpeedMinZoom, MoveSpeedMaxZoom, zoom) * 
                damping * Time.deltaTime;

            Vector3 position = transform.localPosition;
            position += direction * distance;

            transform.localPosition = Bounds ? 
                transform.localPosition = ClampPosition(position) : position;
        }

        private void AdjustPosition()
        {
            float xDelta = 
                screenPan.Lft.Contains(Input.mousePosition) ? 
                -1 : screenPan.Rht.Contains(Input.mousePosition) ? 1 : 0;
            float zDelta = 
                screenPan.Up.Contains(Input.mousePosition) ? 
                1 : screenPan.Dwn.Contains(Input.mousePosition) ? -1 : 0;

            if (xDelta != 0 || zDelta != 0) 
                AdjustPosition(xDelta, zDelta);
        }

        private Vector3 ClampPosition(Vector3 position)
        {
            float xMin = Bounds.center.x - Bounds.size.x / 2;
            float xMax = Bounds.center.x + Bounds.size.x / 2;
            float zMin = Bounds.center.z - Bounds.size.z / 2;
            float zMax = Bounds.center.z + Bounds.size.z / 2;

            position.x = Mathf.Clamp(position.x, xMin, xMax);
            position.z = Mathf.Clamp(position.z, zMin, zMax);
            
            return position;
        }
    }
}