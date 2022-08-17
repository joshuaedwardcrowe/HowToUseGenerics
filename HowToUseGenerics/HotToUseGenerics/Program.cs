using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;

namespace HotToUseGenerics
{
    enum BookCategory
    {
        Fantasy,
        Science
    }
    
    class Book
    {
        public BookCategory Category { get; set; }

        public Book(BookCategory category)
        {
            Category = category;
        }
    }

    class FantasyBook : Book
    {
        public FantasyBook() : base(BookCategory.Fantasy)
        {
            
        }
    }

    class ScienceBook : Book
    {
        public ScienceBook() : base(BookCategory.Science)
        {
            
        }
    }


    interface IBookCaseShelf<TBookType> where TBookType : Book
    {
        IList<TBookType> BooksOnShelf { get; set;s }
    }

    class BookCaseShelf : IBookCaseShelf<Book>
    {
        public virtual IList<Book> BooksOnShelf { get; set; } = new List<Book>();
    }

    class SpecificBookCaseShelf<TSpecificBook> : IBookCaseShelf<TSpecificBook> where TSpecificBook : Book
    {
        public IList<TSpecificBook> BooksOnShelf { get; set; }
    }

    class FantasyBookCaseShelf : SpecificBookCaseShelf<FantasyBook>
    {
    }

    class BookCase<TBookType> where TBookType : Book
    {
        public IList<IBookCaseShelf<TBookType>> ShelvesInCase { get; set; } = new List<IBookCaseShelf<TBookType>>();
    }
    
    
    class Program
    {
        static void Main(string[] args)
        {
            var bookCase = new BookCase<Book>();
            var bookCaseShelf = new BookCaseShelf();
            bookCaseShelf.BooksOnShelf.Add(new FantasyBook());
            bookCaseShelf.BooksOnShelf.Add(new ScienceBook());

            var fantasyBooksFromBookCase = bookCase
                .ShelvesInCase
                .SelectMany(shelves => shelves.BooksOnShelf)
                .OfType<FantasyBook>();


            var fantasyBookCase = new BookCase<FantasyBook>();
            var fantasyBookCaseShelf = new FantasyBookCaseShelf();
            fantasyBookCaseShelf.BooksOnShelf.Add(new FantasyBook());

            var fantasyBooksFromFantasyBookCase = fantasyBookCase
                .ShelvesInCase
                .SelectMany(shelves => shelves.BooksOnShelf)
                .ToList();
        }
    }
}