using UnityEngine;
using UnityEngine.UI;
using System;
public class PostUI : MonoBehaviour {
    public Text title;
    public Text sub;
    public Text age;
    public Text user;
    public Text flair;
    public Text comments;
    public Text score;
    string permalink;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void setTitle(string str)
    {
        title.text = str;
    }

    public void setSub(string str)
    {
        sub.text = "r/"+str;
    }

    public void setAge(TimeSpan t)
    {
        string str;

        if (t.Days > 1)
        {
            str = t.Days.ToString() + "d";
        }
        else if (t.Hours > 1)
        {
            str = t.Hours.ToString() + "h";
        }
        if (t.Minutes > 1)
        {
            str = t.Minutes.ToString() + "m";
        }
        else
        {
            str = "<1m";
        }
        
        age.text = str;
    }

    public void setUser(string str)
    {
        user.text = "u/"+str;
    }

    public void setFlair(string str)
    {
        flair.text = str;
    }

    public void setComments(string str)
    {
        comments.text = str + " comments";
    }

    public void setScore(string str)
    {
        score.text = str;
    }

    public void setPermalink(string str)
    {
        permalink = str;
    }
}
