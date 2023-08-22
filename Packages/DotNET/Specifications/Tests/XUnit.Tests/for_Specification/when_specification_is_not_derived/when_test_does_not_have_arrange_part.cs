// Copyright (c) woksin-org. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Xunit;

namespace Woksin.Extensions.Specifications.XUnit.for_Specification.when_specification_is_not_derived;

public class when_test_does_not_have_arrange_part : Specification
{
    bool result;

    void Because() => result = true;

	[Fact]
	public void should_do_act_part() => Assert.True(result);
}