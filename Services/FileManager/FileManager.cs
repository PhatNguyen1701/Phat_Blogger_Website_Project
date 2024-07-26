using PhotoSauce.MagicScaler;

namespace Phat_Blogger_Website.Services.FileManager
{
    public class FileManager : IFileManager
    {
        private string _imagePath;

        //Sử dụng Configuration vì tại appsetting.json ta có thiết lập đường dẫn tới file ảnh
        public FileManager(IConfiguration config)
        {
            _imagePath = config["Path:Images"];
        }

        //implement phương thức truy cập file đã lưu
        public FileStream ImageStream(string image)
        {
            return new FileStream(Path.Combine(_imagePath, image), FileMode.Open, FileAccess.Read);
        }

        public bool RemoveImage(string image)
        {
            //So sánh hình ảnh thông qua đường dẫn, nếu file có tồn tại sẽ xóa nó
            try
            {
                var file = Path.Combine(_imagePath, image);
                if (File.Exists(file))
                {
                    File.Delete(file);
                }
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        //implement phương thức lưu file
        public async Task<string> SaveImage(IFormFile image)
        {
            try
            {
                //Tại đây ta đảm bảo rằng các thư mục liên quan chứa hình ảnh sẽ phải tồn tại khi web chạy
                //không nên sử dụng đường dẫn vì sẽ dễ xảy ra lỗi khó kiểm soát
                var save_path = Path.Combine(_imagePath);
                if (!Directory.Exists(save_path))
                {
                    //Nếu ko tồn tại đường dẫn ta sẽ khởi tạo lại nó
                    Directory.CreateDirectory(save_path);
                }

                //Ta khởi tạo mine và tên cho hình ảnh bằng ngày giờ đc tạo
                var mine = image.FileName.Substring(image.FileName.LastIndexOf("."));
                var fileName = $"img_{DateTime.Now.ToString("dd-MM-yy-HH-mm-ss")}{mine}";

                //Sau khi đã có mine và file name, ta đưa chúng vào luồng để đưa vào thư mục
                //Create 1 FileStream dùng pt copytoasync đưa vào luồng
                using (var fileStream = new FileStream(Path.Combine(save_path, fileName), FileMode.Create))
                {
                    //Ta dùng đối tượng MagicImage đã tạo để xuất hình ảnh lên web sau khi đã hiệu chỉnh
                    MagicImageProcessor.ProcessImage(image.OpenReadStream(), fileStream, ImageOptions());
                }

                return fileName;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return "Image Error";
            }
        }
        //Cài đặt thêm PhotoSauce.MagicScaler để hiệu chỉnh kích thước ảnh tải lên
        //Using thư viện
        private ProcessImageSettings ImageOptions() => new ProcessImageSettings
        {
            Width = 800,
            Height = 500,
            ResizeMode = CropScaleMode.Crop,
            SaveFormat = FileFormat.Jpeg,
            JpegQuality = 100,
            JpegSubsampleMode = ChromaSubsampleMode.Subsample420,
        };
    }
}
