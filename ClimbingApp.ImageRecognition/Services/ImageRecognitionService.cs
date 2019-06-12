using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Google.Api.Gax;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
using Google.Cloud.Vision.V1;
using Grpc.Auth;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using static Google.Cloud.Vision.V1.Product.Types;

namespace ClimbingApp.ImageRecognition.Services
{
    public class ImageRecognitionService : IImageRecognitionService
    {
        private readonly IMapper mapper;
        private readonly ILogger logger;

        public ImageRecognitionService(IMapper mapper, ILogger<ImageRecognitionService> logger)
        {
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task GetTargetSets()
        {
            GoogleCredential cred = this.CreateCredentials();
            var channel = new Channel(ProductSearchClient.DefaultEndpoint.Host, ProductSearchClient.DefaultEndpoint.Port, cred.ToChannelCredentials());

            try
            {
                var client = ProductSearchClient.Create(channel);
                // var productSet = await this.GetProductSets(client, 100);

               // productSet.
 
            }
            finally
            {
                await channel.ShutdownAsync();
            }
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
                var productSet = await this.CreateProductSet(client, options);
            }
            finally
            {
                await channel.ShutdownAsync();
            }
        }

        public async Task<IEnumerable<Target>> GetTargets(string targetSetId, int page, int pageSize)
        {
            GoogleCredential cred = this.CreateCredentials();
            var channel = new Channel(ProductSearchClient.DefaultEndpoint.Host, ProductSearchClient.DefaultEndpoint.Port, cred.ToChannelCredentials());

            try
            {
                var client = ProductSearchClient.Create(channel);

                ListProductsInProductSetRequest request = new ListProductsInProductSetRequest
                {
                    ProductSetName = new ProductSetName("climbingapp-241211", "europe-west1", targetSetId),
                    PageSize = pageSize,
                };

                PagedAsyncEnumerable<ListProductsInProductSetResponse, Product> response = client.ListProductsInProductSetAsync(request);
                IEnumerable<Product> products = await response.AsAsyncEnumerable().ToArray();
                IEnumerable<Target> targets = await Task.WhenAll(products.Select(p => this.LoadReferenceImagesAndMapToTarget(client, p, pageSize)));

                return targets;
            }
            finally
            {
                await channel.ShutdownAsync();
            }
        }

        public async Task<Target> GetTarget(string targetId)
        {
            GoogleCredential cred = this.CreateCredentials();
            var channel = new Channel(ProductSearchClient.DefaultEndpoint.Host, ProductSearchClient.DefaultEndpoint.Port, cred.ToChannelCredentials());

            try
            {
                var client = ProductSearchClient.Create(channel);
                Product product = await this.GetProduct(client, targetId);
                return await this.LoadReferenceImagesAndMapToTarget(client, product, 100);
            }
            finally
            {
                await channel.ShutdownAsync();
            }
        }

