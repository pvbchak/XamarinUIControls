using System;
using System.Linq;
using Xamarin.Forms;

namespace XamarinUIControls
{
    public partial class ImageListControl : Grid
    {
        public static readonly BindableProperty ImagesProperty = BindableProperty.Create(
            propertyName: nameof(Images),
            returnType: typeof(ImageList),
            declaringType: typeof(ImageListControl),
            defaultValue: null,
            defaultBindingMode: BindingMode.OneWay,
            propertyChanged: HandleImagesPropertyChanged);

        public ImageList Images
        {
            get
            {
                return (ImageList)GetValue(ImagesProperty);
            }
            set
            {
                SetValue(ImagesProperty, value);
            }
        }

        private static void HandleImagesPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var imageInfoControl = (ImageListControl)bindable;
            var images = (ImageList)newValue;
            if (imageInfoControl != null && images != null)
            {
                imageInfoControl.CountGrid.Children.Clear();
                imageInfoControl.CountGrid.ColumnDefinitions.Clear();
                imageInfoControl.CountGrid.ColumnDefinitions = new ColumnDefinitionCollection();
                for (int i = 0; i < images.Count; i++)
                {
                    imageInfoControl.CountGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Star });
                }

                imageInfoControl.CountGrid.Children.AddHorizontal(new Button() { BackgroundColor = Color.White });
                for (int i = 1; i < images.Count; i++)
                {
                    imageInfoControl.CountGrid.Children.AddHorizontal(new Button() { BackgroundColor = Color.WhiteSmoke });
                }

                var defaultImage = images.FirstOrDefault();
                imageInfoControl.ImageControl.Source = new UriImageSource() { Uri = new Uri(defaultImage.PhotoUrl), CachingEnabled = true };
                imageInfoControl.TitleLabel.Text = defaultImage.Title;
                imageInfoControl.SubTitleLabel.Text = defaultImage.SubTitle;
            }
        }

        public ImageListControl()
        {
            this.InitializeComponent();
        }
    }
}
