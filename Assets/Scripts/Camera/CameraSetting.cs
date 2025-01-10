using Unity.Cinemachine;
using UnityEngine;

namespace MySampleEx
{
    public class CameraSetting : MonoBehaviour
    {
        #region Variables
        public CinemachineCamera freeLookCamera;
        public Transform follow;
        public Transform lookAt;
        #endregion

        private void Awake()
        {
            UpdateCameraSettings(); 
        }

        void UpdateCameraSettings()
        {
            freeLookCamera.Follow = follow;
            freeLookCamera.LookAt = lookAt; 
        }
    }
}