using System;
using System.Windows.Forms;

namespace ClThreadIndex
{
    public partial class Form1 : Form
    {
        int pc;
        Users myUsers;
        Page myPage;

        public Form1()
        {
            InitializeComponent();
        }

        private void btnGetSource_Click(object sender, EventArgs e)
        {
            
            pc = 0;
            lblPostCount.Text = "";
            if (txtThreadURL.Text.Contains("thecolorless.net/posts/"))
            {
                myUsers = new Users(txtThreadURL.Text);
                myPage = new Page(txtThreadURL.Text, "0");

                btnGetSource.Enabled = false;

                while (myPage.getNextPage())
                {
                    lblThreadTitle.Text = myPage.ThreadTitle;
                    myUsers.addUsers(myPage);
                    pc += myPage.PostCount;
                    lblPostCount.Text = pc.ToString();
                    Refresh();
                }

                if (myUsers.myUsers.Count > 0)
                {
                    myUsers.doTheCreep();
                    myUsers.generateHTML(myPage.ThreadTitle);
                    myUsers.myUsers.Clear();
                }
                btnGetSource.Enabled = true;
            }
            else
            {
                MessageBox.Show("Not a valid thread! Try again :3");
            }
        }
    }
}
