using System;
using System.Collections.Generic;

namespace HSMLibrary.Scene
{
    using Generics;

    /*
    public class SceneCallSet
    {
        public Type SceneType = null;
        public object PostParams = null;
    }
    */

    public class SceneCallStack : Singleton<SceneCallStack>
    {
        private LinkedList<Type> sceneCallDeque = new LinkedList<Type>();

        public void Push(Type set)
        {
            sceneCallDeque.AddLast(set);
            
            if (sceneCallDeque.Count > 200)
                sceneCallDeque.RemoveFirst();
        }

        public Type Pop()
        {
            if (sceneCallDeque.Count > 0)
            {
                Type ret = sceneCallDeque.Last.Value;
                sceneCallDeque.RemoveLast();
                return ret;
            }
            else
            {
                return null;
            }
        }

        public void Clear()
        {
            sceneCallDeque.Clear();
        }
    }
}
