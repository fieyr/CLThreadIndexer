﻿using System;
using System.Collections.Generic;

namespace ClThreadIndex
{
    class Post
    {
        private String dq = "\"";
        public String UserName { get; set; }
        public List<Link> Links = new List<Link>();
        public String Gravatar { get; set; }
        public int PageNum { get; set; }
        
        public Post(String postSource,int pageNum)
        {
            getUserName(postSource);
            getGravatar(postSource);
            this.PageNum = pageNum;
            getLinks(postSource);
        }

        //Get Username
        private void getUserName(String postSource)
        {
            String PostAuthorContent = getSingleString(postSource, "<div class=" + dq + "author vcard" + dq + ">", "</div>");
            this.UserName = getSingleString(PostAuthorContent, "/users/", dq, true);
        }

        //Get Gravatar
        private void getGravatar(String postSource)
        {
            this.Gravatar = getSingleString(postSource, "http://gravatar.com", "?");
        }

        //Find all links in post
        private void getLinks(String postSource)
        {
            String postContent = getSingleString(postSource, "<div class=" + dq + "formatted entry-content" + dq + ">", "</div>");
            List<String> links = getMultipleStrings(postContent, "http://", dq);

            addLinks(links, this.PageNum);
        }

        //Acquire links from the passed array of links.
        public void acquireLinks(List<Link> links)
        {
            bool linkFound;
            foreach (var l in links)
            {
                linkFound = false;
                foreach (var myLink in Links)
                {
                    if (l.LinkURL == myLink.LinkURL)
                    {
                        linkFound = true;
                        break;
                    }
                }
                if (linkFound == false)
                    Links.Add(new Link(l.LinkURL, l.PageNum));
            }
        }

        //Add Links to Post
        public void addLinks(List<String> links, int pageNum)
        {
            bool linkFound;
            foreach (var l in links)
            {
                linkFound = false;
                foreach (var link in Links)
                {
                    if (l == link.LinkURL)
                    {
                        linkFound = true;
                        break;
                    }
                }
                if (linkFound == false)
                    Links.Add(new Link(l,pageNum));
            }
        }

        //Finds the first occurence of string within argument postSource using provided tokens to search.
        //useStartOffset is used to exclude the startToken from the returned substring.
        private String getSingleString(String postSource, String startToken, String endToken, bool useStartOffset = false)
        {
            String result = "";
            int StartIndex = postSource.IndexOf(startToken);
            int EndIndex = postSource.IndexOf(endToken, StartIndex);

            if (useStartOffset == false)
            {
                result = postSource.Substring(StartIndex, EndIndex - StartIndex);
            }
            else
            {
                result = postSource.Substring(StartIndex + startToken.Length, (EndIndex - StartIndex) - startToken.Length);
            }
            
            return result;
        }
        public List<String> getMultipleStrings(String postSource, String startToken, String endToken)
        {
            List<String> strings = new List<String>();
            int StartIndex = 0;
            int EndIndex = 0;

            StartIndex = postSource.IndexOf(startToken);
            if (StartIndex > -1)
                EndIndex = postSource.IndexOf(endToken, StartIndex);

            while (StartIndex != -1)
            {
                if (EndIndex != -1)
                {
                    String s = postSource.Substring(StartIndex, EndIndex - StartIndex);
                    strings.Add(s);

                    StartIndex = postSource.IndexOf(startToken, EndIndex);
                }
                else if (EndIndex == -1)
                {
                    StartIndex = StartIndex + startToken.Length;
                    StartIndex = postSource.IndexOf(startToken, StartIndex);
                }
                if (StartIndex > -1)
                    EndIndex = postSource.IndexOf(endToken, StartIndex);
            }

            return strings;
        }
    }
}