        public async Task<Target> CreateTarget(string displayName, string description, IReadOnlyDictionary<string, string> labels, byte[] referenceImageBinaries)
        {
            GoogleCredential cred = this.CreateCredentials();
            var channel = new Channel(ProductSearchClient.DefaultEndpoint.Host, ProductSearchClient.DefaultEndpoint.Port, cred.ToChannelCredentials());

            try
            {
                var client = ProductSearchClient.Create(channel);
                var storage = await StorageClient.CreateAsync(cred);

                string productId = Guid.NewGuid().ToString();
                var createProductOptions = new CreateProductOptions
                {
                    ProjectID = "climbingapp-241211",
                    ComputeRegion = "europe-west1",
                    ProductID = productId,
                    ProductCategory = "apparel",
                    DisplayName = displayName,
                    Description = description,
                    ProductLabels = labels,
                };
                Product product = await this.CreateProduct(client, createProductOptions);

                var addProductOptions = new AddProductToProductSetOptions
                {
                    ProjectID = "climbingapp-241211",
                    ComputeRegion = "europe-west1",
                    ProductID = product.ProductName.ProductId,
                    ProductSetId = "climbing-routes-1",
                };
                await this.AddProductToProductSet(client, addProductOptions);

                string referenceImageId = Guid.NewGuid().ToString();
                await this.UploadFile(storage, "climbing-routes-images", referenceImageId, referenceImageBinaries);

                var createReferenceImageOptions = new CreateReferenceImageOptions
                {
                    ProjectID = "climbingapp-241211",
                    ComputeRegion = "europe-west1",
                    ProductID = product.ProductName.ProductId,
                    ReferenceImageID = referenceImageId,
                    ReferenceImageURI = $"gs://climbing-routes-images/{referenceImageId}",
                };
                Google.Cloud.Vision.V1.ReferenceImage referenceImage = await this.CreateReferenceImage(client, createReferenceImageOptions);

                Target target = this.mapper.Map<Target>(product);
                target.ReferenceImages = new ReferenceImage[] { this.mapper.Map<ReferenceImage>(referenceImage) };

                return target;
            }
            finally
            {
                await channel.ShutdownAsync();
            }
        }

        public async Task DeleteTarget(string targetSetId, string targetId)
        {
            GoogleCredential cred = this.CreateCredentials();
            var channel = new Channel(ProductSearchClient.DefaultEndpoint.Host, ProductSearchClient.DefaultEndpoint.Port, cred.ToChannelCredentials());

            try
            {
                var client = ProductSearchClient.Create(channel);
                var storage = await StorageClient.CreateAsync(cred);

                IEnumerable<Google.Cloud.Vision.V1.ReferenceImage> referenceImages = await this.GetReferenceImages(client, targetId, 100);
                await Task.WhenAll(referenceImages.Select(async r =>
                {
                    await this.DeleteReferenceImage(client, targetId, r.ReferenceImageName.ReferenceImageId);
                    await this.DeleteFile(storage, "climbing-routes-images", r.ReferenceImageName.ReferenceImageId);
                }));

                await this.RemoveProductFromProductSet(client, targetSetId, targetId);
                await this.DeleteProduct(client, targetId);

            }
            finally
            {
                await channel.ShutdownAsync();
            }
        }

        public async Task<TargetSearchResults> QuerySimilarTargets(byte[] image)
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
                this.logger.LogError(e, "The google cloud image recognition service threw an error.");
                return new TargetSearchResults
                {
                    Results = new TargetSearchResultEntry[0],
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


        //private async Task<ProductSet> GetProductSets(ProductSearchClient client, int pageSize)
        //{
        //    var request = new ListProductSetsRequest
        //    {
        //        ParentAsLocationName = new LocationName("climbingapp-241211", "europe-west1"),
        //        PageSize = pageSize,
        //    };

        //    return await client.ListProductSets(request);
        //}

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
                    ProductCategory = opts.ProductCategory,
                    Description = opts.Description ?? string.Empty,
                },
                ProductId = opts.ProductID
            };

            foreach(var label in opts.ProductLabels)
            {
                request.Product.ProductLabels.Add(new KeyValue { Key = label.Key, Value = label.Value });
            }

            // The response is the product with the `name` field populated.
            var product = await client.CreateProductAsync(request);

