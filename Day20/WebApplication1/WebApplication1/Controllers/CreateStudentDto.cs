namespace WebApplication1.Controllers
{
    public class CreateStudentDto
    {
        public string? Name { get; internal set; }
        public string? Email { get; internal set; }
        public int Age { get; internal set; }
    }
}