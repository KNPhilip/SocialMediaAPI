namespace TweetAPI.Dtos
{
    public class ResponseDto<T>
    {
        public T? Data { get; set; }
        public required bool Success { get; set; }
        public string? Error { get; set; }
        public required bool IsNew { get; set; }

        public static ResponseDto<T> SuccessResponse(T data) =>
            new() { Data = data, Success = true, Error = null, IsNew = false };

        public static ResponseDto<T> ErrorResponse(string message) =>
            new() { Success = false, Error = message, IsNew = false };

        public static ResponseDto<T> CreatedResponse(T data) =>
            new() { Data = data, Success = true, Error = null, IsNew = true };
    }
}