            return product;
        }

        private async Task<Product> GetProduct(ProductSearchClient client, string productId)
        {
            var request = new GetProductRequest
            {
                ProductName = new ProductName("climbingapp-241211", "europe-west1", productId),
            };

            return await client.GetProductAsync(request);
        }

        private async Task DeleteProduct(ProductSearchClient client, string productId)
        {
            var request = new DeleteProductRequest
            {
                ProductName = new ProductName("climbingapp-241211", "europe-west1", productId),
            };

            await client.DeleteProductAsync(request);
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

        private async Task RemoveProductFromProductSet(ProductSearchClient client, string productSetId, string productId)
        {
            var request = new RemoveProductFromProductSetRequest
            {
                ProductSetName = new ProductSetName("climbingapp-241211", "europe-west1", productSetId),
                ProductAsProductName = new ProductName("climbingapp-241211", "europe-west1", productId),
            };

            await client.RemoveProductFromProductSetAsync(request);
        }

        private async Task<Google.Cloud.Vision.V1.ReferenceImage> CreateReferenceImage(ProductSearchClient client, CreateReferenceImageOptions opts)
        {
            var request = new CreateReferenceImageRequest
            {
                // Get the full path of the product.
                ParentAsProductName = new ProductName(opts.ProjectID, opts.ComputeRegion, opts.ProductID),
                ReferenceImageId = opts.ReferenceImageID,
                // Create a reference image.
                ReferenceImage = new Google.Cloud.Vision.V1.ReferenceImage
                {
                    Uri = opts.ReferenceImageURI
                }
            };

            var referenceImage = await client.CreateReferenceImageAsync(request);

            return referenceImage;
        }

        private async Task DeleteReferenceImage(ProductSearchClient client, string productId, string referenceImageId)
        {
            var request = new DeleteReferenceImageRequest
            {
                ReferenceImageName = new ReferenceImageName("climbingapp-241211", "europe-west1", productId, referenceImageId)
            };

            await client.DeleteReferenceImageAsync(request);
        }

        private async Task UploadFile(StorageClient storage, string bucketName, string objectName, byte[] image)
        {
            using (var stream = new MemoryStream(image))
            {
                await storage.UploadObjectAsync(bucketName, objectName, null, stream);
            }
        }

        private async Task DeleteFile(StorageClient storage, string bucketName, string objectName)
        {
            await storage.DeleteObjectAsync(bucketName, objectName);
        }

        private GoogleCredential CreateCredentials()
        {
            return GoogleCredential.FromFile("../ClimbingApp-8385749116e7.json");
        }

        private async Task<TargetSearchResults> GetSimilarProductsFile(ImageAnnotatorClient imageAnnotatorClient, GetSimilarProductsOptions opts)
        {
            // Create annotate image request along with product search feature.
            Image image = Image.FromBytes(opts.ImageBinaries);

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
            ProductSearchResults results = await imageAnnotatorClient.DetectSimilarProductsAsync(image, productSearchParams);
            return this.mapper.Map<TargetSearchResults>(results);
        }

        private async Task<Target> LoadReferenceImagesAndMapToTarget(ProductSearchClient client, Product product, int pageSize)
        {
            IEnumerable<Google.Cloud.Vision.V1.ReferenceImage> referenceImages = await this.GetReferenceImages(client, product, pageSize);

            Target target = this.mapper.Map<Target>(product);
            target.ReferenceImages = this.mapper.Map<IEnumerable<ReferenceImage>>(referenceImages);

            return target;
        }

        private async Task<IEnumerable<Google.Cloud.Vision.V1.ReferenceImage>> GetReferenceImages(ProductSearchClient client, Product product, int pageSize)
        {
            ListReferenceImagesRequest referenceImageRequest = new ListReferenceImagesRequest
            {
                ParentAsProductName = product.ProductName,
                PageSize = pageSize,
            };

            return await client.ListReferenceImagesAsync(referenceImageRequest).AsAsyncEnumerable().ToArray();
        }

        private async Task<IEnumerable<Google.Cloud.Vision.V1.ReferenceImage>> GetReferenceImages(ProductSearchClient client, string productId, int pageSize)
        {
            ListReferenceImagesRequest referenceImageRequest = new ListReferenceImagesRequest
            {
                ParentAsProductName = new ProductName("climbingapp-241211", "europe-west1", productId),
                PageSize = pageSize,
            };

            return await client.ListReferenceImagesAsync(referenceImageRequest).AsAsyncEnumerable().ToArray();
        }
    }
}
