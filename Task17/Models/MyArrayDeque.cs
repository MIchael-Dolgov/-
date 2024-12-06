using System;

namespace Task17.Models
{

    public class MyArrayDeque<T> : ITestable<T> where T : IComparable<T>
    {
        private T[] _elements;
        private int _head;
        private int _tail;
        
        //Methods for Task17=====================================================

        public void Set(int index, T newValue)
        {
            int size = Size();
            if (index < 0 || index >= size)
            {
                throw new ArgumentOutOfRangeException(nameof(index),
                    "Индекс находится за пределами допустимого диапазона.");
            }

            _elements[_head + index] = newValue;
        }
        public void Add(int index, T value)
        {
            int size = Size();

            if (index < 0 || index > size)
            {
                throw new ArgumentOutOfRangeException(nameof(index), "Индекс находится за пределами допустимого диапазона.");
            }

            // Если массив заполнен, расширяем его.
            if (size == _elements.Length)
            {
                T[] newElements = new T[_elements.Length * 2];
                for (int i = 0; i < size; i++)
                {
                    newElements[i] = _elements[_head + i];
                }
                _elements = newElements;
                _head = 0;
                _tail = size - 1;
            }

            // Сдвигаем элементы вправо, начиная с заданного индекса.
            for (int i = _tail; i >= _head + index; i--)
            {
                _elements[i + 1] = _elements[i];
            }

            // Вставляем новый элемент и обновляем указатель хвоста.
            _elements[_head + index] = value;
            _tail++;
        }
        
        public int Get(T value)
        {
            for (int i = _head; i <= _tail; i++)
            {
                if (_elements[i]?.Equals(value) == true)
                {
                    return i;
                }
            }
            return -1;
        }

        /*
        public void Remove(object obj)
        {
            for (int i = _head; i <= _tail; i++)
                if (Equals(obj, _elements[i]))
                {
                    for (int j = i; j < _tail; j++)
                        _elements[j] = _elements[j + 1];
                    _tail--;
                    i--;
                }
        }
        */
        public void Remove(T obj)
        {
            for (int i = _head; i <= _tail; i++)
                if (Equals(obj, _elements[i]))
                {
                    for (int j = i; j < _tail; j++)
                        _elements[j] = _elements[j + 1];
                    _tail--;
                    i--;
                }
        }
        //======================================================================
        
        public MyArrayDeque()
        {
            _elements = new T[16];
            _head = 0;
            _tail = -1;
        }
        
        public MyArrayDeque(T[] arr)
        {
            _elements = new T[arr.Length];
            for (int i = 0; i < arr.Length; i++)
                _elements[i] = arr[i];
            _head = 0;
            _tail = arr.Length - 1;
        }

        public MyArrayDeque(int numElements)
        {
            _elements = new T[numElements];
            _head = 0;
            _tail = -1;
        }

        public int Size()
        {
            return _tail - _head + 1;
        }
        
        public void Add(T element)
        {
            if (_tail + 1 < _elements.Length)
            {
                _elements[++_tail] = element;
                return;
            }

            /*
            if (Size() < _elements.Length)
            {
                for (int i = --_head; i < _tail; i++)
                    _elements[i] = _elements[i + 1];
                return;
            }
            */

            T[] newElements = new T[2 * (_elements.Length + 1)];
            for (int i = _head; i <= _tail; i++)
                newElements[i] = _elements[i];
            _tail++;
            newElements[_tail] = element;
            _elements = newElements;
        }
        
        public void AddAll(T[] arr)
        {
            for (int i = 0; i < arr.Length; i++)
                Add(arr[i]);
        }

        public void Clear()
        {
            _head = 0;
            _tail = -1;
        }

        public bool Contains(object obj)
        {
            for (int i = _head; i <= _tail; i++)
                if (Equals((obj, _elements[i])))
                    return true;
            return false;
        }

        public bool ContainsAll(T[] arr)
        {
            bool flag;
            for (int i = 0; i < arr.Length; i++)
            {
                flag = false;
                for (int j = _head; j <= _tail; j++)
                    if (Equals(arr[i], _elements[j]))
                        flag = true;
                if (!flag)
                    return false;
            }

            return true;
        }

