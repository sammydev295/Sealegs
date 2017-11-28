using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.ProjectOxford.Vision;
using Microsoft.ProjectOxford.Vision.Contract;

using PCLStorage;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Sealegs.Clients.Portable
{
    public interface IAzureCognitiveServices
    {
        /// <summary>
        /// Picture recognition 
        /// </summary>
        /// <param name="photoStream">Picture to analyse and recognize</param>
        /// <returns>Picture description (item1) and level of confidence in recognition (item2) as a tuple. In case of an erro the level of confidence in recognition 
        /// returned will be -1 and the first item of tuple will be the erro</returns>
        Task<Tuple<string, double>> DescribeAsync(Stream photo);

        Task<Tuple<string, bool>> DescribeTextAsync(Stream photoStream);
    }
}
