using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Linq;

public class IPManager : MonoBehaviour
{
    public static string GetLocalIPAddress()
    {
        var host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (var ip in host.AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                return ip.ToString();
            }
        }
        return "127.0.0.1"; // Возвращает "localhost", если не найден IPv4 адрес
    }
}
