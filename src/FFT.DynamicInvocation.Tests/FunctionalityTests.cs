// Copyright (c) True Goodwill. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace FFT.DynamicInvocation.Tests
{
  using FFT.DynamicInvocation;
  using Microsoft.VisualStudio.TestTools.UnitTesting;

  [TestClass]
  public class FunctionalityTests
  {
    private readonly TestObject _x = new();

    private readonly InheritingTestObject _y = new();

    [TestMethod]
    public void A()
    {
      _x.Invoke("Handle", "arg1");
    }

    //[TestMethod]
    //public void NoArgsReturnVoid()
    //  => Assert.AreEqual(nameof(_x.NoArgsReturnString),  _x.Invoke<TestObject, string>(nameof(_x.NoArgsReturnString)));

    //[TestMethod]
    //public void NoArgsReturnVoidUsingDynamic()
    //  => _x.Invoke(nameof(_x.NoArgsReturnString));
  }

  internal class TestObject
  {
    public void Handle(string arg1) { }

    public void Handle(string arg1, string arg2) { }

    public virtual string Handle(string arg1, string arg2, string arg3) => "Handle";
  }

  internal sealed class InheritingTestObject : TestObject
  {
    public override string Handle(string arg1, string arg2, string arg3) => "New Handle";
  }
}
