using Chat.Infrastructure.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chat.Infrastructure.Enum
{
	public enum Error {
		[StringValue("An error occured")]
		GenericError
	}

    public enum AuthError
    {
		[StringValue("User not found in store")]
		UserNotFound,
		[StringValue("Wrong password")]
		InvalidCredentials,
		[StringValue("This account has been temporarily locked out")]
		LockedOut,
		[StringValue("This username is already taken")]
		UserAlreadyExists,
		[StringValue("This session is already authenticated")]
		AlreadyAuthenticated,
	}

	public enum ChatError
	{
		[StringValue("A chatroom with this user already exists")]
		RoomAlreadyExists
	}

}
