using Microsoft.Extensions.Options;
using Minio;
using Minio.DataModel.Args;
using Recruiting.Interfaces;
using Recruiting.Options;

namespace Recruiting.Services;

public class MinioStorageService : IStorageService
{
    private readonly IMinioClient _minioClient;
    private readonly MinioOptions _options;

    public MinioStorageService(IMinioClient minioClient, IOptions<MinioOptions> options)
    {
        _minioClient = minioClient;
        _options = options.Value;
    }

    public async Task EnsureBucketExistsAsync(CancellationToken ct = default)
    {
        var beArgs = new BucketExistsArgs().WithBucket(_options.BucketName);
        bool found = await _minioClient.BucketExistsAsync(beArgs, ct);
        if (!found)
        {
            var mbArgs = new MakeBucketArgs().WithBucket(_options.BucketName);
            await _minioClient.MakeBucketAsync(mbArgs, ct);
        }
    }

    public async Task<string> UploadAsync(Stream stream, string fileName, string contentType, long size, CancellationToken ct = default)
    {
        await EnsureBucketExistsAsync(ct);

        // Формируем уникальный ключ: 2026/11/guid_filename.pdf
        var objectKey = $"{DateTime.UtcNow:yyyy/MM}/{Guid.NewGuid()}_{fileName}";

        var putObjectArgs = new PutObjectArgs()
            .WithBucket(_options.BucketName)
            .WithObject(objectKey)
            .WithStreamData(stream)
            .WithObjectSize(size)
            .WithContentType(contentType);

        await _minioClient.PutObjectAsync(putObjectArgs, ct);
        return objectKey;
    }

    public async Task<Stream> DownloadAsync(string objectKey, CancellationToken ct = default)
    {
        var memoryStream = new MemoryStream();
        var getObjectArgs = new GetObjectArgs()
            .WithBucket(_options.BucketName)
            .WithObject(objectKey)
            .WithCallbackStream(stream => stream.CopyTo(memoryStream));

        await _minioClient.GetObjectAsync(getObjectArgs, ct);
        memoryStream.Position = 0;
        return memoryStream;
    }

    public async Task DeleteAsync(string objectKey, CancellationToken ct = default)
    {
        var removeObjectArgs = new RemoveObjectArgs()
            .WithBucket(_options.BucketName)
            .WithObject(objectKey);

        await _minioClient.RemoveObjectAsync(removeObjectArgs, ct);
    }
}