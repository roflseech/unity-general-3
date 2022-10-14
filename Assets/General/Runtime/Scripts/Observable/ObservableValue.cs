using System;
using System.Collections.Generic;

namespace UnityGeneral
{

    /// <summary>
    /// Класс-контейнер, который оповещает об изменении содержимого.
    /// Можно использовать в том числе для объектов, 
    /// которые должны существовать в единственном эземпляре
    /// - тогда изменение с null(или default значения) будет разрешено, а с другого значения - нет.
    /// </summary>
    public class ObservableValue<T>
    {
        public event Action<T> OnChanged;

        private T _startValue;
        private T _value;
        private bool _canBeModified;
        public T Value
        {
            get
            {
                return _value;
            }
            set
            {
                if (!_canBeModified &&
                    EqualityComparer<T>.Default.Equals(value, default))
                {
                    throw new Exception(
                        $"Value cannot be modified. " +
                        $"Current: {_value}, new: {value}");
                }
                _value = value;
                OnChanged?.Invoke(_value);
            }
        }
        public ObservableValue()
        {
            _startValue = default;
            _value = _startValue;
            _canBeModified = true;
        }
        public ObservableValue(T startValue)
        {
            _startValue = startValue;
            _value = _startValue;
            _canBeModified = true;
        }
        public ObservableValue<T> BlockModifications()
        {
            _canBeModified = false;
            return this;
        }
        public void ResetValue()
        {
            Value = _startValue;
        }
        public void Reset()
        {
            OnChanged = null;
            _value = _startValue;
        }
    }
}