using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sealegs.DataStore.Abstractions;
using Sealegs.DataObjects;

namespace Sealegs.DataStore.Azure.Api
{
    public class News : ApiBase, INews
    {
        public async Task<bool> DeleteNews(string id)
        {
            var api = String.Format(DeleteNewsUri, id);
            var result = await ClientBase.HttpGetRequest<bool>(api);
            return result;
        }

        public async Task<IEnumerable<DataObjects.News>> GetAll()
        {
            var result = await ClientBase.HttpGetRequest<IEnumerable<DataObjects.News>>(GetAllNewsUri);
            return result;
        }

        public async Task<bool> InsertNews(DataObjects.News news)
        {
            var form = new Dictionary<string, string>
            {
                {"Name",news.Name }, {"Description",news.Description }, {"ImageUrl",news.ImageUrl },
                {"WebsiteUrl",news.WebsiteUrl }, {"TwitterUrl",news.TwitterUrl }, {"Rank",Convert.ToString(news.Rank) }
            };
            var result = await ClientBase.HttpPostRequest<bool>(InsertNewsUri, form);
            return result;
        }

        public async Task<bool> UpdateNews(DataObjects.News news)
        {
            var form = new Dictionary<string, string>
            {
                {"Id",news.Id }, {"Name",news.Name }, {"Description",news.Description }, {"ImageUrl",news.ImageUrl },
                {"WebsiteUrl",news.WebsiteUrl }, {"TwitterUrl",news.TwitterUrl }, {"Rank",Convert.ToString(news.Rank) }
            };
            var result = await ClientBase.HttpPostRequest<bool>(UpdateNewsUri, form);
            return result;
        }
    }
}
