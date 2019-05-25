using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
using Google.Cloud.Vision.V1;
using Grpc.Auth;
using Grpc.Core;

namespace ClimbingApp.ImageRecognition.Services
{
    public class ImageRecognitionService : IImageRecognitionService
    {
        public ImageRecognitionService()
        {
        }

        public async Task CreateTargetSet(string targetSetId, string displayName)
        {
            GoogleCredential cred = this.CreateCredentials();
            var channel = new Channel(ProductSearchClient.DefaultEndpoint.Host, ProductSearchClient.DefaultEndpoint.Port, cred.ToChannelCredentials());

            try
            {
                var client = ProductSearchClient.Create(channel);

                var options = new CreateProductSetsOptions
                {
                    ProjectID = "climbingapp-241211",
                    ComputeRegion = "europe-west1",
                    ProductSetId = targetSetId,
                    ProductSetDisplayName = displayName,
                };
                await this.CreateProductSet(client, options);
            }
            finally
            {
                await channel.ShutdownAsync();
            }
        }

        public async Task CreateTarget(string targetId, string displayName, byte[] referenceImage)
        {
            GoogleCredential cred = this.CreateCredentials();
            var channel = new Channel(ProductSearchClient.DefaultEndpoint.Host, ProductSearchClient.DefaultEndpoint.Port, cred.ToChannelCredentials());

            try
            {
                var client = ProductSearchClient.Create(channel);
                var storage = await StorageClient.CreateAsync(cred);

                var createProductOptions = new CreateProductOptions
                {
                    ProjectID = "climbingapp-241211",
                    ComputeRegion = "europe-west1",
                    ProductID = targetId,
                    ProductCategory = "apparel",
                    DisplayName = "6aPlus-Schieber",
                };
                Product product = await this.CreateProduct(client, createProductOptions);

                var addProductOptions = new AddProductToProductSetOptions
                {
                    ProjectID = "climbingapp-241211",
                    ComputeRegion = "europe-west1",
                    ProductID = targetId,
                    ProductSetId = "climbing-routes-1",
                };
                await this.AddProductToProductSet(client, addProductOptions);
                await this.UploadFile(storage, "climbing-routes-images", targetId.ToString(), referenceImage);

                var createReferenceImageOptions = new CreateReferenceImageOptions
                {
                    ProjectID = "climbingapp-241211",
                    ComputeRegion = "europe-west1",
                    ProductID = targetId,
                    ReferenceImageID = targetId,
                    ReferenceImageURI = $"gs://climbing-routes-images/{targetId}",
                };
                await this.CreateReferenceImage(client, createReferenceImageOptions);
            }
            finally
            {
                await channel.ShutdownAsync();
            }
        }

        public async Task<QueryResult> QuerySimilarTargets(byte[] image)
        {
            GoogleCredential cred = this.CreateCredentials();
            var channel = new Channel(ProductSearchClient.DefaultEndpoint.Host, ProductSearchClient.DefaultEndpoint.Port, cred.ToChannelCredentials());

            try
            {
                var imageAnnotatorClient = ImageAnnotatorClient.Create(channel);

                var options = new GetSimilarProductsOptions
                {
                    ProjectID = "climbingapp-241211",
                    ComputeRegion = "europe-west1",
                    ProductSetId = "climbing-routes-1",
                    ProductCategory = "apparel",
                    Filter = string.Empty,
                    ImageBinaries = image,
                };

                return await this.GetSimilarProductsFile(imageAnnotatorClient, options);
            }
            catch(AnnotateImageException e)
            {
                return new QueryResult
                {
                    Results = new QueryResultEntry[0],
                };
            }
            finally
            {
                await channel.ShutdownAsync();
            }
        }

        private async Task<ProductSet> CreateProductSet(ProductSearchClient client, CreateProductSetsOptions opts)
        {
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

            return response;
        }

        private async Task<Product> CreateProduct(ProductSearchClient client, CreateProductOptions opts)
        {
            var request = new CreateProductRequest
            {
                // A resource that represents Google Cloud Platform location.
                ParentAsLocationName = new LocationName(opts.ProjectID, opts.ComputeRegion),
                // Set product category and product display name
                Product = new Product
                {
                    DisplayName = opts.DisplayName,
                    ProductCategory = opts.ProductCategory
                },
                ProductId = opts.ProductID
            };

            // The response is the product with the `name` field populated.
            var product = await client.CreateProductAsync(request);

            return product;
        }

        private async Task AddProductToProductSet(ProductSearchClient client, AddProductToProductSetOptions opts)
        {
            var request = new AddProductToProductSetRequest
            {
                // Get the full path of the products
                ProductAsProductName = new ProductName(opts.ProjectID, opts.ComputeRegion, opts.ProductID),
                // Get the full path of the product set.
                ProductSetName = new ProductSetName(opts.ProjectID, opts.ComputeRegion, opts.ProductSetId),
            };

            await client.AddProductToProductSetAsync(request);
        }

        private async Task<ReferenceImage> CreateReferenceImage(ProductSearchClient client, CreateReferenceImageOptions opts)
        {
            var request = new CreateReferenceImageRequest
            {
                // Get the full path of the product.
                ParentAsProductName = new ProductName(opts.ProjectID, opts.ComputeRegion, opts.ProductID),
                ReferenceImageId = opts.ReferenceImageID,
                // Create a reference image.
                ReferenceImage = new ReferenceImage
                {
                    Uri = opts.ReferenceImageURI
                }
            };

            var referenceImage = await client.CreateReferenceImageAsync(request);

            return referenceImage;
        }

        private async Task UploadFile(StorageClient storage, string bucketName, string objectName, byte[] image)
        {
            using (var stream = new MemoryStream(image))
            {
                await storage.UploadObjectAsync(bucketName, objectName, null, stream);
            }
        }

        private GoogleCredential CreateCredentials()
        {
            return GoogleCredential.FromFile("/Users/lehmamic/Data/Misc/ClimbingApp/ClimbingApp/ClimbingApp-8385749116e7.json");
        }

        private async Task<QueryResult> GetSimilarProductsFile(ImageAnnotatorClient imageAnnotatorClient, GetSimilarProductsOptions opts)
        {
            // Create annotate image request along with product search feature.
            Google.Cloud.Vision.V1.Image image = Google.Cloud.Vision.V1.Image.FromBytes(opts.ImageBinaries);

            // Product Search specific parameters
            var productSearchParams = new ProductSearchParams
            {
                ProductSetAsProductSetName = new ProductSetName(opts.ProjectID,
                                                                opts.ComputeRegion,
                                                                opts.ProductSetId),
                ProductCategories = { opts.ProductCategory },
                Filter = opts.Filter
            };

            // Search products similar to the image.
            var results = await imageAnnotatorClient.DetectSimilarProductsAsync(image, productSearchParams);

            return new QueryResult
            {
                Results = results.Results.Select(r => new QueryResultEntry
                {
                    TargetId = r.Product.ProductName.ProductId,
                    DisplayName = r.Product.DisplayName,
                    Description = r.Product.Description,
                    Score = r.Score,
                }).ToArray()
            };
        }
    }
}
