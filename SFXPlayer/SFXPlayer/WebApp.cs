using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace SFXPlayer
{
    class WebApp
    {
        const string SFXProtocol = "ws-SFX-protocol";
        private static string Prefix = "http://+:";
        public static UInt16 wsPort = 3030;
        static HttpListener Listener = null;
        
        public static void Start()
        {
            if (Listener != null) return;
            if (!CheckIfPermitted()) return;
            try {
                Listener = new HttpListener();
                Listener.Prefixes.Add(Prefix + wsPort.ToString() + "/");
                Listener.Start();
                Listener.BeginGetContext(GetContextCallback, null);
                Program.mainForm.DisplayChanged += UpdateWebAppsWithDisplayChangeAsync;
            } catch (Exception) {
                Listener = null;
            }
        }

        private static bool CheckIfPermitted() {
            var process = new Process {
                StartInfo =
                    {
                        FileName = "netsh",
                        Arguments = "http show urlacl url=" + Prefix + wsPort.ToString() + "/",
                        UseShellExecute = false,
                        RedirectStandardOutput = true
                    }
            };
            process.Start();
            var output = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
            Debug.WriteLine(output);

            //parse the output
            if (output.Contains("User: NT AUTHORITY\\Authenticated Users")) return true;

            if (MessageBox.Show("Do you wish to enable the web server for the Remote Control?", Application.ProductName, MessageBoxButtons.YesNo) != DialogResult.Yes) return false;

            //add our url to the ACL list
            try {
                process = new Process {
                    StartInfo =
                        {
                        FileName = "netsh",
                        Arguments = "http add urlacl url=" + Prefix + wsPort.ToString() + "/" + " user=\"NT AUTHORITY\\Authenticated Users\"",
                        UseShellExecute = true,
                        Verb = "runas"
                    }
                };
                process.Start();
            } catch (Exception e) {
                MessageBox.Show("Unable to start web server for remote app.\r\n" + e.Message);
                return false;
            }
            process.WaitForExit();
            Debug.WriteLine(output);
            return true;
        }

        public static async void StopAsync() {
            while (webSockets.Count > 0) {
                WebSocket ws = webSockets[0];
                await ws.CloseAsync(WebSocketCloseStatus.NormalClosure, "SFX Player closed", CancellationToken.None);
                while (ws.State != WebSocketState.Closed && ws.State != WebSocketState.Aborted) ;
                lock (webSockets) {
                    webSockets.Remove(ws);
                }
            }
            Listener?.Stop();
            Listener = null;
            Program.mainForm.DisplayChanged -= UpdateWebAppsWithDisplayChangeAsync;
        }

        static Dictionary<string, string> contentTypes = new Dictionary<string, string>() {
            { ".html", "text/html" },
            { ".js", "text/javascript" },
            { ".ico", "image/x-icon" }
        };
        static Dictionary<string, bool> contentBinary = new Dictionary<string, bool>() {
            { ".html", false },
            { ".js", false },
            { ".ico", true }
        };

        //Incoming
        static void GetContextCallback(IAsyncResult ar)
        {
            if (Listener == null) return;
            var context = Listener.EndGetContext(ar);
            var NowTime = DateTime.UtcNow;
            var response = context.Response;
            string responseString;
            
            Listener.BeginGetContext(GetContextCallback, null);

            if (context.Request.IsWebSocketRequest) {
                ProcessWebSocketRequest(context);
                return;
            }

            var url = context.Request.RawUrl;
            if (string.IsNullOrEmpty(url) || url == "/") url = "/index.html";
            url = url.Replace("/", ".\\");
            string filename = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "html", url);
            if (!File.Exists(filename)) {
                response.StatusCode = (int)HttpStatusCode.NotFound;
                response.Close();
                Debug.WriteLine("File not found: " + context.Request.RawUrl);
                return;
            }

            string fileExt = Path.GetExtension(filename);
            if (!contentBinary.ContainsKey(fileExt)) {
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                response.Close();
                Debug.WriteLine("Bad request (file type not supported): " + context.Request.RawUrl);
                return;
            }

            response.ContentType = contentTypes[fileExt];

            if (contentBinary[fileExt]) {
                //binary file
                using (FileStream fs = File.OpenRead(filename)) {
                    //response is HttpListenerContext.Response...
                    response.ContentLength64 = fs.Length;
                    response.SendChunked = false;
                    //response.ContentType = System.Net.Mime.MediaTypeNames.Application.Octet;
                    response.AddHeader("Content-disposition", "attachment; filename=" + Path.GetFileName(filename));

                    byte[] buffer = new byte[64 * 1024];
                    int read;
                    using (BinaryWriter bw = new BinaryWriter(response.OutputStream)) {
                        while ((read = fs.Read(buffer, 0, buffer.Length)) > 0) {
                            bw.Write(buffer, 0, read);
                            bw.Flush(); //seems to have no effect
                        }

                        bw.Close();
                    }

                    response.StatusCode = (int)HttpStatusCode.OK;
                    response.StatusDescription = "OK";
                    response.OutputStream.Close();
                    Debug.WriteLine("Sent binary file: {1}", "GetContextCallback", Path.GetFileName(filename));
                }
            } else {
                //text file
                responseString = File.ReadAllText(filename);

                byte[] buffer = Encoding.UTF8.GetBytes(responseString);

                response.ContentLength64 = buffer.LongLength;
                response.OutputStream.Write(buffer, 0, buffer.Length);
                response.StatusCode = (int)HttpStatusCode.OK;
                response.OutputStream.Close();
                Debug.WriteLine("Sent text file: {1}", "GetContextCallback", Path.GetFileName(filename));
            }
        }
        private static List<WebSocket> webSockets = new List<WebSocket>();
        private static byte[] LastMessage = null;

        public static bool Serving => Listener != null;

        private async static void ProcessWebSocketRequest(HttpListenerContext context) {
            WebSocketContext wsContext = null;
            try {
                wsContext = await context.AcceptWebSocketAsync(SFXProtocol);
            } catch (Exception e) {
                context.Response.StatusCode = 500;
                context.Response.Close();
                Debug.WriteLine("Exception: {0}", e);
                return;
            }
            WebSocket ws = wsContext.WebSocket;
            if (LastMessage != null) {
                await ws.SendAsync(new ArraySegment<byte>(LastMessage, 0, LastMessage.Length), WebSocketMessageType.Text, true, CancellationToken.None);
            }

            lock (webSockets) {
                webSockets.Add(ws);
            }

            try {
                byte[] receiveBuffer = new byte[1024];
                while (ws.State == WebSocketState.Open) {
                    WebSocketReceiveResult receiveResult = await ws.ReceiveAsync(new ArraySegment<byte>(receiveBuffer), CancellationToken.None);
                    if (receiveResult.MessageType == WebSocketMessageType.Close) {
                        lock (webSockets) {
                            webSockets.Remove(ws);
                        }
                        await ws.CloseAsync(WebSocketCloseStatus.NormalClosure, "Close requested by remote", CancellationToken.None);
                    } else {
                        //TODO: code for received messages goes here!
                        string strXML = Encoding.UTF8.GetString(receiveBuffer, 0, receiveResult.Count);
                        //Debug.WriteLine(strXML);
                        XmlDocument xml = new XmlDocument();
                        xml.LoadXml(strXML);
                        var nodes = xml.SelectNodes("command");
                        foreach (XmlNode childrenNode in nodes) {
                            //Debug.WriteLine(childrenNode.Name);
                            //Debug.WriteLine(childrenNode.InnerText);
                            string command = childrenNode.InnerText.ToLower();
                            switch (command){
                                case "play":
                                    Program.mainForm.PlayNextCue();
                                    break;
                                case "stop":
                                    Program.mainForm.StopAll();
                                    break;
                                case "previous":
                                    Program.mainForm.PreviousCue();
                                    break;
                                case "next":
                                    Program.mainForm.NextCue();
                                    break;
                            }
                        }
                    }
                }
            } catch (Exception e) {
                Debug.WriteLine("Exception: {0}", e);
            } finally {
                if (ws != null) {
                    lock (webSockets) {
                        webSockets.Remove(ws);
                    }
                    ws.Dispose();
                }
            }
        }

        private static async void UpdateWebAppsWithDisplayChangeAsync(object sender, DisplaySettings e) {
            if (e != null) {
                LastMessage = Encoding.UTF8.GetBytes(e.SerializeToXmlString());
            } else {
                LastMessage = null;
            }
            if (LastMessage != null) {
                foreach (WebSocket ws in webSockets) {
                    await ws.SendAsync(new ArraySegment<byte>(LastMessage, 0, LastMessage.Length), WebSocketMessageType.Text, true, CancellationToken.None);
                }
            }
        }
    }
}
