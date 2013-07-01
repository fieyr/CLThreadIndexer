using System;

namespace ClThreadIndex
{
    class Image
    {
        public string ImageURL { get; set; }
        public int PageNum { get; set; }

        public Image(String imageURL,int pageNum)
        {
            this.ImageURL = imageURL;
            this.PageNum = pageNum;
        }

        public int getPageNum()
        {
            return this.PageNum==0 ? 1: (this.PageNum / 20) + 1;
        }
    }
}
