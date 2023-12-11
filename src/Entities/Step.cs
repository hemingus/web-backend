namespace web_backend.Entities
{
    public class Step
    {
        public string StepId { get; set; }
        public string Description { get; set; }
        public Boolean IsComplete { get; set; }
        public string TimeCreated { get; set; }

        public Step(string description)
        {
            StepId = Guid.NewGuid().ToString();
            Description = description;
            IsComplete = false;
            TimeCreated = DateTime.Now.ToString();

        }
    }
}
