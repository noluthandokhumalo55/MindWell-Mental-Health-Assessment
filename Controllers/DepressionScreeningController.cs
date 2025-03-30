using Microsoft.AspNetCore.Mvc;
using Mindwell.Models;

namespace Mindwell.Controllers
{
    public class DepressionScreeningController : Controller
    {
        // Timer duration in seconds (5 minutes)
        private const int TimerDuration = 300;

        // This method loads the initial mental health assessment view
        public IActionResult Index()
        {
            return View("~/Views/Blocks/DepressionScreening.cshtml");
        }

        // This method handles the form submission and processes the answers
        [HttpPost]
        public IActionResult Submit(MentalHealthAssessmentModel model)
        {
            // Ensure the model is valid
            if (ModelState.IsValid)
            {
                // Calculate the depression score based on the selected answers
                // Each answer ranges from 0 to 3, so the sum will give a score from 0 to 9
                int score = model.Answers.Sum();

                // Ensure the score is within the expected range (0-9)
                score = Math.Min(score, 9); // If score exceeds 9, limit it to 9

                // Store the score in TempData (or the database, as needed)
                TempData["Score"] = score;

                // Redirect to the "Thank You" page with the score
                return RedirectToAction("ThankYou");
            }

            // If the model is invalid, reload the form with validation errors
            return View("~/Views/Blocks/DepressionScreening.cshtml", model);
        }

        // This method displays a "Thank You" page and shows the score
        public IActionResult ThankYou()
        {
            // Retrieve the score from TempData
            var score = TempData["Score"] ?? 0;

            // Display the score in the view
            ViewBag.Score = score;

            return View("~/Views/Blocks/ThankYou.cshtml");
        }
    }
}
