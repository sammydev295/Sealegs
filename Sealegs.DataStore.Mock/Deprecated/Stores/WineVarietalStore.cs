using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;


using Sealegs.DataObjects;
using Sealegs.DataStore.Abstractions;

namespace Sealegs.DataStore.Mock
{
    public class WineVarietalStore : BaseStore<WineVarietal>, IWineVarietalStore
    {
        #region Properties

        public override string Identifier => "WineVarietals";

        #endregion

        #region CTOR

        public WineVarietalStore()
        {
        }

        #endregion

        #region IWineStore implementation

        public async Task<IEnumerable<WineVarietal>> GetAll(bool forceRefresh = false )
        {
            if (!initialized)
                await InitializeStore();

            var wineVarietals = new ObservableCollection<WineVarietal>()
            {
                //new WineVarietal() { WineVarietalId = new Guid("644C3116-6A95-49E6-BE8A-D5A2E6FEBDDA"), VarietalName = "Chardonnay" },
                //new WineVarietal() { WineVarietalId = new Guid("644C3116-6A95-49E6-BE8A-D5A2E6FEBDDA"), VarietalName = "Dolcetto" },
                //new WineVarietal() { WineVarietalId = new Guid("644C3116-6A95-49E6-BE8A-D5A2E6FEBDDA"), VarietalName = "Gewürztraminer" },
                //new WineVarietal() { WineVarietalId = new Guid("644C3116-6A95-49E6-BE8A-D5A2E6FEBDDA"), VarietalName = "Merlot" },
                //new WineVarietal() { WineVarietalId = new Guid("644C3116-6A95-49E6-BE8A-D5A2E6FEBDDA"), VarietalName = "Mourvedre" },
                //new WineVarietal() { WineVarietalId = new Guid("644C3116-6A95-49E6-BE8A-D5A2E6FEBDDA"), VarietalName = "Petite Syrah" }
            };

            return wineVarietals;
        }

        #endregion

        #region IBaseStore implementation

        bool initialized = false;
        public async override Task InitializeStore()
        {
            if (initialized)
                return;

            initialized = true;
        }

        #endregion
    }
}
