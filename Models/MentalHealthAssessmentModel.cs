using System.ComponentModel.DataAnnotations;

namespace Mindwell.Models
{
    public class MentalHealthAssessmentModel
    {
        // A property to store the answers selected by the user (array of integers)
        [Required]
        public int[] Answers { get; set; }
    }
}
