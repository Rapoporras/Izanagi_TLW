using System;
using UnityEngine;

namespace GlobalVariables
{
    public class BaseComponentVariable<T> : MonoBehaviour
    {
        [field: SerializeField] public string VariableName { get; private set; }
        [SerializeField] private T _value;

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

        public static implicit operator T(BaseComponentVariable<T> componentVariable)
        {
            return componentVariable.Value;
        }
    }
}