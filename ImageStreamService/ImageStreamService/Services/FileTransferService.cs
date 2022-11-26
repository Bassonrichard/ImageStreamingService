using Grpc.Core;

namespace ImageStreamService.Services
{
    public class FileTransferService : FileTransfer.FileTransferBase
    {
        private readonly ILogger<FileTransferService> _logger;
        public FileTransferService(ILogger<FileTransferService> logger)
        {
            _logger = logger;
        }

        public override async Task<FileResponse> UploadFile(IAsyncStreamReader<ChunkMsg> requestStream, ServerCallContext context)
        {
            _logger.LogInformation($"Got Message");

            await foreach (var request in requestStream.ReadAllAsync())
            {
                await File.WriteAllBytesAsync(request.FileName, request.Chunk.ToByteArray());
                return new FileResponse { FilePath = request.FileName };
            }

            return new FileResponse { FilePath = null };
        }
    }
}