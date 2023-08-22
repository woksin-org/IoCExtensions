// Copyright (c) woksin-org. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using NUnit.Framework;

namespace Woksin.Extensions.Specifications.NUnit.for_Specification.when_specification_is_not_derived;

public class when_test_has_asyncronous_arange_and_act : Specification
{
    int some_dependency;
    bool result;

    async Task Establish()
	{
		await Task.Delay(100);
		some_dependency = 32;
	}

    async Task Because()
	{
		await Task.Delay(100);
		result = true;
	}

	[Test]
	public void should_do_arrange_part() => Assert.AreEqual(32, some_dependency);
	
	[Test]
	public void should_do_act_part() => Assert.True(result);
}