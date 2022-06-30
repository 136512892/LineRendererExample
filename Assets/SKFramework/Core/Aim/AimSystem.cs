using UnityEngine;

namespace SK.Framework
{
    public class AimSystem : MonoBehaviour
    {
        #region NonPublic Variables
        private static AimSystem instance;
        [SerializeField] private bool toggle;
        [SerializeField] private Camera mainCamera;
        [SerializeField] private LayerMask aimLayer;
        [SerializeField] private float aimMaxDistance = 10f;
        [SerializeField] private AimMode aimMode = AimMode.Mouse;
        #endregion

        #region Public Properties
        public static AimSystem Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<AimSystem>() ?? new GameObject("[SKFramework.Aim]").AddComponent<AimSystem>();
                }
                return instance;
            }
        }
        public bool Toggle
        {
            get
            {
                return toggle;
            }
            set
            {
                if (toggle != value)
                {
                    toggle = value;
                    if (CurrentAimableObject != null)
                    {
                        CurrentAimableObject.Exit();
                        CurrentAimableObject = null;
                    }
                }
            }
        }
        public IAimableObject CurrentAimableObject { get; private set; }
        #endregion

        #region NonPublic Methods
        private void Start()
        {
            if (mainCamera == null)
            {
                mainCamera = Camera.main ?? FindObjectOfType<Camera>();
            }
            aimMaxDistance = aimMode == AimMode.Mouse ? float.MaxValue : aimMaxDistance;
        }
        private void Update()
        {
            if (!toggle) return;
            Ray ray = aimMode == AimMode.Mouse
                ? mainCamera.ScreenPointToRay(UnityEngine.Input.mousePosition)
                : mainCamera.ViewportPointToRay(Vector2.one * .5f);
            if (Physics.Raycast(ray, out RaycastHit hit, aimMaxDistance, aimLayer))
            {
                IAimableObject obj = hit.collider.GetComponent<IAimableObject>();
                if (obj != CurrentAimableObject)
                {
                    CurrentAimableObject?.Exit();
                    CurrentAimableObject = obj;
                    CurrentAimableObject?.Enter();
                }
            }
            else
            {
                if (CurrentAimableObject != null)
                {
                    CurrentAimableObject.Exit();
                    CurrentAimableObject = null;
                }
            }
            CurrentAimableObject?.Stay();
        }
        private void OnDestroy()
        {
            instance = null;
        }
        #endregion
    }
}