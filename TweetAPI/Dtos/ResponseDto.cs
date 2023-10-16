namespace TweetAPI.Dtos
{
    public class ResponseDto<T>
    {
        public T? Data { get; set; }
        public required bool Success { get; set; }
        public string? Error { get; set; }
        public required int? NewId { get; set; }

        public static ResponseDto<T> SuccessResponse(T data) =>
            new() { Data = data, Success = true, Error = null, NewId = null };

        public static ResponseDto<T> ErrorResponse(string message) =>
            new() { Success = false, Error = message, NewId = null };

        public static ResponseDto<T> CreatedResponse(T data, int newId) =>
            new() { Data = data, Success = true, Error = null, NewId = newId };
    }
}
