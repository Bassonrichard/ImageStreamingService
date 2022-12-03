using Google.Protobuf;
using Grpc.Core;
using Grpc.Net.Client;
using ImageStreamService;
using System.Diagnostics;

Console.WriteLine("Hello, World!");

string ImageName = "IMG_3733.jpeg";

using var channel = GrpcChannel.ForAddress("http://localhost:5253");
var client = new FileTransfer.FileTransferClient(channel);
var chunk = await GetChunck();

using var call = client.UploadFile();
await call.RequestStream.WriteAsync(chunk);

Stopwatch stopwatch = new Stopwatch();

stopwatch.Start();
FileResponse fileresponse = await call.ResponseAsync;
stopwatch.Stop();

Console.WriteLine($"It took: {stopwatch.ElapsedMilliseconds} ms to upload {fileresponse.FilePath}");

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