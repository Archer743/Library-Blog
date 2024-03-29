﻿using CommentClass = Library_Web_App.Data.Entities.Comment;

namespace Library_Web_App.Data.ViewModels.Comment
{
	public class CommentInfoViewModel
	{
		public CommentInfoViewModel(CommentClass data, string userRoleColor)
		{
			Data = data;
			UserRoleColor = userRoleColor;
		}

		public CommentClass Data { get; private set; }

		public string UserRoleColor { get; private set; }
	}
}
