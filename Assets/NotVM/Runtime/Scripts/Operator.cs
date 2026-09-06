using System;

namespace NotBura.Packages
{
    public static class Operator
    {
        #region 

        public static void LoadConstant(ref Context context)
        {
            ref var _stack = ref context.Stack;
            ref var _reader = ref context.Reader;

            var _value = _reader.Read<int>();

            _stack.Push(_value);
        }

        public static void LoadArgument(ref Context context)
        {
            ref var _stack = ref context.Stack;
            ref var _reader = ref context.Reader;
            ref var _frame = ref context.Frame;

            var _offset = _reader.Read<byte>();
            var _value = _frame.Read<int>(_offset * 4);

            _stack.Push(_value);
        }

        #endregion 

        #region logic

        public static void And(ref Context context)
        {
            ref var _stack = ref context.Stack;

            var _lhs = _stack.Pop<bool>();
            var _rhs = _stack.Pop<bool>();

            var _result = _lhs && _rhs;
            _stack.Push(_result);
        }

        public static void Or(ref Context context)
        {
            ref var _stack = ref context.Stack;

            var _lhs = _stack.Pop<bool>();
            var _rhs = _stack.Pop<bool>();

            var _result = _lhs || _rhs;
            _stack.Push(_result);
        }

        public static void Not(ref Context context)
        {
            ref var _stack = ref context.Stack;

            var _value = _stack.Pop<bool>();

            var _result = !_value;
            _stack.Push(_result);
        }

        public static void Return(ref Context context)
        {
            ref var _stack = ref context.Stack;

            var _value = _stack.Pop<bool>();

            var _result = !_value;
            _stack.Push(_result);
        }

        #endregion logic

        #region comparison

        /// <summary>
        /// Equals
        /// </summary>
        public static void Equals(ref Context context)
        {
            ref var _stack = ref context.Stack;

            var _lhs = _stack.Pop<int>();
            var _rhs = _stack.Pop<int>();

            var _result = _lhs == _rhs;
            _stack.Push(_result);
        }

        /// <summary>
        /// Not Equals
        /// </summary>
        public static void NotEquals(ref Context context)
        {
            ref var _stack = ref context.Stack;

            var _lhs = _stack.Pop<int>();
            var _rhs = _stack.Pop<int>();

            var _result = _lhs != _rhs;
            _stack.Push(_result);
        }

        /// <summary>
        /// Greater Than
        /// </summary>
        public static void GreaterThan(ref Context context)
        {
            ref var _stack = ref context.Stack;

            var _lhs = _stack.Pop<int>();
            var _rhs = _stack.Pop<int>();

            var _result = _lhs > _rhs;
            _stack.Push(_result);
        }

        /// <summary>
        /// Greater Equals
        /// </summary>
        public static void GreaterThanEquals(ref Context context)
        {
            ref var _stack = ref context.Stack;

            var _lhs = _stack.Pop<int>();
            var _rhs = _stack.Pop<int>();

            var _result = _lhs >= _rhs;
            _stack.Push(_result);
        }

        /// <summary>
        /// Less Than
        /// </summary>
        public static void LessThan(ref Context context)
        {
            ref var _stack = ref context.Stack;

            var _lhs = _stack.Pop<int>();
            var _rhs = _stack.Pop<int>();

            var _result = _lhs < _rhs;
            _stack.Push(_result);
        }

        /// <summary>
        /// Less Than Equals
        /// </summary>
        public static void LessThanEquals(ref Context context)
        {
            ref var _stack = ref context.Stack;

            var _lhs = _stack.Pop<int>();
            var _rhs = _stack.Pop<int>();

            var _result = _lhs <= _rhs;
            _stack.Push(_result);
        }

        /// <summary>
        /// Sequence Equals
        /// </summary>
        public static void SequenceEquals(ref Context context)
        {
            ref var _stack = ref context.Stack;

            var _lhs = _stack.Pop(4);
            var _rhs = _stack.Pop(4);

            var _result = _lhs.SequenceEqual(_rhs);
            _stack.Push(_result);
        }

        #endregion comparison

        public static void Cast(ref Context context)
        {
            ref var _stack = ref context.Stack;

            var _source = _stack.Pop<int>();
        }

        #region artimetic

        public static void AddI4(ref Context context)
        {
            ref var _stack = ref context.Stack;

            var _lhs = _stack.Pop<int>();
            var _rhs = _stack.Pop<int>();

            var _result = _lhs + _rhs;
            _stack.Push(_result);
        }

        public static void SubstactI4(ref Context context)
        {
            ref var _stack = ref context.Stack;

            var _lhs = _stack.Pop<int>();
            var _rhs = _stack.Pop<int>();

            var _result = _lhs - _rhs;
            _stack.Push(_result);
        }

        public static void MultiplyI4(ref Context context)
        {
            ref var _stack = ref context.Stack;

            var _lhs = _stack.Pop<int>();
            var _rhs = _stack.Pop<int>();

            var _result = _lhs * _rhs;
            _stack.Push(_result);
        }

        public static void DivideI4(ref Context context)
        {
            ref var _stack = ref context.Stack;

            var _lhs = _stack.Pop<int>();
            var _rhs = _stack.Pop<int>();

            var _result = _lhs / _rhs;
            _stack.Push(_result);
        }

        public static void Mod(ref Context context)
        {
            ref var _stack = ref context.Stack;

            var _lhs = _stack.Pop<int>();
            var _rhs = _stack.Pop<int>();

            var _result = _lhs % _rhs;
            _stack.Push(_result);
        }

        #endregion artimetic
    }
}
