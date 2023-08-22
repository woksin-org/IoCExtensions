// Copyright (c) woksin-org. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Woksin.Extensions.Specifications.MSTest.for_Specification.when_specification_is_not_derived;

[TestClass]
public class when_test_does_not_have_arrange_or_act_part : Specification
{
	[TestMethod]
	public void should_work() => Assert.IsTrue(true);
}