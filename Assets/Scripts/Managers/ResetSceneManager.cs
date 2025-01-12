using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utils;

namespace Managers
{
    public class ResetSceneManager : MonoBehaviour
    {
        private List<IResettable> _resettableObjects;

        private void Start()
        {
            _resettableObjects = FindAllDataResettableObjects();
        }

        private List<IResettable> FindAllDataResettableObjects()
        {
            IEnumerable<IResettable> resettableObjects = 
                FindObjectsOfType<MonoBehaviour>(true).OfType<IResettable>();

            return new List<IResettable>(resettableObjects);
        }

        public void ResetObjects()
        {
            foreach (var obj in _resettableObjects)
            {
                obj.ResetObject();
            }
        }
    }
}