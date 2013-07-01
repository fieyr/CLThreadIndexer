using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ClThreadIndex
{
    class Users
    {
        public String BaseURL { get; set; }
        public List<Post> myUsers = new List<Post>();
        private String URL { get; set; }
        public Users(String url)
        {
            this.BaseURL = url.Split('?')[0] + "?offset=";
        }
        public void addUsers(Page myPage)
        {
            foreach (var post in myPage.PostsHeavyWithImage)
            {
                myUsers.Add(post);
            }
        }

        //Iterate sequentially through each post, get user, read forward to find all other posts created by that user.
        //Add images to the first post from the posts found after the first. After images from each other post are added to the first, delete that other post.
        public void doTheCreep()
        {
            int current = 0;
            int reading = 1;

            while (current < myUsers.Count)
            {
                while (reading < myUsers.Count)
                {
                    if (myUsers[current].UserName == myUsers[reading].UserName)
                    {
                        foreach (var image in myUsers[reading].Images)
                        {
                            myUsers[current].addImage(image.ImageURL,image.PageNum);
                        }
                        myUsers.RemoveAt(reading);
                    }
                    else
                    {
                        reading++;
                    }
                }
                current++;
                reading = current + 1;
            }
        }

        public IEnumerable<Post> orderByUser()
        {
            IEnumerable<Post> query = myUsers.OrderBy(user => user.UserName);
            return query;
        }

        public void generateHTML(String threadTitle)
        {
            String dq = "\"";
            IEnumerable<Post> users = orderByUser();

            StringBuilder s = new StringBuilder();


            s.AppendLine("<!DOCTYPE html>");
            s.AppendLine("<html>");
            s.AppendLine("<head>");
            s.AppendLine("\t<meta charset=" + dq + "utf-8" + dq + ">");
            s.AppendLine("</head>");
            s.AppendLine("<body>");
            s.AppendLine("<h1>"+ threadTitle + "</h1>");
            foreach (var user in users)
            {
                //Add Gravatar and Username
                s.AppendLine("\t<div class=" + dq +"user" + dq +">");
                s.AppendLine("\t\t<a href=" + dq + "http://thecolorless.net/users/" + user.UserName + dq + ">");
                s.AppendLine("\t\t\t<img src=" + dq + user.Gravatar + dq + ">" + "</br>" + user.UserName);
                s.AppendLine("\t\t</a>");
                s.AppendLine("\t</div>");
                
                //Add Images and Image Info
                int imageNum = 1;
                foreach (var image in user.Images)
	            {
                    s.AppendLine("\t<div class=" + dq + "links" + dq + ">");
                    s.AppendLine("\t\t<div class=" + dq + "linkset" + dq + ">");
                    s.AppendLine("\t\t\t<a href=" + dq + this.BaseURL + image.PageNum + dq + " title=" + dq + "Page " + image.getPageNum() + dq + ">Page " + image.getPageNum() + "</a>");
                    s.AppendLine("\t\t\t<a href=" + dq + image.ImageURL + dq + " title=" + dq + "Link " + imageNum + dq + ">Link " + imageNum + "</a>");
                    s.AppendLine("\t\t</div>");
                    s.AppendLine("\t</div>");
                    imageNum++;
	            }
            }

            s.AppendLine("</body>");
            s.AppendLine("</html>");

            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "HTML File|*.html";
            saveFileDialog1.FileName = threadTitle + ".html";

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                System.IO.File.WriteAllText(@"" + saveFileDialog1.FileName, s.ToString());
            }
        }
    }
}
