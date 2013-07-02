using System;
using System.Collections.Generic;
using System.Net;
using System.Windows.Forms;

namespace ClThreadIndex
{
    class Page
    {
        public int PageNum { get; set; }
        public string BaseURL;
        public string PageSource { get; set; }
        public List<Post> PostsHeavyWithLink = new List<Post>();
        public int PostCount { get; set; }
        public String ThreadTitle { get; set; }

        WebClient client = new WebClient();
        public Page(String url, String offset)
        {
            int pNum;
            bool result = Int32.TryParse(offset, out pNum);
            this.BaseURL = url.Split('?')[0] + "?offset=";
            this.PageNum = pNum; //If parse fails, use default 0
            getThreadTitle(getPage(this.BaseURL + this.PageNum,false));
        }

        public bool getNextPage()
        {
            PostsHeavyWithLink = new List<Post>();
            String url = this.BaseURL + this.PageNum;
            this.PageSource = getPage(url);
            bool result = getAllPosts();
            this.PageNum += 20;
            return result;
        }

        private void getThreadTitle(String pageSource)
        {
            if (pageSource != "")
            {
                String dq = "\"";
                String titleStartToken = "<h1 class=" + dq + "title entry-title" + dq + ">";
                String titleEndToken = "</h1>";

                int titleStart = pageSource.IndexOf(titleStartToken);
                int titleEnd = pageSource.IndexOf(titleEndToken, titleStart);

                this.ThreadTitle = pageSource.Substring(titleStart + titleStartToken.Length, (titleEnd - titleStart) - titleStartToken.Length).Trim();
            }
        }

        private String getPage(String url, bool showError = true )
        {
            String pageSource = string.Empty;
            try
            {
                pageSource = client.DownloadString(url);
            }
            catch
            {
                if (showError)
                {
                    MessageBox.Show("Not a valid thread! Try again :3");
                }
            }

            return pageSource;
        }

        private bool getAllPosts()
        {
            String dq = "\"";
            String postStartToken = "<article class=" + dq + "post hentry" + dq + ">";
            String postEndToken = "</article>";

            int postStart = PageSource.IndexOf(postStartToken);
            int postEnd = PageSource.IndexOf(postEndToken);

            int postcount = 0;
            while (postStart != -1)
            {
                String postSource = PageSource.Substring(postStart, postEnd - postStart);
                postcount += 1;
                this.PostCount += 1;
                Post myPost = createPost(postSource);
                if (myPost.Links.Count > 0)
                {
                    PostsHeavyWithLink.Add(myPost);
                }

                postStart = PageSource.IndexOf(postStartToken, postEnd);
                if(postStart > -1)
                    postEnd = PageSource.IndexOf(postEndToken, postStart);

            }
            if (postcount == 0) { postcount = 0; }
            return (postcount > 0);
        }


        public Post createPost(String postSource)
        {
            Post myPost = new Post(postSource,this.PageNum);
            return myPost;
        }
    }
}


