using System;
using System.Collections.Generic;
using System.Text;

namespace Chat.Infrastructure.Model
{
	public interface IEntity<TKey> { TKey Id { get; set; } }
	public interface IEntity : IEntity<string> { }
}
