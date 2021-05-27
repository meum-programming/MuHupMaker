using System.Collections.Generic;
using UnityEngine;

namespace HSMLibrary
{
    public static class UnityExtension
    {
        public static void EnsureLoaded(this GameObject gameObject)
        {
            if (!gameObject.activeInHierarchy)
            {
                gameObject.SetActive(true);
                gameObject.SetActive(false);
            }
        }

        public static void RemoveChilds(this GameObject gameObject)
        {
            var children = new List<GameObject>();
            foreach (Transform child in gameObject.transform) children.Add(child.gameObject);
            children.ForEach(child => child.transform.parent = null);
            children.ForEach(child => GameObject.Destroy(child));
        }

        public static GameObject Find(string objName)
        {
            GameObject result = null;
            foreach (GameObject root in UnityEngine.Object.FindObjectsOfType(typeof(GameObject)))
            {
                if (root.transform.parent == null)
                {
                    result = Find(root, objName, 0);
                    if (result != null) break;
                }
            }

            return result;
        }

        public static GameObject Find(string objName, string tag)
        {
            GameObject result = null;
            foreach (GameObject parent in GameObject.FindGameObjectsWithTag(tag))
            {
                result = Find(parent, objName, 0);
                if (result != null) break;
            }

            return result;
        }

        public static GameObject FindChild(this GameObject item, string objName)
        {
            return Find(item, objName, 0);
        }
        
        public static void AttachChild(this GameObject parent, GameObject child)
        {
            Vector3 backupPos = child.transform.localPosition;
            Quaternion backupRot = child.transform.localRotation;
            Vector3 backupScale = child.transform.localScale;

            child.transform.parent = parent.transform;

            child.transform.localPosition = backupPos;
            child.transform.localRotation = backupRot;
            child.transform.localScale = backupScale;
        }

        public static List<T> GetComponentsOfType<T>(this GameObject gameObject) where T : UnityEngine.Component
        {
            List<T> list = new List<T>();
            var tArr = gameObject.GetComponents<T>();
            for(int i = 0; i < tArr.Length; i++)
            {
                list.Add( tArr[i] );
            }

            return list;
        }

        public static void FindComponentsOfType<T>(this GameObject gameObject, List<T> childList) where T : UnityEngine.Component
        {
            FindComponentsOfType<T>(gameObject.transform, childList);
        }

        public static void FindComponentsOfType<T>(this Transform transform, List<T> childList) where T : UnityEngine.Component
        {
            foreach (Transform child in transform)
            {
                T component = child.GetComponent<T>();
                if (component != null)
                {
                    childList.Add(component);
                }

                FindComponentsOfType<T>(child, childList);
            }
        }

        private static GameObject Find(GameObject item, string objName, int index)
        {
            if (index == 0 && item.name == objName)
                return item;

            if (index < item.transform.childCount)
            {
                GameObject result = Find(item.transform.GetChild(index).gameObject, objName, 0);
                if (result == null)
                {
                    return Find(item, objName, ++index);
                }
                else
                {
                    return result;
                }
            }

            return null;
        }

        public static void SetChildLayer<T>(this GameObject gameObject, int _layer) where T : UnityEngine.Component
        {
            Transform transform = gameObject.transform;
            foreach (Transform child in transform)
            {
                if (child.GetComponent<T>() != null)
                    continue;

                child.gameObject.layer = _layer;
                SetChildLayer(child.gameObject, _layer);
            }
        }

        public static void SetChildLayer(this GameObject gameObject, int _layer)
        {
            Transform transform = gameObject.transform;
            foreach(Transform child in transform)
            {
                child.gameObject.layer = _layer;
                SetChildLayer(child.gameObject, _layer);
            }
        }

        public static void SetParent(this GameObject gameObject, GameObject _parentObj)
        {
            _parentObj.AttachChild(gameObject);
        }

        public static void SetParent(this GameObject gameObject, Transform _parentTrans)
        {
            gameObject.transform.parent = _parentTrans;
        }

        public static GameObject GetParent(this GameObject gameObject)
        {
            Transform parent = gameObject.transform.parent;
            return parent == null ? null : parent.gameObject;
        }

        public static void GetChildren<T>(this Transform transform, List<T> _list) where T : Component
        {
            int childCnt = transform.childCount;
            for (int i = 0; i < childCnt; i++)
            {
                Transform childTrans = transform.GetChild(i);
                T t = childTrans.GetComponent<T>();
                if (t != null)
                    _list.Add(t);

                childTrans.GetChildren(_list);
            }
        }

        public static Vector3 GetWorldPosition(this Transform transform)
        {
            Vector3 worldPos = Vector3.zero;
            Transform curTrans = transform;
            while (curTrans != null)
            {
                worldPos += curTrans.localPosition;
                curTrans = curTrans.parent;
            }

            return worldPos;
        }
    }
}
