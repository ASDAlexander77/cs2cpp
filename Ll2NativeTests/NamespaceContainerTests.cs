// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NamespaceContainerTests.cs" company="">
//   
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Ll2NativeTests
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    using Il2Native.Logic;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using PEAssemblyReader;

    using PdbReader;

    /// <summary>
    ///     Summary description for MSTests
    /// </summary>
    [TestClass]
    public class NamespaceContainerTests
    {
        /// <summary>
        ///     Gets or sets the test context which provides
        ///     information about and functionality for the current test run.
        /// </summary>
        public TestContext TestContext { get; set; }

        [TestMethod]
        public void TestNamespaceContainer_1()
        {
            var nc = new NamespaceContainer<NameAdapter>();
            var name = new NameAdapter("System.Test.Item");

            Parallel.ForEach(Enumerable.Range(1, 1), n => nc.Add(name));

            Assert.AreEqual(1, nc.Count);
        }

        /// </summary>
        [TestMethod]
        public void TestNamespaceContainer_2()
        {
            var nc = new NamespaceContainer<NameAdapter>();
            var name = new NameAdapter("System.Test.Item");

            Parallel.ForEach(Enumerable.Range(1, 1000), n => nc.Add(name));

            Assert.AreEqual(1, nc.Count);
        }

        /// </summary>
        [TestMethod]
        public void TestNamespaceContainer_3()
        {
            var nc = new NamespaceContainer<NameAdapter>();
            var name = "System.Test.Item";

            Parallel.ForEach(Enumerable.Range(1, 1000), n => nc.Add(new NameAdapter(string.Concat(name, n))));

            Assert.AreEqual(1000, nc.Count);
        }

        /// </summary>
        [TestMethod]
        public void TestNamespaceContainer_4()
        {
            var nc = new NamespaceContainer<NameAdapter>();
            var name = "System.Test.Item";

            Parallel.ForEach(Enumerable.Range(1, 10000), n => nc.Add(new NameAdapter(string.Concat(name, n % 1000))));

            Assert.AreEqual(1000, nc.Count);
        }

        /// </summary>
        [TestMethod]
        public void TestNamespaceContainer_5()
        {
            var nc = new NamespaceContainer<NameAdapter>();
            var name = "System.Test.Item.";

            Parallel.ForEach(Enumerable.Range(1, 1000), n => nc.Add(new NameAdapter(string.Concat(name, n % 1000))));

            Assert.AreEqual(1000, nc.Count);
        }

        /// </summary>
        [TestMethod]
        public void TestNamespaceContainer_6()
        {
            var nc = new NamespaceContainer<NameAdapter>();
            var name = new NameAdapter("System.Test.Item");
            var count = 0;

            Parallel.ForEach(
                Enumerable.Range(1, 10000),
                n =>
                    {
                        if (!nc.Contains(name))
                        {
                            nc.Add(name);
                            count++;
                        }
                    });

            Assert.AreEqual(count, nc.Count);
        }

        internal class NameAdapter : PEAssemblyReader.IName
        {
            public NameAdapter(string name)
            {
                this.Value = name;
            }

            protected string Value { get; set; }

            public int CompareTo(object obj)
            {
                return string.Compare(this.Value, obj.ToString(), System.StringComparison.Ordinal);
            }

            public override bool Equals(object obj)
            {
                var n = obj as NameAdapter;
                if (n != null)
                {
                    return n.Value == this.Value;
                }

                return base.Equals(obj);
            }

            public override int GetHashCode()
            {
                return this.Value.GetHashCode();
            }

            public string AssemblyQualifiedName { get; private set; }

            public string AssemblyFullyQualifiedName { get; private set; }

            public IType DeclaringType { get; private set; }

            public string FullName { get; private set; }

            public string MetadataFullName { get; private set; }

            public string MetadataName { get; private set; }

            public string Name { get; private set; }

            public string Namespace
            {
                get
                {
                    return this.Value;
                }
            }
        }

        #region Additional test attributes

        // You can use the following additional attributes as you write your tests:
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }

        #endregion
    }
}