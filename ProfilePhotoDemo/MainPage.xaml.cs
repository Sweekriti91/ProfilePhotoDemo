using Microsoft.Maui.Graphics.Platform;
using IImage = Microsoft.Maui.Graphics.IImage;

namespace ProfilePhotoDemo
{

public partial class MainPage : ContentPage
{
     private IImage _image;
    private float _rotationAngle = 0;
    private float _scale = 1;
    private float _startScale = 1;
    private Point _startPoint;
    private Point _currentTranslation;

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
                _scale = 1; // Reset scale
                _currentTranslation = new Point(0, 0); // Reset translation

                if (_image != null)
                {
                    image.Drawable = new ImageDrawable(_image, _rotationAngle, _scale, _currentTranslation);
                    image.Invalidate();
                }
            }
        }
    }

    void rotatePhotoClock_Clicked(System.Object sender, System.EventArgs e)
    {
            if (_image != null)
            {
                _rotationAngle = (_rotationAngle + 90) % 360;
                image.Drawable = new ImageDrawable(_image, _rotationAngle, _scale, _currentTranslation);
                image.Invalidate();
            }
    }

    void pickPhoto_Clicked(System.Object sender, System.EventArgs e)
    {
        PickPhoto();
    }

    private void OnPinchUpdated(object sender, PinchGestureUpdatedEventArgs e)
    {
        if (e.Status == GestureStatus.Started)
        {
            // Store the initial scale factor
            _startScale = _scale;
        }
        else if (e.Status == GestureStatus.Running)
        {
            // Calculate the scale factor
            _scale = (float)(_startScale * e.Scale);
            image.Drawable = new ImageDrawable(_image, _rotationAngle, _scale, _currentTranslation);
            image.Invalidate();
        }
    }

    private void OnPanUpdated(object sender, PanUpdatedEventArgs e)
    {
        if (_image == null)
            return;
        if (e.StatusType == GestureStatus.Running)
        {
            // Update the translation point
            _currentTranslation = new Point(_currentTranslation.X + e.TotalX, _currentTranslation.Y + e.TotalY);
            image.Drawable = new ImageDrawable(_image, _rotationAngle, _scale, _currentTranslation);
            image.Invalidate();
        }
    }
}

    public class ImageDrawable : IDrawable
    {
        private readonly IImage _image;
        private readonly float _rotationAngle;
        private readonly float _scale;
        private readonly Point _translation;

        public ImageDrawable(IImage image, float rotationAngle, float scale, Point translation)
        {
            _image = image;
            _rotationAngle = rotationAngle;
            _scale = scale;
            _translation = translation;
        }
        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            if (_image == null)
                return;

            var width = 350;
            var height = 350;
            var scaledWidth = width * _scale;
            var scaledHeight = height * _scale;
            var centerX = (float)(dirtyRect.Width / 2);
            var centerY = (float)(dirtyRect.Height / 2);

            // var pathCircle = new PathF().AppendCircle(centerX, centerY, Math.Min(centerX, centerY));
            // var clipCircle = pathCircle.AppendCircle(centerX, centerY, Math.Min(centerX, centerY));
            PathF path = new PathF();
            path.AppendCircle(centerX, centerY, Math.Min(centerX, centerY));

            // Apply translation, rotation, and scaling transformations and circle clipping
           canvas.SaveState();
            canvas.ClipPath(path);
            canvas.Translate((float)(_translation.X + (dirtyRect.Width - scaledWidth) / 2), (float)(_translation.Y + (dirtyRect.Height - scaledHeight) / 2));
            canvas.Scale(_scale, _scale);
            canvas.Rotate(_rotationAngle, width / 2, height / 2);
            canvas.DrawImage(_image, 0, 0, width, height);
            canvas.RestoreState();
        }
    }

}
