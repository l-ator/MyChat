using Chat.Infrastructure.Attributes;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Chat.Infrastructure.Helpers
{
    public class EnumHelper
    {
		public static string GetStringValue(object enumMember)
		{
			var fieldInfo = enumMember.GetType().GetField(enumMember.ToString());

			var attributes = (StringValueAttribute[])fieldInfo.GetCustomAttributes<StringValueAttribute>();

			if (attributes != null && attributes.Length > 0)
				return attributes[0].Value;
			else return enumMember.ToString();
		}
	}
}
