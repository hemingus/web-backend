namespace web_backend.Models.DTOs
{
    public class SubtaskUpdateOrderDto
    {
        public int Order { get; set; }
        public SubtaskUpdateOrderDto(int order)
        {
            Order = order;
        }
    }
}
