﻿using System;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace XamarinUIControls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
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

        private int currentImageIndex { get; set; }
        private double panX { get; set; }
        
        public ImageListControl()
        {
            this.InitializeComponent();
        }

        private void PanGesture_PanUpdated(object sender, PanUpdatedEventArgs e)
        {
            switch (e.StatusType)
            {
                case GestureStatus.Started:
                    this.panX = 0;
                    break;
                case GestureStatus.Running:
                    this.panX = e.TotalX;
                    break;
                case GestureStatus.Completed:
                    if (this.panX > 50)
                    {
                        ShowPrevious(this);
                    }
                    else if (this.panX < -50)
                    {
                        ShowNext(this);
                    }
                    break;
            }
        }

        private static void HandleImagesPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var imageListControl = (ImageListControl)bindable;
            var imageList = (ImageList)newValue;
            if (imageListControl != null && imageList != null)
            {
                imageListControl.CountGrid.Children.Clear();
                imageListControl.CountGrid.RowDefinitions.Clear();
                imageListControl.CountGrid.ColumnDefinitions.Clear();
                imageListControl.CountGrid.ColumnDefinitions = new ColumnDefinitionCollection();
                for (int index = 0; index < imageList.Count; index = index + 1)
                {
                    imageListControl.CountGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Star });

                    var button = new Button() { BackgroundColor = Color.Gray };
                    var buttonId = index;
                    button.Clicked += (sender, e) =>
                    {
                        ShowImage(imageListControl, buttonId);
                    };

                    imageListControl.CountGrid.Children.Add(button);
                    Grid.SetColumn(button, index);
                }

                imageListControl.CountGrid.IsVisible = (imageList.Count > 1);

                imageListControl.currentImageIndex = 0;
                ShowImage(imageListControl, 0);
            }
        }

        private static void ShowImage(ImageListControl imageListControl, int imageIndex)
        {
            var newImage = imageListControl.Images[imageIndex];
            imageListControl.ImageControl.Source = new UriImageSource()
            {
                Uri = new Uri(newImage.PhotoUrl),
                CachingEnabled = true,
                CacheValidity = TimeSpan.FromMinutes(5)
            };

            imageListControl.TitleLabel.Text = newImage.Title;
            imageListControl.SubTitleLabel.Text = newImage.SubTitle;
            if (string.IsNullOrWhiteSpace(newImage.Title) && string.IsNullOrWhiteSpace(newImage.SubTitle))
            {
                imageListControl.TitleBox.IsVisible = false;
            }
            else
            {
                imageListControl.TitleBox.IsVisible = true;
            }

            imageListControl.CountGrid.Children.ElementAt(imageListControl.currentImageIndex).BackgroundColor = Color.Gray;
            imageListControl.CountGrid.Children.ElementAt(imageIndex).BackgroundColor = Color.White;
            imageListControl.currentImageIndex = imageIndex;
        }

        private static void ShowNext(ImageListControl imageListControl)
        {
            if (imageListControl.currentImageIndex >= imageListControl.Images.Count - 1)
            {
                return;
            }

            ShowImage(imageListControl, imageListControl.currentImageIndex + 1);
        }

        private static void ShowPrevious(ImageListControl imageListControl)
        {
            if (imageListControl.currentImageIndex <= 0)
            {
                return;
            }

            ShowImage(imageListControl, imageListControl.currentImageIndex - 1);
        }
    }
}
