using System;
using System.IO;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Vision.V1;
using Grpc.Auth;
using Grpc.Core;
using Microsoft.AspNetCore.Mvc;

namespace ClimbingApp.ImageRecognition.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageRecognitionController : ControllerBase
    {
        private static string access_key = "aaa";
        private static string secret_key = "bbb";

        // GET api/values
        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var hmacsha1 = new HMACSHA1(Encoding.ASCII.GetBytes("c457ffd14713ca5567cff879e2111010435124a1"));
            //hmacsha1.Key = E;
            hmacsha1.Initialize();

            var client = new HttpClient();
            client.BaseAddress = new Uri("https://cloudreco.vuforia.com");

            // var image = File.ReadAllBytes("/Users/lehmamic/Downloads/6aplus-supervision-test.png");



            byte[] image;

            var imageFile = System.IO.File.Open("/Users/lehmamic/Downloads/6aplus-supervision-test.jpg", FileMode.Open);
            using (var reader = new BinaryReader(imageFile))
            {
                image = reader.ReadBytes((int)imageFile.Length);
            }

            var bytes = new ByteArrayContent(image);
            MultipartFormDataContent httpContent = new MultipartFormDataContent
            {
                { bytes, "image", "image.png" }
            };

            var requestMessage = new HttpRequestMessage(HttpMethod.Post, "/v1/query");
            requestMessage.Content = httpContent;

            var date = DateTime.UtcNow.ToString("R");
            requestMessage.Headers.Add("Date", date);

            var signature = CreateSignature(requestMessage.Content, date);
            string authentication = string.Format("VWS {0}:{1}", access_key, signature);
            Console.WriteLine("Authentication: " + authentication);
            requestMessage.Headers.Add("Authorization", authentication);

            var result = await client.SendAsync(requestMessage);
            Console.WriteLine(await result.Content.ReadAsStringAsync());
            return Ok();
        }

        private static string ConvertMD5Hex(byte[] content)
        {
            MD5 md5 = MD5.Create();
            var contentMD5bytes = md5.ComputeHash(content);

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < contentMD5bytes.Length; i++)
            {
                sb.Append(contentMD5bytes[i].ToString("x2"));
            }

            return sb.ToString();
        }

        private static string CreateSignature(HttpContent content, string date)
        {
            byte[] body;
            using (var ms = new MemoryStream())
            {
                content.CopyToAsync(ms).GetAwaiter().GetResult();
                //await Request.Body.CopyToAsync(ms);
                body = ReadAllBytes(ms);  // returns base64 encoded string JSON result
            }
            //var bodyStream = content.ReadAsStreamAsync().Result;
            //var body = ReadAllBytes(bodyStream);

            string stringToSign = string.Format(
                "{0}\n{1}\n{2}\n{3}\n{4}",
                "POST",
                ConvertMD5Hex(body),
                content.Headers.ContentType,
                date,
                "/v1/query");
            //string stringToSign = "POST\ne968b50511317fef3bf509feab7cd31c\nmultipart/form-data\nSat, 04 May 2019 19:32:38 GMT\n/v1/query";
            Console.WriteLine("String to sign: " + stringToSign);

            HMACSHA1 sha1 = new HMACSHA1(Encoding.ASCII.GetBytes(secret_key));
            byte[] sha1Bytes = Encoding.ASCII.GetBytes(stringToSign);

            using (var stream = new MemoryStream(sha1Bytes))
            {
                byte[] sha1Hash = sha1.ComputeHash(stream);
                var signature = Convert.ToBase64String(sha1Hash);
                Console.WriteLine("Signature :" + signature);
                return signature;
            }
        }

        public static byte[] ReadAllBytes(Stream source)
        {
            long originalPosition = source.Position;
            source.Position = 0;

            try
            {
                byte[] readBuffer = new byte[4096];
                int totalBytesRead = 0;
                int bytesRead;
                while ((bytesRead = source.Read(readBuffer, totalBytesRead, readBuffer.Length - totalBytesRead)) > 0)
                {
                    totalBytesRead += bytesRead;
                    if (totalBytesRead == readBuffer.Length)
                    {
                        int nextByte = source.ReadByte();
                        if (nextByte != -1)
                        {
                            byte[] temp = new byte[readBuffer.Length * 2];
                            Buffer.BlockCopy(readBuffer, 0, temp, 0, readBuffer.Length);
                            Buffer.SetByte(temp, totalBytesRead, (byte)nextByte);
                            readBuffer = temp;
                            totalBytesRead++;
                        }
                    }
                }

                byte[] buffer = readBuffer;
                if (readBuffer.Length != totalBytesRead)
                {
                    buffer = new byte[totalBytesRead];
                    Buffer.BlockCopy(readBuffer, 0, buffer, 0, totalBytesRead);
                }
                return buffer;
            }
            finally
            {
                source.Position = originalPosition;
            }
        }

        private static async Task<object> CreateProductSet(CreateProductSetsOptions opts)
        {
            GoogleCredential cred = GoogleCredential.FromFile("/Users/lehmamic/Data/Misc/google-cloud-auth/ClimbingApp-a330ecf26da9.json");
            Channel channel = new Channel(
                ProductSearchClient.DefaultEndpoint.Host,
                ProductSearchClient.DefaultEndpoint.Port,
                cred.ToChannelCredentials()
                );

            try
            {
                var client = ProductSearchClient.Create();

                // Create a product set with the product set specification in the region.
                var request = new CreateProductSetRequest
                {
                    // A resource that represents Google Cloud Platform location
                    ParentAsLocationName = new LocationName(opts.ProjectID, opts.ComputeRegion),
                    ProductSetId = opts.ProductSetId,
                    ProductSet = new ProductSet
                    {
                        DisplayName = opts.ProductSetDisplayName
                    }
                };

                // The response is the product set with the `name` populated
                var response = await client.CreateProductSetAsync(request);

                Console.WriteLine($"Product set name: {response.DisplayName}");

                return 0;
            }
            finally
            {
                await channel.ShutdownAsync();
            }
        }
    }
}
