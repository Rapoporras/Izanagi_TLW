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
                        constantValue = value;
                        break;
                    case ValueType.Variable:
                        variableValue.Value = value;
                        break;
                    case ValueType.Component:
                        variableValue.Value = value;
                        break;
                }
            }
        }

        public static implicit operator T(BaseReference<V, C, T> reference)
        {
            return reference.Value;
        }
    }
}