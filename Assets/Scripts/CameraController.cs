using UnityEngine;

public class CameraController : MonoBehaviour {

    public float maxCameraDistance;
    public float minCameraDistance;
    public float perspectiveZoomSpeed;

    Camera cam;

	void Start ()
    {
        cam = Camera.main;
	}

	void Update ()
    {
        RotateCamera();
        Zoom();
    }

    // Rotate the camera around the center of the map using your finger.  Speed and direction of rotation based on finger movement
    void RotateCamera ()
    {
        if (Input.touchCount == 1)
        {
            Touch rotationalTouch = Input.GetTouch(0);

            // Check if the player has moved their finger across the screen
            if (rotationalTouch.phase == TouchPhase.Moved)
            {
                // Get the direction in which the finger moved
                Vector2 direction = rotationalTouch.deltaPosition.normalized;
                // Find the velocity of the movement
                float xVelocity = (rotationalTouch.deltaPosition.x / rotationalTouch.deltaTime) / 2;
                if (direction.x > 0)
                {
                    transform.RotateAround(Vector3.zero, Vector3.up, xVelocity * Time.deltaTime);
                }
                else if (direction.x < 0)
                {
                    transform.RotateAround(Vector3.zero, -Vector3.up, -xVelocity * Time.deltaTime);
                }
            }
        }
    }

    // Use pinch to zoom the camera in and out between set amounts.
    void Zoom ()
    {
        if (Input.touchCount == 2)
        {
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            // Find the position in the previous frame of each touch.
            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            // Find the magnitude of the vector between the touches in each frame.
            float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

            // Find the difference in the distances between each frame.
            float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

            cam.fieldOfView += deltaMagnitudeDiff * perspectiveZoomSpeed;
            cam.fieldOfView = Mathf.Clamp(cam.fieldOfView, minCameraDistance, maxCameraDistance);
        }
    }

}
