namespace web_backend.Models
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
