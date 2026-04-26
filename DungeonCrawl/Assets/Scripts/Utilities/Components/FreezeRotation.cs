using UnityEngine;

namespace Utilities.Components
{
    public class FreezeRotation : MonoBehaviour
    {
        [SerializeField] private bool freezeX;
        [SerializeField] private bool freezeY;
        [SerializeField] private bool freezeZ;
        
        [SerializeField] private bool setToZero = true;
        
        private Quaternion _initialRotation;
        
        private void Start()
        {
            _initialRotation = transform.rotation;
            if (setToZero)
                transform.rotation = Quaternion.identity;
        }

        private void LateUpdate()
        {
            var currentRotation = transform.rotation;
            var newRotation = _initialRotation;

            if (!freezeX)
                newRotation.x = setToZero ? 0 : currentRotation.x;
            if (!freezeY)
                newRotation.y = setToZero ? 0 : currentRotation.y;
            if (!freezeZ)
                newRotation.z = setToZero ? 0 : currentRotation.z;
            
            transform.rotation = newRotation;
        }
    }
}