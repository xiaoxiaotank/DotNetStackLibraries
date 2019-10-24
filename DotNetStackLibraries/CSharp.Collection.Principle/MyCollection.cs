using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace CSharp.Collection.Principle
{
    /// <summary>
    /// 简洁的实现方式（利用了foreach语法糖）
    /// </summary>
    /// <typeparam name="T"></typeparam>
    class MyCollection1<T> : IEnumerable, IEnumerable<T>
    {
        private T[] _data;

        public MyCollection1(T[] data)
        {
            _data = data;
            if(_data == null)
            {
                _data = Array.Empty<T>();
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            foreach (var i in _data)
            {
                yield return i;
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    /// <summary>
    /// 初始的原始方式（不使用foreach语法糖）
    /// </summary>
    /// <typeparam name="T"></typeparam>
    class MyCollection2<T> : IEnumerable, IEnumerable<T>
    {
        private T[] _data;

        public MyCollection2(T[] data)
        {
            _data = data;
            if (_data == null)
            {
                _data = Array.Empty<T>();
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new Enumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public struct Enumerator : IEnumerator, IEnumerator<T>
        {
            private readonly MyCollection2<T> _collection;
            private int _index;

            public T Current => _collection._data[_index]; 

            object IEnumerator.Current => Current;

            internal Enumerator(MyCollection2<T> collection)
            {
                _collection = collection;
                _index = -1;
            }

            public bool MoveNext() => ++_index < _collection._data.Length;

            public void Reset()
            {
                _index = -1;
            }

            public void Dispose()
            {
                
            }
        }
    }
}
