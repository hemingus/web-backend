namespace web_backend.Models
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
