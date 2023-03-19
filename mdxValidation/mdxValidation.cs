using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace mdxValidation
{
    public interface IValidation
    {
        Boolean validate(XElement validationContent);
    }

    internal class mdxFXValidation : IValidation
    {
        bool IValidation.validate(XElement Content)
        {
            return true;
        }
    }

    public static class mdxValidator
    {
        public static IValidation ValidatorForItem(string mdxType)
        {
            return new mdxFXValidation();
        }

    }

}
