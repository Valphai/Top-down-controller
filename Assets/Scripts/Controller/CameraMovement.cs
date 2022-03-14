using UnityEngine;

namespace TopDownController
{
    public class CameraMovement : MonoBehaviour
    {
        public float StickMinZoom, StickMaxZoom;
        public float SwivelMinZoom, SwivelMaxZoom;
        public float MoveSpeedMinZoom, MoveSpeedMaxZoom;
        public float RotationSpeed;
        public BoxCollider Bounds;
        // [HideInInspector]
        public Character LockedChara;
        private Transform swivel, stick;
        private EdgeRect screenPan;
        private float zoom = 1f;
        private float rotationAngle;
        private float screenEdgePan = 25f;
        private const float yRot = 50f;

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

            if (Input.GetKey(KeyCode.Y))
            {
                LockCharacter();
            }
        }

        public void LockCharacter()
        {
            if (LockedChara != null)
            {
                Vector3 position = LockedChara.transform.localPosition;
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
            LockedChara = null;

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