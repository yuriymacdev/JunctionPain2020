using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.Text;

using System.Net;
using System.Net.Sockets;

using System.Threading;

public class WhiteSquareBehaviour : MonoBehaviour
{
    private Vector3 pos_last;
    private Thread input_thread;

    Vector2 last_input_report;

    // Start is called before the first frame update
    void Start()
    {
        pos_last = gameObject.transform.position;

        input_thread = new Thread(this.inputAcquireThreadWork);
        input_thread.Start();
    }

    private static Vector2 getInput(UdpClient client) {
        Vector3 rpy = new Vector3(0, 0, 0);
        try
        {
            // client.Connect("xxx.com", 11000);
            // Byte[] sendBytes = Encoding.ASCII.GetBytes("Is anybody there?");

            // client.Send(sendBytes, sendBytes.Length);

            // UdpClient udpClientB = new UdpClient();
            // udpClientB.Send(sendBytes, sendBytes.Length, "127.0.0.1", 11000);

            IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 11000);

            Byte[] receiveBytes = client.Receive(ref RemoteIpEndPoint);
            string returnData = Encoding.ASCII.GetString(receiveBytes);

            string[] tokens = returnData.Split(' ');
            rpy.x = float.Parse(tokens[0]);
            rpy.y = float.Parse(tokens[1]);
            rpy.z = float.Parse(tokens[2]);
        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
        }

        return new Vector2() {x = rpy.z, y = -rpy.y};
    }

    private void inputAcquireThreadWork()
    {
        Debug.Log("Thread Start");
        UdpClient udpClient;
        udpClient = new UdpClient(11000);
        try
        {
            while (true) {
                // Debug.Log("Do work");
                var input_data = WhiteSquareBehaviour.getInput(udpClient);
                print(input_data.ToString());

                last_input_report = input_data;
            }
        } catch (ThreadAbortException e) {
            Debug.Log("Thread Abort Exception: " + e.ToString());
        } finally {
            Debug.Log("Job is done");
        }
        udpClient.Close();
        Debug.Log("Thread Exit");
    }

    private Vector3 velocity = new Vector3(0, 0, 0);

    void Update()
    {
        // gameObject.transform.Rotate(0, 0, 100 * Time.deltaTime);

        Vector3 accel = new Vector3(last_input_report.x, last_input_report.y, 0) * Time.deltaTime * 0.3f;
        velocity = velocity*0.1f + accel;
        gameObject.transform.position += velocity; 
    }

    void OnDestroy() {
        Debug.Log("Destroying...");
        if (!(input_thread is null) && input_thread.IsAlive) {
            input_thread.Abort();
            input_thread.Join();
        }
        Debug.Log("Destroyed");
    }

   public Vector3 GetOutPutData()
    {
        return last_input_report;

    }
}
