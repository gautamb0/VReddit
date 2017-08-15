using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using RedditSharp;
using RedditSharp.Things;
using System.Collections;
using System.Threading;

public class Posts : MonoBehaviour
{
    public GameObject postObject;
    IEnumerable<Post> hot;
    ListingObject[] displayedPosts;
    ListingObject aDisplayedPost;
    string[] testStrings;
    public static Reddit reddit;
    PostLoader postLoader;

    // Use this for initialization
    void Start()
    {
        postLoader = new PostLoader();
        displayedPosts = new ListingObject[15];
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            new Thread(() =>
            {
                reddit = new Reddit(AuthHandler.AccessToken);
                Debug.Log("Authenticated");
            }).Start();

        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            postLoader.reddit = reddit;
            postLoader.Start();
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
           
        }
        if (postLoader != null)
        {
            if (postLoader.Update())
            {
                // Alternative to the OnFinished callback
                testStrings = postLoader.postTitles;
                displayPosts(testStrings);
                postLoader = null;
            }
        }
    }

    IEnumerator loadSub()
    {
        reddit = new Reddit(AuthHandler.AccessToken);
        Subreddit rAll = reddit.RSlashAll;
        hot = rAll.Hot.Take(15);
        yield return (0);
    }

    void displayPosts(string[] displayedPosts)
    {
        for (int i = 0; i < 15; i++)
        {
            var currentPost = Instantiate(postObject);
            currentPost.transform.SetParent(this.transform);
            currentPost.transform.localScale = Vector3.one;
            Vector3 tempPos = currentPost.transform.localPosition;
            tempPos.z = 0;
            currentPost.transform.localPosition = tempPos;

            PostUI currentPostUI = currentPost.GetComponent<PostUI>();
            currentPostUI.setTitle(displayedPosts[i]);
            /*currentPostUI.setSub(post.SubredditName);
            currentPostUI.setAge(post.Created - post.FetchedAt);
            currentPostUI.setUser(post.AuthorName);
            currentPostUI.setFlair(post.LinkFlairText);
            currentPostUI.setComments(post.CommentCount.ToString());
            currentPostUI.setScore(post.Score.ToString());*/
        }
    }

    void listPosts()
    {
        new Thread(() =>
        {

            Subreddit rAll = reddit.RSlashAll;
            int i = 0;
            hot = rAll.Hot.Take(15);
            foreach (var post in hot)
            {
                aDisplayedPost.Title = post.Title;
                Debug.Log(post.Title);

                //displayedPosts[i].Title = post.Title;
                /*temp.SubredditName = post.SubredditName;
                temp.Age = post.Created - post.FetchedAt;
                temp.AuthorName = post.AuthorName;
                temp.LinkFlairText = post.LinkFlairText;
                temp.CommentCount = post.CommentCount.ToString();
                temp.Score = post.Score.ToString();
                displayedPosts.Add(temp);*/
                i++;
            }


        }).Start();
    }
}
    /*
    void displayPostsOld()
    {

        var currentPost = Instantiate(postObject);
        currentPost.transform.SetParent(this.transform);
        currentPost.transform.localScale = Vector3.one;
        Vector3 tempPos = currentPost.transform.localPosition;
        tempPos.z = 0;
        currentPost.transform.localPosition = tempPos;

        PostUI currentPostUI = currentPost.GetComponent<PostUI>();
        Cell cell = new Cell();

        CellProd prod = new CellProd(cell, 15, reddit);  // Use cell for storage, 
                                                 // produce 20 items

        Thread producer = new Thread(new ThreadStart(prod.ThreadRun));

        // Threads producer and consumer have been created, 
        // but not started at this point.
        for (int i = 0; i < 15; i++)
        {
            Debug.Log(cell.cellArray[i]);
            var currentPost = Instantiate(postObject);
            currentPost.transform.SetParent(this.transform);
            currentPost.transform.localScale = Vector3.one;
            Vector3 tempPos = currentPost.transform.localPosition;
            tempPos.z = 0;
            currentPost.transform.localPosition = tempPos;

            PostUI currentPostUI = currentPost.GetComponent<PostUI>();
            currentPostUI.setTitle(cell.cellArray[i]);
            currentPostUI.setSub(post.SubredditName);
            currentPostUI.setAge(post.Created - post.FetchedAt);
            currentPostUI.setUser(post.AuthorName);
            currentPostUI.setFlair(post.LinkFlairText);
            currentPostUI.setComments(post.CommentCount.ToString());
            currentPostUI.setScore(post.Score.ToString());
        }
        try
        {
            producer.Start();

            //producer.Join();   // Join both threads with no timeout
                               // Run both until done.

            // threads producer and consumer have finished at this point.
            

        }
        catch (ThreadStateException e)
        {
            Debug.Log(e);  // Display text of exception
           
        }
        catch (ThreadInterruptedException e)
        {
            Debug.Log(e);  // This exception means that the thread
                                   // was interrupted during a Wait
           
        }

    }

}

    public class CellProd
    {
        Reddit reddit;
        IEnumerable<Post> hot;
        Cell cell;         // Field to hold cell object to be used
        int quantity = 1;  // Field for how many items to produce in cell

        public CellProd(Cell box, int request, Reddit reddit)
        {
            cell = box;          // Pass in what cell object to be used
            quantity = request;  // Pass in how many items to produce in cell
            this.reddit = reddit;
        }
        public void ThreadRun()
        {
            Subreddit rAll = reddit.RSlashAll;
            hot = rAll.Hot.Take(15);
            foreach (var post in hot)
            {
                Debug.Log(post.Title);

                cell.WriteToCell(post.Title);
            }
        }
    }

    public class Cell
    {
        string cellContents;         // Cell contents
        public string[] cellArray = new string[15];
        int index = 0;
        bool readerFlag = false;  // State flag



        public void WriteToCell(string n)
        {
            lock (this)  // Enter synchronization block
            {
                 cellContents = n;
                cellArray[index] = cellContents;

            index++;
                Debug.Log("Produce: {0}"+ cellContents);
               
            }   // Exit synchronization block
        }
    }
    */