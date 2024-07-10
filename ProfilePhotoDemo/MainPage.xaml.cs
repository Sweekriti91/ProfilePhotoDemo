using Microsoft.Maui.Graphics.Platform;
using IImage = Microsoft.Maui.Graphics.IImage;

namespace ProfilePhotoDemo
{

public partial class MainPage : ContentPage
{
     private IImage _image;
    private float _rotationAngle = 0;
    public MainPage()
	{
        InitializeComponent();
    }

    public async void PickPhoto()
    {
        if (MediaPicker.Default.IsCaptureSupported)
        {
            FileResult photo = await MediaPicker.Default.PickPhotoAsync();
            if (photo != null)
            {
                var stream = await photo.OpenReadAsync();

                _image = PlatformImage.FromStream(stream);
                _rotationAngle = 0; // Reset rotation angle

                if (_image != null)
                {
                    image.Drawable = new ImageDrawable(_image, _rotationAngle);
                    image.Invalidate();
                }
            }
        }
    }

    void rotatePhotoClock_Clicked(System.Object sender, System.EventArgs e)
    {
            // var service = new PlatformBitmapExportService();
            // var context = service.CreateContext(350, 350);
            // var canvas = context.Canvas;

            // // draw using canvas
            // var imageRotate = context.Image;
            // canvas.Rotate(90);
            // // canvas.DrawImage(imageDraw, 10, 10, 350, 350);
            // Draw(canvas, new RectF(10, 10, 350, 350));
            // image.Drawable = this;

            if (_image != null)
            {
                _rotationAngle = (_rotationAngle + 90) % 360;
                image.Drawable = new ImageDrawable(_image, _rotationAngle);
                image.Invalidate();
            }
    }

    void pickPhoto_Clicked(System.Object sender, System.EventArgs e)
    {
        PickPhoto();
    }
}

    public class ImageDrawable : IDrawable
    {
        private readonly IImage _image;
        private readonly float _rotationAngle;

        public ImageDrawable(IImage image, float rotationAngle)
        {
            _image = image;
            _rotationAngle = rotationAngle;
        }

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            if (_image == null)
                return;

            var width = 350;
            var height = 350;

            // Apply rotation transformation
            canvas.SaveState();
            canvas.Rotate(_rotationAngle, width / 2, height / 2);
            canvas.DrawImage(_image, 0, 0, width, height);
            canvas.RestoreState();
        }
    }

}
