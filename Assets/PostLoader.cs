using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RedditSharp;
using RedditSharp.Things;
using System.Linq;

public class PostLoader : ThreadedJob
{
    public Reddit reddit;
    IEnumerable<Post> hot;
    public ListingObject[] displayedPosts;
    public string[] postTitles;
    Cell cell;

    public PostLoader()
    {
        cell = new Cell();
    }
    protected override void ThreadFunction()
    {
        Subreddit rAll = reddit.RSlashAll;
        hot = rAll.Hot.Take(15);
        int i = 0;
        foreach (var post in hot)
        {
            Debug.Log(post.Title);
            cell.WriteToCell(post.Title);

        }
        Debug.Log("all posts printed");
    }
    protected override void OnFinished()
    {
        postTitles = cell.cellArray;
        Debug.Log("Finished");
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
                Debug.Log("Produce: {0}" + cellContents);

            }   // Exit synchronization block
        }
    }
