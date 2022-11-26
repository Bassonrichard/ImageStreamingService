using Google.Protobuf;
using Grpc.Core;
using Grpc.Net.Client;
using ImageStreamService;

Console.WriteLine("Hello, World!");

string ImageName = "IMG_3734.jpeg";

using var channel = GrpcChannel.ForAddress("http://localhost:5292");
var client = new FileTransfer.FileTransferClient(channel);
var chunk = await GetChunck();

using var call = client.UploadFile();
await call.RequestStream.WriteAsync(chunk);


FileResponse fileresponse = await call.ResponseAsync;

Console.WriteLine(fileresponse.FilePath);

async Task<ChunkMsg> GetChunck()
{
    byte[] fs = await File.ReadAllBytesAsync(ImageName);

    return new ChunkMsg
    {
        FileName = ImageName,
        FileSize = fs.Length,
        Chunk = ByteString.CopyFrom(fs),
    };
}