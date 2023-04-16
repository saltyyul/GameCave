using System.ComponentModel.DataAnnotations;

namespace GameCave.Data.Attributes
{
    public class EnsureOneElementAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value is List<int> list)
            {
                return list.Count > 0;
            }

            return false;
        }
    }
}
