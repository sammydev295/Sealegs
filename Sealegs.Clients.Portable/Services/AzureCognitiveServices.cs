using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PCLStorage;

using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.ProjectOxford.Vision;
using Microsoft.ProjectOxford.Vision.Contract;

namespace Sealegs.Clients.Portable
{
    /// <summary>
    /// https://azure.microsoft.com/en-us/services/cognitive-services/
    /// </summary>
    public class AzureCognitiveServices : IAzureCognitiveServices
    {
        /// <summary>
        /// Picture recognition 
        /// </summary>
        /// <param name="photoStream">Picture to analyse and recognize</param>
        /// <returns>Picture description (item1) and level of confidence in recognition (item2) as a tuple. In case of an erro the level of confidence in recognition 
        /// returned will be -1 and the first item of tuple will be the error</returns>
        public async Task<Tuple<string, double>> DescribeAsync(Stream photoStream)
        {
            try
            {
                var client = new VisionServiceClient(Addresses.COMPUTER_VISION_API_KEY);
                AnalysisResult result = await client.DescribeAsync(photoStream);

                // Parse the result
                return new Tuple<string, double>(result.Description.Captions[0].Text, Math.Floor(result.Description.Captions[0].Confidence * 100));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"ERROR: {ex.Message}");
                return new Tuple<string, double>($"ERROR: {ex.Message}", -1);
            }
        }

        /// <summary>
        /// Text Extraction (OCR'ing)
        /// </summary>
        /// <param name="photoStream">Picture to analyse and extract text from</param>
        /// <returns>Recognized text from picture. In case of an erro null is returned</returns>
        public async Task<Tuple<string, bool>> DescribeTextAsync(Stream photoStream)
        {
            try
            {
                var client = new VisionServiceClient(Addresses.COMPUTER_VISION_API_KEY);
                OcrResults ocrResults = await client.RecognizeTextAsync(photoStream);

                var text = String.Empty;
                foreach (var region in ocrResults.Regions)
                {
                    foreach (var line in region.Lines)
                    {
                        foreach (var word in line.Words)
                        {
                            text = $"{text} {word.Text}";
                        }
                    }
                }

                // Parse the result
                return new Tuple<string, bool>(text, true);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"ERROR: {ex.Message}");
                return new Tuple<string, bool>(ex.Message, false);
            }
        }

    }
}
