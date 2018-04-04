using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace FileOps.Common
{
    public class OneOrZeroElementCollection<T> : IEnumerable<T>
    {
        private readonly IEnumerable<T> _values;

        public OneOrZeroElementCollection()
        {
            _values = new T[0];
        }

        public OneOrZeroElementCollection(T value)
        {
            _values = new[] { value };
        }


        public bool IsEmpty => !_values.Any();

        public IEnumerator<T> GetEnumerator()
        {
            return _values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

    }
}
