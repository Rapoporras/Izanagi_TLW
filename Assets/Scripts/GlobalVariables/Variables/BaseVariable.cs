using System;
using UnityEngine;

namespace GlobalVariables
{
    public abstract class BaseVariable<T> : ScriptableObject
    {
#if UNITY_EDITOR
        [Multiline, SerializeField] protected string _developerDescription = "";
#endif

        [SerializeField, Space(10)] private T _value;
        
        public event Action<T> OnValueChanged;
        public T Value
        {
            get => _value;
            set
            {
                _value = value;
                OnValueChanged?.Invoke(_value);
            }
        }

        public static implicit operator T(BaseVariable<T> variable)
        {
            return variable.Value;
        }
    }
}