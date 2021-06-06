using System.Threading;
using System;
using System.Text;
using System.Net;
using System.Threading.Tasks;
using OpenCvSharp;
using System.IO;
using System.Collections.Generic;

namespace Robot.VirtualWindow {
	public class VirtualWindowHost : IDisposable {
		private string pageBody = string.Empty;

		private HttpListener listener;
		private string url = "http://localhost:2020/";

		private CancellationTokenSource cancellationTokenSource = default;

		private Mat empty = null;
		private List<VirtualWindow> virtualWindows = null;

		public VirtualWindowHost() {
			this.pageBody = File.ReadAllText(@"src/VirtualWindow/index.html");

			this.empty = Mat.Zeros(320, 320, MatType.CV_32FC4);

			this.virtualWindows = new List<VirtualWindow>();

			this.cancellationTokenSource = new CancellationTokenSource();

			this.listener = new HttpListener();
			this.listener.Prefixes.Add(url);
			this.listener.Start();

			Task task = HandleIncomingConnections(cancellationTokenSource.Token);
		}

		public void AddVirtualWindow(VirtualWindow virtualWindow) {
			this.virtualWindows.Add(virtualWindow);
		}

		private async Task HandleIncomingConnections(CancellationToken ct) {
			var responseEntity = new byte[0] { };

			while (!ct.IsCancellationRequested) {
				HttpListenerContext ctx = await this.listener.GetContextAsync();

				HttpListenerRequest req = ctx.Request;
				HttpListenerResponse resp = ctx.Response;

				if (req.Url.AbsolutePath.StartsWith("/image")) {
					int index = Convert.ToInt32(req.Url.AbsolutePath.Substring(6, 1));

					var image = this.empty;

					if (index < this.virtualWindows.Count) {
						image = this.virtualWindows[index].GetImage();
					}

					resp.ContentType = "image/png";

					Cv2.ImEncode(".png", image, out var imageData);

					await resp.OutputStream.WriteAsync(imageData, 0, imageData.Length);
				} else {
					byte[] data = Encoding.UTF8.GetBytes(pageBody);

					resp.ContentType = "text/html";
					resp.ContentEncoding = Encoding.UTF8;
					resp.ContentLength64 = data.LongLength;

					await resp.OutputStream.WriteAsync(data, 0, data.Length);

					resp.Close(responseEntity, false);
				}
			}
		}

		public void Dispose() {
			this.cancellationTokenSource.Cancel();
			
			this.listener.Close();
		}
	}
}