        public bool IsEmpty() => Size() == 0;



        public void RemoveAll(T[] arr)
        {
            for (int i = 0; i < arr.Length; i++)
                Remove(arr[i]);
        }

        public void RetainAll(T[] arr)
        {
            bool flag;
            for (int i = _head; i <= _tail; i++)
            {
                flag = false;
                for (int j = 0; j < arr.Length; j++)
                    if (Equals((_elements[i], arr[j])))
                        flag = true;
                if (!flag)
                    Remove(arr[i]);
            }
        }

        public T[] ToArray()
        {
            T[] arr = new T[Size()];
            int index = 0;
            for (int i = _head; i <= _tail; i++)
            {
                arr[index] = _elements[i];
                index++;
            }

            return arr;
        }

        public T Element()
        {
            if (Size() == 0)
                throw new Exception("Deque is empty");
            return _elements[_head];
        }

        private int Amount(T element)
        {
            int amount = 0;
            for (int i = _head; i <= _tail; i++)
                if (Equals(element, _elements[i]))
                    amount++;
            return amount;
        }

        public bool Offer(T element)
        {
            int oldAmount = Amount(element);
            Add(element);
            int newAmount = Amount(element);
            if (oldAmount != newAmount)
                return true;
            return false;
        }

        public T Peek()
        {
            if (Size() == 0)
                return default!; //Возвращаем значение по уполчанию
            return _elements[_head];
        }

        public T Poll()
        {
            if (Size() == 0)
                return default!;
            _head++;
            return _elements[_head - 1];
        }

        public void AddFirst(T element)
        {
            if (_head - 1 >= 0)
            {
                _head--;
                _elements[_head] = element;
                return;
            }

            if (Size() < _elements.Length)
            {
                _tail++;
                for (int i = _tail; i > _head; i--)
                    _elements[i] = _elements[i - 1];
                _elements[_head] = element;
                return;
            }

            T[] newElements = new T[2 * (_elements.Length + 1)];
            for (int i = _head; i <= _tail; i++)
                newElements[i + 1] = _elements[i];
            newElements[_head] = element;
            _elements = newElements;
        }

        public void AddLast(T element)
        {
            Add(element);
        }

        public T GetFirst()
        {
            return Element();
        }

        public T GetLast()
        {
            if (Size() == 0)
                throw new Exception("Deque is empty");
            return _elements[_tail];
        }

        public bool OfferFirst(T element)
        {
            if (Size() == _elements.Length)
                return false;
            AddFirst(element);
            return true;
        }

        public bool OfferLast(T element)
        {
            if (Size() == _elements.Length)
                return false;
            AddLast(element);
            return true;
        }

        public T Pop()
        {
            if (Size() == 0)
                throw new Exception("Deque is empty");
            return Poll();
        }

        public void Push(T element)
        {
            AddFirst(element);
        }

        public T PeekFirst()
        {
            return Peek();
        }

        public T PeekLast()
        {
            if (Size() == 0)
                return default!;
            return _elements[_tail];
        }

        public T PollFirst()
        {
            return Poll();
        }

        public T PollLast()
        {
            if (Size() == 0)
                return default!;
            _tail--;
            return _elements[_tail + 1];
        }

        public T RemoveFirst()
        {
            return Pop();
        }

        public T RemoveLast()
        {
            if (Size() == 0)
                throw new Exception("Deque is empty");
            _tail--;
            return _elements[_tail + 1];
        }

        public bool RemoveFirstOccurance(object obj)
        {
            for (int i = _head; i <= _tail; i++)
                if (Equals(obj, _elements[i]))
                {
                    for (int j = i; j < _tail; j++)
                        _elements[j] = _elements[j + 1];
                    _tail--;
                    return true;
                }

            return false;
        }

        public bool RemoveLastOccurance(object obj)
        {
            for (int i = _tail; i >= _head; i--)
                if (Equals(obj, _elements[i]))
                {
                    for (int j = i; j < _tail; j++)
                        _elements[j] = _elements[j + 1];
                    _tail--;
                    return true;
                }

            return false;
        }
    }
}