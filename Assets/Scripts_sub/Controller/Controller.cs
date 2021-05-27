using UnityEngine;

namespace HSMLibrary
{
    public class Controller : MonoBehaviour
    {
        public string musicName = string.Empty;

        public virtual void OnPressed(Vector3 _touchPos) { }
        public virtual void OnClick(Vector3 _touchPos) { }
        public virtual void OnMoved(Vector3 _touchPos) { }
        public virtual void OnDrag(Vector3 _diffVec) { }
        public virtual void OnSwipe(Vector3 _diffVec) { }
	    public virtual void OnPinchToZoom(float _legnth) { }
    }
}
