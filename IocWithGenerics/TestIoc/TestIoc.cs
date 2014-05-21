
using IocWithGenerics;
using NUnit.Framework;

namespace TestIoc
{
    [TestFixture]
    public class TestIoc
    {
        [Test]
        public void TestFunctions()
        {
            var test = new SdbIoc();
            test.Register<IMyInt,MyImpl>();
            var resolved = test.Resolve<IMyInt>();
            Assert.IsTrue(resolved.GetType() == typeof(MyImpl));
        }
    }

    public interface IMyInt
    {
    }

    public class MyImpl : IMyInt
    {
    }
}
