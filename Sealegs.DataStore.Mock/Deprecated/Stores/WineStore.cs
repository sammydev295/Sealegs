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
    public class WineStore : BaseStore<Wine>, IWineStore
    {
        #region Properties

        ObservableCollection<Wine> wines;

        public override string Identifier => "Wines";

        /// <summary>
        /// Mock data
        /// </summary>
        string[] images => new string[] {
                "wine-5884__340.jpg",
                "bottle-147690__340.png",
                "champagne-146885__340.png",
                "frogs-1650657__340.jpg",
                "bordeaux-150955__340.png",
                "frogs-1650658__340.jpg",
                "wine-bottle-1209934__340.jpg",
                "wine-1209022__340.jpg",
                "wine-1839024__340.jpg",
                "white-wine-1761771__340.jpg",
            };

        #endregion

        #region CTOR

        public WineStore()
        {
        }

        #endregion

        #region IWineStore implementation

        #region GetAllByLocker

        public async Task<IEnumerable<Wine>> GetAllByLocker(string lockerId, bool force)
        {
            if (!initialized)
                await InitializeStore();

            wines = new ObservableCollection<Wine>();

            int number = 0;
            for (int n = 0; n < 15; n++)
                for (int i = 0; i < images.Length; i++)
                    wines.Add(new Wine()
                    {
                        ImagePath = ImagesURI + $"/{lockerId}/" + images[i],
                        WineTitle = $"Wine {number++}",
                        Quantity = n + 1
                    });

            return wines;
        }

        #endregion

        #region GetSingle

        public async Task<Wine> GetSingle(int id)
        {
            return new Wine()
            {
                WineTitle = "Wine 10",
                ImagePath = "Sealegs.Clients.UI.Images.wine.jpg"
            };
        }

        #endregion

        #endregion

        #region IBaseStore implementation

        bool initialized = false;
        public async override Task InitializeStore()
        {
            if (initialized)
                return;

            initialized = true;
        }

        public Task<IEnumerable<Wine>> GetAllWines()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
