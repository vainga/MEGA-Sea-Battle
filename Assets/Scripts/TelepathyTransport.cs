using System;
using System.Net;
using UnityEngine;
using Mirror;
using Telepathy;

public class TelepathyTransport : Transport
{
    private Telepathy.Client client;
    private Telepathy.Server server;

    [SerializeField] private ushort port = 7777;
    [SerializeField] private int serverMaxMessageSize = 16 * 1024;

    void Awake()
    {
        client = new Telepathy.Client(serverMaxMessageSize);
        server = new Telepathy.Server(serverMaxMessageSize);
        client.OnConnected += OnClientConnectedInternal;
        client.OnData += OnClientDataInternal;
        client.OnDisconnected += OnClientDisconnectedInternal;
        server.OnConnected += OnServerConnectedInternal;
        server.OnData += OnServerDataInternal;
        server.OnDisconnected += OnServerDisconnectedInternal;
    }

    public override bool Available()
    {
        return Application.platform != RuntimePlatform.WebGLPlayer;
    }

    public override bool ClientConnected() => client.Connected;

    public override void ClientConnect(string address)
    {
        client.Connect(address, port);
    }

    public override void ClientSend(ArraySegment<byte> segment, int channelId)
    {
        client.Send(segment.Array);
    }

    public override void ClientDisconnect()
    {
        client.Disconnect();
    }

    public override Uri ServerUri()
    {
        UriBuilder builder = new UriBuilder();
        builder.Scheme = "tcp";
        builder.Host = Dns.GetHostName();
        builder.Port = port;
        return builder.Uri;
    }

    public override bool ServerActive() => server.Active;

    public override void ServerStart()
    {
        server.Start(port);
    }

    public override void ServerSend(int connectionId, ArraySegment<byte> segment, int channelId)
    {
        server.Send(connectionId, segment.Array);
    }

    public override void ServerDisconnect(int connectionId)
    {
        server.Disconnect(connectionId);
    }

    public override void ServerStop()
    {
        server.Stop();
    }

    public override int GetMaxPacketSize(int channelId)
    {
        return serverMaxMessageSize;
    }

    public override void Shutdown()
    {
        server.Stop();
        client.Disconnect();
    }

    public override string ServerGetClientAddress(int connectionId)
    {
        return server.GetClientAddress(connectionId);
    }

    private void OnClientConnectedInternal()
    {
        OnClientConnected.Invoke();
    }

    private void OnClientDataInternal(ArraySegment<byte> data)
    {
        OnClientDataReceived.Invoke(data, Channels.Reliable);
    }

    private void OnClientDisconnectedInternal()
    {
        OnClientDisconnected.Invoke();
    }

    private void OnServerConnectedInternal(int connectionId)
    {
        OnServerConnected.Invoke(connectionId);
    }

    private void OnServerDataInternal(int connectionId, ArraySegment<byte> data)
    {
        OnServerDataReceived.Invoke(connectionId, data, Channels.Reliable);
    }

    private void OnServerDisconnectedInternal(int connectionId)
    {
        OnServerDisconnected.Invoke(connectionId);
    }
}
