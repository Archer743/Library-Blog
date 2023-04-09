namespace Library_Web_App.Data.ViewModels.Comment
{
	public class BookCommentsViewModel
	{
		public BookCommentsViewModel(int bookId, List<CommentExtendedViewModel> comments)
		{
			BookId = bookId;
            Comments = comments;
		}

		public int BookId { get; private set; }

		public List<CommentExtendedViewModel> Comments { get; private set; }
    }
}
