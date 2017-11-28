using System;
using System.Threading.Tasks;
using Sealegs.DataObjects;
using Sealegs.DataStore.Abstractions;

using Xamarin.Forms;
using Sealegs.DataStore.Azure;

namespace Sealegs.DataStore.Azure
{
    public class CategoryStore : BaseStore<Category>, ICategoryStore
    {
        public override string Identifier => "Category";
    }
}

