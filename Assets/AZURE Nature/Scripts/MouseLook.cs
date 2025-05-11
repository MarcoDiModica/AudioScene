using UnityEngine;
namespace AzureNature
{
    public class MouseLook : MonoBehaviour
    {
        public float mouseSensitivity;
        public Transform playerBody;
        private float xAxisClamp;
        public AudioPauseManager audioPauseManager;
        private bool isPaused = false;

        private void Awake()
        {
            xAxisClamp = 0.0f;
            audioPauseManager = FindFirstObjectByType<AudioPauseManager>();
            isPaused = audioPauseManager.isPaused;
            UpdateCursorState();
        }

        private void Update()
        {
            isPaused = audioPauseManager.isPaused;
            UpdateCursorState();

            if (!isPaused)
            {
                CameraRotation();
            }
        }

        private void UpdateCursorState()
        {
            if (isPaused)
            {
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
        }

        private void CameraRotation()
        {
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;
            xAxisClamp += mouseY;
            if (xAxisClamp > 90.0f)
            {
                xAxisClamp = 90.0f;
                mouseY = 0.0f;
                ClampXAxisRotationToValue(270.0f);
            }
            else if (xAxisClamp < -90.0f)
            {
                xAxisClamp = -90.0f;
                mouseY = 0.0f;
                ClampXAxisRotationToValue(90.0f);
            }
            transform.Rotate(Vector3.left * mouseY);
            playerBody.Rotate(Vector3.up * mouseX);
        }

        private void ClampXAxisRotationToValue(float value)
        {
            Vector3 eulerRotation = transform.eulerAngles;
            eulerRotation.x = value;
            transform.eulerAngles = eulerRotation;
        }
    }
}