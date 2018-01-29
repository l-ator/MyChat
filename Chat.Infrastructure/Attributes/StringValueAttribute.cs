using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Chat.Infrastructure.Attributes
{
	public sealed class StringValueAttribute : Attribute
	{
		public string Value { get; private set; }

		public StringValueAttribute(string value)
		{
			this.Value = value;
		}
	}
}
