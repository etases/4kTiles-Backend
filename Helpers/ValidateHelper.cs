using System.ComponentModel.DataAnnotations;

namespace _4kTiles_Backend.Helpers
{
    public static class ValidationHelper
    {

        /// <summary>
        /// Validate annotated object
        /// </summary>
        /// <param name="obj">object</param>
        /// <param name="results">list of errors</param>
        /// <returns>if object validated successfully</returns>
        public static bool Validate(this object obj, out List<ValidationResult> results)
        {
            ValidationContext context = new ValidationContext(obj, null, null);
            results = new List<ValidationResult>();
            return Validator.TryValidateObject(obj, context, results, true);
        }
    }
}