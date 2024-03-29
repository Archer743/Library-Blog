﻿using Library_Web_App.Data.Entities;
using Library_Web_App.Data.ViewModels.Book;

namespace Library_Web_App.Service.Interfaces
{
	public interface IBookService
	{
        List<BookIndexViewModel> GetAll();

        Book GetById(int id);

        BookInfoViewModel GetByIdExtendedInfo(string userName, int id);

        void Add(Book book);

        void Edit(Book book);

        void Delete(int id);

        void Like(int bookId, string userName);

        void Dislike(int bookId, string userName);

        void AddComment(int bookId, string userName, string message);

        int DeleteCommentById(int id);
    }
}
