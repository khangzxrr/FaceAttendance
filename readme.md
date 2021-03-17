# Điểm danh bằng webcam

* Hỗ trợ API phát hiện, nhận diện khuôn mặt
* Hỗ trợ trường lớp Điểm danh thông qua webcam (BUY TO USE)

viết trên webapp asp.net core MVC

## Ý tưởng:

Giáo viên sử dụng web, đăng nhập, chọn lớp và sinh viên sẽ lần lượt bước qua webcam, hệ thống sẽ tự nhận diện và check điểm danh vào web app sau đó giáo viên sẽ nhấn save để lưu lại điểm danh lớp hiện tại

Ok, ý tưởng là vậy. Tuy nhiên app sẽ phải có những quản lí khác như:

* Trường sẽ phải mua bản quyền có thời hạn để tạo lớp, thêm giáo viên, cấp quyền giáo viên có thể xem/ chỉnh sửa điểm danh. Bản quyền có thể kéo dài 1 năm, 2 năm,... bản dùng thử có thể kéo dài 36 giờ

* Nếu không sử dụng điểm danh, thì các developer có thể mua API phát hiện, nhận diện khuôn mặt. Bản quyền sau khi mua sẽ được gia hạn 5K request / ngày, 10K request / ngày. Kéo dài 1 tháng,...

## Định hướng cách làm:

1. webapp sẽ phải tương tác được với webcam và quan trọng hơn phải tương tác được với hệ thống nhận diện khuôn mặt, hiện tại mình sẽ sử dụng [DLIB](http://dlib.net/imaging.html) (thư viện xử lí ảnh). Tuy nhiên, Dlib viết bằng C++ và chúng ta đang sử dụng [asp.net CORE](https://docs.microsoft.com/en-us/aspnet/core/?view=aspnetcore-5.0) nên không có thư viện warp dlib sang C#, vì vậy phải xây dựng một cách giao tiếp với C++ trên server **(Đang làm)**
    * Build DLIb kết hợp CUDA CUDNN để tăng hiệu suất **(Xong)**
    * Xây dựng một restful API kết hợp với DLIB **(Đang làm)**
2. Xây dựng giao thức API từ DLIB C++ sang webapp c# **(Chưa làm)**
