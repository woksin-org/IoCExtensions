// Copyright (c) woksin-org. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Woksin.Extensions.Specifications.MSTest.for_Specification.when_specification_is_not_derived;

[TestClass]
public class when_test_has_syncronous_arange_and_act : Specification
{
    int some_dependency;
    bool result;

    void Establish() => some_dependency = 32;

    void Because() => result = true;

	[TestMethod]
	public void should_do_arrange_part() => Assert.AreEqual(32, some_dependency);
	
	[TestMethod]
	public void should_do_act_part() => Assert.IsTrue(result);
}