using Grpc.Core;

namespace ImageStreamService.Services
{
    public class FileTransferService : FileTransfer.FileTransferBase
    {
        private readonly ILogger<FileTransferService> _logger;
        private readonly IStorageService _storageService;

        public FileTransferService(ILogger<FileTransferService> logger, IStorageService storageService)
        {
            _logger = logger;
            _storageService = storageService;
        }

        public override async Task<FileResponse> UploadFile(IAsyncStreamReader<ChunkMsg> requestStream, ServerCallContext context)
        {
            _logger.LogInformation($"Got Message");

            await foreach (var request in requestStream.ReadAllAsync())
            {
                using (Stream stream = new MemoryStream(request.Chunk.ToByteArray()))
                {
                    await _storageService.UploadBlob(stream, request.FileName, "Recipes");
                }

                return new FileResponse { FilePath = request.FileName };
            }

            return new FileResponse { FilePath = null };
        }
    }
}