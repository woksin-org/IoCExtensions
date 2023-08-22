// Copyright (c) woksin-org. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using NUnit.Framework;

namespace Woksin.Extensions.Specifications.NUnit.for_Specification.when_specification_is_not_derived;

public class when_test_has_syncronous_arange_and_act : Specification
{
    int some_dependency;
    bool result;

    void Establish() => some_dependency = 32;

    void Because() => result = true;

	[Test]
	public void should_do_arrange_part() => Assert.AreEqual(32, some_dependency);
	
	[Test]
	public void should_do_act_part() => Assert.True(result);
}