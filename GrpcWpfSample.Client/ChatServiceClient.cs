using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using GrpcWpfSample.Common;
//using Grpc.Net.Client;
//using Grpc.Net.Client.Web;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace GrpcWpfSample.Client
{
    public class ChatServiceClient
    {
        private readonly Chat.ChatClient m_client;

        public ChatServiceClient()
        {
            //var httpClient = new HttpClient(new GrpcWebHandler(GrpcWebMode.GrpcWeb, new HttpClientHandler()));
            //var channel = GrpcChannel.ForAddress("https://WeatherForecasts→blazor-2→elv1s42.mockqa.xyz", new GrpcChannelOptions { HttpClient = httpClient });

            // create insecure channel
            m_client = new Chat.ChatClient(
                //new Channel("localhost", 50052, ChannelCredentials.Insecure));
                new Channel("Chat→grpc-wpf→elv1s42.mockqa.xyz", 443, ChannelCredentials.SecureSsl));
                //new Channel("xn--chatgrpc-wpfelv1s42-3h2lia.mockqa.xyz", 443, ChannelCredentials.Insecure));
        }

        public async Task Write(ChatLog chatLog)
        {
            await m_client.WriteAsync(chatLog);
        }

        public IAsyncEnumerable<ChatLog> ChatLogs()
        {
            var call = m_client.Subscribe(new Empty());

            // I do not want to expose gRPC such as IAsyncStreamReader or AsyncServerStreamingCall.
            // I also do not want to bother user of this class with asking to dispose the call object.

            return call.ResponseStream
                .ToAsyncEnumerable()
                .Finally(() => call.Dispose());
        }
    }
}
