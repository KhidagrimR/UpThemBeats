using UnityEngine;

namespace AtoneWorldBend
{
    [ExecuteAlways] // Pour que cela fonctionne en éditeur, en inspecteur de prefab et en mode play
    public class CameraBendCorrection : MonoBehaviour
    {
        // Le but de cette fonction est de corriger le problème de disparition des objets lorsqu'ils s'approchent de la caméra. 
        // Le problème survient quand la géométrie réelle passe derrière la caméra, dépassant son near clip plane, et donc sortant du pipeline de rendu 
        // Même si la surface déformée par le shader reste dans le champ, elle ne sera alors plus rendue et disparaîtra.

        // Peut être ne pas travailler avec des objets trop gros (composer le tronçon standard de plusieurs sous-sections en enfant)
        public enum MATRIX_TYPE { Perspective, Orthographic }

        public MATRIX_TYPE matrixType = MATRIX_TYPE.Perspective;
        [Range(1, 179)]
        public float fieldOfView = 90;
        public float size = 5;
        public bool nearClipPlaneSameAsCamera = true;
        public float nearClipPlaneCustom = 0.3f;


#if UNITY_EDITOR
        public bool visualizeInEditor;
#endif

        Camera activeCamera;    // Adapter pour cinemachine si nécessaire

        private void OnEnable()
        {
            if (activeCamera == null)
                activeCamera = GetComponent<Camera>();
        }

        private void OnDisable()
        {
            if (activeCamera != null)
                activeCamera.ResetCullingMatrix();
        }

        private void Start()
        {
            if (activeCamera == null)
                activeCamera = GetComponent<Camera>();
        }

        private void Update()
        {
            if (activeCamera == null)
                activeCamera = GetComponent<Camera>();

            if (activeCamera == null)
            {
                enabled = false;
                return;
            }


            if (nearClipPlaneCustom >= activeCamera.farClipPlane)
                nearClipPlaneCustom = activeCamera.farClipPlane - 0.01f;


            if (matrixType == MATRIX_TYPE.Perspective)
            {
                fieldOfView = Mathf.Clamp(fieldOfView, 1, 179);
                activeCamera.cullingMatrix = Matrix4x4.Perspective(
                    fieldOfView, 
                    1, 
                    activeCamera.nearClipPlane, 
                    activeCamera.farClipPlane) 
                * activeCamera.worldToCameraMatrix;
            }
            else
            {
                size = size < 1 ? 1 : size;
                activeCamera.cullingMatrix = Matrix4x4.Ortho(-size, size, -size, size, (nearClipPlaneSameAsCamera ? activeCamera.nearClipPlane : nearClipPlaneCustom), activeCamera.farClipPlane) * activeCamera.worldToCameraMatrix;
            }
        }

        private void Reset()
        {
            if (activeCamera != null)
            {
                activeCamera.ResetCullingMatrix();

                fieldOfView = activeCamera.fieldOfView;
                size = activeCamera.orthographicSize;
                nearClipPlaneCustom = activeCamera.nearClipPlane;
            }
        }
#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (activeCamera == null || visualizeInEditor == false)
                return;


            Gizmos.color = Color.magenta * 0.85f;

            if (matrixType == MATRIX_TYPE.Perspective)
            {
                Gizmos.matrix = transform.localToWorldMatrix;
                Gizmos.DrawFrustum(Vector3.zero, fieldOfView, activeCamera.farClipPlane, activeCamera.nearClipPlane, 1);
            }
            else
            {
                //Top
                Vector3 c = transform.position + transform.forward * (nearClipPlaneSameAsCamera ? activeCamera.nearClipPlane : nearClipPlaneCustom);

                Vector3 t1 = c + transform.up * size - transform.right * size;
                Vector3 t2 = c + transform.up * size + transform.right * size;
                Vector3 t3 = c - transform.up * size - transform.right * size;
                Vector3 t4 = c - transform.up * size + transform.right * size;

                Gizmos.DrawLine(t1, t2);
                Gizmos.DrawLine(t2, t4);
                Gizmos.DrawLine(t3, t4);
                Gizmos.DrawLine(t3, t1);


                //Bottom
                c = transform.position + transform.forward * activeCamera.farClipPlane;

                Vector3 b1 = c + transform.up * size - transform.right * size;
                Vector3 b2 = c + transform.up * size + transform.right * size;
                Vector3 b3 = c - transform.up * size - transform.right * size;
                Vector3 b4 = c - transform.up * size + transform.right * size;

                Gizmos.DrawLine(b1, b2);
                Gizmos.DrawLine(b2, b4);
                Gizmos.DrawLine(b3, b4);
                Gizmos.DrawLine(b3, b1);


                //Connection
                Gizmos.DrawLine(t1, b1);
                Gizmos.DrawLine(t2, b2);
                Gizmos.DrawLine(t3, b3);
                Gizmos.DrawLine(t4, b4);
            }
        }
#endif
    }

}