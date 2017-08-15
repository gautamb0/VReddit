using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;
using RedditSharp;
using RedditSharp.Things;
using System.Collections;
using System.Threading;

public class InitListing : MonoBehaviour {
    private ListingObject[] listingObjects;
    public static Reddit reddit;
    WWW www;
    IEnumerable<Post> hot;
    // Use this for initialization
    void Start () {
        Debug.Log("hello");
        string url = "http://httpbin.org/get?var1=value2&amp;var2=value2";
        www = new WWW(url);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            initReddit();
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            //Debug.Log("WWW Ok!: " + www.text);
            PopulateTest();
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            listComments(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            listComments(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            listComments(2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            listComments(3);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            listComments(4);
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            listComments(5);
        }
    }

   void PopulateTest()
    {
        
        new Thread(() =>
        {
            reddit = new Reddit(AuthHandler.AccessToken);
            Subreddit rAll = reddit.RSlashAll;
        hot = rAll.Hot.Take(25);
        }).Start();
    }

    
    void PopulateText()
    {

            Subreddit rAll = reddit.RSlashAll;
        hot = rAll.Hot.Take(25);
        Transform[] listingTransforms = GetComponentsInChildren<Transform>();
        TextMesh[] textMeshes = GetComponentsInChildren<TextMesh>();
        

        for (int i = 0; i < listingTransforms.Length; i++)
        {
            listingTransforms[i].localPosition = new Vector3(-2.5f, 3.0f - i * 0.5f, 0.0f);
        }

        
        int listingIndex = 0;
        foreach (var post in hot)
        {
            Debug.Log(post.Title);
            textMeshes[listingIndex].text = post.Title;
            listingIndex++;
        }
        Debug.Log("done");

}

    void listComments(int index)
    {
        Debug.Log("listcomments");
        var firstPost = hot.ElementAt(index);

        foreach (var comment in firstPost.Comments.Take(2))
        {
            Debug.Log(comment.Body);
            RecurseComments(comment.Comments, 1);
        }
        Debug.Log("done");
    }

    IEnumerator WaitForRequest(WWW www)
    {
        while (!www.isDone)
            yield return null;

        // check for errors
        if (www.error == null)
        {
            Debug.Log("WWW Ok!: " + www.data);
        }
        else {
            Debug.Log("WWW Error: " + www.error);
        }
    }


    /* public static List<Comment> GetAllComments(this Post post)
      {
          var comments = new List<Comment>();

          Action<IList<Comment>> GetCommentsRecursive = null;
          GetCommentsRecursive = delegate (IList<Comment> children)
          {
              if (children == null || children.Count == 0)
                  return;

              comments.AddRange(children);

              foreach (var child in children)
                  GetCommentsRecursive(child.Comments);
          };

          GetCommentsRecursive(post.Comments);
          return comments;
      }*/

    public void RecurseComments(IList<Comment> children, int depth)
    {
        if (children == null || children.Count == 0)
            return;
        //Debug.Log(children.First().Body);

        foreach (var child in children)
        {
            string str = "";
            for (int i = 0; i < depth;i++)
            {
                str += ">";
            }
            str += child.Body;
            Debug.Log(str);
            RecurseComments(child.Comments, depth+1);
        }
    }

    public void initReddit()
    {
        reddit = new Reddit(AuthHandler.AccessToken);
        PopulateText();
    }
}
