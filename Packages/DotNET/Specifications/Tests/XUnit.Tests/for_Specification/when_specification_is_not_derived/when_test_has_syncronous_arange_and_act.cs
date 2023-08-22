// Copyright (c) woksin-org. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Xunit;

namespace Woksin.Extensions.Specifications.XUnit.for_Specification.when_specification_is_not_derived;

public class when_test_has_syncronous_arange_and_act : Specification
{
    int some_dependency;
    bool result;

    void Establish() => some_dependency = 32;

    void Because() => result = true;

	[Fact]
	public void should_do_arrange_part() => Assert.Equal(32, some_dependency);
	
	[Fact]
	public void should_do_act_part() => Assert.True(result);
}