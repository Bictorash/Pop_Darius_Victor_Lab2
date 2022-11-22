﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Pop_Darius_Victor_Lab2.Models;
using Pop_Darius_Victor_Lab2.Data;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace Pop_Darius_Victor_Lab2.Pages.Books
{
    [Authorize(Roles = "Admin")]
    public class CreateModel : BookCategoriesPageModel
    {
        private readonly Pop_Darius_Victor_Lab2.Data.Pop_Darius_Victor_Lab2Context _context;

        public CreateModel(Pop_Darius_Victor_Lab2.Data.Pop_Darius_Victor_Lab2Context context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            ViewData["PublisherID"] = new SelectList(_context.Set<Publisher>(), "ID", "PublisherName");
            ViewData["AuthorID"] = new SelectList(_context.Set<Author>(), "ID", "FullName");
            var book = new Book();
            book.BookCategories = new List<BookCategory>();
            PopulateAssignedCategoryData(_context, book);
            return Page();
        }

        [BindProperty]
        public Book Book { get; set; }
        public async Task<IActionResult> OnPostAsync(string[] selectedCategories)
        {
            //var newBook = new Book();
            var newBook = Book;
            if (selectedCategories != null)
            {
                newBook.BookCategories = new List<BookCategory>();
                foreach (var cat in selectedCategories)
                {
                    var catToAdd = new BookCategory
                    {
                        CategoryID = int.Parse(cat)
                    };
                    newBook.BookCategories.Add(catToAdd);
                }
            }
            //if (await TryUpdateModelAsync<Book>(
            //newBook,
            //"Book",
            //i => i.Title, i => i.Author,
            //i => i.Price, i => i.PublishingDate, i => i.PublisherID))
            //{
                _context.Book.Add(newBook);
                await _context.SaveChangesAsync();
                return RedirectToPage("./Index");
            //}
            PopulateAssignedCategoryData(_context, newBook);
            return Page();
        }
    }
}