using System.Collections.Generic;

namespace XamarinUIControls
{
    public class ImageList : List<Image>
    {
    }

    public class Image
    {
        public string PhotoUrl { get; set; }
        public string Title { get; set; }
        public string SubTitle { get; set; }
    }
}
