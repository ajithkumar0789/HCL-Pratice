namespace TrainingManagementSystem.Models;

public class Enrollment
{
    public int EnrollmentId { get; set; }
    public string LearnerId { get; set; }
    public string CourseCode { get; set; }
    public DateTime EnrollmentDate { get; set; }

    public decimal BaseFee { get; set; }
    public decimal DiscountAmount { get; set; }
    public decimal FinalFee { get; set; }

    public EnrollmentStatus Status { get; set; }

    public Enrollment(
        int enrollmentId,
        string learnerId,
        string courseCode,
        decimal baseFee,
        decimal discountAmount,
        decimal finalFee)
    {
        EnrollmentId = enrollmentId;
        LearnerId = learnerId;
        CourseCode = courseCode;
        EnrollmentDate = DateTime.Now;

        BaseFee = baseFee;
        DiscountAmount = discountAmount;
        FinalFee = finalFee;

        Status = EnrollmentStatus.Active;
    }
}