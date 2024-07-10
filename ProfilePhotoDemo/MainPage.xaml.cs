using Microsoft.Maui.Graphics.Platform;
using IImage = Microsoft.Maui.Graphics.IImage;

namespace ProfilePhotoDemo;

public partial class MainPage : ContentPage, IDrawable
{
    public IImage imageDraw { get; set; }
    public MainPage()
	{
        InitializeComponent();
    }

    public void Draw(ICanvas canvas, RectF dirtyRect)
    {
        if (imageDraw != null)
        {
            canvas.DrawImage(imageDraw, 10, 10, 350, 350);
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

    void rotatePhotoClock_Clicked(System.Object sender, System.EventArgs e)
    {
            var service = new PlatformBitmapExportService();
            var context = service.CreateContext(350, 350);
            var canvas = context.Canvas;

            // draw using canvas
            var imageRotate = context.Image;
            canvas.Rotate(90);
            // canvas.DrawImage(imageDraw, 10, 10, 350, 350);
            Draw(canvas, new RectF(10, 10, 350, 350));
            image.Drawable = this;
    }

    void pickPhoto_Clicked(System.Object sender, System.EventArgs e)
    {
        PickPhoto();
    }
}


