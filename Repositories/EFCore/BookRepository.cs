﻿using Entities;
using Repositories.Contracts;

namespace Repositories.EFCore
{
    public class BookRepository : RepositoryBase<Book>, IBookRepository
    {
        public BookRepository(RepositoryContext context) : base(context)
        {
        }

        public void CreateOneBook(Book book)
        {
            Create(book);
        }

        public void DeleteOneBook(Book book)
        {
            Delete(book);
        }

        public IQueryable<Book> GetAllBooks(bool trackChanges)
        {
            return FindAll(trackChanges);
        }

        public Book GetOneBookById(int id, bool trackChanges)
        {
            return FindByCondition(b => b.Id.Equals(id), trackChanges).FirstOrDefault();
        }

        public void UpdateOneBook(Book book)
        {
            Update(book);
        }
    }
}
