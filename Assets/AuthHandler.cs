using UnityEngine;
using System;
using System.Net;
using System.Threading;
using System.Text;
using RedditSharp;
using DefaultWebAgent = RedditSharp.WebAgent;

public class AuthHandler : MonoBehaviour {
    //Custom redirect URI and client ID. Port and path are customizable. Reddit gives you a client ID.
    public string URI = "http://localhost:8080/test";
    public string clientID = "-myF2l7jiNxLBA";

    //In case reddit some day changes this...
    public string OauthUrl = "https://oauth.reddit.com";

    private readonly HttpListener _listener = new HttpListener();
    private Func<HttpListenerRequest, string> _responderMethod;
   
    private IWebAgent _webAgent;
    private AuthProvider _authProvider;
    public static string AccessToken;
    // Use this for initialization
    void Start () {

        //Have to set up a web agent before initializing a Reddit object, since we need to authenticate.
        DefaultWebAgent defaultAgent = new DefaultWebAgent();
        DefaultWebAgent.UserAgent = "VReddit";
        DefaultWebAgent.RateLimit = DefaultWebAgent.RateLimitMode.Pace;
        DefaultWebAgent.Protocol = "https";
        DefaultWebAgent.RootDomain = "oauth.reddit.com";
        
        _webAgent = defaultAgent;
        _authProvider = new AuthProvider(clientID, "", URI, _webAgent);
        string prefixes = URI + "/";

        Debug.Log(prefixes);
        _listener.Prefixes.Add(prefixes);
        _responderMethod = SendResponse;
        Run();
        
	}
	
	// Update is called once per frame
    void Update()
    {
        
    }

    public void Stop()
    {
        _listener.Stop();
        _listener.Close();
    }

    //Host a mini web server to handle the redirect URI. This isn't an iOS or Android app, so this seems to be the easiest way
    //to deal with this.
    //Based on Simple C# Web Server - https://codehosting.net/blog/BlogEngine/post/Simple-C-Web-Server.aspx
    public void Run()
    {
        _listener.Start();

        //Listen on a worker thread
        ThreadPool.QueueUserWorkItem((o) =>
        {
            Debug.Log("Webserver running...");
            try
            {
                while (_listener.IsListening)
                {
                    ThreadPool.QueueUserWorkItem((c) =>
                    {
                        var ctx = c as HttpListenerContext;
                        try
                        {
                            string rstr = _responderMethod(ctx.Request);
                            byte[] buf = Encoding.UTF8.GetBytes(rstr);
                            ctx.Response.ContentLength64 = buf.Length;
                            ctx.Response.OutputStream.Write(buf, 0, buf.Length);

                        }
                        catch { } // suppress any exceptions
                        finally
                        {
                            // always close the stream
                            ctx.Response.OutputStream.Close();
                        }
                    }, _listener.GetContext());
                }
            }
            catch { } // suppress any exceptions
        });
        Application.OpenURL("https://www.reddit.com/api/v1/authorize?client_id=" + clientID + "&response_type=code&state=RANDaOM_STRING&redirect_uri=" + URI + "&scope=read+identity");
    }

    //Sends the response which you see in the browser.
    private string SendResponse(HttpListenerRequest request)
    {
        //Debug.Log(request.Url.Query);
        // Parse the query string variables
        string[] parts =request.Url.Query.Split(new char[] { '?', '&','=' });

        if (parts[1].Equals("state") && parts[2].Equals("RANDaOM_STRING")&&parts[3].Equals("code"))
        {
            Debug.Log("Response looks good. Code is: " + parts[4]);
            AccessToken = _authProvider.GetOAuthToken(parts[4], false);
            Debug.Log("Access token: " + AccessToken);

            return string.Format("<HTML><BODY>Authenticated! You may return to VReddit.<br></BODY></HTML>");
        }
        Stop();
        return string.Format("<HTML><BODY>Something went wrong. Please try again.<br></BODY></HTML>");
        
    }

}
