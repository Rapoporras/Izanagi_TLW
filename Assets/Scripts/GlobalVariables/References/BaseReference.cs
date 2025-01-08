using System;

namespace GlobalVariables
{
    [Serializable]
    public abstract class BaseReference<V, C, T> where V : BaseVariable<T>
        where C : BaseComponentVariable<T>
    {
        public enum ValueType
        {
            Constant, Variable, Component
        }
        
        public ValueType valueType;
        public T constantValue;
        public V variableValue;
        public C componentValue;
        
        protected BaseReference() {}

        protected BaseReference(T value)
        {
            valueType = ValueType.Constant;
            constantValue = value;
        }

        public event Action<T> OnConstantValueChanged;

        public T Value
        {
            get
            {
                switch (valueType)
                {
                    case ValueType.Constant:
                        return constantValue;
                    case ValueType.Variable:
                        return variableValue;
                    case ValueType.Component:
                        return componentValue;
                }

                return constantValue;
            }
            set
            {
                switch (valueType)
                {
                    case ValueType.Constant:
                        OnConstantValueChanged?.Invoke(value);
                        constantValue = value;
                        break;
                    case ValueType.Variable:
                        variableValue.Value = value;
                        break;
                    case ValueType.Component:
                        componentValue.Value = value;
                        break;
                }
            }
        }

        public void AddListener(Action<T> listener)
        {
            switch (valueType)
            {
                case ValueType.Constant:
                    OnConstantValueChanged += listener;
                    break;
                case ValueType.Variable:
                    variableValue.OnValueChanged += listener;
                    break;
                case ValueType.Component:
                    componentValue.OnValueChanged += listener;
                    break;
            }
        }

        public void RemoveListener(Action<T> listener)
        {
            switch (valueType)
            {
                case ValueType.Constant:
                    OnConstantValueChanged -= listener;
                    break;
                case ValueType.Variable:
                    variableValue.OnValueChanged -= listener;
                    break;
                case ValueType.Component:
                    componentValue.OnValueChanged -= listener;
                    break;
            }
        }

        public static implicit operator T(BaseReference<V, C, T> reference)
        {
            return reference.Value;
        }
    }
}