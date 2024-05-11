
using Microsoft.Extensions.Logging;
using MinimalApiSample.Components;
using MinimalApiSample.Entities;
using System.Net;
using System.Xml.Linq;

namespace MinimalApiSample.RouterClasses
{
    public class ArticleRouter : RouterBase
    {
        public ArticleRouter(ILogger<ArticleRouter> logger)
        {
            UrlFragment = "article";
            Logger = logger;
        }

        /// <summary>
        /// GET a collection of data
        /// </summary>
        /// <returns>An IResult object</returns>
        protected virtual IResult Get()
        {
            Logger.LogInformation("Getting all articles");
            return Results.Ok(GetAll());
        }

        /// <summary>
        /// Get a collection of Article objects
        /// </summary>
        /// <returns>A list of Article objects</returns>
        protected virtual List<Article> GetAll()
        {
            Logger.LogInformation("Getting all articles");
            return new List<Article> {
                new Article {
                    ArticleID = 200001,
                    Name = "An introduction to Ruby",
                    Author = "Hooshmand akbari"
                    },
                    new Article {
                    ArticleID = 200002,
                    Name = "An introduction to C++",
                    Author = "Adam D.L"
                    }
                };
        }

        /// <summary>
        /// GET a single row of data
        /// </summary>
        /// <returns>An IResult object</returns>
        protected virtual IResult Get(int id)
        {
            Article? current = GetAll()
            .Find(p => p.ArticleID == id);
            if (current != null)
            {
                return Results.Ok(current);
            }
            else
            {
                return Results.NotFound();
            }
        }


        public override void AddRoutes(WebApplication app)
        {
            app.MapGet($"/{UrlFragment}", () => Get());

            app.MapGet($"/{UrlFragment}/{{id:int}}",
            (int id) => Get(id));
        }


    }
}
