namespace web_backend.Models.DTOs
{
    public class StepUpdateOrderDto
    {
        public int Order { get; set; }
        public StepUpdateOrderDto(int order)
        {
            Order = order;
        }
    }
}
