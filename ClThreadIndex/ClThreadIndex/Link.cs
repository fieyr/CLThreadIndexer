using System;

namespace ClThreadIndex
{
    class Link
    {
        public string LinkURL { get; set; }
        public int PageNum { get; set; }

        public Link(String linkURL,int pageNum)
        {
            this.LinkURL = linkURL;
            this.PageNum = pageNum;
        }

        public int getPageNum()
        {
            return this.PageNum==0 ? 1: (this.PageNum / 20) + 1;
        }
    }
}
