namespace web_backend.Models.DTOs
{
    public class TaskEntityUpdateOrderDto
    {
        public int Order { get; set; }
        public TaskEntityUpdateOrderDto(int order)
        {
            Order = order;
        }
    }
}
