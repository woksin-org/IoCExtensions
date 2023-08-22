// Copyright (c) woksin-org. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using NUnit.Framework;

namespace Woksin.Extensions.Specifications.NUnit.for_Specification.when_specification_is_not_derived;

public class when_test_does_not_have_arrange_or_act_part : Specification
{
	[Test]
	public void should_work() => Assert.True(true);
}