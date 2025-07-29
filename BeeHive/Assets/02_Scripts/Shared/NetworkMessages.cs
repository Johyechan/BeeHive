using Mirror;

public struct RegisterRoomMessage : NetworkMessage
{
    public string roomName;
    public string ip;
}

public struct SearchRoomMessage : NetworkMessage
{
    public string roomName;
}

public struct SearchRoomResponseMessage : NetworkMessage
{
    public string ip;
}
