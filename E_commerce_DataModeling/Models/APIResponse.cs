namespace E_commerce_DataModeling.Models
{
    public class APIResponse<T>
    {
        public T Result { get; set; }
        public int ResponseCode { get; set; }
        public Boolean Success { get; set; }
        public string Message { get; set; }
        public List<string> Errors { get; set; }

    }
}
