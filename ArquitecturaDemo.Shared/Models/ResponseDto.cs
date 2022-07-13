namespace ArquitecturaDemo.Shared.Models
{
    public class ResponseDto<T>
    {
        public bool IsSuccess { get; set; } = true;
        public T? Value { get; set; }
        public string? DisplayMessage { get; set; }
        public string? ErrorMessage { get; set; }
    }
}