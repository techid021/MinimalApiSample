
using Microsoft.Extensions.Logging;
using MinimalApiSample.Components;
using MinimalApiSample.Entities;
using System.Net;
using System.Xml.Linq;

namespace MinimalApiSample.RouterClasses
{
    public class BookRouter : RouterBase
    {
        public BookRouter(ILogger<BookRouter> logger)
        {
            UrlFragment = "book";
            Logger = logger;
        }

        /// <summary>
        /// GET a collection of data
        /// </summary>
        /// <returns>An IResult object</returns>
        protected virtual IResult Get()
        {
            Logger.LogInformation("Getting all books");
            return Results.Ok(GetAll());
        }

        /// <summary>
        /// Get a collection of Book objects
        /// </summary>
        /// <returns>A list of Book objects</returns>
        protected virtual List<Book> GetAll()
        {
            Logger.LogInformation("Getting all books");
            return new List<Book> {
                new Book {
                    BookID = 100001,
                    Name = "An introduction to C# 8.0",
                    Author = "John Doe",
                    Publisher = "Wrox Press",
                    IBAN = "987-45-345-1"
                    },
                    new Book {
                    BookID = 100002,
                    Name = "An introduction to Java",
                    Author = "Adam",
                    Publisher = "Wrox Press",
                    IBAN = "987-46-942-8"
                    },
                    new Book {
                    BookID = 100003,
                    Name = "An introduction to Python",
                    Author = "Jadi",
                    Publisher = "Apress",
                    IBAN = "987-32-601-4"
                    }
                };
        }

        /// <summary>
        /// GET a single row of data
        /// </summary>
        /// <returns>An IResult object</returns>
        protected virtual IResult Get(int id)
        {
            Book? current = GetAll()
            .Find(p => p.BookID == id);
            if (current != null)
            {
                return Results.Ok(current);
            }
            else
            {
                return Results.NotFound();
            }
        }

        /// <summary>
        /// INSERT new data
        /// </summary>
        /// <returns>An IResult object</returns>
        protected virtual IResult Post(Book entity)
        {
            // Generate a new ID
            entity.BookID = GetAll()
            .Max(p => p.BookID) + 1;
            // TODO: Insert into data store
            // Return the new object created
            return Results.Created(
            $"/{UrlFragment}/{entity.BookID}", entity);
        }

        /// UPDATE existing data
        /// </summary>
        /// <returns>An IResult object</returns>
        protected virtual IResult Put(int id, Book entity)
        {
            IResult ret;
            Book? current = GetAll()
            .Find(p => p.BookID == id);
            if (current != null)
            {
                current.Name = entity.Name;
                current.Author = entity.Author;
                current.Publisher = entity.Publisher;
                current.IBAN = entity.IBAN;
                // TODO: Update the data store
                // Return the updated entity
                ret = Results.Ok(current);
            }
            else
            {
                ret = Results.NotFound();
            }

            return ret;
        }

        /// <summary>
        /// DELETE a single row
        /// </summary>
        /// <returns>An IResult object</returns>
        protected virtual IResult Delete(int id)
        {
            IResult ret;
            Book? current = GetAll()
            .Find(p => p.BookID == id);
            if (current != null)
            {
                GetAll().Remove(current);
                // Return NoContent
                ret = Results.NoContent();
            }
            else
            {
                ret = Results.NotFound();
            }
            return ret;
        }


        public override void AddRoutes(WebApplication app)
        {
            app.MapGet($"/{UrlFragment}", () => Get());

            app.MapGet($"/{UrlFragment}/{{id:int}}",
            (int id) => Get(id));

            app.MapPost($"/{UrlFragment}",
            (Book entity) => Post(entity));

            app.MapPut($"/{UrlFragment}/{{id:int}}",
            (int id, Book entity) => Put(id, entity));

            app.MapDelete($"/{UrlFragment}/{{id:int}}",
            (int id) => Delete(id));
        }


    }
}
