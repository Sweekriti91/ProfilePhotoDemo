using AndroidX.Lifecycle;
using Google.Android.Material.Color.Utilities;
using Microsoft.Maui.Graphics.Platform;
using System;
using IImage = Microsoft.Maui.Graphics.IImage;

namespace ProfilePhotoDemo;

public partial class MainPage : ContentPage, IDrawable
{
    public IImage imageDraw { get; set; }
    public MainPage()
	{
        usePhoto.IsVisible = false;
        rotatePhotoClock.IsVisible = false;
        image.IsVisible = true;
       
        takePhoto.Clicked += async (sender, args) =>
        {
            TakePhoto();

            usePhoto.IsVisible = true;
            image.IsVisible = true;
            rotatePhotoClock.IsVisible = true;
        };

        pickPhoto.Clicked += async (sender, args) =>
        {
            PickPhoto();
           
            usePhoto.IsVisible = true;
            rotatePhotoClock.IsVisible = true;
            image.IsVisible = true;
        };

        rotatePhotoClock.Clicked += async (sender, args) =>
        {
            var service = new PlatformBitmapExportService();
            var context = service.CreateContext(350, 350);
            var canvas = context.Canvas;

            // draw using canvas
            var imageRotate = context.Image;
            canvas.Rotate(90);
            canvas.DrawImage(imageDraw, 10, 10, 350, 350);
            Draw(canvas, new RectF(10, 10, 350, 350));
            image.Drawable = this;
        };

    }

    public void Draw(ICanvas canvas, RectF dirtyRect)
    {
        if (imageDraw != null)
        {
            canvas.DrawImage(imageDraw, 10, 10, 350, 350);
        }
    }


    string base64image = "";
    public async void TakePhoto()
    {
        if (MediaPicker.Default.IsCaptureSupported)
        {
            FileResult photo = await MediaPicker.Default.CapturePhotoAsync();

            if (photo != null)
            {
                var stream = await photo.OpenReadAsync();

                var img = PlatformImage.FromStream(stream);

                if (img != null)
                {
                    imageDraw = img;
                    image.Drawable = this;
                }
            }
        }
    }

    public async void PickPhoto()
    {
        if (MediaPicker.Default.IsCaptureSupported)
        {
            FileResult photo = await MediaPicker.Default.PickPhotoAsync();


            if (photo != null)
            {

                var stream = await photo.OpenReadAsync();

                var img = PlatformImage.FromStream(stream);

                if (img != null)
                {
                    imageDraw = img;
                    image.Drawable = this;
                }
            }
        }
    }
}


