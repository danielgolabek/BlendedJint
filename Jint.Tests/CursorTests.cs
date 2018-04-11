using System;
using System.Collections.Generic;
using System.Text;
using Jint;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xunit;
using Assert = Xunit.Assert;

namespace Jint.Tests
{
    [TestClass]
    public class CursorTests
    {
        [TestMethod]
        public void ForLoop_OverTwoItems()
        {
            Engine engine = new Engine();
            {
                engine.SetValue("cursor", new Cursor(new List<object> { 1, 2 }));
                var results = engine.Execute(
                    @"
                    function main()
                    {
                        var items = [];
                        for (var item = cursor.first(); cursor.hasNext(); item = cursor.next()) {
                            items.push(item);
                        }
                        return items;
                    }
                    main();
                ");

                var items = results.GetCompletionValue().ToObject() as object[];
                Assert.Equal(2, items.Length);
            }
        }
    }

    public class Cursor
    {
        private IEnumerable<object> _enumerable;
        private IEnumerator<object> _enumerator;

        public Cursor(IEnumerable<object> enumerable)
        {
            _enumerable = enumerable;
            _enumerator = enumerable.GetEnumerator();
        }

        public object first()
        {
            _enumerator.MoveNext();
            return _enumerator.Current;
        }

        public bool hasNext()
        {
            return _enumerator.MoveNext();
                return false;
        }

        public object next()
        {
            return _enumerator.Current;
        }

    }
}
