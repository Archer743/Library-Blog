﻿using Library_Web_App.Data;
using Library_Web_App.Data.Entities;
using Library_Web_App.Data.ViewModels.Book;
using Library_Web_App.Data.ViewModels.Comment;
using Library_Web_App.Migrations;
using Library_Web_App.Service.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Library_Web_App.Service
{
    public class BookService : IBookService
    {
        private readonly ApplicationContext context;
        private readonly ILikeService likeService;
        private readonly ICommentService commentService;

        public BookService(ApplicationContext context, ILikeService likeService, ICommentService commentService)
        {
            this.context = context;
            this.likeService = likeService;
            this.commentService = commentService;
        }

        public List<BookIndexViewModel> GetAll()
            => context.Books.Select(book => new BookIndexViewModel(book)).ToList();

        public Book GetById(int id)
            => context.Books
                      .FirstOrDefault(book => id == book.Id);

        public BookInfoViewModel GetByIdExtendedInfo(string userName, int id)
        {
            Book book = GetById(id);

            // ако книга с подаденото id не е намерено
            if (book == null)
                return null;

            int likeCount = likeService.GetLikesCount(id);
            List<CommentInfoViewModel> comments = commentService.GetCommentsExtendedInfo(id);
            bool isLiked = likeService.IsBookLikedByCurUser(id, userName);
            
            return new BookInfoViewModel(book, likeCount, comments, isLiked);
        }
        public void Add(Book book)
        {
            context.Books.Add(book);
            context.SaveChanges();
        }

        public void Edit(Book book)
        {
            context.Books.Update(book);
            context.SaveChanges();
        }

        public void Delete(int id)
        {
            Book book = GetById(id);

            if (book == null)
                return;

            context.Books.Remove(book);

            likeService.DeleteAllBookLikes(id);
            commentService.DeleteAllBookComments(id);
            
            context.SaveChanges();
        }

        public void Like(int bookId, string userName)
            => likeService.Like(bookId, userName);

        public void Dislike(int bookId, string userName)
            => likeService.Dislike(bookId, userName);

        public void AddComment(int bookId, string userName, string message)
            => commentService.AddComment(bookId, userName, message);

        public int DeleteCommentById(int id)
            => commentService.DeleteCommentById(id);
    }
}
