﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net;

namespace httplisten
{
    class HttpListenerContextClass
    {
        HttpListenerContext mhttplistenercontext;
        Thread handleThread;
        public HttpListenerContextClass(HttpListenerContext p)
        {
            mhttplistenercontext = p;
            handleThread = new Thread(new ThreadStart(handlethreadfunc));
            handleThread.IsBackground = true;
            handleThread.Start();
        }
        void handlethreadfunc()
        {
            HttpListenerRequest request = mhttplistenercontext.Request;
            System.IO.Stream input = request.InputStream;
            byte[] array = new byte[request.ContentLength64];
            input.Read(array, 0, (int)request.ContentLength64);//larg file may encounter error
            string utfString = Encoding.UTF8.GetString(array, 0, array.Length);

            HttpListenerResponse response = mhttplistenercontext.Response;
            // Construct a response.
            string responseString = "<HTML><BODY> Hello world!</BODY></HTML>";
            responseString += utfString;
            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);
            // Get a response stream and write the response to it.
            response.ContentLength64 = buffer.Length;
            System.IO.Stream output = response.OutputStream;
            output.Write(buffer, 0, buffer.Length);
            Thread.Sleep(30);
            input.Close();
            output.Close();
        }
    }
}
